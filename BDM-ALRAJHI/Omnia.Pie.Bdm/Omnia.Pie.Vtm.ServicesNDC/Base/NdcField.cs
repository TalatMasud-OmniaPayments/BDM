namespace Omnia.Pie.Vtm.ServicesNdc
{
	public class NdcField
	{
		public bool Result { get; set; }
		public string MessageClass { get; set; }
		public string MessageSubClass { get; set; }
		/// <summary>
		/// Logical Unit Number
		/// </summary>
		public string Luno { get; set; }
		public string TimeVariantNumber { get; set; }
		public string TopofReceiptTransactionFlag { get; set; }
		public string MessageCoOrdinationNumber { get; set; }
		public string Track2 { get; set; }
		public string Track3 { get; set; }
		public string OperationCodeData { get; set; }
		public string AmountEntryField { get; set; }
		public string PinBuffer { get; set; }
		public string GeneralPurposeBufferB { get; set; }
		public string GeneralPurposeBufferC { get; set; }
		public string Track1Identifier { get; set; }
		public string Track1 { get; set; }
		public string TransactionStatusDataIdentifier { get; set; }
		public string LastTransactionStatusData { get; set; }
		public string CSPDataId { get; set; }
		public string CSPData { get; set; }





		public string CheckDestinationData_OnlyNDC { get; set; }
		public string NumberOfCoins_OnlyNDC { get; set; }
		public string NumberOfNotes_OnlyNDC { get; set; }
		public string NumberOfCoins_OnlyDDC { get; set; }
		public string DollarAmountOfCoins_OnlyDDC { get; set; }
		public string PrinterFlag { get; set; }
		public string DepositBuffer { get; set; }
		public string DepositBuffer2 { get; set; }
		public string VoiceScrCtrlData { get; set; }
		public string VoiceData { get; set; }
		public string VoiceDataw { get; set; }
		public string WincorCashInScreen1 { get; set; }
		public string WincorCashInScreen2 { get; set; }
		public string Track3Data_OnlyDDC { get; set; }
		public string Track2Data_OnlyDDC { get; set; }
		public string Track1Data_OnlyDDC { get; set; }
		public string StatementPrintData_OnlyNDC { get; set; }
		public string MsgSeqNOTVN { get; set; }
		public string NextState { get; set; }
		public string NumberOfDispense { get; set; }
		public string TranSerialNO { get; set; }
		public string[] FunctionID { get; set; }
		public string[] ScreenNumbers { get; set; }
		public string WincorCashInScreen3 { get; set; }
		public string MsgCoordinationNumber { get; set; }
		public string ReceiptData { get; set; }
		public string JournalData { get; set; }
		public string DepositPrintData { get; set; }
		public string EndorseCheckData_OnlyNDC { get; set; }
		public string EMVIcc { get; set; }
		public string MacData { get; set; }
		public string MacOfThisMessage { get; set; }
		public string CardReturnRetainFlag { get; set; }
	}
}