namespace Omnia.Pie.Vtm.Devices.Emv
{
	using AxNHMWIEMVLib;
	using AxNXCardReaderXLib;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System;
	using System.Threading.Tasks;

	public class ProcessEmv
	{
		private readonly EmvProcessHelper _emvHelper;
		private readonly AxNXCardReaderX _cardReaderCom;
		private string ClassName = "ProcessEmvStage ";
		private bool appProcessStatus;
		private string transactionType;
		private string track2Data;
		private string pan;
		private string AID;
		private string AIDLabel;
		readonly ILogger Logger;

		public ProcessEmv(ILogger logger, AxNHMwiEmv emvCom, AxNXCardReaderX cardReaderCom)
		{
			Logger = logger;
			_emvHelper = new EmvProcessHelper(logger, emvCom);
			_cardReaderCom = cardReaderCom;

			Logger.Info(ClassName + " Initialize()");

			HasChip = false;
			HasChipErrors = false;
			track2Data = string.Empty;
			pan = string.Empty;
			AID = string.Empty;
			AIDLabel = string.Empty;
		}

		public string AccountNumber { get; set; }
		public string Track2Data { get { return track2Data; } }
		public string PanData { get { return pan; } }
		public string MSRFallbackTagList { get { return "9F390180"; } }
		public string ApplicationID { get { return AID; } }
		public string ApplicationLabel { get { return AIDLabel; } }
		public bool HasChip { get; set; }
		public bool HasChipErrors { get; set; }
		public bool CryptogramFailure { get; set; }

		public async Task Activate()
		{
			appProcessStatus = false;
			CryptogramFailure = false;
			transactionType = string.Empty;

			Logger.Info(ClassName + " Activate()");

			try
			{
				HasChip = await new InitChipOperation(_cardReaderCom, Logger).ExecuteAsync();
			}
			catch (Exception ex)
			{
				Logger.Exception(ex);
				Logger.Info(ClassName + " Activate() - Card Chip Read Failed. Setting hasChip to false.");
				HasChip = false;
			}
		}

		public bool ProcessApplication(int pAmount, string pTransactionType)
		{
			Logger.Info(ClassName + " ProcessApplication()");

			if (HasChip && !HasChipErrors)
			{
				_emvHelper.Account = AccountNumber;
				_emvHelper.Amount = new decimal(pAmount);
				transactionType = pTransactionType;
				_emvHelper.TransactionType = pTransactionType;
				_emvHelper.CurrencyCode = "0784";//BaseConfigurations.EMV.CurrencyCode;
				_emvHelper.CountryCode = "0784";//BaseConfigurations.EMV.CountryCode;

				try
				{
					if (_emvHelper.SetupKernel())
					{
						if (_emvHelper.ProcessCardApplication())
						{
							AID = _emvHelper.SelectedAID;
							AIDLabel = _emvHelper.SelectedAIDLabel;

							if (_emvHelper.ProcessRestrictions())
							{
								appProcessStatus = true;

								// populate track2
								track2Data = _emvHelper.GetTrack2Data();
								track2Data = track2Data
									.Replace("A", ":")
									.Replace("B", ";")
									.Replace("C", "<")
									.Replace("D", "=")
									.Replace("E", ">")
									.Replace("F", "?")
									.TrimEnd('?');

								pan = _emvHelper.GetPanData();
								Logger.Info($"{ClassName} {nameof(ProcessApplication)} EMV Track2.length = {track2Data?.Length}");
							}
						}
					}
				}
				catch (UnexpectedChipReadException ex)
				{
					HasChip = false;
					Logger.Info($"{ClassName} -> UnexpectedChipReadException!!! {ex.Message}");
					throw;
				}
				catch (Exception ex)
				{
					HasChipErrors = true;
					Logger.Exception(ex);
				}
			}

			return appProcessStatus;
		}

		public string ProcessPIN(string pinBlock)
		{
			Logger.Info(ClassName + $"ProcessPIN.length({pinBlock?.Length})");
			if (appProcessStatus)
			{
				if (_emvHelper.ProcessCVMAndPIN(pinBlock))
				{
					try
					{
						string emvTags = _emvHelper.CollectEMVTags();

						return emvTags;
					}
					catch (CryptogramFailureException cfe)
					{
						this.CryptogramFailure = true;
						Logger.Exception(cfe);
						throw;
					}
				}
			}

			Logger.Info(ClassName + "ProcessPIN() -> returning empty emv tags");
			return string.Empty;
		}

		public void HandleUnsuccessfulOnlineApproval()
		{
			Logger.Info(ClassName + "HandleUnsuccessfulOnlineApproval()");

			if (!HasChip && HasChipErrors)
				return;

			_emvHelper.HandleUnsuccessfulOnlineApproval();
		}

