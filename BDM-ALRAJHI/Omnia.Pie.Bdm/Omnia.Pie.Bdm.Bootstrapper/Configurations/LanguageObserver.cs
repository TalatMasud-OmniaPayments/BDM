namespace Omnia.Pie.Bdm.Bootstrapper.Configurations
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Windows;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Extensions;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Framework.Interface.Reports;

	internal class LanguageObserver : ILanguageObserver
	{
		private const string PrimaryFontResourceKey = "Fonts.Primary";

		private static Dictionary<Language, string> LanguageCultureNameDictionary = new Dictionary<Language, string>
		{
			[Language.English] = "en-US",
			[Language.Arabic] = "ar-AE"
		};

		private MainWindow MainView { get; }
		private IReportsManager ReportsManager { get; }

		public LanguageObserver(MainWindow mainView, IReportsManager reportsManager)
		{
			ReportsManager = reportsManager ?? throw new ArgumentNullException(nameof(reportsManager));
			MainView = mainView ?? throw new ArgumentNullException(nameof(mainView));
		}

		private Language language;
		public Language Language
		{
			get { return language; }
			set
			{
				if (language != value)
				{
					language = value;
					UpdateLanguage(language);
				}
			}
		}

		private void UpdateLanguage(Language language)
		{
			var cultureName = LanguageCultureNameDictionary[language];

			var culture = new CultureInfo(cultureName);
			CultureInfo.CurrentUICulture = culture;
			Properties.Resources.Culture = culture;
            Vtm.Framework.Properties.Resources.Culture = culture; // for Receipts
			ReportsManager.Culture = culture;

			var isRightToLeft = culture.TextInfo.IsRightToLeft;
			MainView.FlowDirection = isRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

			var resourceDictionary = Application.Current.FindResourceDictionaryByKey(PrimaryFontResourceKey);
			resourceDictionary.Remove(PrimaryFontResourceKey);
			resourceDictionary.Add(PrimaryFontResourceKey, resourceDictionary[$"{PrimaryFontResourceKey}.{cultureName}"]);
		}
	}
}