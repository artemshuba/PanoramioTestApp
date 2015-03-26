using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Command;
using PanoramioLib;
using PanoramioTestApp.Collection;
using PanoramioTestApp.Controls.Maps;
using PanoramioTestApp.Extensions;
using PanoramioTestApp.Helpers;
using PanoramioTestApp.Services;

namespace PanoramioTestApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private PhotosCollection _photos;
        private bool _showPhotos;
        private DependencyObject _mapAreaPin;
        private CancellationTokenSource _photosCancellationToken = new CancellationTokenSource();

        #region Commands

        /// <summary>
        /// Go to photo view
        /// </summary>
        public RelayCommand<PanoramioPhoto> GoToPhotoViewCommand { get; private set; }

        /// <summary>
        /// Calls on map tap
        /// </summary>
        public RelayCommand<MapTappedEventArgs> MapTapCommand { get; private set; }

        #endregion

        /// <summary>
        /// Reference to map control
        /// </summary>
        public MapView MapControl { get; set; }

        /// <summary>
        /// Photos
        /// </summary>
        public PhotosCollection Photos
        {
            get { return _photos; }
            set
            {
                Set(ref _photos, value);
            }
        }

        /// <summary>
        /// Show photos
        /// </summary>
        public bool ShowPhotos
        {
            get { return _showPhotos; }
            set { Set(ref _showPhotos, value); }
        }

        public MainViewModel()
        {
            RegisterTasks("location", "photos");

            InitializeCommands();
        }

        public override void Activate(NavigationMode navigationMode)
        {
            if (navigationMode == NavigationMode.New)
            {
                Load();
            }
        }

        private void InitializeCommands()
        {
            MapTapCommand = new RelayCommand<MapTappedEventArgs>(args =>
            {
                MapControl.SetPushpinLocation(args.Location, _mapAreaPin);
                MapControl.SetView(args.Location, MapControl.Zoom);

                CancelPhotos();
                LoadPhotosForLocation(args.Location, _photosCancellationToken.Token);
            });

            GoToPhotoViewCommand = new RelayCommand<PanoramioPhoto>(photo =>
            {
                NavigateToPage("/PhotoView", new Dictionary<string, object>()
                {
                    {"photo", photo},
                    {"photos", _photos.ToList()}
                });
            });
        }

        private async void Load()
        {
            TaskStarted("location", "Определение местоположения");

            var location = await GeoHelper.GetCurrentLocation();
            _mapAreaPin = MapControl.Create(location, new Point(0.5, 0.5));

            TaskFinished("location");

            MapControl.SetView(location, 15);
            LoadPhotosForLocation(location, _photosCancellationToken.Token);
        }

        private async void LoadPhotosForLocation(BasicGeoposition location, CancellationToken token)
        {
            ShowPhotos = true;
            TaskStarted("photos");

            try
            {
                var photos = await DataService.GetPhotos(location);
                if (!token.IsCancellationRequested)
                {
                    if (photos == null || photos.Photos.IsNullOrEmpty())
                    {
                        TaskError("photos", "Нет фотографий для указанной области");
                    }
                    else
                        Photos = new PhotosCollection(photos.Photos) { Location = location };
                }
            }
            catch (Exception ex)
            {
                LoggingService.Log(ex);

                TaskError("photos", "Не удалось загрузить фотографии");
            }

            TaskFinished("photos");
        }

        private void CancelPhotos()
        {
            _photosCancellationToken.Cancel();

            Debug.WriteLine("Photos cancelled");

            _photosCancellationToken = new CancellationTokenSource();
        }

        private void NavigateToPage(string page, Dictionary<string, object> parameters = null)
        {
            Type type;
            if (page.StartsWith("/MainView"))
                type = Type.GetType("PanoramioTestApp.MainPage", false);
            else
                type = Type.GetType("PanoramioTestApp.View." + page.Substring(1), false);
            if (type == null)
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
                return;
            }

            if (typeof(Page).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
            {
                var frame = (Frame)Window.Current.Content;
                frame.Navigate(type, parameters);
            }
            else throw new Exception("Unable to navigate to page " + page);
        }
    }
}