		public bool ProcessOnlineApproval(string respCode, string aRPCData, string issuScriptData1, string issuScriptData2)
		{
			Logger.Info(ClassName + "ProcessOnlineApproval()");

			if (!HasChip && HasChipErrors)
				return false;

			if (CryptogramFailure)
			{
				Logger.Info(ClassName + "-[ProcessOnlineApproval] CryptogramFailure encountered earlier, terminating the process with failed result");
				_emvHelper.StartOnlineApproval(null, 0, null, 0, null, "AAC");

				return false;
			}

			string acquirerCID = string.Empty;
			if (respCode == "00")
			{
				if (transactionType == EmvTransactionCode.BALANCE_INQUIRY || transactionType == EmvTransactionCode.GENERAL_INQUIRY)
				{
					Logger.Info(ClassName + "ProcessOnlineApproval() - Transaction Type: Inquiry. Will use AAC");
					// for M-TIP 18 Test 01 Scenario 01 
					acquirerCID = "AAC";
				}
				else
				{
					acquirerCID = "TC";
				}
			}
			else
			{
				acquirerCID = "AAC";
			}

			bool result = false;

			try
			{
				if (respCode == string.Empty)
				{
					Logger.Info(ClassName + "-[ProcessOnlineApproval] RespCode is Empty");
					result = _emvHelper.StartOnlineApproval(null, 0, null, 0, null, acquirerCID);
					Logger.Info(ClassName + "-[ProcessOnlineApproval] Online Approval: " + result);
				}
				else if (respCode != string.Empty && aRPCData == string.Empty && issuScriptData1 == string.Empty && issuScriptData2 == string.Empty)
				{
					Logger.Info(ClassName + "-[ProcessOnlineApproval] RespCode is Not Empty but aRPCData is Empty");
					result = _emvHelper.StartOnlineApproval(respCode, 0, null, 0, null, acquirerCID);
					Logger.Info(ClassName + "-[ProcessOnlineApproval] Online Approval: " + result);
				}
				else if (respCode != string.Empty && aRPCData != string.Empty && issuScriptData1 == string.Empty && issuScriptData2 == string.Empty)
				{
					Logger.Info(ClassName + "-[ProcessOnlineApproval] RespCode is Not Empty and aRPCData is Not Empty. Both Issuer is Empty but will proceed with normal processing");
					result = _emvHelper.StartOnlineApproval(respCode, aRPCData.Length, aRPCData, 0, null, acquirerCID);
					Logger.Info(ClassName + "-[ProcessOnlineApproval] Online Approval: " + result);
				}
				else if (respCode != string.Empty && aRPCData != string.Empty && (issuScriptData1 != string.Empty || issuScriptData2 != string.Empty))
				{
					string issuScriptData = string.Empty;
					int issuScriptDataLen = 0;

					if (issuScriptData1 != string.Empty)
					{
						string lenInHex = (issuScriptData1.Length / 2).ToString("X");
						string lenInHexCorrected = (lenInHex.Length < 2 ? "0" + lenInHex : lenInHex);
						issuScriptData = "71" + lenInHexCorrected + issuScriptData1;
						issuScriptDataLen = issuScriptData.Length;
					}
					else
					{
						string lenInHex = (issuScriptData2.Length / 2).ToString("X");
						string lenInHexCorrected = (lenInHex.Length < 2 ? "0" + lenInHex : lenInHex);
						issuScriptData = "72" + lenInHexCorrected + issuScriptData2;
						issuScriptDataLen = issuScriptData.Length;
					}

					Logger.Info(ClassName + "-[ProcessOnlineApproval] RespCode is Not Empty and aRPCData is Not Empty. One of the Issuer is Not Empty so will proceed with normal processing");
					result = _emvHelper.StartOnlineApproval(respCode, aRPCData.Length, aRPCData, issuScriptDataLen, issuScriptData, acquirerCID);
					Logger.Info(ClassName + "-[ProcessOnlineApproval] Online Approval: " + result);
				}

				string tags = _emvHelper.CollectEMVTags();

				Logger.Info(ClassName + "-[ProcessOnlineApproval] Online Approval is Done. Getting updated tags list once again.");
				Logger.Info(ClassName + $"-[ProcessOnlineApproval] length {tags?.Length}");
			}
			catch (Exception ex)
			{
				Logger.Exception(ex);
			}

			return result;
		}

		public async Task Deactivate()
		{
			Logger.Info(ClassName + " Deactivate()");

			if (HasChip)
			{
				try
				{
					await new PowerOffChipOperation(_cardReaderCom, Logger).ExecuteAsync();
				}
				catch (Exception ex)
				{
					Logger.Exception(ex);
				}
			}
		}
	}
}