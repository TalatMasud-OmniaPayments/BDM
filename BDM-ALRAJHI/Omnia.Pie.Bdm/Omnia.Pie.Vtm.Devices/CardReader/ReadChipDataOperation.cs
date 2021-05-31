namespace Omnia.Pie.Vtm.Devices.CardReader
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
	using AxNXCardReaderXLib;
	using Omnia.Pie.Client.Journal.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Constants;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using Omnia.Pie.Vtm.Framework.Interface;

	internal class ReadChipDataOperation : DeviceOperation<EmiratesId>
	{
		readonly CardReader cardReader;
		AxNXCardReaderX ax => (AxNXCardReaderX)cardReader.Ax;

		public ReadChipDataOperation(ILogger logger, IJournal journal, CardReader cardReader)
			: base(nameof(ReadChipDataOperation), logger, journal)
		{
			this.cardReader = cardReader;
		}

		protected override async Task<EmiratesId> OnInvoke()
		{
			await SelectApplication(1);
			await SelectEfFile(2, 1);
			var file1 = await ReadSelectedBinaryEfFile(0, 230);

			await SelectEfFile(2, 3);
			var file3 = await ReadSelectedBinaryEfFile(230, 230);

			await SelectEfFile(2, 5);
			var file5 = await ReadSelectedBinaryEfFile(178, 230);

			//Logger.Info("Idn and cn file bytes: " + file1 != null ? BitConverter.ToString(file1) : "");
			//Logger.Info("Cardholder file bytes: " + file3 != null ? BitConverter.ToString(file3) : "");
			//Logger.Info("Cardholder ex file bytes: " + file5 != null ? BitConverter.ToString(file5) : "");

			var emiratesId = new EmiratesId();

			if (file1 != null)
			{
				emiratesId.Id = Encoding.ASCII.GetString(ExtractFieldBytesByDataTag(file1, CardChipTags.IDN));
				emiratesId.CardNumber = Encoding.ASCII.GetString(ExtractFieldBytesByDataTag(file1, CardChipTags.CN));
				Logger.Info(" Eida cardholder id number is " + MaskCardNumber(emiratesId.Id));
			}
			else
				Logger.Info("Failed to read idn and cn file bytes from the eida card.");

			if (file3 != null)
			{
				emiratesId.Sex = Encoding.ASCII.GetString(ExtractFieldBytesByDataTag(file3, CardChipTags.SEX));
				emiratesId.DateOfBirth = BcdToDate(ExtractFieldBytesByDataTag(file3, CardChipTags.DATEOFBIRTH));
				emiratesId.ExpiryDate = BcdToDate(ExtractFieldBytesByDataTag(file3, CardChipTags.EXPIRYDATE));
				emiratesId.FullName = Encoding.UTF8.GetString(ExtractFieldBytesByDataTag(file3, CardChipTags.FULLNAME));
				Logger.Info(" Eida cardholder FullName is " + emiratesId.FullName);
				Logger.Info(" Eida cardholder card expiry date is " + emiratesId.ExpiryDate);
			}
			else
				Logger.Info("Failed to read cardholder file bytes data from the eida card.");

			if (file5 != null)
			{
				emiratesId.PassportExpiryDate = BcdToDate(ExtractFieldBytesByDataTag(file5, CardChipTags.PASSPORTEXPIRYDATE));
				emiratesId.PassportIssueDate = BcdToDate(ExtractFieldBytesByDataTag(file5, CardChipTags.PASSPORTISSUEDATE));
				emiratesId.PassportNumber = Encoding.UTF8.GetString(ExtractFieldBytesByDataTag(file5, CardChipTags.PASSPORTNUMBER));
				emiratesId.ResidencyExpiryDate = BcdToDate(ExtractFieldBytesByDataTag(file5, CardChipTags.RESIDENCYEXPIRYDATE));
				emiratesId.ResidencyNumber = Encoding.UTF8.GetString(ExtractFieldBytesByDataTag(file5, CardChipTags.RESIDENCYNUMBER));

				if (emiratesId.PassportNumber.IndexOf("PS-") > -1)
					emiratesId.PassportNumber = emiratesId.PassportNumber.Replace("PS-", "");

				//Logger.Info(" Eida cardholder passport expiry date is " + emiratesId.PassportExpiryDate);
				//Logger.Info(" Eida cardholder passport issue date is " + emiratesId.PassportIssueDate);
				//Logger.Info(" Eida cardholder passport number is " + emiratesId.PassportNumber);
			}
			else
				Logger.Error("Failed to read cardholder ex file bytes data from the eida card.");
			return emiratesId;
		}

		async Task<byte[]> chipIO(byte[] command)
		{
			Logger.Info("[ExecuteCommand(command)] Executing command onto the eida card chip: " + BitConverter.ToString(command));
			object[] objArray = new object[command.Length];
			for (int index = 0; index < command.Length; ++index)
				objArray[index] = (object)command[index];
			return await cardReader.ChipIOOperation.StartAsync(() => ax.ChipIO(1, "SEND", objArray, Timeout.Operation));
		}

		public async Task<byte[]> SelectEfFile(byte dataTag1, byte dataTag2)
		{
			Logger.Info($"{ClassName} [SelectEfFile(dataTag1={dataTag1}, dataTag2={dataTag2})] ");
			return await chipIO(new byte[8] { 0, 164, 0, 0, 2, dataTag1, dataTag2, 0 });
		}

		public async Task<byte[]> SelectApplication(byte appId)
		{
			Logger.Info($"{ClassName} [SelectApplication({appId})] ");
			return await chipIO(new byte[18] { 0, 164, 4, 0, 12, 160, 0, 0, 2, 67, 0, 19, 0, 0, 0, 1, appId, 0 });
		}

		// ??? 
		byte offset;
		string ClassName = "ApduCommandExecutor";

		// to simplify...
		async Task<byte[]> ReadSelectedBinaryEfFileChunk(int offset, byte chunkSize)
		{
			Logger.Info($"{ClassName} [ReadSelectedBinaryEfFileChunk({offset},{chunkSize},{this.offset})] ");
			this.offset = (byte)(offset & (int)byte.MaxValue);
			return await chipIO(new byte[5] { 0, 176, (byte)(offset >> 8), this.offset, chunkSize });
		}

		// to simplify...
		public async Task<byte[]> ReadSelectedBinaryEfFile(byte maxOffset = 226, byte binaryChunk = 230)
		{
			Logger.Info($"{ClassName} [ReadSelectedBinaryEfFile({maxOffset}, {binaryChunk})] ");
			var result = new List<byte>();
			int chunkOffset = 0;
			offset = 0;
			while (true)
			{
				byte[] numArray = await this.ReadSelectedBinaryEfFileChunk(chunkOffset, binaryChunk);
				if (numArray == null)
					break;
				result.AddRange(numArray);
				if (numArray.Length >= (int)binaryChunk && (int)offset != (int)maxOffset)
					chunkOffset += (int)binaryChunk;
				else
					break;
			}
			return result.ToArray();
		}

		// To refactor to using string date format
		static string BcdToDate(byte[] bcds)
		{
			if (bcds == null || bcds.Length == 0)
				return "";

			string str = BitConverter.ToString(bcds).Replace("-", "");
			return str.Substring(0, 4) + "-" + str.Substring(4, 2) + "-" + str.Substring(6);
		}

		// To exstract more general meaning and move to utilities
		static byte[] ExtractFieldBytesByDataTag(byte[] rawBytes, byte[] dataTag)
		{
			if (rawBytes == null)
				return null;

			for (int i = 0; i < rawBytes.Length - 1; ++i)
			{
				if ((int)rawBytes[i] == (int)dataTag[0] && (int)rawBytes[i + 1] == (int)dataTag[1])
				{
					int count = int.Parse(BitConverter.ToString(new byte[] { rawBytes[i + 3] }), NumberStyles.HexNumber);
					byte[] numArray = new byte[count];
					Buffer.BlockCopy(rawBytes, i + 4, numArray, 0, count);
					return numArray;
				}
			}

			return new byte[] { };
		}

        private static string MaskCardNumber(string v)
        {
            if (string.IsNullOrEmpty(v))
                return "";

            char MaskSymbol = '*';
            string MaskPattern = "^(.{4})(.+)(.{4})$";

            var result = v;

            if (!string.IsNullOrEmpty(result))
            {
                result = Regex.Replace(result, MaskPattern, m => $"{m.Groups[1]}{new String(MaskSymbol, m.Groups[2].Length)}{m.Groups[3]}");
            }

            return result;
        }
    }
}