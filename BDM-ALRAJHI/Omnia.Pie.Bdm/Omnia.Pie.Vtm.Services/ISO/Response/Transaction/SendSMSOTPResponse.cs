using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Services.ISO.Response.Transaction
{
	public class SendSMSOTP
	{
		public string Otp { get; set; }
		public string Uuid { get; set; }
	}


	public class SendSMSOTPResponse : ResponseBase<SendSMSOTP>
	{
		public SendSMSOTP SendSmsOtp { get; set; }
	}
}
