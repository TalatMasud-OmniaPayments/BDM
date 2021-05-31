using System;
using System.Windows.Media.Imaging;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Devices.Interface.Entities;

namespace Omnia.Pie.Vtm.Devices.Test.ViewModels
{
	public class ChequeAcceptorViewModel : DeviceViewModel
	{
		public ChequeAcceptorViewModel()
		{
			AcceptCheques = new OperationViewModel<ChequeArray>(async () =>
			{
				var x = new ChequeArray();
				x.Cheques = await Model.AcceptChequesAsync();
				if(x.Cheques != null)
					foreach(var i in x.Cheques)
						if(i.FrontImage == null)
							i.FrontImage = ChequeImageStub;
				return x;
			})
			{
				Id = nameof(Model.AcceptChequesAsync)
			};
			StoreCheques = new OperationViewModel(() => Model.StoreChequesAsync())
			{
				Id = nameof(Model.StoreChequesAsync)
			};
			Cancel = new OperationViewModel(() => Model.CancelAcceptChequesAsync())
			{
				Id = nameof(Model.CancelAcceptChequesAsync)
			};
			RollBackCheques = new OperationViewModel(() => Model.RollbackChequesAsync())
			{
				Id = nameof(Model.RollbackChequesAsync)
			};
			RetractCheques = new OperationViewModel(() => Model.RetractChequesAsync())
			{
				Id = nameof(Model.RetractChequesAsync)
			};
		}

		new IChequeAcceptor Model => (IChequeAcceptor)base.Model;
		readonly BitmapImage ChequeImageStub = new BitmapImage(new Uri("pack://application:,,,/hdfc-bank-cheque.jpg"));

		public bool HasMediaInserted => Model.HasMediaInserted;

		public OperationViewModel<ChequeArray> AcceptCheques { get; private set; }
		public OperationViewModel Cancel { get; private set; }
		public OperationViewModel StoreCheques { get; private set; }
		public OperationViewModel RollBackCheques { get; private set; }
		public OperationViewModel RetractCheques { get; private set; }

		public override void Load()
		{
			base.Load();
			RaisePropertyChanged(nameof(HasMediaInserted));
		}
	}

	public class ChequeArray
	{
		public Cheque[] Cheques { get; set; }
	}
}