using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace TestCallASPAPI.Model
{
    public class MediaInfo : INotifyPropertyChanged, IDisposable
    {
        private System.Collections.ObjectModel.ObservableCollection<String> Source;
        private int CountLikes;
        private int CountComments;
        private String Location;
        private String Date;
        private String PictureKey;
        private Boolean Visibility;
        private Stream Stream;
        private BitmapImage Image;
        private bool Like;

        public bool IsLike
        {
            get
            {
                return this.Like;
            }
            set
            {
                this.Like = value;
                OnPropertyChanged(nameof(IsLike));
            }
        }
        public BitmapImage GetBitmapImage
        {
            get
            {
                return this.Image;
            }
            set
            {
                this.Image = value;
                OnPropertyChanged(nameof(GetBitmapImage));
            }
        }

        public Stream GetStreamImage
        {
            get
            {
                return this.Stream;
            }
            set
            {
                this.Stream = value;
            }
        }
        public async Task<BitmapImage> GetImage(String url)
        {
            BitmapImage image = new BitmapImage();
            WebClient client = new WebClient();
            var buffer = await client.DownloadDataTaskAsync(new Uri(url));
            GetStreamImage = new MemoryStream(buffer);
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.None;
            image.StreamSource = GetStreamImage;
            image.EndInit();
            image.Freeze();
            client.Dispose();       
            return image;
        }
        public bool GetVisibility
        {
            get
            {
                return this.Visibility;
            }
            set
            {
                this.Visibility = value;
                OnPropertyChanged(nameof(GetVisibility));
            }
        }

        public string GetPictureKey
        {
            get
            {
                return this.PictureKey;
            }
            set
            {
                this.PictureKey = value;
                OnPropertyChanged(nameof(GetPictureKey));
            }
        }
        public MediaInfo()
        {
            this.Source = new System.Collections.ObjectModel.ObservableCollection< string>();
        }
        public string GetLocation
        {
            get
            {
                return this.Location;
            }
            set
            {
                this.Location = value;
                OnPropertyChanged(nameof(GetLocation));
            }
        }


        public int GetCountLikes
        {
            get
            {
                return this.CountLikes;
            }
            set
            {
                this.CountLikes = value;
                OnPropertyChanged(nameof(GetCountLikes));
            }
        }
        public int GetCountComments
        {
            get
            {
                return this.CountComments;
            }
            set
            {
                this.CountComments = value;
                OnPropertyChanged(nameof(GetCountComments));
            }
        }
        public System.Collections.ObjectModel.ObservableCollection<string> GetSourceImage
        {
            get
            {
                return this.Source;
            }
            set
            {
                this.Source = value;
                OnPropertyChanged(nameof(GetSourceImage));
            }
        }
        public string GetDate
        {
            get
            {
                return this.Date;
            }
            set
            {
                this.Date = value;
                OnPropertyChanged(nameof(GetDate));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public void Dispose()
        {
            if (GetStreamImage != null)
            {
                GetStreamImage.Close();
                GetStreamImage.Dispose();
                GetStreamImage = null;
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
            }
        }
    }
}
