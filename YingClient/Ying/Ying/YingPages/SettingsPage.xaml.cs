using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Ying.Pages
{
    public partial class SettingsPage : Page
    {

        public SettingsPage()
        {
            InitializeComponent();
            BackButton.Click += (s,e) => NavigationService.GoBack();
        }

        private async void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as TabControl).SelectedIndex == 1)
            {
                await _gameDownloadControl.GetVersionListFromNetAsync();
            }
            e.Handled = true;
        }
    }
}
