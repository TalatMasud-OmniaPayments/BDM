using Microsoft.Practices.EnterpriseLibrary.Logging;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.PIE.VTA.Common;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Omnia.PIE.VTA.Views.Workflow
{
	public partial class Main : Page
	{
		WorkflowItems WorkFlowItems = new WorkflowItems();

		public Main()
		{
			InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				pnlCategories.Children.Clear();

				foreach (var item in WorkFlowItems.ItemsList)
				{
					if (item.ParentId == 0)
					{
						var btnMainItems = new ToggleButton
						{
							Name = "btn" + item.ItemName,
							CommandParameter = item.Id
						};

						var style = Application.Current.FindResource("TransparentIconToggleButton") as Style;
						if (style != null)
							btnMainItems.Style = style;

						var tbCategory = new TextBlock();
						tbCategory.TextAlignment = TextAlignment.Center;
						tbCategory.TextWrapping = TextWrapping.Wrap;
						tbCategory.Text = item.ItemLabel;

						btnMainItems.Content = tbCategory;
						btnMainItems.Height = 55;
						btnMainItems.Width = 140;
						btnMainItems.Margin = new Thickness(5);
						btnMainItems.Click += new RoutedEventHandler(btnMainItem_Click);

						pnlCategories.Children.Add(btnMainItems);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		private void btnMainItem_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				pnlSubItems.Children.Clear();
				//pnlActions.Children.Clear();

				if (GlobalInfo.SelectedWorkflowItem != string.Empty)
				{
					foreach (var ctrl in pnlCategories.Children)
					{
						if (ctrl.GetType() == typeof(ToggleButton))
						{
							var btn = (ToggleButton)ctrl;

							if (btn.Name == GlobalInfo.SelectedWorkflowItem && btn.IsChecked == true)
							{
								GlobalInfo.SelectedWorkflowItem = string.Empty;
								btn.IsChecked = false;
								break;
							}
						}
					}
				}

				if (e.OriginalSource is ToggleButton)
				{
					var btn = e.OriginalSource as ToggleButton;

					if (btn == null)
						return;

					GlobalInfo.SelectedWorkflowItem = btn.Name;

					var parm = 0;
					int.TryParse(btn.CommandParameter.ToString(), out parm);
					var children = WorkFlowItems.ItemsList.Where(x => x.ParentId == parm).ToList();

					foreach (var item in children)
					{
						var btnSubItem = new ToggleButton();
						btnSubItem.Name = "btn" + item.ItemName;
						btnSubItem.CommandParameter = item.CommandType;

						var tbCategory = new TextBlock();
						tbCategory.TextAlignment = TextAlignment.Center;
						tbCategory.TextWrapping = TextWrapping.Wrap;
						tbCategory.Text = item.ItemLabel;
						tbCategory.Margin = new Thickness(5);

						btnSubItem.Content = tbCategory;
						var style = Application.Current.FindResource("TransparentIconToggleButton") as Style;

						if (style != null)
						{
							btnSubItem.Style = style;
						}

						btnSubItem.Height = 55;
						btnSubItem.Width = 160;
						btnSubItem.Margin = new Thickness(5);
						btnSubItem.Click += new RoutedEventHandler(btnSubItem_Click);

						pnlSubItems.Children.Add(btnSubItem);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		private void btnSubItem_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				//pnlActions.Children.Clear();

				if (GlobalInfo.SelectedWorkflowSubItem != string.Empty)
				{
					foreach (var ctrl in pnlSubItems.Children)
					{
						if (ctrl.GetType() == typeof(ToggleButton))
						{
							ToggleButton btn = (ToggleButton)ctrl;

							if (btn.Name == GlobalInfo.SelectedWorkflowSubItem && btn.IsChecked == true)
							{
								GlobalInfo.SelectedWorkflowSubItem = string.Empty;
								btn.IsChecked = false;
								break;
							}
						}
					}
				}

				if (e.OriginalSource is ToggleButton)
				{
					ToggleButton btn = e.OriginalSource as ToggleButton;

					if (btn == null)
						return;

					GlobalInfo.SelectedWorkflowItemAction = btn.Name;

					var action = btn.CommandParameter.ToString();
					var commandType = StatusEnum.BackButtonCode;
					Enum.TryParse(action, out commandType);

					MainWindow.Instance.NavigateWorkflow(commandType);
				}

				//if (e.OriginalSource is ToggleButton)
				//{
				//    ToggleButton btn = e.OriginalSource as ToggleButton;

				//    if (btn == null)
				//        return;

				//    GlobalInfo.SelectedWorkflowSubItem = btn.Name;

				//    var parm = 0;
				//    int.TryParse(btn.CommandParameter.ToString(), out parm);
				//    var item = WorkFlowItems.ItemsList.Where(x => x.Id == parm).FirstOrDefault();

				//    for (int i = 1; i <= 2; i++)
				//    {
				//        ToggleButton btnItemAction = new ToggleButton();
				//        TextBlock tbCategory = new TextBlock();

				//        var style = Application.Current.FindResource("TransparentIconToggleButton") as Style;

				//        if (style != null)
				//        {
				//            btnItemAction.Style = style;
				//        }

				//        tbCategory.TextAlignment = TextAlignment.Center;
				//        tbCategory.TextWrapping = TextWrapping.Wrap;

				//        if (i == 1)
				//        {
				//            btnItemAction.Name = "btnAuthenticateWithDebitCreditCard";
				//            btnItemAction.CommandParameter = item.UseDebitCard;
				//            tbCategory.Text = "Authenticate With Debit/Credit Card";
				//        }
				//        else
				//        {
				//            btnItemAction.Name = "btnAuthenticateWithEmiratesID";
				//            btnItemAction.CommandParameter = item.UseEmiratesId;
				//            tbCategory.Text = "Authenticate With Emirates ID";
				//        }

				//        btnItemAction.Content = tbCategory;
				//        btnItemAction.Height = 55;
				//        btnItemAction.Width = 160;
				//        btnItemAction.Margin = new Thickness(5);
				//        btnItemAction.Click += new RoutedEventHandler(btnAction_Click);

				//        pnlActions.Children.Add(btnItemAction);
				//    }
				//}
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		///// <summary>
		///// Handles the Click event of the btnAction control.
		///// </summary>
		///// <param name="sender">The source of the event.</param>
		///// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
		//private void btnAction_Click(object sender, RoutedEventArgs e)
		//{
		//    if (GlobalInfo.SelectedWorkflowItemAction != string.Empty)
		//    {
		//        foreach (var ctrl in pnlActions.Children)
		//        {
		//            if (ctrl.GetType() == typeof(ToggleButton))
		//            {
		//                ToggleButton btn = (ToggleButton)ctrl;

		//                if (btn.Name == GlobalInfo.SelectedWorkflowItemAction && btn.IsChecked == true)
		//                {
		//                    GlobalInfo.SelectedWorkflowItemAction = string.Empty;
		//                    btn.IsChecked = false;
		//                    break;
		//                }
		//            }
		//        }
		//    }

		//    if (e.OriginalSource is ToggleButton)
		//    {
		//        ToggleButton btn = e.OriginalSource as ToggleButton;

		//        if (btn == null)
		//            return;

		//        GlobalInfo.SelectedWorkflowItemAction = btn.Name;

		//        var action = btn.CommandParameter.ToString();
		//        var commandType = StatusEnum.BackButtonCode;
		//        Enum.TryParse(action, out commandType);

		//        MainWindow.Instance.NavigateWorkflow(commandType);
		//    }
		//}

		/// <summary>
		/// Ends the current session.
		/// </summary>
		public void EndCurrentSession()
		{
			pnlSubItems.Children.Clear();
			//pnlActions.Children.Clear();
			GlobalInfo.SelectedWorkflowSubItem = string.Empty;
			GlobalInfo.SelectedWorkflowItemAction = string.Empty;

			if (GlobalInfo.SelectedWorkflowItem != string.Empty)
			{
				foreach (var ctrl in pnlCategories.Children)
				{
					if (ctrl.GetType() == typeof(ToggleButton))
					{
						ToggleButton btn = (ToggleButton)ctrl;

						if (btn.Name == GlobalInfo.SelectedWorkflowItem && btn.IsChecked == true)
						{
							GlobalInfo.SelectedWorkflowItem = string.Empty;
							btn.IsChecked = false;
							break;
						}
					}
				}
			}
		}
	}
}