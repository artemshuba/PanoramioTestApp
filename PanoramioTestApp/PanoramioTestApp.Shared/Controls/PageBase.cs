using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Command;
using PanoramioTestApp.Common;
using PanoramioTestApp.ViewModel;

namespace PanoramioTestApp.Controls
{
    public class PageBase : Page
    {
        private NavigationHelper _navigationHelper;

        public NavigationHelper NavigationHelper
        {
            get { return _navigationHelper; }
        }

        public PageBase()
        {
            _navigationHelper = new NavigationHelper(this);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var vm = DataContext as ViewModelBase;
            if (vm != null)
            {
                vm.Activate(e.NavigationMode);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            var vm = DataContext as ViewModelBase;
            if (vm != null)
            {
                vm.Deactivate();
            }
        }
    }
}
