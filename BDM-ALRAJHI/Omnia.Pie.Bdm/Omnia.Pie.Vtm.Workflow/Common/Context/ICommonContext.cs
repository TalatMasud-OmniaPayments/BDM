using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Workflow.Common.Context
{
	public interface ICommonContext
	{
		bool IsCanceled { get; set; }
	}
}
