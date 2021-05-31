namespace Omnia.Pie.Vtm.Devices.Interface.Constants
{
	public static class Timeout
	{
		public const int Infinite = 0;
		public const int _20Sec = 20000;
		public const int _30Sec = 30000;
		public const int _1Min = 60000;
		public const int _90Sec = 90000;
		public const int _5Min = 300000;

		public const int AwaitTaken = _20Sec;
		public const int Operation = _20Sec;
		public const int Insert = _90Sec;
		public const int Initialize = _1Min;
		public const int Scan = _1Min;
	}
}