namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels.Products
{
	using System;
	using System.Windows.Input;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.DelegateCommand;
	using System.ComponentModel.DataAnnotations;
	using Omnia.Pie.Bdm.Bootstrapper.Properties;

	public class SendMoreInfoViewModel : BaseViewModel, ISendMoreInfoViewModel
	{
		public SendMoreInfoViewModel()
		{

		}

		public string _headingMessage;
		public string HeadingMessage
		{
			get
			{
				_headingMessage = Resources.ResourceManager.GetString($"LabelHeadingMessage{_productType}", Resources.Culture);
				return _headingMessage;
			}
			set { SetProperty(ref _headingMessage, value); }
		}

		public string _detailMessage;
		public string DetailMessage
		{
			get
			{
				_detailMessage = Resources.ResourceManager.GetString($"LabelDetailMessage{_productType}", Resources.Culture);
				return _detailMessage;
			}
			set { SetProperty(ref _detailMessage, value); }
		}

		public string _productType = "HomeFinance";
		public string ProductType
		{
			get { return _productType; }
			set { SetProperty(ref _productType, value); }
		}

		public string Source
		{
			get
			{
				return $"/Resources/Images/Products/{ProductType}.png";
			}
		}

		private string _email;
		[EmailAddress(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.ValidationInvalidEmailAddress))]
		public string Email
		{
			get { return _email; }
			set { SetProperty(ref _email, value); }
		}

		private string _mobile;
		[MaxLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.ValidationInvalidMobileNo))]
		[MinLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.ValidationInvalidMobileNo))]
		public string Mobile
		{
			get { return _mobile; }
			set { SetProperty(ref _mobile, value); }
		}

		public Action<string, string> SendMoreInfoAction { get; set; }

		private ICommand _productSelectedCommand;
		public ICommand SendMoreInfoCommand
		{
			get
			{
				if (_productSelectedCommand == null)
					_productSelectedCommand = new DelegateCommand(
						() => SendProductInfo(),
						() => SendInfoCommand()
				);

				return _productSelectedCommand;
			}
		}

		private void SendProductInfo()
		{
			if (Validate())
			{
				if (!string.IsNullOrEmpty(Mobile))
					Mobile = $"971{Mobile?.Substring(1)}";

				SendMoreInfoAction?.Invoke(Email, Mobile);
			}
		}

		private bool SendInfoCommand()
		{
			return (!string.IsNullOrEmpty(_email) || !string.IsNullOrEmpty(_mobile));
		}

		public void Dispose()
		{

		}
	}
}