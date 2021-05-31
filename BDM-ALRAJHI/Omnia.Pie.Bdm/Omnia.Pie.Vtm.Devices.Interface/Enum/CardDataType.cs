namespace Omnia.Pie.Vtm.Devices.Interface.Enum
{
	public enum CardDataType
	{
		/// <summary>
		/// Regural track data
		/// </summary>
		ISO1 = 1,
		/// <summary>
		/// Regural track data
		/// </summary>
		ISO2 = 2,
		/// <summary>
		/// Regural track data
		/// </summary>
		ISO3 = 4,
		WM = 8,
		MP = 16,
		/// <summary>
		///  Chip data, e.g. for EmiratesID
		/// </summary>
		CHIP = 32,
		FLUXINACTIVE = 64,
		FRONTIMAGE = 128,
		BACKIMAGE = 256,
		EMBOSS = 512
	}
}