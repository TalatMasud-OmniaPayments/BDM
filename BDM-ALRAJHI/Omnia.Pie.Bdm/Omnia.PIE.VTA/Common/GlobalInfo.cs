using System.Security;

namespace Omnia.PIE.VTA.Common
{
	public static class GlobalInfo
	{
		public static string TellerId { get; set; }
		public static string TellerName { get; set; }
		public static string TellerPassword { get; set; }
		public static string PhoneNumber { get; set; }
		public static string PhonePassword { get; set; }

		public static SecureString AccessToken { get; set; }
		public static string State { get; set; }
		public static string Code { get; set; }

		public static TellerState TellerState { get; set; }

		public static bool IsLogin { get; set; }
		public static bool IsAuthenticated { get; set; }

		public static double RemoteWindowWidth { get; set; }
		public static double RemoteWindowHeight { get; set; }

		public static string SelectedWorkflowItem { get; set; }
		public static string SelectedWorkflowSubItem { get; set; }
		public static string SelectedWorkflowItemAction { get; set; }

		public static bool DashboardSelected { get; set; }
		public static string IdentityToken { get; set; }
	}
}