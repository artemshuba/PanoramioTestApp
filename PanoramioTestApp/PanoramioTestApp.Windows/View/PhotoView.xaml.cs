using System;
using System.Collections.Generic;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using PanoramioLib;
using PanoramioTestApp.Controls;
using PanoramioTestApp.ViewModel;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace PanoramioTestApp.View
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class PhotoView : PageBase
    {
        private PhotoViewModel _viewModel;

        public PhotoView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var p = e.Parameter as Dictionary<string, object>;
            if (p != null)
            {
                var photo = (PanoramioPhoto)p["photo"];
                var photos = (List<PanoramioPhoto>)p["photos"];
                FlipView.ItemsSource = photos;
                FlipView.SelectedItem = photo;

                _viewModel = new PhotoViewModel(photo, photos);
                this.DataContext = _viewModel;
                //Image.Source = new BitmapImage(new Uri(photo.PhotoOriginalFileUrl));
                //Image.Width = ImageScrollViewer.ViewportWidth;

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
            var photo = FlipView.SelectedItem as PanoramioPhoto;
            if (photo == null)
                return;

            args.Request.Data.Properties.Title = "Поделиться";
            args.Request.Data.Properties.Description = photo.PhotoTitle;

            if (!string.IsNullOrWhiteSpace(photo.PhotoOriginalFileUrl))
                args.Request.Data.SetBitmap(RandomAccessStreamReference.CreateFromUri(new Uri(photo.PhotoOriginalFileUrl)));

            if (!string.IsNullOrWhiteSpace(photo.PhotoUrl))
                args.Request.Data.SetUri(new Uri(photo.PhotoUrl));
        }

        private void PhotoView_OnLoaded(object sender, RoutedEventArgs e)
        {
            //Image.Width = ImageScrollViewer.ViewportWidth;
            //Image.Height = ImageScrollViewer.ViewportHeight;
        }
    }
}
