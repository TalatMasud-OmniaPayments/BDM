using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Devices.Interface.Entities
{
	public class DepositDenomination
	{
		public int Denomination { get; set; }
		public int Quantity { get; set; }
		public int Amount => Denomination * Quantity;
	}
}
