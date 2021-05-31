namespace Omnia.Pie.Vtm.DataAccess.Interface
{
	using System.Threading.Tasks;
	using Omnia.Pie.Vtm.DataAccess.Interface.Entities;

	public interface IJournalMessageStore
	{
		Task ClearAll();
		Task Delete(JournalMessage message);
		Task<JournalMessage> Get();
		Task Save(JournalMessage message);
	}
}