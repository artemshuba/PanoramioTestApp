using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Pickers;
using GalaSoft.MvvmLight.Command;
using PanoramioLib;
using Windows.Web.Http;
using PanoramioTestApp.Helpers;
using PanoramioTestApp.Services;

namespace PanoramioTestApp.ViewModel
{
    public class PhotoViewModel : ViewModelBase
    {
        private PanoramioPhoto _photo;

        #region Commands

        /// <summary>
        /// Share
        /// </summary>
        public RelayCommand ShareCommand { get; private set; }

        /// <summary>
        /// Save
        /// </summary>
        public RelayCommand SaveCommand { get; private set; }

        #endregion

        public PhotoViewModel(PanoramioPhoto photo, List<PanoramioPhoto> allPhotos)
        {
            _photo = photo;

            RegisterTasks("photo");

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ShareCommand = new RelayCommand(Share);

            SaveCommand = new RelayCommand(Save);
        }

        private void Share()
        {
            DataTransferManager.ShowShareUI();
        }

        private async void Save()
        {

            try
            {
                var fileSavePicker = new FileSavePicker();
                fileSavePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                fileSavePicker.FileTypeChoices.Add("Изображение", new List<string>() { ".png", ".jpg" });
                fileSavePicker.SuggestedFileName = Path.GetFileName(_photo.PhotoOriginalFileUrl);

#if WINDOWS_APP
                TaskStarted("photo", "Сохранение");

                var photoStream = await GetPhotoStream();
                var file = await fileSavePicker.PickSaveFileAsync();

                await FileHelper.SaveFile(file, photoStream);
#else
                fileSavePicker.PickSaveFileAndContinue();
#endif
            }
            catch (Exception ex)
            {
                LoggingService.Log(ex);

                TaskError("photo", "Не удалось сохранить фотографию");
            }

            TaskFinished("photo");
        }

        private async Task<Stream> GetPhotoStream()
        {
            var httpClient = new HttpClient();
            var photoStream = await httpClient.GetInputStreamAsync(new System.Uri(_photo.PhotoOriginalFileUrl));
            return photoStream.AsStreamForRead();
        }

#if WINDOWS_PHONE_APP
        public async void ContinueFileSavePicker(FileSavePickerContinuationEventArgs args)
        {
            TaskStarted("photo", "Сохранение");

            try
            {
                var photoStream = await GetPhotoStream();
                await FileHelper.SaveFile(args.File, photoStream);

            }
            catch (Exception ex)
            {
                LoggingService.Log(ex);

                TaskError("photo", "Не удалось сохранить фотографию");

            }

            TaskFinished("photo");
        }
#endif
    }
}
