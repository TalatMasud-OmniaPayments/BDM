namespace Omnia.Pie.Vtm.Framework.Extensions
{
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Windows;

	public static class UIExtensions
	{
		public static T GetDataContextFromViewOrViewModel<T>(object view) where T : class
		{
			T viewAsT = view as T;

			if (viewAsT != null)
			{
				return viewAsT;
			}

			var element = view as FrameworkElement;

			if (element != null)
			{
				var vmAsT = element.DataContext as T;
				return vmAsT;
			}

			return null;
		}

		public static ResourceDictionary FindResourceDictionaryByKey(this Application application, string resourceKey)
		{
			return FindResourceDictionary(resourceKey, application.Resources.MergedDictionaries);
		}

		private static ResourceDictionary FindResourceDictionary(string resourceKey, Collection<ResourceDictionary> resourceDictionaries)
		{
			return resourceDictionaries.Select(d => 
						d.Keys.OfType<string>().Contains(resourceKey) ? d : FindResourceDictionary(resourceKey, d.MergedDictionaries))
						.FirstOrDefault(d => d != null);
		}
	}
}