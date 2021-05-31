using Omnia.Pie.Vtm.ServicesNdc.Interface.Entities;
using System.Collections.Generic;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	public interface ICardImageListViewModel : IExpirableBaseViewModel
	{
		NdcCard SelectedCard { get; set; }
		List<NdcCard> Cards { get; set; }
	}
}