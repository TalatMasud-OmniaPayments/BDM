namespace Omnia.Pie.Vtm.Devices.Interface.Entities
{
	public class CardChipTags
	{
		public static byte[] IDN = new byte[2] { 225, 1 };
		public static byte[] CN = new byte[2] { 225, 2 };
		public static byte[] SEX = new byte[2] { 227, 12 };
		public static byte[] DATEOFBIRTH = new byte[2] { 67, 15 };
		public static byte[] EXPIRYDATE = new byte[2] { 67, 7 };
		public static byte[] FULLNAME = new byte[2] { 227, 11 };
		public static byte[] PASSPORTEXPIRYDATE = new byte[2] { 69, 42 };
		public static byte[] PASSPORTISSUEDATE = new byte[2] { 69, 41 };
		public static byte[] PASSPORTNUMBER = new byte[2] { 229, 38 };
		public static byte[] RESIDENCYEXPIRYDATE = new byte[2] { 69, 29 };
		public static byte[] RESIDENCYNUMBER = new byte[2] { 229, 28 };
	}
}