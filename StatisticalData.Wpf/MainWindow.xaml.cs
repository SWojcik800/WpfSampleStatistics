using StatisticalData.Infrastructure;
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
            var items = await _statisticalDataAccessor.GetAll();

            StatisticsDataGrid.ItemsSource = items;
            
        }
    }
}
