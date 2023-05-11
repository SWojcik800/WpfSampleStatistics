using StatisticalData.Infrastructure;
using StatisticalData.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
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

        public MainWindow(IStatisticalDataAccessor statisticalDataAccessor)
        {
            _statisticalDataAccessor = statisticalDataAccessor;
            InitializeComponent();
            InitializeGridData();


        }

        protected async Task InitializeGridData()
        {
            
            await RefreshGridItemsAsync();           
        }

        
        private async Task RefreshGridItemsAsync()
        {
            var items = await _statisticalDataAccessor.GetAll();
    
            StatisticsDataGrid.ItemsSource = items.Take(20).ToList();
        }

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

            if(newItem is not null)
                _statisticalDataAccessor.Create(newItem)
                    .ContinueWith((x) => RefreshGridItemsAsync());
        }

        private void StatisticsDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (StatisticsDataGrid.SelectedItems.Count > 0)
                {
                    
                    var selectedItems = StatisticsDataGrid.SelectedItems.Cast<AreaItem>().ToList();
                    var selectedItemsIds = selectedItems.Select(x => x.Id).ToList();

                    _statisticalDataAccessor.Delete(selectedItemsIds)
                        .ContinueWith((x) => RefreshGridItemsAsync());
                }
            }
        }
    }
}
