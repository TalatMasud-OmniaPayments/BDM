namespace Omnia.Pie.Vtm.Workflow.RequestLC
{
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.Workflow.RequestLC.Context;
	using System.Windows.Media.Imaging;
	using Omnia.Pie.Vtm.Services.Interface;

	internal class RequestLCContext : BaseContext, IRequestLCContext
	{
		public BitmapSource Signature { get; set; }
		public CustomerDetail CustomerDetail { get; set; }
		public Attachment Attachment { get; set; }
		public string TSNno { get; set ; }
		public bool SelfServiceMode { get; set; }
		public bool SendSms { get; set; }
		public bool SendEmail { get; set; }
	}
}