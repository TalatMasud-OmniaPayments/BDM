namespace Omnia.Pie.Vtm.Workflow.RequestNLC.Context
{
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using System.Windows.Media.Imaging;

	public class RequestNLCContext : BaseContext, IRequestNLCContext
	{
		public BitmapSource Signature { get; set; }
		public CustomerDetail CustomerDetail { get; set ; }
		public Attachment Attachment { get; set; }
		public string TSNno { get; set; }
		public bool SelfServiceMode { get; set; }
		public bool SendSms { get; set; }
		public bool SendEmail { get; set; }
	}
}