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
	public partial class AuthenticationFlow : Page
	{
		WorkflowItems WorkFlowItems = new WorkflowItems();

		public AuthenticationFlow()
		{
			InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{

		}

		private void ToggleButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var btn = e.OriginalSource as ToggleButton;

				if (btn == null)
					return;

				pnlSubItems.Children.Clear();
				//pnlActions.Children.Clear();

				if (GlobalInfo.SelectedWorkflowItem != string.Empty)
				{
					if (Leads.Name == GlobalInfo.SelectedWorkflowItem && Leads.IsChecked == true)
					{
						GlobalInfo.SelectedWorkflowItem = string.Empty;
						Leads.IsChecked = false;
					}

					//if (Deposits.Name == GlobalInfo.SelectedWorkflowItem && Deposits.IsChecked == true)
					//{
					//	GlobalInfo.SelectedWorkflowItem = string.Empty;
					//	Deposits.IsChecked = false;
					//}
				}

				//if (btn.Name == Deposits.Name)
				//{
				//	GlobalInfo.SelectedWorkflowItem = Deposits.Name;
				//}
				//else
				{
					GlobalInfo.SelectedWorkflowItem = Leads.Name;
				}

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
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		/// <summary>
		/// Handles the Click event of the btnSubItem control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
		private void btnSubItem_Click(object sender, RoutedEventArgs e)
		{
			try
			{
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
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}
	}
}