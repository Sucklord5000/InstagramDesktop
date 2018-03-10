using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;
using System.Net;

namespace TestCallASPAPI.Model
{
    public class FeedItem : INotifyPropertyChanged
    {
        private ObservableCollection<Comment> Comment = 
            new ObservableCollection<Model.Comment>();
        private Human human;
        private Comment UnderPhoto;
        private int countLike;
        private Stream Stream;
        private BitmapImage Image;


        public Comment GetCommentUnderPhoto
        {
            get
            {
                return this.UnderPhoto;
            }
            set
            {
                this.UnderPhoto = value;
                OnPropertyChanged(nameof(GetCommentUnderPhoto));
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
                OnPropertyChanged(nameof(GetStreamImage));
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

        public ObservableCollection<Comment> Comments
        {
            get
            {
                return this.Comment;
            }
            set
            {
                this.Comment = value;
                OnPropertyChanged(nameof(Comments));
            }
        }

        public Human Human
        {
            get
            {
                return this.human;
            }
            set
            {
                this.human = value;
                OnPropertyChanged(nameof(Human));
            }
        }

        public int CountLike
        {
            get
            {
                return this.countLike;
            }
            set
            {
                this.countLike = value;
                OnPropertyChanged(nameof(CountLike));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
