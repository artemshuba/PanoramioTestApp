using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace PanoramioTestApp.Controls
{
    public sealed partial class ImageControl : UserControl
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof(ImageSource), typeof(ImageControl), new PropertyMetadata(default(ImageSource), OnSourcePropertyChanged));

        private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ImageControl)d;
            var oldBi = e.OldValue as BitmapImage;
            if (oldBi != null)
            {
                oldBi.DownloadProgress -= control.BitmapImageDownloadProgress;
                oldBi.ImageOpened -= control.BitmapImageOpened;
                oldBi.ImageFailed -= control.BitmapImageFailed;
            }

            var bi = e.NewValue as BitmapImage;
            if (bi != null)
            {
                bi.DownloadProgress += control.BitmapImageDownloadProgress;
                bi.ImageOpened += control.BitmapImageOpened;
                bi.ImageFailed += control.BitmapImageFailed;
                control.Image.Source = bi;
            }
        }

        private void BitmapImageOpened(object sender, RoutedEventArgs routedEventArgs)
        {
            var bi = (BitmapImage) sender;
            bi.DownloadProgress -= BitmapImageDownloadProgress;
            bi.ImageOpened -= BitmapImageOpened;
            bi.ImageFailed -= BitmapImageFailed;

            ProgressBar.Visibility = Visibility.Collapsed;

            var s = (Storyboard)Resources["ImageLoadedAnim"];
            s.Begin();
        }

        private void BitmapImageFailed(object sender, RoutedEventArgs routedEventArgs)
        {
            var bi = (BitmapImage)sender;
            bi.DownloadProgress -= BitmapImageDownloadProgress;
            bi.ImageOpened -= BitmapImageOpened;
            bi.ImageFailed -= BitmapImageFailed;

            ProgressBar.Visibility = Visibility.Collapsed;
        }

        private void BitmapImageDownloadProgress(object sender, DownloadProgressEventArgs e)
        {
            ProgressBar.Value = e.Progress;
        }

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }


        public ImageControl()
        {
            this.InitializeComponent();
        }

        private void Timeline_OnCompleted(object sender, object e)
        {
            Placeholder.Visibility = Visibility.Collapsed;
        }
    }
}
