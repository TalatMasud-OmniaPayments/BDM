namespace Omnia.Pie.Vtm.Devices.Emv
{
	using AxNHMWIEMVLib;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System;
	using System.Globalization;
	using System.Text;

	internal class EmvProcessHelper
	{
		private AxNHMwiEmv _AxNHMwiEmv;
		private readonly ILogger Logger;

		public EmvProcessHelper(ILogger logger, AxNHMwiEmv ax)
		{
			Logger = logger;
			this._AxNHMwiEmv = ax;
		}

		public decimal Amount { get; set; }
		public string Account { set; get; }
		public string CurrencyCode { set; get; }
		public string CountryCode { set; get; }
		public string TransactionType { set; get; }
		public string SelectedAID { get; set; }
		public string SelectedAIDLabel { get; set; }

		public bool SetupKernel()
		{
			try
			{
				_AxNHMwiEmv.EMV_ActivateDebug();
				var kType = _AxNHMwiEmv.EMV_SetKernelType(6);
                
				if (kType != 1)
					return false;

				var sI = _AxNHMwiEmv.EMV_SystemInitial();

				if (sI != 1)
				{
					Logger.Info(nameof(EmvProcessHelper) + $"[EMV_SystemInitial] Failed please check TERM_DATA.ini");
					return false;
				}
			}
			catch (Exception e)
			{
				Logger.Error(nameof(EmvProcessHelper) + $"Exeption: {e}");
				Logger.Exception(e);
			}

			return true;
		}

		public bool ProcessCardApplication()
		{
			var bCandidate = _AxNHMwiEmv.EMV_Sel_BuildCandidateApp();

			Logger.Info(nameof(EmvProcessHelper) + $"[EMV_Sel_BuildCandidateApp] Success? {(bCandidate == 1 ? "YES" : "NO")}");

			if (bCandidate != 1)
			{
				throw new CardDeclinedException("The use of this card has been declined. Please contact your bank.");
			}

			var appList = string.Empty;
			var getAppList = _AxNHMwiEmv.EMV_Sel_GetAppListEx(ref appList);

			Logger.Info(nameof(EmvProcessHelper) + $"[EMV_Sel_GetAppListEx] Success? {(getAppList == 1 ? "YES" : "NO")}");

			if (getAppList != 1)
				return false;

			var appListCount = Convert.ToInt32(appList.Substring(2, 2), 16);
			Logger.Info(nameof(EmvProcessHelper) + $"[EMV_Sel_GetAppListEx] Application Count: {appListCount}");

			var selectResult = 0;

			if (appListCount == 0)
			{
				Logger.Info(nameof(EmvProcessHelper) + $"[EMV_Sel_GetAppListEx] Card Has No AID/Application - Process not supported");
				return false;
			}
			else if (appListCount == 1)
			{
				Logger.Info(nameof(EmvProcessHelper) + $"[EMV_Sel_GetAppListEx] Selecting the only AID found in the chip");
				selectResult = _AxNHMwiEmv.EMV_Sel_FinalAppSelection(0);
			}
			else
			{
				selectResult = _AxNHMwiEmv.EMV_Sel_FinalAppSelection(0);
			}
            
			switch (selectResult)
			{
				case 1:
					Logger.Info(nameof(EmvProcessHelper) + "AID Selection Success");
					break;
				case -301:
					Logger.Info(nameof(EmvProcessHelper) + "AID Selection failed (count is greater than 1). Need to perform extended list selection.");
					//TODO: call EMV_Sel_GetAppListEx(), set selectResult = 1
					break;
			}

			if (selectResult != 1)
				return false;

			var rApplicationID = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._9F06);
			SelectedAID = rApplicationID;

			var rApplicationLabel = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._9F12);
			if (string.IsNullOrEmpty(rApplicationLabel))
				rApplicationLabel = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._50);

			SelectedAIDLabel = HexToAscii(rApplicationLabel);

			return true;
		}

		public bool ProcessRestrictions()
		{
			Logger.Info(nameof(EmvProcessHelper) + "[ProcessRestrictions] ");

			string date = DateTime.Now.ToString("yyMMddHHmmss");
			string nAmount = ValidateAmount(Amount);
			//string nAmountOther = "000000000000";

			int seqNo = GenerateTransactionSeqNo();

			Logger.Info(nameof(EmvProcessHelper) + $" Setting up EMV_InitAppProcess({TransactionType}, {nAmount}, {date}, {seqNo})");
			int initAppP = _AxNHMwiEmv.EMV_InitAppProcess(TransactionType, nAmount, date, seqNo);

			if (initAppP != 1)
			{
				Logger.Info(nameof(EmvProcessHelper) + $"EMV_InitAppProcess({TransactionType}, {nAmount}, {date}, {seqNo}) = Failed");
				throw new UnexpectedChipReadException("Error encountered while processing the chip. Will use msr from this point.");
			}

			if (_AxNHMwiEmv.EMV_ReadAppData() != 1)
			{
				Logger.Info(nameof(EmvProcessHelper) + "EMV_ReadAppData Failed");
				return false;
			}

			int rCurrencyCode = _AxNHMwiEmv.EMV_TlvStoreVal((int)EmvTag._5F2A, CurrencyCode);
			int rTransactionType = _AxNHMwiEmv.EMV_TlvStoreVal((int)EmvTag._9C, this.TransactionType);
			int rAuthAmount = _AxNHMwiEmv.EMV_TlvStoreVal((int)EmvTag._9F02, nAmount);
			//int rAmountOther = _AxNHMwiEmv.EMV_TlvStoreVal((int)Tags._9F03, nAmountOther);
			int rCountryCode = _AxNHMwiEmv.EMV_TlvStoreVal((int)EmvTag._9F1A, this.CountryCode);


			if (_AxNHMwiEmv.EMV_OffDataAuth() != 1)
			{
				Logger.Info(nameof(EmvProcessHelper) + "EMV_OffDataAuth Failed");
				return false;
			}

			if (_AxNHMwiEmv.EMV_ProcRestriction() != 1)
			{
				Logger.Info(nameof(EmvProcessHelper) + "EMV_ProcRestriction Failed");
				return false;
			}

			return true;
		}

		public bool ProcessCVMAndPIN(string pPinBlock)
		{
			try
			{
				if (_AxNHMwiEmv.EMV_CardholderVerify1(pPinBlock) != 1)
				{
					Logger.Info(nameof(EmvProcessHelper) + "EMV_CardholderVerify Failed");
					return false;
				}

				if (_AxNHMwiEmv.EMV_TerminalRiskMgmt() != 1)
				{
					Logger.Info(nameof(EmvProcessHelper) + "EMV_CardholderVerify Failed");
					return false;
				}

				string cardResult = string.Empty;
				if (_AxNHMwiEmv.EMV_ActionAnalysis(ref cardResult) != 1)
				{
					Logger.Info(nameof(EmvProcessHelper) + "EMV_ActionAnalysis Failed");
					return false;
				}

				Logger.Info(nameof(EmvProcessHelper) + $"EMV_ActionAnalysis result: {cardResult}");

				//TODO
				// cardResult results to Z1 for offline, '  ' for online
				// but all transactions will be proccessed online in base24
			}
			catch (Exception e)
			{
				Logger.Error(nameof(EmvProcessHelper) + $"Exeption: {e}");
				Logger.Exception(e);
			}

			return true;
		}
        
		public string Pad(String value, int length, String with)
		{
			StringBuilder result = new StringBuilder(length);
			// Pre-fill a String value
			result.Append(Fill(Math.Max(0, length - value.Length), with));
			result.Append(value);

			return result.ToString();
		}

		public string DecToHex(int dec)
		{
			char[] hexDigits = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
				'A', 'B', 'C', 'D', 'E', 'F'};
			String hex = "";
			while (dec != 0)
			{
				int rem = dec % 16;
				hex = hexDigits[rem] + hex;
				dec = dec / 16;
			}
			return hex;
		}
		public string Fill(int length, String with)
		{
			StringBuilder sb = new StringBuilder(length);
			while (sb.Length < length)
			{
				sb.Append(with);
			}
			return sb.ToString();
		}
		public string CollectEMVTags()
		{
			Logger.Info(nameof(EmvProcessHelper) + "[CollectEMVTags] ");

			var str = new StringBuilder();

			try
			{
				string rTrack2Data = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._57);
				string track2Length = (rTrack2Data.Length / 2) > 10 ? Pad(DecToHex((rTrack2Data.Length / 2)), 2, "0") : Pad((rTrack2Data.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._57.ToString().Substring(1)}{track2Length}{rTrack2Data}");

				string rPAN = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._5A);
				string panLength = (rPAN.Length / 2) > 10 ? Pad(DecToHex((rPAN.Length / 2)), 2, "0") : Pad((rPAN.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._5A.ToString().Substring(1)}{panLength}{rPAN}");

				string rPanSeqNo = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._5F34);
				string panSeqLength = (rPanSeqNo.Length / 2) > 10 ? Pad(DecToHex((rPanSeqNo.Length / 2)), 2, "0") : Pad((rPanSeqNo.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._5F34.ToString().Substring(1)}{(panSeqLength)}{rPanSeqNo}");

				string rAppExpDate = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._5F24);
				string appExpDateLength = (rAppExpDate.Length / 2) > 10 ? Pad(DecToHex((rAppExpDate.Length / 2)), 2, "0") : Pad((rAppExpDate.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._5F24.ToString().Substring(1)}{appExpDateLength}{rAppExpDate}");

				string rApplicationID = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._9F06);
				string applicationIDLength = (rApplicationID.Length / 2) > 10 ? Pad(DecToHex((rApplicationID.Length / 2)), 2, "0") : Pad((rApplicationID.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._9F06.ToString().Substring(1)}{applicationIDLength}{rApplicationID}");

				this.SelectedAID = rApplicationID;

				string rApplicationLabel = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._9F12);
				if (string.IsNullOrEmpty(rApplicationLabel))
					rApplicationLabel = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._50);
				string applicationLabelLength = (rApplicationLabel.Length / 2) > 10 ? Pad(DecToHex((rApplicationLabel.Length / 2)), 2, "0") : Pad((rApplicationLabel.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._50.ToString().Substring(1)}{applicationLabelLength}{rApplicationLabel}");

				this.SelectedAIDLabel = HexToAscii(rApplicationLabel);

				string rTransactionDate = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._9A);
				string transactionDateLength = (rTransactionDate.Length / 2) > 10 ? Pad(DecToHex((rTransactionDate.Length / 2)), 2, "0") : Pad((rTransactionDate.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._9A.ToString().Substring(1)}{transactionDateLength}{rTransactionDate}");

				string rTransactionType = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._9C);
				string transactionTTypeLength = (rTransactionType.Length / 2) > 10 ? Pad(DecToHex((rTransactionType.Length / 2)), 2, "0") : Pad((rTransactionType.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._9C.ToString().Substring(1)}{transactionTTypeLength}{rTransactionType}");

				string rTransactionCurrencyCode = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._5F2A);
				string transactionCurrencyLength = (rTransactionCurrencyCode.Length / 2) > 10 ? Pad(DecToHex((rTransactionCurrencyCode.Length / 2)), 2, "0") : Pad((rTransactionCurrencyCode.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._5F2A.ToString().Substring(1)}{transactionCurrencyLength}{rTransactionCurrencyCode}");

				string rAppTransactionCounter = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._9F36);
				string transactionCounterLength = (rAppTransactionCounter.Length / 2) > 10 ? Pad(DecToHex((rAppTransactionCounter.Length / 2)), 2, "0") : Pad((rAppTransactionCounter.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._9F36.ToString().Substring(1)}{transactionCounterLength}{rAppTransactionCounter}");

				string rIssuerAppData = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._9F10);
				string issuerAppLength = (rIssuerAppData.Length / 2) > 10 ? Pad(DecToHex((rIssuerAppData.Length / 2)), 2, "0") : Pad((rIssuerAppData.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._9F10.ToString().Substring(1)}{issuerAppLength}{rIssuerAppData}");

				string rAIP = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._82);
				string aipLength = (rAIP.Length / 2) > 10 ? Pad(DecToHex((rAIP.Length / 2)), 2, "0") : Pad((rAIP.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._82.ToString().Substring(1)}{aipLength}{rAIP}");

				string rTerminalCountryCode = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._9F1A);
				string terminalCountryCodeLength = (rTerminalCountryCode.Length / 2) > 10 ? Pad(DecToHex((rTerminalCountryCode.Length / 2)), 2, "0") : Pad((rTerminalCountryCode.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._9F1A.ToString().Substring(1)}{terminalCountryCodeLength}{rTerminalCountryCode}");

				string rTVR = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._95);
				string tvrLength = (rTVR.Length / 2) > 10 ? Pad(DecToHex((rTVR.Length / 2)), 2, "0") : Pad((rTVR.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._95.ToString().Substring(1)}{tvrLength}{rTVR}");

				string rUnpredictableNumber = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._9F37);
				string unpredictableNumberLength = (rUnpredictableNumber.Length / 2) > 10 ? Pad(DecToHex((rUnpredictableNumber.Length / 2)), 2, "0") : Pad((rUnpredictableNumber.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._9F37.ToString().Substring(1)}{unpredictableNumberLength }{rUnpredictableNumber}");

				string rCryptogram = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._9F26);
				string cryptogramLength = (rCryptogram.Length / 2) > 10 ? Pad(DecToHex((rCryptogram.Length / 2)), 2, "0") : Pad((rCryptogram.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._9F26.ToString().Substring(1)}{cryptogramLength}{rCryptogram}");

				string rCryptogramInfoData = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._9F27);
				string cryptogramInfoLength = (rCryptogramInfoData.Length / 2) > 10 ? Pad(DecToHex((rCryptogramInfoData.Length / 2)), 2, "0") : Pad((rCryptogramInfoData.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._9F27.ToString().Substring(1)}{cryptogramInfoLength}{rCryptogramInfoData}");

				string rAuthAmount = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._9F02);
				string authAmountLength = (rAuthAmount.Length / 2) > 10 ? Pad(DecToHex((rAuthAmount.Length / 2)), 2, "0") : Pad((rAuthAmount.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._9F02.ToString().Substring(1)}{authAmountLength}{rAuthAmount}");

				//string rAmountOther = _AxNHMwiEmv.EMV_GetVal((int)Tags._9F03);
				//emvSb.Append($"{Tags._9F03.ToString().Substring(1)}={rAmountOther},");

				string rTransactionSequenceNumber = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._9F41);
				string transactionSequenceNumberLength = (rTransactionSequenceNumber.Length / 2) > 10 ? Pad(DecToHex((rTransactionSequenceNumber.Length / 2)), 2, "0") : Pad((rTransactionSequenceNumber.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._9F41.ToString().Substring(1)}{transactionSequenceNumberLength}{rTransactionSequenceNumber}");

				string rIFDSerialNumber = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._9F1E);
				string ifdSerialNumberLength = (rIFDSerialNumber.Length / 2) > 10 ? Pad(DecToHex((rIFDSerialNumber.Length / 2)), 2, "0") : Pad((rIFDSerialNumber.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._9F1E.ToString().Substring(1)}{ifdSerialNumberLength}{rIFDSerialNumber}");

				string rTerminalCapabilities = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._9F33);
				string terminalCapabilitiesLength = (rTerminalCapabilities.Length / 2) > 10 ? Pad(DecToHex((rTerminalCapabilities.Length / 2)), 2, "0") : Pad((rTerminalCapabilities.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._9F33.ToString().Substring(1)}{terminalCapabilitiesLength}{rTerminalCapabilities}");

				string rTerminalType = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._9F35);
				string terminalTypeLength = (rTerminalType.Length / 2) > 10 ? Pad(DecToHex((rTerminalType.Length / 2)), 2, "0") : Pad((rTerminalType.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._9F35.ToString().Substring(1)}{terminalTypeLength}{rTerminalType}");

				string rCVMList = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._8E);
				string cvmListLength = (rCVMList.Length / 2) > 10 ? Pad(DecToHex((rCVMList.Length / 2)), 2, "0") : Pad((rCVMList.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._8E.ToString().Substring(1)}{cvmListLength}{rCVMList}");

				string rCVMResults = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._9F34);
				string cvmResultsLength = (rCVMResults.Length / 2) > 10 ? Pad(DecToHex((rCVMResults.Length / 2)), 2, "0") : Pad((rCVMResults.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._9F34.ToString().Substring(1)}{cvmResultsLength}{rCVMResults}");

				string rPOSEntryMode = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._9F39);
				string posEntryModeLength = (rPOSEntryMode.Length / 2) > 10 ? Pad(DecToHex((rPOSEntryMode.Length / 2)), 2, "0") : Pad((rPOSEntryMode.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._9F39.ToString().Substring(1)}{posEntryModeLength}{rPOSEntryMode}");

				string rTSI = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._9B);
				string tsiLength = (rTSI.Length / 2) > 10 ? Pad(DecToHex((rTSI.Length / 2)), 2, "0") : Pad((rTSI.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._9B.ToString().Substring(1)}{tsiLength}{rTSI}");

				string rTransactionCategoryCode = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._9F53);
				string transactionCategoryCodeLength = (rTransactionCategoryCode.Length / 2) > 10 ? Pad(DecToHex((rTransactionCategoryCode.Length / 2)), 2, "0") : Pad((rTransactionCategoryCode.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._9F53.ToString().Substring(1)}{transactionCategoryCodeLength}{rTransactionCategoryCode}");

				string rDedicatedFileName = _AxNHMwiEmv.EMV_GetVal((int)EmvTag._84);
				string dedicatedFIleNameLength = (rDedicatedFileName.Length / 2) > 10 ? Pad(DecToHex((rDedicatedFileName.Length / 2)), 2, "0") : Pad((rDedicatedFileName.Length / 2).ToString(), 2, "0");
				str.Append($"{EmvTag._84.ToString().Substring(1)}{dedicatedFIleNameLength}{rDedicatedFileName}");

                /*
				Logger.Info(nameof(EmvProcessHelper) + $" - Track2Data length: {rTrack2Data?.Length}");
				Logger.Info(nameof(EmvProcessHelper) + $" - PAN length: {rPAN?.Length}");
				Logger.Info(nameof(EmvProcessHelper) + $" - PanSeqNo : {rPanSeqNo}");
				Logger.Info(nameof(EmvProcessHelper) + $" - AppExpDate : {rAppExpDate}");
				Logger.Info(nameof(EmvProcessHelper) + $" - ApplicationID : {rApplicationID}");
				Logger.Info(nameof(EmvProcessHelper) + $" - ApplicationLabel : {rApplicationLabel}");
				Logger.Info(nameof(EmvProcessHelper) + $" - TransactionDate : {rTransactionDate}");
				Logger.Info(nameof(EmvProcessHelper) + $" - TransactionType : {rTransactionType}");
				Logger.Info(nameof(EmvProcessHelper) + $" - TransactionCurrencyCode : {rTransactionCurrencyCode}");
				Logger.Info(nameof(EmvProcessHelper) + $" - AppTransactionCounter : {rAppTransactionCounter}");
				Logger.Info(nameof(EmvProcessHelper) + $" - IssuerAppData : {rIssuerAppData}");
				Logger.Info(nameof(EmvProcessHelper) + $" - AIP : {rAIP}");
				Logger.Info(nameof(EmvProcessHelper) + $" - TerminalCountryCode : {rTerminalCountryCode}");
				Logger.Info(nameof(EmvProcessHelper) + $" - TVR : {rTVR}");
				Logger.Info(nameof(EmvProcessHelper) + $" - UnpredictableNumber : {rUnpredictableNumber}");
				Logger.Info(nameof(EmvProcessHelper) + $" - Cryptogram : {rCryptogram}");
				Logger.Info(nameof(EmvProcessHelper) + $" - CryptogramInfoData : {rCryptogramInfoData}");
				Logger.Info(nameof(EmvProcessHelper) + $" - AuthAmount : {rAuthAmount}");
				//Logger.Info(nameof(EmvProcessHelper) + $" - AmountOther : {rAmountOther}");
				Logger.Info(nameof(EmvProcessHelper) + $" - TransactionSequenceNumber : {rTransactionSequenceNumber}");
				Logger.Info(nameof(EmvProcessHelper) + $" - IFDSerialNumber : {rIFDSerialNumber}");
				Logger.Info(nameof(EmvProcessHelper) + $" - TerminalCapabilities : {rTerminalCapabilities}");
				Logger.Info(nameof(EmvProcessHelper) + $" - TerminalType : {rTerminalType}");
				Logger.Info(nameof(EmvProcessHelper) + $" - CVMList : {rCVMList}");
				Logger.Info(nameof(EmvProcessHelper) + $" - CVMResults : {rCVMResults}");
				Logger.Info(nameof(EmvProcessHelper) + $" - POSEntryMode : {rPOSEntryMode}");
				Logger.Info(nameof(EmvProcessHelper) + $" - TSI : {rTSI}");
				Logger.Info(nameof(EmvProcessHelper) + $" - TransactionCategoryCode : {rTransactionCategoryCode}");
				Logger.Info(nameof(EmvProcessHelper) + $" - DedicatedFileName : {rDedicatedFileName}");
                */

				string displayTags = str.ToString();
                
				if (rCryptogramInfoData == "00")    // this means cryptogram fails and emv process need not to continue with online authorization
				{
					throw new CryptogramFailureException("Generating Cryptogram Failed. Tag 9F27 returns 00.");
				}
			}
			catch (CryptogramFailureException)
			{
				throw;
			}
			catch (Exception e)
			{
				Logger.Error(nameof(EmvProcessHelper) + $"Exeption: {e}");
				Logger.Exception(e);
			}

			return str.ToString();
		}

		public bool StartOnlineApproval(string respCode, int aRPCLen, string aRPCData, int isuScriptLen, string isuScriptData, string acquirerCID)
		{
			Logger.Info(nameof(EmvProcessHelper) + "[StartOnlineApproval] ");

			try
			{
				if (!string.IsNullOrEmpty(respCode))
				{

					if (respCode == "00" && string.IsNullOrEmpty(aRPCData))
					{
						// If no ARPC data received but tag 81 is success then provide 0's with len of 20
						Logger.Info(nameof(EmvProcessHelper) + $"[StartOnlineApproval] modifying ARPC Data to have custom values");
						aRPCData = new string('0', 20);
					}

					byte[] ba = Encoding.Default.GetBytes(respCode);
					var hexString = BitConverter.ToString(ba);
					respCode = hexString.Replace("-", "");

					if (aRPCData != string.Empty)
					{
						//M-TIP50.Test.01.Scenario.01 
						// kernel should be able to handle more than 8 bytes long ARPC data
						// but length should not be more that 16 bytes long
						if (aRPCData.Length < 32)
						{
							if (aRPCData.Substring(aRPCData.Length - 4) != "3030")
								aRPCData = aRPCData + respCode;
						}
						else if (aRPCData.Length > 32)
						{
							if (aRPCData.Substring(aRPCData.Length - 4) == "3030")
							{
								aRPCData = aRPCData.Substring(aRPCData.Length - 4);
							}
						}

						aRPCLen = aRPCData.Length;
					}
				}

				int rOnlineApproval = _AxNHMwiEmv.EMV_OnlineApproval3(respCode, aRPCLen, aRPCData, isuScriptLen, isuScriptData, acquirerCID);

				if (rOnlineApproval == 1)
				{
					Logger.Info(nameof(EmvProcessHelper) + $"EMV_OnlineApproval3.....returns {rOnlineApproval} : 1=Approved, other value=cancelled");

					int rCompleteProcess = _AxNHMwiEmv.EMV_CompleteProcess();

					Logger.Info(nameof(EmvProcessHelper) + $"EMV_CompleteProcess.....returns {rCompleteProcess} : 7=Approved, other value =Declined");

					if (rCompleteProcess == 7)
					{
						Logger.Info("Online Approval Approved");
						return true;
					}
					else
					{
						Logger.Info("Online Approval Declined");
					}
				}
				else
				{
					Logger.Info("Online Approval Cancelled");
				}
			}
			catch (Exception e)
			{
				Logger.Error(nameof(EmvProcessHelper) + $"Exeption: {e}");
				Logger.Exception(e);
			}

			return false;
		}

		public string GetTrack2Data()
		{
			return _AxNHMwiEmv.EMV_GetVal((int)EmvTag._57);
		}

		public string GetPanData()
		{
			return _AxNHMwiEmv.EMV_GetVal((int)EmvTag._5A);
		}

		public void HandleUnsuccessfulOnlineApproval()
		{
			int rOnlineApproval = _AxNHMwiEmv.EMV_DefaultApproval();
			if (rOnlineApproval == 1)
				Logger.Info("Default Online Approval");
		}

		private int GenerateTransactionSeqNo()
		{
			Logger.Info(nameof(EmvProcessHelper) + "[GenerateTransactionSeqNo] Retriveing LastSequenceNumber from system registry");

			var lastValue = SystemRegistry.Instance().FromEMV("LastSequenceNumber");
			var intVal = int.Parse(lastValue);
			intVal++;

			try
			{
				SystemRegistry.Instance().ToEMV("LastSequenceNumber", intVal.ToString());
			}
			catch (Exception ex)
			{
				Logger.Exception(ex);
			}
            
			return intVal;
		}

		private string ValidateAmount(decimal amount)
		{
			var intAmount = Convert.ToInt32(decimal.Multiply(amount, 100M));
			var desiredLength = (12 - intAmount.ToString().Length);
			var sb = new StringBuilder();

			for (int i = 0; i < desiredLength; i++)
			{
				sb.Append("0");
			}

			sb.Append(intAmount.ToString());
			Logger.Info(nameof(EmvProcessHelper) + $"Validated amount: {sb.ToString()}");
			return sb.ToString();
		}

		private string HexToAscii(string hexString)
		{
			if (string.IsNullOrEmpty(hexString))
				return string.Empty;

			var sb = new StringBuilder();
			try
			{
				for (var i = 0; i < hexString.Length; i += 2)
				{
					var hs = hexString.Substring(i, 2);
					sb.Append(Convert.ToString(Convert.ToChar(int.Parse(hs, NumberStyles.HexNumber))));
				}
			}
			catch (Exception ex)
			{
				Logger.Error($"Error converting Hex:{Environment.NewLine}{ex.StackTrace}");
			}

			return sb.ToString();
		}
	}
}