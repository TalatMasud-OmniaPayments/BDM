using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Omnia.PIE.VTA.Views
{
    /// <summary>
    /// Interaction logic for PagedItemControl.xaml
    /// </summary>
    public partial class PagedItemControl : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedItemControl"/> class.
        /// </summary>
        public PagedItemControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>
        /// The items source.
        /// </value>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// The items source property
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(PagedItemControl), new PropertyMetadata(null, OnItemsSourceChanged));

        /// <summary>
        /// Called when [items source changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        public static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (PagedItemControl)d;
            control.SubscribeNotifyCollectionChanged(e.OldValue, e.NewValue);
            control.RefreshInternalItems(reset: true);
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>
        /// The selected item.
        /// </value>
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// The selected item property
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(PagedItemControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selection active.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is selection active; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelectionActive
        {
            get { return (bool)GetValue(IsSelectionActiveProperty); }
            set { SetValue(IsSelectionActiveProperty, value); }
        }

        /// <summary>
        /// The is selection active property
        /// </summary>
        public static readonly DependencyProperty IsSelectionActiveProperty =
            DependencyProperty.Register("IsSelectionActive", typeof(bool), typeof(PagedItemControl), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets the item template.
        /// </summary>
        /// <value>
        /// The item template.
        /// </value>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        /// The item template property
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(PagedItemControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        /// <summary>
        /// The page size property
        /// </summary>
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(PagedItemControl), new PropertyMetadata(5, OnPageSizeChanged));

        public static void OnPageSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((PagedItemControl)d).RefreshInternalItems(reset: true);

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        public int CurrentPage
        {
            get { return (int)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register("CurrentPage", typeof(int), typeof(PagedItemControl), new PropertyMetadata(1, OnCurrentPageChanged));

        public static void OnCurrentPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((PagedItemControl)d).RefreshInternalItems();

        #region Navigation

        private ICommand previousPageCommand;
        public ICommand PreviousPageCommand
        {
            get
            {
                if (this.previousPageCommand == null)
                {
                    this.previousPageCommand = new DelegateCommand(
                        GoToPreviousPage,
                        () => CurrentPage > 1 && PageCount > 0);
                }

                return this.previousPageCommand;
            }
        }

        private void GoToPreviousPage()
        {
            if (CurrentPage > 1 && PageCount > 0)
            {
                --CurrentPage;
            }
        }

        private ICommand nextPageCommand;
        public ICommand NextPageCommand
        {
            get
            {
                if (this.nextPageCommand == null)
                {
                    this.nextPageCommand = new DelegateCommand(
                        GoToNextPage,
                        () => CurrentPage < PageCount && PageCount > 0);
                }

                return this.nextPageCommand;
            }
        }

        private void GoToNextPage()
        {
            if (CurrentPage < PageCount && PageCount > 0)
            {
                ++CurrentPage;
            }
        }

        private ICommand firstPageCommand;
        public ICommand FirstPageCommand
        {
            get
            {
                if (this.firstPageCommand == null)
                {
                    this.firstPageCommand = new DelegateCommand(
                        GoToFirstPage,
                        () => CurrentPage != 1 && PageCount > 0);
                }

                return this.firstPageCommand;
            }
        }

        private void GoToFirstPage()
        {
            if (CurrentPage != 1 && PageCount > 0)
            {
                CurrentPage = 1;
            }
        }

        private ICommand lastPageCommand;
        public ICommand LastPageCommand
        {
            get
            {
                if (this.lastPageCommand == null)
                {
                    this.lastPageCommand = new DelegateCommand(
                        GoToLastPage,
                        () => CurrentPage != PageCount && PageCount > 0);
                }

                return this.lastPageCommand;
            }
        }

        /// <summary>
        /// Goes to last page.
        /// </summary>
        private void GoToLastPage()
        {
            if (CurrentPage != PageCount && PageCount > 0)
            {
                CurrentPage = PageCount;
            }
        }

        /// <summary>
        /// Raises the commands can execute changed.
        /// </summary>
        private void RaiseCommandsCanExecuteChanged()
        {
            ((DelegateCommand)PreviousPageCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)NextPageCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)FirstPageCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)LastPageCommand).RaiseCanExecuteChanged();
        }

        #endregion Navigation

        #region Internal

        private int pageCount;
        public int PageCount
        {
            get { return this.pageCount; }
            set
            {
                if (this.pageCount != value)
                {
                    this.pageCount = value;
                    RaisePropertyChanged(nameof(PageCount));
                    RaisePropertyChanged(nameof(IsNavigationPanelAvailable));
                }
            }
        }

        private IEnumerable internalItemsSource;
        public IEnumerable InternalItemsSource
        {
            get { return this.internalItemsSource; }
            set
            {
                if (this.internalItemsSource != value)
                {
                    this.internalItemsSource = value;
                    RaisePropertyChanged(nameof(InternalItemsSource));
                }
            }
        }

        public bool IsNavigationPanelAvailable => PageCount > 1;

        /// <summary>
        /// Subscribes the notify collection changed.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private void SubscribeNotifyCollectionChanged(object oldValue, object newValue)
        {
            var oldCollection = oldValue as INotifyCollectionChanged;
            if (oldCollection != null)
            {
                oldCollection.CollectionChanged -= ItemsSource_CollectionChanged;
            }

            var newCollection = newValue as INotifyCollectionChanged;
            if (newCollection != null)
            {
                newCollection.CollectionChanged += ItemsSource_CollectionChanged;
            }
        }

        /// <summary>
        /// Handles the CollectionChanged event of the ItemsSource control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => RefreshInternalItems(reset: true);

        private bool SuppressRefreshInternalItems { get; set; }
        private void RefreshInternalItems(bool reset = false)
        {
            if (SuppressRefreshInternalItems) return;

            SuppressRefreshInternalItems = true;

            if (reset)
            {
                PageCount = 0;
                CurrentPage = 1;
                InternalItemsSource = null;
            }

            if (ItemsSource != null)
            {
                var items = ItemsSource.Cast<object>();

                if (reset)
                {
                    var count = items.Count();
                    PageCount = Convert.ToInt32(Math.Ceiling((double)count / PageSize));
                }

                InternalItemsSource = items.Skip((CurrentPage - 1) * PageSize).Take(PageSize);
            }

            SuppressRefreshInternalItems = false;

            RaiseCommandsCanExecuteChanged();
        }

        private class DelegateCommand : ICommand
        {
            private Action ExecuteInternal { get; }
            private Func<bool> CanExecuteInternal { get; }

            public DelegateCommand(Action execute, Func<bool> canExecute = null)
            {
                ExecuteInternal = execute;
                CanExecuteInternal = canExecute;
            }

            public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

            #region ICommand

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => CanExecuteInternal?.Invoke() ?? true;

            public void Execute(object parameter) => ExecuteInternal?.Invoke();

            #endregion ICommand
        }

        #endregion Internal

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion INotifyPropertyChanged
    }
}
