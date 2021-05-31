namespace Omnia.Pie.Vtm.Ndc.Server.Test
{
	using Omnia.Pie.Vtm.ServicesNdc.Base;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Net.Sockets;
	using System.Text;

	class Program
	{
		const int PORT_NO = 10085;
		const string SERVER_IP = "127.0.0.1";
		const string FieldSeparator = "\u001C";
		const string GroupSeparator = "\u001D";
		const string EscapeCharacter = "\u001B";
		const string GreaterThan = "\u003E";
		const string ShiftOutCharacter = "\u000E";

		static void Main(string[] args)
		{
            /*
			var tcpClient = new NDCSSLClient(null);
			tcpClient.Ssl = true;
			tcpClient.StartClientAsync();
            */

			//var localAdd = IPAddress.Parse(SERVER_IP);
			//var listener = new TcpListener(localAdd, PORT_NO);

			//Console.WriteLine("Listening...");
			//listener.Start();

			//var client = listener.AcceptTcpClient();
			//var nwStream = client.GetStream();
			//byte[] buffer = new byte[client.ReceiveBufferSize];

			//while (true)
			//{
			//	var bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

			//	//---convert the data received into a string---
			//	var dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
			//	Console.WriteLine("Received : " + dataReceived);

			//	Console.WriteLine("Sending back : " + dataReceived);

			//	var a = GetCashWithdrawalDebitCardResponse().Select(Convert.ToByte).ToArray();
			//	nwStream.Write(a, 0, a.Length);

			//	//var b = GetCashWithdrawalCrediCardResponse().Select(Convert.ToByte).ToArray();
			//	//nwStream.Write(b, 0, b.Length);

			//	//var c = GetCashWithdrawalReversalResopnse().Select(Convert.ToByte).ToArray();
			//	//nwStream.Write(c, 0, c.Length);

			//	//var d = GetMiniStatementResponse().Select(Convert.ToByte).ToArray();
			//	//nwStream.Write(d, 0, d.Length);

			//	//var e = GetCashDepositCreditCardInsert().Select(Convert.ToByte).ToArray();
			//	//nwStream.Write(e, 0, e.Length);

			//	//var f = GetCashDepositCreditCardManualEntry().Select(Convert.ToByte).ToArray();
			//	//nwStream.Write(f, 0, f.Length);
			//}
		}

		private static string GetCashWithdrawalDebitCardResponse()
		{
			var stringBuilder = new StringBuilder();

			stringBuilder.Append("4", "Message Class");
			stringBuilder.Append(" ", "Message Sub-Class");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000", "LUNO");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("", "Message Sequence Number (not set)");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("405", "Next State Id");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("01", "Number of type 1 (Cassette 1)");
			stringBuilder.Append("00", "Number of type 2 (Cassette 2)");
			stringBuilder.Append("00", "Number of type 3 (Cassette 3)");
			stringBuilder.Append("00", "Number of type 4 (Cassette 4)");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("6091", "Transaction Serial Number");
			stringBuilder.Append("A", "Function Identifier");
			stringBuilder.Append("405", "Screen Number");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(":", "Message Coordiation Number");
			stringBuilder.Append("0", "Card Return/Retain Flag");
			stringBuilder.Append("1", "Printer Flag");
			stringBuilder.Append(EscapeCharacter, "EscapeCharacter");
			stringBuilder.Append(EscapeCharacter, "EscapeCharacter");
			stringBuilder.Append("(1========================================", "");
			stringBuilder.Append(EscapeCharacter, "EscapeCharacter");
			stringBuilder.Append("(>", "");
			stringBuilder.Append(ShiftOutCharacter, "ShiftOutCharacter");
			stringBuilder.Append("5WITHDRAWAL", "");
			stringBuilder.Append(EscapeCharacter, "EscapeCharacter");
			stringBuilder.Append("(1", "");
			stringBuilder.Append(ShiftOutCharacter, "ShiftOutCharacter");
			stringBuilder.Append("2DATE", "");
			stringBuilder.Append(ShiftOutCharacter, "ShiftOutCharacter");
			stringBuilder.Append("6HOUR", "");
			stringBuilder.Append(ShiftOutCharacter, "ShiftOutCharacter");
			stringBuilder.Append("8OP.", "");
			stringBuilder.Append(ShiftOutCharacter, "ShiftOutCharacter");
			stringBuilder.Append("6ATM 16/08/17", "");
			stringBuilder.Append(ShiftOutCharacter, "ShiftOutCharacter");
			stringBuilder.Append("213:27:33", "");
			stringBuilder.Append(ShiftOutCharacter, "ShiftOutCharacter");
			stringBuilder.Append("4006006006", "");
			stringBuilder.Append(ShiftOutCharacter, "ShiftOutCharacter");
			stringBuilder.Append("3AHBD0002", "");
			stringBuilder.Append(EscapeCharacter, "EscapeCharacter");
			stringBuilder.Append("(1CARD NBR.      :", "");
			stringBuilder.Append(ShiftOutCharacter, "ShiftOutCharacter");
			stringBuilder.Append("14714846938592602", "");
			stringBuilder.Append("ACCOUNT NBR.   :", "");
			stringBuilder.Append(ShiftOutCharacter, "ShiftOutCharacter");
			stringBuilder.Append("12851986018", "");
			stringBuilder.Append(EscapeCharacter, "EscapeCharacter");
			stringBuilder.Append("(1TRN. AMOUNT    :", "");
			stringBuilder.Append(ShiftOutCharacter, "ShiftOutCharacter");
			stringBuilder.Append("1AED 100.00", "");
			stringBuilder.Append("TRN. NBR       :", "");
			stringBuilder.Append(ShiftOutCharacter, "ShiftOutCharacter");
			stringBuilder.Append("1826091", "");
			stringBuilder.Append(EscapeCharacter, "EscapeCharacter");
			stringBuilder.Append("(1RESPONSE CODE  :", "");
			stringBuilder.Append(ShiftOutCharacter, "ShiftOutCharacter");
			stringBuilder.Append("1000", "");
			stringBuilder.Append("========================================", "");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("5CAM910AB7FD6B32931FB83A30308A023030", "EMV Response");

			return stringBuilder.ToString();
		}

		private static string GetCashWithdrawalCrediCardResponse()
		{
			StringBuilder sb = new StringBuilder();
			sb = new StringBuilder();
			sb.Append("4 000405030108016350A405102(15       CORNICHE BRANCH\n");
			sb.Append("\n");
			sb.Append("2DATE6TIME8SEQ.5ATM\n");
			sb.Append("23/08/17210:11:0948263503AHBD0002\n");
			sb.Append("\n");
			sb.Append("(>2CASH WITHDRAWAL\n");
			sb.Append("\n");
			sb.Append("(1CARD N0.         :1471484------2602\n");
			sb.Append("ACCOUNT NO.  :52851986018\n");
			sb.Append("(1AMOUNT         :1AED 9000.00\n");
			sb.Append("\n");
			sb.Append("\n");
			sb.Append("(1AVAILABLE BAL :2AED 10183.15\n");
			sb.Append("\n");
			sb.Append("\n");
			sb.Append("(15SUCCESSFUL TRANSACTION\n");
			sb.Append("\n");
			sb.Append("\n");
			sb.Append("\n");
			sb.Append("(12SEND LAHA TO 2494\n");
			sb.Append("2TO KNOW MORE ABOUT THE FIRST\n");
			sb.Append("2SCENTED  CREDIT CARD IN UAE\n");
			sb.Append("\n");
			sb.Append("11(1========================================\n");
			sb.Append("(>5WITHDRAWAL\n");
			sb.Append("(12DATE6HOUR8OP.6ATM\n");
			sb.Append("23/08/17210:11:0940060060063AHBD0002\n");
			sb.Append("(1CARD NBR.      :14714846938592602\n");
			sb.Append("ACCOUNT NBR.   :12851986018\n");
			sb.Append("(1TRN. AMOUNT    :1AED 9000.00\n");
			sb.Append("TRN. NBR       :1826350\n");
			sb.Append("(1RESPONSE CODE  :1000\n");
			sb.Append("========================================5CAM910A20B0D7EE2BF2BA2730308A023030");
			return sb.ToString();
		}

		private static string GetCashWithdrawalReversalResopnse()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("E4 00042212344112(15       CORNICHE BRANCH\n");
			sb.Append("\n");
			sb.Append("2DATE6TIME8SEQ.5ATM\n");
			sb.Append("23/08/17210:11:0948263503AHBD0002\n");
			sb.Append("\n");
			sb.Append("(14WITHDRAWAL - CANCELED\n");
			sb.Append("\n");
			sb.Append("(1CARD N0.      :2471484------2602\n");
			sb.Append("AMOUNT         :1AED 9000.00\n");
			sb.Append("\n");
			sb.Append("TRANSACTION IS CANCELED\n");
			sb.Append("\n");
			sb.Append("\n");
			sb.Append("\n");
			sb.Append("\n");
			sb.Append("\n");
			sb.Append("\n");
			sb.Append("\n");
			sb.Append("\n");
			sb.Append("(1=THANK YOU\n");
			sb.Append(";FOR USING AL RAJHI ATM1(1========================================\n");
			sb.Append("(>TRANSACTION CANCELED\n");
			sb.Append("(12DATE6HOUR8OP.6ATM\n");
			sb.Append("23/08/17210:11:0948263503AHBD0002\n");
			sb.Append("(1CARD NBR.      :14714846938592602\n");
			sb.Append("ACCOUNT NBR.   :12851986018\n");
			sb.Append("(1TRN. AMOUNT    :1AED 9000.00\n");
			sb.Append("(1RESPONSE CODE  :1480\n");
			sb.Append("========================================");
			return sb.ToString();
		}

		private static string GetMiniStatementResponse()
		{
			StringBuilder sb = new StringBuilder();
			//sb.Append((char) Integer.parseInt("03", 16));
			//sb.Append((char) Integer.parseInt("90", 16));
			sb.Append("4 00017463545174302(12DATE6HOUR8OP.6ATM\n");
			sb.Append("23/08/17210:14:2241140993AHBD0002\n");
			sb.Append("\n");
			sb.Append("(1<MINI STATEMENT\n");
			sb.Append("\n");
			sb.Append("(1CARD N0.         :1471484------2602\n");
			sb.Append("ACCOUNT NO.  :52851986018\n");
			sb.Append("DATE7DESCRIPTION6AMOUNT\n");
			sb.Append("10-09-17 ATM CARD CHA         2.00-\n");
			sb.Append("10-09-17 ATM WITHDRAW       200.00-\n");
			sb.Append("10-09-17 ATM WITHDRAW       500.00-\n");
			sb.Append("10-09-17 ATM WITHDRAW      1000.00-\n");
			sb.Append("10-09-17 ATM WITHDRAW      1000.00-\n");
			sb.Append("10-09-17 ATM WITHDRAW      9000.00-\n");
			sb.Append("10-09-17 ATM WITHDRAW      9000.00+\n");
			sb.Append("\n");
			sb.Append("AVAIL.BALANCE             19183.15 CR\n");
			sb.Append("\n");
			sb.Append("(12SEND LAHA TO 2494\n");
			sb.Append("2TO KNOW MORE ABOUT THE FIRST\n");
			sb.Append("2SCENTED  CREDIT CARD IN UAE\n");
			sb.Append("\n");
			sb.Append("11(1========================================\n");
			sb.Append("(>2SHORT STATEMENT\n");
			sb.Append("(12DATE6HOUR8OP.6ATM\n");
			sb.Append("23/08/17210:14:2241140993AHBD0002\n");
			sb.Append("(1CARD NBR.      :14714846938592602\n");
			sb.Append("ACCOUNT NBR.   :12851986018\n");
			sb.Append("(1RESPONSE CODE  :1000\n");
			sb.Append("========================================\n");
			sb.Append("(1TRN. NBR       :18263545CAM910A73FE99062B1E22B630308A023030");
			return sb.ToString();
		}

		private static string GetCashDepositCreditCardInsert()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("4 00062808015096296@@P2155\\(1[27;80mFC517400XXXXXX0003IC(1XXXXXXXXXXXXXXXXXXXX101\n");
			sb.Append("(15DEPOSIT TO CARD\n");
			sb.Append("\n");
			sb.Append("CARD NUMBER    :1517400------0003\n");
			sb.Append("ACCT NUMBER    :15174000009560003\n");
			sb.Append("TSN. NO. :70801");
			return sb.ToString();
		}

		private static string GetCashDepositCreditCardManualEntry()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("4 00062908035096296@@P2155\\(1[27;80mFC517400XXXXXX0003IC(1XXXXXXXXXXXXXXXXXXXX401\n");
			sb.Append("(15DEPOSIT TO CARD\n");
			sb.Append("\n");
			sb.Append("CARD NUMBER    :1517400------0003\n");
			sb.Append("ACCT NUMBER    :15174000009560003\n");
			sb.Append("TSN. NO. :70803");
			return sb.ToString();
		}
	}

	public static class StringBuilderExtensions
	{
		private static IList<KeyValuePair<string, string>> _dictionary = new List<KeyValuePair<string, string>>();

		public static StringBuilder Append(this StringBuilder stringBuilder, string value, string definition)
		{
			if (!string.IsNullOrEmpty(definition))
			{
				_dictionary.Add(new KeyValuePair<string, string>(definition, value));
			}

			stringBuilder.Append(value);
			return stringBuilder;
		}

		public static StringBuilder Append(this StringBuilder stringBuilder, char value, string definition)
		{
			if (!string.IsNullOrEmpty(definition))
			{
				_dictionary.Add(new KeyValuePair<string, string>(definition, value.ToString()));
			}

			stringBuilder.Append(value);

			return stringBuilder;
		}

		public static string ToStringExtend(this StringBuilder stringBuilder)
		{
			foreach (var current in _dictionary)
			{
				string str = current.Value;
				if (current.Value == '\u001c'.ToString() || current.Value == '\u001d'.ToString())
				{
					str = ".";
				}
			}

			_dictionary.Clear();
			return stringBuilder.ToString();
		}
	}
}