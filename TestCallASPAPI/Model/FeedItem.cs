using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace TestCallASPAPI.Model
{
    public class FeedItem : INotifyPropertyChanged
    {
        private ObservableCollection<Comment> Comment = 
            new ObservableCollection<Model.Comment>();
        private Human human;
        private int countLike;
        private string image;

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

        public string Image
        {
            get
            {
                return this.image;
            }
            set
            {
                this.image = value;
                OnPropertyChanged(nameof(Image));
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
