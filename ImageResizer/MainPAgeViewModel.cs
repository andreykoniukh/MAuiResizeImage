using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using IImage = Microsoft.Maui.Graphics.IImage;
namespace ImageResizer
{
    public class MainPAgeViewModel
    {
        public MainPAgeViewModel()
        {
            _images = new ObservableCollection<ImagesViewModel> ();
        }

        #region Attach Command

        private Command _attachCommand;
        public Command AttachCommand => _attachCommand ?? (_attachCommand = new Command<object>(ExecuteAttachCommand, CanExecuteAttachCommand));

        public async void ExecuteAttachCommand(object parameter)
        {
            var result = await App.Current.MainPage.DisplayActionSheet("Attach image", "Cancel", null, "Camera", "Camera and Resize");
            if (result != null)
            {
                var file = await MediaPicker.Default.CapturePhotoAsync();// CrossMedia.Current.TakePhotoAsync(options);
                var newFile = Path.Combine(FileSystem.CacheDirectory, file.FileName);
                using (var stream = await file.OpenReadAsync())
                using (var newStream = File.OpenWrite(newFile))
                    await stream.CopyToAsync(newStream);
               
                var PhotoPath = newFile;
                var content =       System.IO.File.ReadAllBytes(PhotoPath);

                if (result == "Camera and Resize")
                {
                    IImage newImage = PlatformImage.FromStream(new MemoryStream(content));
                    newImage = newImage.Downsize(newImage.Width-100, newImage.Height-100, true);
                    //  newImage = newImage.Resize(newImage.Height\2, newImage.Width\2, true);
                    content = newImage.AsBytes();
                }
                _images.Add(new ImagesViewModel { Name=Guid.NewGuid().ToString() ,Source= ImageSource.FromStream(() => new MemoryStream(content))

                });
                OnPropertyChanged(nameof(Images));
            }
        }

        public bool CanExecuteAttachCommand(object parameter)
        {
            //TODO: Implement CanExecuteAttachCommand
            return true;
        }

        #endregion

        #region Images Property

        private ObservableCollection<ImagesViewModel> _images;

        public ObservableCollection<ImagesViewModel> Images
        {
            get { return _images; }
            set
            {
                if (_images != value)
                {
                    _images = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion  
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
        {

        }
    }
   
}
