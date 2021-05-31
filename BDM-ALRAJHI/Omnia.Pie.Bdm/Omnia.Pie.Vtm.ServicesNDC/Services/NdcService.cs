namespace Omnia.Pie.Vtm.ServicesNdc
{
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
    using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
    using Omnia.Pie.Vtm.Framework.Exceptions;
	using Omnia.Pie.Vtm.Framework.Extensions;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.ServicesNdc.Interface;
	using Omnia.Pie.Vtm.ServicesNdc.Interface.Entities;
    using Omnia.Pie.Vtm.ServicesNdc.Interface.Exceptions;
    using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Threading.Tasks;

	public class NdcService : ServiceBase, INdcService
	{
		public NdcService(IResolver container) : base(container)
		{

		}

		#region Pin Validation

		public async Task<bool> ValidatePinAsync(string track2, string pin)
		{
			if (string.IsNullOrEmpty(track2))
				throw new ArgumentNullException(nameof(track2));
			if (string.IsNullOrEmpty(pin))
				throw new ArgumentNullException(nameof(pin));

			return await ExecuteFaultHandledOperationAsync<string, bool>(async c =>
			{
				var response = await ExecuteServiceAsync<string, byte[]>(GetPinValidationRequest(track2, pin));
				return ToPinValidationResponse(response);
			});
		}

		private bool ToPinValidationResponse(string message)
		{
			var data = message.Split(FieldSeparatorChar, '\n');

            try
            {
                _logger?.Info($"Response Code: {data[3]}");
            }
            catch (IndexOutOfRangeException)
            {
                throw new HostNoResponseException("PIN Validation", "Expected response from the host not found.");
            }

			if (data[3] == "026")
			{
                
				throw new InvalidPinException();
			}
			else if (data[3] != "025")
			{
				if (message.ToLower().Contains("your card has been retained"))
				{
					throw new CardCaptureException();
				}
				if (message.ToLower().Contains("maximum pin retry exceeded"))
				{
					throw new ExceededPinException();
				}
				throw new InvalidPinException();
			}
			return true;
		}

		private string GetPinValidationRequest(string track2, string pin)
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("1", "Message Class");
			stringBuilder.Append("1", "Message Sub-Class");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000", "LUNO");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("1", "data");
			stringBuilder.Append("9", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(";", "data");
			stringBuilder.Append(track2, "data");
			stringBuilder.Append("?", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("IAAAA   ", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000000000000", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(pin, "pinblock");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("26087100000000000000000000", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("CAM", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("9F2701409F260885C0D0F3395098D69F100706010A0360A800950580800480009B027800", "emv data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("U", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("V", "data");

			return stringBuilder.ToStringExtend();
		}

		#endregion

		#region CashWithdrawal

		public async Task<CashWithdrawal> CashWithdrawalDebitCardAsync(string track2, string pin, string iccRequest, string amount, string transactionCode, bool hasMultipleAccounts = false, string accountNo = "")
		{
			if (string.IsNullOrEmpty(track2))
				throw new ArgumentNullException(nameof(track2));
			if (string.IsNullOrEmpty(pin))
				throw new ArgumentNullException(nameof(pin));
			if (string.IsNullOrEmpty(amount))
				throw new ArgumentNullException(nameof(amount));

			return await ExecuteFaultHandledOperationAsync<string, CashWithdrawal>(async c =>
			{
				var response = await ExecuteServiceAsync<string, byte[]>(GetCashwithdrawalRequest(track2, pin, iccRequest, amount, transactionCode));
				if (hasMultipleAccounts)
				{
					var accounts = GetAccountsList(response);
					var accountFdk = GetAccountFdk(accountNo, accounts);
					var accountSelectedResponse = await ExecuteServiceAsync<string, byte[]>(GetAccountSelectedRequest(accountFdk), true);

					return await ToCashWithdrawalAsync(accountSelectedResponse);
				}
				else
				{
					return await ToCashWithdrawalAsync(response);
				}
			});
		}

		private string GetAccountFdk(string accountNo, List<Account> lookUp)
		{
			string[] FDKMap = { "I", "H", "G", "F", "A", "B", "C", "D" };
			int ind = 0;
			foreach (var item in lookUp)
			{
				if (item.Number == accountNo)
				{
					return FDKMap[ind];
				}
				ind++;
			}
			return "";
		}

		public List<Account> GetAccountsList(string message)
		{
			
			List<Account> accounts = new List<Account>();
			if (!string.IsNullOrEmpty(message))
			{
				var array = message.Split(FieldSeparatorChar, EscapeCharacterChar, ShiftOutCharacterChar, FieldCharacterChar, '\n');
				foreach (var item in array)
				{
					if (!string.IsNullOrEmpty(item) && item.Length == 12 && !IsNumeric(item[0].ToString()) && !IsNumeric(item[1].ToString()))
					{
						accounts.Add(new Account() { Number = item.Substring(2) });
					}
				}
			}
			return accounts;
		}

		private bool IsNumeric(string s)
		{
			string[] chars = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
			foreach (var item in chars)
			{
				if (s.ToLower() == item)
					return false;
			}
			return true;
		}

		public async Task<string> CashWithdrawalReversalAsync()
		{
			return await ExecuteFaultHandledOperationAsync<string, string>(async c =>
			{
				var response = await ExecuteServiceAsync<string, byte[]>(GetCashwithdrawalReversalRequest());
				return ToReversal(response);
			});
		}

		private string ToReversal(string message)
		{
		
			if (!string.IsNullOrEmpty(message))
			{
				var array = message.Split(new char[] { FieldSeparatorChar, '\n' }, StringSplitOptions.RemoveEmptyEntries);

				int resInd = 0;
				foreach (var item in array)
				{
					if (item.ToLower().Contains("response code"))
						break;
					resInd++;
				}

				var resCode = array[resInd].Split(':')[1].Split('1')[1];
				//var balance = array[balInd].Split(':')[1].Split('1')[1];
				if (resCode == "480")
				{
					_logger?.Info("Reversal Successful.");
					// Success
					//result.AvailableBalance = double.Parse(balance);
				}
				else
				{
					// Failure
					throw new Exception("NDC Service failed.");
				}
			}
            else
            {
                // Failure
                throw new Exception("NDC Service failed. No reponse from the server or not connected to server");
            }

            //←(1RESPONSE CODE  :♫1480
            return message;
		}

		private async Task<CashWithdrawal> ToCashWithdrawalAsync(string message)
		{

			var result = new CashWithdrawal();

			if (!string.IsNullOrEmpty(message))
			{
				var array = message.Split(FieldSeparatorChar, EscapeCharacterChar, FieldCharacterChar, '\n');

				int resInd = 0;
				foreach (var item in array)
				{
					try
					{
						if (item.ToLower().Contains("response code"))
							break;
					}
					catch { }

					resInd++;
				}

				int authInd = 0;
				foreach (var item in array)
				{
					try
					{
						if (item.ToLower().Contains("trn. nbr"))
							break;
					}
					catch { }

					authInd++;
				}

				int balInd = 0;
				foreach (var item in array)
				{
					try
					{
						if (item.ToLower().Contains("available bal"))
							break;
					}
					catch { }

					balInd++;
				}

				int emvInd = 0;
				foreach (var item in array)
				{
					try
					{
						if (item.ToLower().Contains("cam"))
							break;
					}
					catch { }

					emvInd++;
				}

				var resCode = array[resInd]?.Split(ShiftOutCharacterChar)[1]?.Substring(1);

                try
                {
                    _logger?.Info($"Response Code: {resCode}");
                }
                catch (Exception)
                {
                }

                var iccData = "";
				var authCode = "";
				var balance = "0";

				try
				{
					balance = array[balInd].Split(':')[1].Substring(5);
				}
				catch (Exception)
				{
					balance = "0";
				}

				try
				{
					iccData = emvInd > 0 && emvInd < array.Length ? array[emvInd] : "";
					authCode = array[authInd].Split(':')[1].Substring(2); // Split('1')[1] // :1 826091

                }
				catch (Exception ex)
				{
					if (resCode == "000")
					{
						await CashWithdrawalReversalAsync();
						throw ex;
					}
				}

				if (resCode.Trim() == "000")
				{
					if (iccData.Length > 0)
					{
						if (iccData.ToLower().Contains("cam"))
						{
							iccData = iccData.Substring(iccData.ToLower().IndexOf("cam") + 3);
						}
					}

                    await CashWithrawalAdviceConfirmationAsync();

                    // Success
                    result.AuthCode = authCode;
					result.IccData = iccData;
					result.AvailableBalance = double.Parse(balance);

				}
				else if (resCode.Trim() == "010")
				{
					throw new InsufficientAccountBalanceException();
				}
				else if (resCode.Trim() == "116")
				{
					throw new InsufficientAccountBalanceException();
				}
				else if (resCode.Trim() == "121")
				{
					throw new ExceededWithdrawalLimitException();
				}
				else if (resCode.Trim() == "123")
				{
					throw new ExceededWithdrawalFrequencyException();
				}
				else if (resCode.Trim() == "124")
				{
					throw new ExceededPerTransactionLimitException();
				}
				else if (resCode.Trim() == "125")
				{
					throw new CardNotInServiceException();
				}
				else if (resCode.Trim() == "150")
				{
					throw new ExceededWithdrawalLimitException();
				}
                else if (resCode.Trim() == "128")
                {
                    if (iccData.Length > 0)
                    {
                        if (iccData.ToLower().Contains("cam"))
                        {
                            iccData = iccData.Substring(iccData.ToLower().IndexOf("cam") + 3);
                            result.IccData = iccData;
                        }
                    }


                    throw new CryptographicErrorException("Cryptographic Error");
                }
				else
				{
                    // Failure
                    throw new HostNoResponseException("Cash Withdrawal", "Expected response from the host not found.");
                }
			}
            else
            {
                // Failure
                throw new HostNoResponseException("Cash Withdrawal", "Expected response from the host not found.");
            }

            return result;
		}

		private string GetAccountSelectedRequest(string fdk)
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("1", "Message Class");
			stringBuilder.Append("1", "Message Sub-Class");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000", "LUNO");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("1", "Top of Receipt Transaction Flag");
			stringBuilder.Append("9", "Message Coordination Number");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(fdk, "FDK");
			stringBuilder.Append(FieldSeparator, "Field Separator");

			return stringBuilder.ToStringExtend();
		}

		private string GetCashwithdrawalRequest(string track2, string pin, string iccRequest, string amount, string transactionCode)
		{
			if (string.IsNullOrEmpty(iccRequest)) // iccRequest = emv
				iccRequest = ""; //"9F2701809F0607A000000003101057134714846938592602D22042215510641800000F5A0847148469385926025F3401009F26085BC0126CD6F6755D9F100706010A03A0200082023C009F3602007F8C159F02069F03069F1A0295055F2A029A039C019F37049F3303604020500A566973612044656269749F120A566973612044656269748407A00000000310109F02060000000100009F1A020784950580800480005F2A0207849A031708169C01019F3704472A3BF2";

			if (amount.Length < 12)
			{
				string dataToAppend = "";
				for (int i = 1; i <= 12 - amount.Length; i++)
				{
					dataToAppend += "0";
				}
				amount = dataToAppend + amount;
			}

			var stringBuilder = new StringBuilder();
			stringBuilder.Append("1", "Message Class");
			stringBuilder.Append("1", "Message Sub-Class");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000", "LUNO");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			//stringBuilder.Append(TimeVarientNumber, "Time Varient Number");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("1", "Top of Receipt Transaction Flag");
			stringBuilder.Append("9", "Message Coordination Number");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(";" + track2 + "?", "Field Separator"); /// need to see if ; and ? coming from card ???
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(transactionCode, "Operation Code Data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(amount, "Operation Code Data"); // needs to be 12 digit and last 2 are for fills
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(pin, "PIN Buffer"); // 16 Digits C66B825EF54B2C6E => 1234 should be like :=08:77:0<<01690
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			//stringBuilder.Append("26088100000000000000000000", "Operation Code Data");
			stringBuilder.Append("21639100000000000000000000", "Operation Code Data");

			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("U", "CSP Data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("V", "CSP Data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("5", "Smart Card Data Id");
			stringBuilder.Append("CAM0004", "CSP Data");
			stringBuilder.Append(iccRequest, "Emv Data");

			return stringBuilder.ToStringExtend();
		}

		private string GetCashwithdrawalReversalRequest()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("2", "Message Class");
			stringBuilder.Append("2", "Message Sub-Class");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000", "LUNO");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("A", "Reversal Reason");
			//stringBuilder.Append(FieldSeparator, "Field Separator");
			//stringBuilder.Append("D1", "Data");
			//stringBuilder.Append(FieldSeparator, "Field Separator");
			//stringBuilder.Append("0", "Data");
			//stringBuilder.Append(FieldSeparator, "Field Separator");
			//stringBuilder.Append("00", "Data");
			//stringBuilder.Append(FieldSeparator, "Field Separator");
			//stringBuilder.Append("CAM", "Data");
			//stringBuilder.Append(FieldSeparator, "Field Separator");
			//stringBuilder.Append("9F2701409F26087301D6747D6340E79F100706010A03602000950580800480009B027800", "Data");
			//stringBuilder.Append(FieldSeparator, "Field Separator");
			//stringBuilder.Append("1", "Data");

			return stringBuilder.ToStringExtend();
		}

        public async Task CashWithrawalAdviceConfirmationAsync()
        {
            // From Keep Alive Msg
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("2", "Message Class");
            stringBuilder.Append("2", "Message Sub-Class");
            stringBuilder.Append("\u001c", "Field Separator");
            stringBuilder.Append("000", "LUNO");
            stringBuilder.Append("\u001c", "Field Separator");
            stringBuilder.Append("\u001c", "Field Separator");
            stringBuilder.Append("B", "data");
            
            await ExecuteServiceAsync<string, byte[]>(stringBuilder.ToStringExtend(), false);
        }

        #endregion

        #region Cash Deposit Credit Card

        public async Task<CashDeposit> CashDepositCCAsync(string track2, List<DepositDenomination> amount, string totalAmount = "")
		{
			if (string.IsNullOrEmpty(track2))
				throw new ArgumentNullException(nameof(track2));
			if (amount == null)
				throw new ArgumentNullException(nameof(amount));

			var amountConverted = ProcessAmount(amount);

			return await ExecuteFaultHandledOperationAsync<string, CashDeposit>(async c =>
			{
				var responseCCInquiry = await ExecuteServiceAsync<string, byte[]>(GenerateCashDepositCrediCardInquiryMessage(track2), true);
				if (CheckCCInquiryResponse(responseCCInquiry))
				{
					await GetReadyAsync();
					await SendExtraCCDepositMessageAsync();

					var response = await ExecuteServiceAsync<string, byte[]>(GetCashDepositCreditCardRequest(track2, amountConverted), true);
					if (ToCashDepositCCConfirmation(response, totalAmount))
					{
						var confirmationResponse = await ExecuteServiceAsync<string, byte[]>(GetCashDepositCreditCardConfirmationRequest(), true);
						return ToCashDepositCC(confirmationResponse);
					}
				}

				throw new Exception("NDC Service failed.");

			});
		}

		private async Task<bool> SendExtraCCDepositMessageAsync()
		{
			return await ExecuteFaultHandledOperationAsync<string, bool>(async c =>
			{
				var response = await ExecuteServiceAsync<string, byte[]>(GenerateCashDepositReadyMessage(), false);
				return true;
			});
		}

		private static string GenerateCashDepositReadyMessage()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("1", "Message Class");
			stringBuilder.Append("2", "Message Sub-Class");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000", "LUNO");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("w", "data");
			stringBuilder.Append("0 !     ", "data");
			stringBuilder.Append("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~", "data");
			stringBuilder.Append("       ", "data");
			stringBuilder.Append("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~", "data");
			stringBuilder.Append("       ", "data");
			stringBuilder.Append("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~", "data");
			stringBuilder.Append(" ! ", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("0", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("1401000003000008", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("0", "data");

			return stringBuilder.ToStringExtend();
		}

		private bool CheckCCInquiryResponse(string message)
		{
			if (!string.IsNullOrEmpty(message))
			{
				var array = message.Split(FieldSeparatorChar, EscapeCharacterChar, FieldCharacterChar, '\n');
				if (array[3] == "628" || array[3] == "629")
				{
					return true;
				}
			}
			return false;
		}

		private static string GenerateCashDepositCrediCardInquiryMessage(string cardNumber)
		{
			//11.000...12.;5174000009560003=20052015552326600000?..CC AB  C.000000000000.....2080110000000000000000000000000000000000000000000000.U.V.w0501
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("1", "Message Class");
			stringBuilder.Append("1", "Message Sub-Class");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000", "LUNO");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("1", "data");
			stringBuilder.Append("2", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("CC  D  D", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000000000000", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(cardNumber, "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			//stringBuilder.Append("?", "data
			stringBuilder.Append("2483610000000000000000000000000000000000000000000000", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("U", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("V", "data");

			return stringBuilder.ToStringExtend();

		}

		private bool ToCashDepositCCConfirmation(string message, string amount)
		{
            try
            {
                if (!string.IsNullOrEmpty(message))
                {
                    var array = message.Split(FieldSeparatorChar, EscapeCharacterChar, ShiftOutCharacterChar, FieldCharacterChar, '\n');
                    if (array.Length > 5 && array[4].ToString() == "040")
                    {
                        if (amount == array[13].Split(' ')[1].Trim())
                            return true;
                        return false;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new HostNoResponseException("Cash Deposit Confirmation", "Expected response from the host not found.");
            }

			
		}

		private string ProcessAmount(List<DepositDenomination> amount)
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>
			{
				{ 10, "01" },
				{ 20, "02" },
				{ 50, "03" },
				{ 100, "04" },
				{ 200, "05" },
				{ 500, "06" },
				{ 1000, "07" }
			};
			string amountConverted = "";
			foreach (var item in amount)
			{
				if (item.Denomination == 10 && item.Quantity > 0)
				{
					amountConverted += "01" + (item.Quantity < 10 ? "0" + item.Quantity : item.Quantity.ToString());
				}
				if (item.Denomination == 20 && item.Quantity > 0)
				{
					amountConverted += "02" + (item.Quantity < 10 ? "0" + item.Quantity : item.Quantity.ToString());
				}
				if (item.Denomination == 50 && item.Quantity > 0)
				{
					amountConverted += "03" + (item.Quantity < 10 ? "0" + item.Quantity : item.Quantity.ToString());
				}
				if (item.Denomination == 100 && item.Quantity > 0)
				{
					amountConverted += "04" + (item.Quantity < 10 ? "0" + item.Quantity : item.Quantity.ToString());
				}
				if (item.Denomination == 200 && item.Quantity > 0)
				{
					amountConverted += "05" + (item.Quantity < 10 ? "0" + item.Quantity : item.Quantity.ToString());
				}
				if (item.Denomination == 500 && item.Quantity > 0)
				{
					amountConverted += "06" + (item.Quantity < 10 ? "0" + item.Quantity : item.Quantity.ToString());
				}
				if (item.Denomination == 1000 && item.Quantity > 0)
				{
					amountConverted += "07" + (item.Quantity < 10 ? "0" + item.Quantity : item.Quantity.ToString());
				}
			}

			return amountConverted;
		}

		private CashDeposit ToCashDepositCC(string message)
		{
			var result = new CashDeposit();

			if (!string.IsNullOrEmpty(message))
			{
				var array = message.Split(FieldSeparatorChar, EscapeCharacterChar, FieldCharacterChar, '\n');

				int resInd = 0;
				foreach (var item in array)
				{
					if (item.ToLower().Contains("response code"))
						break;
					resInd++;
				}

				var resCode = array[resInd].Split(':')[1].Split('1')[1];
				if (resCode == "000") // Success
				{
					int authInd = 1;
					foreach (var item in array)
					{
						try
						{
							if (item.ToLower().Contains("atm"))
								break;
						}
						catch { }

						authInd++;
					}
					var authCode = string.Empty;

					try
					{
						if (authInd != 1)
							result.AuthCode = array[authInd].Split(ShiftOutCharacterChar)[2].Substring(1);
					}
					catch (Exception ex)
					{
						_logger.Exception(ex);
					}
				}
			}
            else
            {
                // Failure
                throw new HostNoResponseException("Cash Deposit", "Expected response from the host not found.");
            }

            return result;
		}

		private string GetCashDepositCreditCardConfirmationRequest()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("1", "Message Class");
			stringBuilder.Append("1", "Message Sub-Class");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000", "LUNO");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("13", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("C", "FDK");
			stringBuilder.Append(FieldSeparator, "Field Separator");


			return stringBuilder.ToStringExtend();
		}

		private string GetCashDepositCreditCardRequest(string track2, string amount)
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("1", "Message Class");
			stringBuilder.Append("1", "Message Sub-Class");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000", "LUNO");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("15", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("CC AC  C", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000000000000", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(track2, "Card number");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("2080110000000000000000000000000000000000000000000000", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("U", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("V", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("w" + amount, "data");


			return stringBuilder.ToStringExtend();
		}

		#endregion

		#region Cash Deposit CC Manual

		public async Task<bool> CashDepositCCManualAsync(string track2, string amount)
		{
			if (string.IsNullOrEmpty(track2))
				throw new ArgumentNullException(nameof(track2));
			if (string.IsNullOrEmpty(amount))
				throw new ArgumentNullException(nameof(amount));

			return await ExecuteFaultHandledOperationAsync<string, bool>(async c =>
			{
				var response = await ExecuteServiceAsync<string, byte[]>(GetCashDepositCreditCardManualRequest(track2, amount));
				return ToCashDepositCCManual(response);
			});
		}

		private bool ToCashDepositCCManual(string response)
		{
			return true;
		}

		private string GetCashDepositCreditCardManualRequest(string track2, string amount)
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("1", "Message Class");
			stringBuilder.Append("1", "Message Sub-Class");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000", "LUNO");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("15", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("CC AC  C", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000000000000", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(track2, "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("2080110000000000000000000000000000000000000000000000", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("U", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("V", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("w" + amount, "data");


			return stringBuilder.ToStringExtend();
		}

		#endregion

		#region Eid Validation

		public async Task<List<NdcCard>> GetEIDACardListAsync(string eidNumber)
		{
			if (string.IsNullOrEmpty(eidNumber))
				throw new ArgumentNullException(nameof(eidNumber));

			return await ExecuteFaultHandledOperationAsync<string, List<NdcCard>>(async c =>
			{
				var response = await ExecuteServiceAsync<string, byte[]>(GetEidaCardListRequest(eidNumber));
				return ToEIDACardListResponse(response);
			});
		}

		private string GetEidaCardListRequest(string eidNumber)
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("1", "Message Class");
			stringBuilder.Append("1", "Message Sub-Class");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000", "LUNO");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("1", "data");
			stringBuilder.Append("1", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(eidNumber, "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(" IIII   ", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000000000000", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("0", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("2040410000000000000000000000000000000000000000000000", "data");

			return stringBuilder.ToStringExtend();
		}

		private List<NdcCard> ToEIDACardListResponse(string message)
		{
            var cardImages = new List<string>();
            var cardNumbers = new List<string>();
            var cards = new List<NdcCard>();

            try
            {
                var data = message.Split(FieldSeparatorChar, EscapeCharacterChar, ShiftOutCharacterChar, FieldCharacterChar, '\n');
                foreach (var item in data)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        if (item.StartsWith("P2"))
                        {
                            cardImages.Add(item.Substring(2));
                        }
                        if (item.Length == 8)
                        {
                            if (item[2] == 'X' && item[3] == 'X')
                            {
                                cardNumbers.Add(item.Substring(2, 6));
                            }
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new HostNoResponseException("Cash Deposit Confirmation", "Expected response from the host not found.");
            }
			

			string[] FDKMap = { "I", "H", "G", "F", "A", "B", "C", "D" };

			for (int i = 0; i < cardNumbers.Count; i++)
			{
				cards.Add(new NdcCard()
				{
					CardNumber = cardNumbers.Count >= i ? cardNumbers[i] : "",
					ImageName = cardImages.Count > i ? cardImages[i + 1] : "",
					CardFDK = FDKMap.Length >= i ? FDKMap[i] : ""
				});
			}

			if (cards.Count > 0)
				return cards;
			else
				throw new EIDCardsNotFoundException("No cards found");
		}

		public async Task<bool> CardSelectedAsync(string eidNumber, string cardFdk, bool waitForResponse = true)
		{
			//if (string.IsNullOrEmpty(eidNumber))
			//	throw new ArgumentNullException(nameof(eidNumber));
			if (string.IsNullOrEmpty(cardFdk))
				throw new ArgumentNullException(nameof(cardFdk));

			return await ExecuteFaultHandledOperationAsync<string, bool>(async c =>
			{
				var response = await ExecuteServiceAsync<string, byte[]>(GetEidaCardSelectedRequest(eidNumber, cardFdk), waitForResponse);
				return waitForResponse == true ? ToEidaCardSelectedResponse(response) : true;
			});
		}

		private string GetEidaCardSelectedRequest(string eidNumber, string cardFdk)
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("1", "Message Class");
			stringBuilder.Append("1", "Message Sub-Class");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000", "LUNO");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("1", "data");
			stringBuilder.Append("2", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(eidNumber, "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(cardFdk, "data"); // card fdk
			stringBuilder.Append(FieldSeparator, "Field Separator");
			return stringBuilder.ToStringExtend();
		}

		private bool ToEidaCardSelectedResponse(string message)
		{
			var data = message.Split(FieldSeparatorChar, '\n');
			foreach (var item in data)
			{
				if (item == "070")
				{
					return true;
				}
			}
			return false;
		}

		public async Task<bool> ValidateEidPinAsync(string eidNumber, string cardFdk, string pinBlock, bool waitForReponse = true)
		{
			if (string.IsNullOrEmpty(eidNumber))
				throw new ArgumentNullException(nameof(eidNumber));
			if (string.IsNullOrEmpty(cardFdk))
				throw new ArgumentNullException(nameof(cardFdk));
			if (string.IsNullOrEmpty(pinBlock))
				throw new ArgumentNullException(nameof(pinBlock));

			return await ExecuteFaultHandledOperationAsync<string, bool>(async c =>
			{
				var response = await ExecuteServiceAsync<string, byte[]>(GetEidPinValidationRequest(eidNumber, cardFdk, pinBlock), waitForReponse);
				return waitForReponse == true ? await ToEidPinValidationResponseAsync(response) : true;
			});
		}

		private string GetEidPinValidationRequest(string eidNumber, string cardFdk, string pinBlock)
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("1", "Message Class");
			stringBuilder.Append("1", "Message Sub-Class");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000", "LUNO");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("1", "data");
			stringBuilder.Append("3", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(eidNumber, "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(" AAAA   ", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000000000000", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(pinBlock, "pinblock");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(cardFdk, "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("2812210000000000000000000000000000000000000000000000", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("U", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("V", "data");

			return stringBuilder.ToStringExtend();
		}

		private async Task<bool> ToEidPinValidationResponseAsync(string message)
		{
            if (string.IsNullOrEmpty(message))
            {
                throw new HostNoResponseException("EID PIN Validation", "Expected response from the host not found.");
            }

			var data = message.Split(FieldSeparatorChar, '\n');

			if (data[3] == "026")
			{
				throw new InvalidPinException();
			}
			else if (data[3] != "025")
			{
				if (message.ToLower().Contains("your card has been retained"))
				{
					throw new CardCaptureException();
				}
				if (message.ToLower().Contains("maximum pin retry exceeded"))
				{
					throw new ExceededPinException();
				}

				throw new Exception();
			}

			await GetReadyAsync();

			return true;
		}

		#endregion

		#region Ready

		public async Task<bool> GetReadyAsync()
		{
			return await ExecuteFaultHandledOperationAsync<string, bool>(async c =>
			{
				var response = await ExecuteServiceAsync<string, byte[]>(GetReadyRequest(), false);
				return true;
			});
		}

		private string GetReadyRequest()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("2", "Message Class");
			stringBuilder.Append("2", "Message Sub-Class");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000", "LUNO");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("B", "data");
			return stringBuilder.ToStringExtend();
		}

		#endregion

		#region Eid Cash Withdrawal

		public async Task<bool> PreCashWithdrawalEIDAsync(string eidNumber, string cardFdk, string pinBlock)
		{
			if (string.IsNullOrEmpty(eidNumber))
				throw new ArgumentNullException(nameof(eidNumber));
			if (string.IsNullOrEmpty(cardFdk))
				throw new ArgumentNullException(nameof(cardFdk));
			if (string.IsNullOrEmpty(pinBlock))
				throw new ArgumentNullException(nameof(pinBlock));

			return await ExecuteFaultHandledOperationAsync<string, bool>(async c =>
			{
				var response = await ExecuteServiceAsync<string, byte[]>(GetPreCashWithdrawalEIDRequest(eidNumber, cardFdk, pinBlock));
				return await ToPreCashWithdrawalEIDResponseAsync(response);
			});
		}

		private string GetPreCashWithdrawalEIDRequest(string eidNumber, string cardFdk, string pinBlock)
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("1", "Message Class");
			stringBuilder.Append("1", "Message Sub-Class");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000", "LUNO");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("1", "data");
			stringBuilder.Append("2", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(eidNumber, "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(" IIIH   ", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000000000000", "data");  // amount always zero
			stringBuilder.Append(FieldSeparator, "Field Separator");
			//stringBuilder.Append(";6<:329=63>23=?;", "pinblock");
			stringBuilder.Append(pinBlock, "pinblock");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(cardFdk, "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("2040710000000000000000000000000000000000000000000000", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("U", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("V", "data");
			return stringBuilder.ToStringExtend();
		}

		private async Task<bool> ToPreCashWithdrawalEIDResponseAsync(string message)
		{
			var data = message.Split(FieldSeparatorChar, '\n');
			//foreach (var item in data)
			//{
			//	if (item == "115")
			//	{
			//		await GetReadyAsync();
			//		return true;
			//	}
			//}
			await GetReadyAsync();
			return true;
		}

		public async Task<CashWithdrawal> ActualCashWithdrawalEIDAsync(string pin, string amount, string eidNumber, string cardFdk)
		{
			if (string.IsNullOrEmpty(eidNumber))
				throw new ArgumentNullException(nameof(eidNumber));
			if (string.IsNullOrEmpty(cardFdk))
				throw new ArgumentNullException(nameof(cardFdk));
			if (string.IsNullOrEmpty(pin))
				throw new ArgumentNullException(nameof(pin));
			if (string.IsNullOrEmpty(amount))
				throw new ArgumentNullException(nameof(amount));

			return await ExecuteFaultHandledOperationAsync<string, CashWithdrawal>(async c =>
			{
				var response = await ExecuteServiceAsync<string, byte[]>(GetCashWithdrawalEIDRequest(pin, amount, eidNumber, cardFdk), true);
				return await ToActualCashWithdrawalAsync(response);
			});
		}

		private string GetCashWithdrawalEIDRequest(string pin, string amount, string eidNumber, string cardFdk)
		{
			if (amount.Length < 12)
			{
				string dataToAppend = "";
				for (int i = 1; i <= 12 - amount.Length; i++)
				{
					dataToAppend += "0";
				}
				amount = dataToAppend + amount;
			}
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("1", "Message Class");
			stringBuilder.Append("1", "Message Sub-Class");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000", "LUNO");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("1", "data");
			stringBuilder.Append("2", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(eidNumber, "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(" AAB A B", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(amount, "data"); // amount
			stringBuilder.Append(FieldSeparator, "Field Separator");
			//stringBuilder.Append(";6<:329=63>23=?;", "pinblock");
			stringBuilder.Append(pin, "pinblock");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(cardFdk, "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("2040710000000000000000000000000000000000000000000000", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("U", "data");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("V", "data");
			return stringBuilder.ToStringExtend();
		}

		private async Task<CashWithdrawal> ToActualCashWithdrawalAsync(string message)
		{
			var result = new CashWithdrawal();
			

			if (!string.IsNullOrEmpty(message))
			{
				var array = message.Split(FieldSeparatorChar, EscapeCharacterChar, FieldCharacterChar, '\n');

				int resInd = 0;
				foreach (var item in array)
				{
					try
					{
						if (item.ToLower().Contains("response code"))
							break;
					}
					catch { }
					resInd++;
				}

				int authInd = 0;
				foreach (var item in array)
				{
					try
					{
						if (item.ToLower().Contains("trn. nbr"))
							break;
					}
					catch { }

					authInd++;
				}

				int balInd = 0;
				foreach (var item in array)
				{
					try
					{
						if (item.ToLower().Contains("available bal"))
							break;
					}
					catch { }

					balInd++;
				}

				int emvInd = 0;
				foreach (var item in array)
				{
					try
					{
						if (item.ToLower().Contains("cam"))
							break;
					}
					catch { }

					emvInd++;
				}

				var resCode = array[resInd]?.Split(ShiftOutCharacterChar)[1]?.Substring(1);
                try
                {
                    _logger?.Info($"Response Code: {resCode}");
                }
                catch (Exception)
                {
                }

                var iccData = "";
				var authCode = "";
				var balance = "0";

				try
				{
					balance = array[balInd].Split(':')[1].Substring(5);
				}
				catch (Exception ex)
				{
					balance = "0";
				}
				try
				{
					iccData = emvInd > 0 && emvInd < array.Length ? array[emvInd] : "";
					authCode = array[authInd].Split(':')[1].Substring(2);
                    
                }
				catch (Exception ex)
				{
					if (resCode == "000")
					{
						await CashWithdrawalReversalAsync();
						throw ex;
					}
				}

				if (resCode == "000")
				{
					if (iccData.Length > 0)
					{
						if (iccData.ToLower().Contains("cam"))
						{
							iccData = iccData.Substring(iccData.ToLower().IndexOf("cam") + 3);
						}
					}

                    await CashWithrawalAdviceConfirmationAsync();

                    // Success
                    result.AuthCode = authCode;
					result.IccData = iccData;
					result.AvailableBalance = double.Parse(balance);
				}
				else if (resCode.Trim() == "010")
				{
					throw new InsufficientAccountBalanceException();
				}
				else if (resCode.Trim() == "116")
				{
					throw new InsufficientAccountBalanceException();
				}
				else if (resCode.Trim() == "121")
				{
					throw new ExceededWithdrawalLimitException();
				}
				else if (resCode.Trim() == "123")
				{
					throw new ExceededWithdrawalFrequencyException();
				}
				else if (resCode.Trim() == "124")
				{
					throw new ExceededPerTransactionLimitException();
				}
				else if (resCode.Trim() == "150")
				{
					throw new ExceededWithdrawalLimitException();
				}
				else
				{
                    // Failure
                    throw new HostNoResponseException("Cash Withdrawal", "Expected response from the host not found.");
                }
			}
            else
            {
                // Failure
                throw new HostNoResponseException("Cash Withdrawal", "Expected response from the host not found.");
            }

            return result;
		}

        #endregion

    }
}