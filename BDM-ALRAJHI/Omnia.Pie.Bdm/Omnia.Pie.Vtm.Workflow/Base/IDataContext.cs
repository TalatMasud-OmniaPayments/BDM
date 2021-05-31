using System.Windows.Media.Imaging;

namespace Omnia.Pie.Vtm.Workflow
{
	public interface IDataContext
	{
		T Get<T>() where T : class;
		bool TryGet<T>(out T t) where T : class;
	}
}