using Omnia.Pie.Supervisor.Shell.Utilities;
using Omnia.Pie.Vtm.Devices.Interface.Entities;
using Omnia.Pie.Vtm.Framework.Base;

namespace Omnia.Pie.Supervisor.Shell.ViewModels
{
	public class MediaUnitViewModel : BindableBase
	{
		MediaUnit model;
		public MediaUnit Model
		{
			get { return model; }
			set
			{
				model = value;
				Bill = Model == null ? "" : Model.Value != 0 ? $"{Model.Currency} {Model.Value}" : "-";
				TypeString = Model?.Type.TrimEnd("CASSETTE");
                CurrencyTitle = "Currency: " + Model.Currency;
                CountTitle = "Count: " + model.TotalCount;
			}
		}

		public string Bill { get; private set; }
		public string TypeString { get; private set; }
        public string CurrencyTitle { get; set; }
        public string CountTitle { get; set; }
        public string Added { get; set; }
	}
}
