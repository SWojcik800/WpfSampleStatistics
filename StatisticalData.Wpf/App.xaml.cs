using Microsoft.Extensions.DependencyInjection;
using StatisticalData.Infrastructure;
using System;
using System.Windows;

namespace StatisticalData.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            
            services.AddScoped<MainWindow>();
            services.AddSingleton<IStatisticalDataAccessor, StatisticalDataAccessor>(); 

            _serviceProvider = services.BuildServiceProvider();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
