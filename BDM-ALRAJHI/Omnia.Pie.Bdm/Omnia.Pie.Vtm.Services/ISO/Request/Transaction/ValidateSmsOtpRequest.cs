using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Services.ISO.Request.Transaction
{
	public class ValidateSmsOtpRequest : RequestBase
	{
		public string Otp { get; set; }
		public string Uuid { get; set; }
	}
}
