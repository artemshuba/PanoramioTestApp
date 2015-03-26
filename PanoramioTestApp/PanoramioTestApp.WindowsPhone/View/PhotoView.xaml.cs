using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using PanoramioLib;
using PanoramioTestApp.Controls;
using PanoramioTestApp.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PanoramioTestApp.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PhotoView : PageBase, IFileSavePickerContinuable
    {
        private PhotoViewModel _viewModel;
        private PanoramioPhoto _photo;

        public PhotoView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var p = e.Parameter as Dictionary<string, object>;
            if (p != null)
            {
                _photo = (PanoramioPhoto)p["photo"];
                Image.Source = new BitmapImage(new Uri(_photo.PhotoOriginalFileUrl));

                _viewModel = new PhotoViewModel(_photo, null);
                this.DataContext = _viewModel;

                DataTransferManager.GetForCurrentView().DataRequested += OnDataRequested;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            DataTransferManager.GetForCurrentView().DataRequested -= OnDataRequested;

        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var photo = _photo;

            args.Request.Data.Properties.Title = photo.PhotoTitle;

            if (!string.IsNullOrWhiteSpace(photo.PhotoOriginalFileUrl))
                args.Request.Data.SetBitmap(RandomAccessStreamReference.CreateFromUri(new Uri(photo.PhotoOriginalFileUrl)));

            if (!string.IsNullOrWhiteSpace(photo.PhotoUrl))
                args.Request.Data.SetUri(new Uri(photo.PhotoUrl));
        }

        private void PhotoView_OnLoaded(object sender, RoutedEventArgs e)
        {
            Image.Width = ImageScrollViewer.ViewportWidth;
            Image.Height = ImageScrollViewer.ViewportHeight;
        }

        public void ContinueFileSavePicker(FileSavePickerContinuationEventArgs args)
        {
            _viewModel.ContinueFileSavePicker(args);
        }
    }
}