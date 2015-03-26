using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace PanoramioTestApp.Controls
{
    public sealed partial class PhotoRibbonControl : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof(object), typeof(PhotoRibbonControl), new PropertyMetadata(default(object)));

        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty PhotoCommandProperty = DependencyProperty.Register(
            "PhotoCommand", typeof(ICommand), typeof(PhotoRibbonControl), new PropertyMetadata(default(ICommand)));

        public ICommand PhotoCommand
        {
            get { return (ICommand)GetValue(PhotoCommandProperty); }
            set { SetValue(PhotoCommandProperty, value); }
        }

        public PhotoRibbonControl()
        {
            this.InitializeComponent();
        }
    }
}
