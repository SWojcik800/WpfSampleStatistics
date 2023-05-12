using StatisticalData.Infrastructure;
using StatisticalData.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
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

namespace StatisticalData.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IStatisticalDataAccessor _statisticalDataAccessor;
        private ListSortDirection? _sortDirection = ListSortDirection.Descending;
        private string _sortCol = "Id";
        private int _skipCount = 0;
        private int _maxResultCount = 50;
        protected long totalItemsCount = 0;

        public MainWindow(IStatisticalDataAccessor statisticalDataAccessor)
        {
            _statisticalDataAccessor = statisticalDataAccessor;
            InitializeComponent();
            InitializeGridData();
            InitializePreviousBtn();

            Title = "Statistical data grid";

        }

        private void InitializePreviousBtn()
        {
            PreviousBtn.IsEnabled = false;
            PreviousBtn.Style = (Style)FindResource("DisabledButtonStyle");
        }

        protected async Task InitializeGridData()
        {
            
            await RefreshGridItemsAsync();           
        }

        #region Loading data
        private async Task RefreshGridItemsAsync()
        {
            var items = await _statisticalDataAccessor.GetAll();

            totalItemsCount = items.Count;
            TotalCountLabel.Content = $"Total items: {totalItemsCount}";

            if (_sortDirection == System.ComponentModel.ListSortDirection.Descending)
                items = items.OrderByDescending(x => typeof(AreaItem).GetProperty(_sortCol).GetValue(x))
                    .ToList();
            else 
            {
                items = items.OrderBy(x => typeof(AreaItem).GetProperty(_sortCol).GetValue(x))
                    .ToList();
            }

            StatisticsDataGrid.ItemsSource = items.Skip(_skipCount).Take(_maxResultCount).ToList();
            
            await Task.CompletedTask;
        }
        private void StatisticsDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                var row = (AreaItem)e.Row.Item;


                switch (row.LevelId)
                {
                    case 3:
                        e.Row.Background = new SolidColorBrush(Colors.Green);
                        break;
                    case 2:
                        e.Row.Background = new SolidColorBrush(Colors.Red);
                        break;
                    case 1:
                        e.Row.Background = new SolidColorBrush(Colors.Yellow);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        #endregion

        #region Create Update Delete
        private void StatisticsDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {                                               
                 var editedItem = (AreaItem)e.Row.Item;
                _statisticalDataAccessor.Update(editedItem);
            }
        }

        private void StatisticsDataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            var newItem = (AreaItem)e.NewItem;

            if (newItem is not null)
                
            {
                _statisticalDataAccessor.Create(newItem);
                RefreshGridItemsAsync();
            }            
        }

        private void StatisticsDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (StatisticsDataGrid.SelectedItems.Count > 0)
                {
                    
                    var selectedItems = StatisticsDataGrid.SelectedItems.Cast<AreaItem>().ToList();
                    var selectedItemsIds = selectedItems.Select(x => x.Id).ToList();

                    _statisticalDataAccessor.Delete(selectedItemsIds);
                    RefreshGridItemsAsync();
                   
                }
            }
        }

        #endregion

        #region Paging

        private long _maxPageNumber
            => (totalItemsCount+_maxResultCount-1)/_maxResultCount;

        private long _pageNumber
            => (_skipCount / _maxResultCount) + 1;


        private void PreviousBtn_Click(object sender, RoutedEventArgs e)
        {
            if(_skipCount > 0)
            {
                _skipCount -= _maxResultCount;
                PageNumberLabel.Content = $"Page number: {_pageNumber}";

                RefreshGridItemsAsync();
                HandleDisablingButtons();
            }


        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {

            if (_skipCount + _maxResultCount > totalItemsCount)
                return;

            _skipCount += _maxResultCount;
            PageNumberLabel.Content = $"Page number: {_pageNumber}";
            RefreshGridItemsAsync();
            HandleDisablingButtons();


        }

        private async Task HandleDisablingButtons()
        {
            if(_pageNumber == 1 && PreviousBtn.IsEnabled)
            {
                PreviousBtn.IsEnabled = false;
                PreviousBtn.Style = (Style)FindResource("DisabledButtonStyle");
            }

            if (_pageNumber != 1 && !PreviousBtn.IsEnabled)
            {
                PreviousBtn.IsEnabled = true;
                PreviousBtn.ClearValue(Button.StyleProperty);
            }

            if(_pageNumber == _maxPageNumber && NextBtn.IsEnabled)
            {
                NextBtn.IsEnabled = false;
                NextBtn.Style = (Style)FindResource("DisabledButtonStyle");
            }

            if(_pageNumber != _maxPageNumber && !NextBtn.IsEnabled)
            {
                NextBtn.IsEnabled = true;
                NextBtn.ClearValue(Button.StyleProperty);
            }
        }

        #endregion



    }
}
