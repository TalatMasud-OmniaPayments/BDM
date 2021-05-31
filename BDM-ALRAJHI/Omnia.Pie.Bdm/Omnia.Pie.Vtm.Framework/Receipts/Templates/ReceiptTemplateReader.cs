namespace Omnia.Pie.Vtm.Framework.Interface.Templates
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using System.Threading.Tasks;

	internal class ReceiptTemplateReader
	{
		private const string TemplateNameFormat = "{0}.cshtml";

		public async Task<string> GetTemplateAsync(string templateId)
		{
			if (string.IsNullOrEmpty(templateId)) throw new ArgumentNullException(nameof(templateId));

			var assembly = Assembly.GetExecutingAssembly();
			var templateName = string.Format(TemplateNameFormat, templateId);
			var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(n => n.Contains("." + templateName));

			if (string.IsNullOrEmpty(resourceName)) throw new ArgumentException($"{templateName} doesn't exist in the resources.");

			using (var reader = new StreamReader(assembly.GetManifestResourceStream(resourceName)))
			{
				return await reader.ReadToEndAsync();
			}
		}
	}
}