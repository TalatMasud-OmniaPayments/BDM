namespace Omnia.Pie.Vtm.Framework.Interface.Engine
{
	using Omnia.Pie.Vtm.Framework.Interface.Templates;
	using Omnia.Pie.Vtm.Framework.Properties;
	using RazorEngine.Configuration;
	using RazorEngine.Templating;
	using System;
	using System.Globalization;
	using System.Threading.Tasks;

	internal class ReceiptFormatter : IReceiptFormatter
	{
		private const string LayoutTemplates = "Layout;LayoutOffus;LayoutHeader;UpdateCustomerDetailsLayout;AccountBillPaymentReceiptLayout;CreditCardBillPaymentReceiptLayout;PostpaidCreditCardBillPaymentReceiptLayout;PrepaidCreditCardBillPaymentReceiptLayout;PostpaidAccountBillPaymentReceiptLayout;PrepaidAccountBillPaymentReceiptLayout";
		private const char LayoutTemplatesSeparator = ';';
		private static ReceiptTemplateReader ReceiptTemplateReader { get; } = new ReceiptTemplateReader();
		private static ITemplateServiceConfiguration TemplateServiceConfiguration { get; } = new TemplateServiceConfiguration() { DisableTempFileLocking = true,/*CachingProvider = new DefaultCachingProvider(t => { })*/	};
		private static IRazorEngineService TemplateService { get; } = RazorEngineService.Create(TemplateServiceConfiguration);

		public ReceiptMetadata Metadata { get; set; }

		#region IReceiptFormatter

		public async Task<string> FormatAsync<TReceipt>(TReceipt receipt, ReceiptFormattingOptions formattingOptions = null)
		{
			string result;

			var templateId = receipt.GetType().Name;

			await CompileLayoutTemplatesAsync();
			await CompileTemplateAsync(templateId, typeof(ReceiptModel<TReceipt>));

			CultureInfo culture = null;
			if (formattingOptions?.Culture != null)
			{
				culture = Resources.Culture;
				Resources.Culture = formattingOptions?.Culture;
			}

			try
			{
				var model = new ReceiptModel<TReceipt>(receipt, Metadata,
					isRightToLeft: Resources.Culture?.TextInfo.IsRightToLeft ?? false,
					isMarkupEnabled: formattingOptions?.IsMarkupEnabled ?? true);

				result = TemplateService.Run(templateId, typeof(ReceiptModel<TReceipt>), model);
			}
			finally
			{
				if (culture != null)
				{
					Resources.Culture = culture;
				}
			}

			return result;
		}

		#endregion IReceiptFormatter

		private async Task CompileTemplateAsync(string templateId, Type modelType)
		{
			if (!TemplateService.IsTemplateCached(templateId, modelType))
			{
				var template = await ReceiptTemplateReader.GetTemplateAsync(templateId);

				TemplateService.AddTemplate(templateId, template);
				TemplateService.Compile(templateId, modelType);
			}
		}

		private async Task CompileLayoutTemplatesAsync()
		{
			var layoutTemplates = LayoutTemplates.Split(LayoutTemplatesSeparator);
			foreach (var layoutTemplate in layoutTemplates)
			{
				await CompileTemplateAsync(layoutTemplate, null);
			}
		}
	}
}