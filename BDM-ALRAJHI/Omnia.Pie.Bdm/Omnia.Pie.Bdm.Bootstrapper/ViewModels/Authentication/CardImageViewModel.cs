namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.ServicesNdc.Interface.Entities;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class CardImageListViewModel : ExpirableBaseViewModel, ICardImageListViewModel
	{
		public CardImageListViewModel()
		{

		}

		public List<NdcCard> Cards { get; set; }

		private NdcCard _selectedCard;
		[Required(ErrorMessageResourceType = typeof(Properties.Resources), ErrorMessageResourceName = nameof(Properties.Resources.ValidationRequired))]
		public NdcCard SelectedCard
		{
			get { return _selectedCard; }
			set { SetProperty(ref _selectedCard, value); }
		}

		public void Dispose()
		{

		}
	}
}