namespace Omnia.Pie.Vtm.DataAccess.Interface
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Omnia.Pie.Vtm.DataAccess.Interface.Entities;

	public interface IRetractedCardStore
	{
		Task ClearAll();
		Task<List<RetractedCard>> GetList();
		Task Save(RetractedCard card);
	}
}