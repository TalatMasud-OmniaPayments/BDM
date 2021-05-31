using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.ChequeDeposit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omnia.Pie.Vtm.Devices.Interface.Entities;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using System.Windows.Input;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Windows;

namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels.ChequeDeposit
{
	public class ChequeDepositConfirmationViewModel : ExpirableBaseViewModel, IChequeDepositConfirmationViewModel
	{
		public Account SelectedAccount { get; set; }
		public Cheque[] DepositedCheques { get; set; }
		public int TotalCheques { get; set; }
		public string ChequeDate { get; set; }
		public string ChequeAmount { get; set; }
		public Visibility DateVisibile { get; set; } = Visibility.Collapsed;
		public Visibility DaysVisible { get; set; } = Visibility.Collapsed;
		private ICommand _rotateCommand;
		public ICommand RotateCommand
		{
			get
			{
				if (_rotateCommand == null)
				{
					_rotateCommand = new DelegateCommand<object>(fun);
				}

				return this._rotateCommand;
			}
		}

		private void fun(object ind)
		{
			Cheque chq = DepositedCheques.Where(x => x.MediaId == int.Parse(ind.ToString())).FirstOrDefault();
			chq = GetTransformedCheque(chq, chq.ChequeImageTransform);
			DepositedCheques[int.Parse(ind.ToString()) - 1] = chq;
			OnPropertyChanged(new PropertyChangedEventArgs("DepositedCheques"));
		}

		public Cheque GetTransformedCheque(Cheque chq, RotateTransform ChequeImageTransform)
		{
			//var needTransform = ChequeImageTransform != null && Math.Abs(ChequeImageTransform.Angle - 180) < 1;
			//if (!needTransform)
			//{
			//	return chq;
			//}
			//else
			//{
			//	chq.ChequeImageTransform.Angle = Math.Abs(ChequeImageTransform.Angle - 180);
			//}
			Logger.Write("Transforming image");
			Logger.Write("Image angle:" + ChequeImageTransform.Angle);
			using (var ms = new MemoryStream())
			{
				JpegBitmapEncoder encoder = new JpegBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(chq.FrontImage));
				encoder.QualityLevel = 100;
				encoder.Save(ms);

				ms.Position = 0;
				BitmapImage bi = new BitmapImage();
				bi.BeginInit();
				bi.StreamSource = ms;
				bi.CacheOption = BitmapCacheOption.OnLoad;

				if (ChequeImageTransform.Angle == 0)
				{
					bi.Rotation = Rotation.Rotate180;
					chq.ChequeImageTransform.Angle = 180;
				}
				else
				{
					bi.Rotation = Rotation.Rotate0;
					chq.ChequeImageTransform.Angle = 0;
				}
				Logger.Write("Image angle after rotation:" + bi.Rotation);
				bi.EndInit();
				bi.Freeze();
				chq.FrontImage = bi;
				return chq;
			}
		}
		public void Dispose()
		{

		}
	}
}
