using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PanoramioTestApp.Controls
{
    public class StatusControl : Control
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(StatusControl), new PropertyMetadata(default(string), OnTextPropertyChanged));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty IsWorkingProperty = DependencyProperty.Register(
            "IsWorking", typeof(bool), typeof(StatusControl), new PropertyMetadata(default(bool), OnIsWorkingPropertyChanged));

        public bool IsWorking
        {
            get { return (bool)GetValue(IsWorkingProperty); }
            set { SetValue(IsWorkingProperty, value); }
        }

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (StatusControl)d;

#if WINDOWS_PHONE_APP
            var statusBar = StatusBar.GetForCurrentView();
            statusBar.ProgressIndicator.Text = (string)e.NewValue;
#else
#endif
        }


        private static void OnIsWorkingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (StatusControl)d;

#if WINDOWS_PHONE_APP
            var statusBar = StatusBar.GetForCurrentView();
            if ((bool)e.NewValue)
                statusBar.ProgressIndicator.ShowAsync();
            else
                statusBar.ProgressIndicator.HideAsync();
#else
            control.Visibility = (bool) e.NewValue ? Visibility.Visible : Visibility.Collapsed;
#endif
        }
    }
}
