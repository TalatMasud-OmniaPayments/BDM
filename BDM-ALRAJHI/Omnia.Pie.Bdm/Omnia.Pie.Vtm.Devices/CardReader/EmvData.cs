namespace Omnia.Pie.Vtm.Devices.Emv
{
	using Omnia.Pie.Vtm.Devices.Interface;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	using System.Threading.Tasks;

    internal class EmvData : IEmvData
    {
        public EmvData(ProcessEmv proc)
        {
            _process = proc;
        }

        private ProcessEmv _process { get; }

        public string ApplicationId { get; private set; }
        public string ApplicationLabel { get; private set; }
        public string CardNumber { get; private set; }
        public string Track2 { get; private set; }

        public bool MsrFallback { get; private set;}

		public string IccData { get; private set; }

		public async Task InitializeAsync(int amount, string transactionType)
		{
            MsrFallback = false;

			await _process.Activate();

            _process.ProcessApplication(amount, transactionType);

            ApplicationId = _process.ApplicationID;
            ApplicationLabel = _process.ApplicationLabel;
            CardNumber = _process.PanData;
            Track2 = _process.Track2Data;

        }
		private string ReplaceCharacters(string pin)
		{
			pin = pin.Replace(':', 'A');
			pin = pin.Replace(';', 'B');
			pin = pin.Replace('<', 'C');
			pin = pin.Replace('=', 'D');
			pin = pin.Replace('>', 'E');
			pin = pin.Replace('?', 'F');

			return pin;
		}
		public string BuildIccData(string pinBlock)
		{
			pinBlock = ReplaceCharacters(pinBlock);
			if (_process.HasChip && !_process.HasChipErrors)
			{
				IccData = _process.ProcessPIN(pinBlock);
			}
			else
			{
				IccData = _process.MSRFallbackTagList;
                MsrFallback = true;
			}
			return IccData;
		}

		public async Task<bool> ValidateCardAutorizationAsync(string iccData)
		{

			if (string.IsNullOrEmpty(iccData))
			{
				return false;
			}

			iccData = Regex.Replace(iccData, @"\s+", "");
			var parsedData = ParseTLV(iccData, "000");

			var responseCode = "";
			var arpcData = "";
			var issuerScriptData1 = "";
			var issuerScriptData2 = "";
			//			int decValue = int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
			string[] tags = iccData.Split(',');
			foreach (var element in parsedData)
			{
				if (element.Key == "8A")
				{
					if (element.Value == "3030")
						responseCode = "00";
					else
						responseCode = "01";
				}
				else if (element.Key == "91")
				{
					arpcData = element.Value;
				}
				else if (element.Key == "71")
				{
					issuerScriptData1 = element.Value;
				}
				else if (element.Key == "72")
				{
					issuerScriptData2 = element.Value;
				}
			}

			bool approvalResult = _process.ProcessOnlineApproval(responseCode, arpcData, issuerScriptData1, issuerScriptData2);

			await _process.Deactivate();

			return approvalResult;
		}

		public Dictionary<string, string> ParseTLV(string tlv, string response_code)
		{
			if (tlv == null || tlv.Length % 2 != 0)
			{
				throw new Exception("Invalid tlv, null or odd length");
			}

			Dictionary<string, string> HashMap = new Dictionary<string, string>();
			List<string> tlvBytes = new List<string>();
			for (int i = 2; i <= tlv.Length; i += 2)
			{
				tlvBytes.Add(tlv.Substring(i - 2, 2));
			}

			if (tlvBytes.Count > 2)
			{
				for (int i = 0; i < tlvBytes.Count;)
				{
					HashMap.Add(tlvBytes[i], GetValue(tlvBytes[i + 1], tlvBytes, i));
					i = i + int.Parse(tlvBytes[i + 1], System.Globalization.NumberStyles.HexNumber) + 2;
				}
			}
			else
			{
				throw new Exception("EMV Validation failed");
			}

			return HashMap;
		}

		public string GetValue(string length, List<string> data, int startInd)
		{
			int decValue = int.Parse(length, System.Globalization.NumberStyles.HexNumber);
			List<string> parsedData = data.GetRange(startInd + 2, decValue);
			string dataProcessed = string.Empty;
			foreach (var item in parsedData)
			{
				dataProcessed += item;
			}

			return dataProcessed;
		}
	}
}