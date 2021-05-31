namespace Omnia.Pie.Vtm.Workflow.RequestNLC.Context
{
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using System.Windows.Media.Imaging;

	public interface IRequestNLCContext
	{
		BitmapSource Signature { get; set; }
		CustomerDetail CustomerDetail { get; set; }
		Attachment Attachment { get; set; }
		string TSNno { get; set; }
		bool SelfServiceMode { get; set; }
		bool SendSms { get; set; }
		bool SendEmail { get; set; }
	}
}