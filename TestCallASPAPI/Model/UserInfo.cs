using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestCallASPAPI.Model
{
    public class UserInfo : INotifyPropertyChanged
    {
        private string SourcePicture;
        private string UserName;
        private string FullName;
        private int CountFollowers;
        private int CountFollowing;
        private int CountPublish;


        public int GetCountPublish
        {
            get
            {
                return this.CountPublish;
            }
            set
            {
                this.CountPublish = value;
                OnPropertyChanged(nameof(GetCountPublish));
            }
        }

        public int GetCountFollowing
        {
            get
            {
                return this.CountFollowing;
            }
            set
            {
                this.CountFollowing = value;
                OnPropertyChanged(nameof(GetCountFollowing));
            }
        }

        public int GetCountFollowers
        {
            get
            {
                return this.CountFollowers;
            }
            set
            {
                this.CountFollowers = value;
                OnPropertyChanged(nameof(GetCountFollowers));
            }
        }
        public string GetFullName
        {
            get
            {
                return this.FullName;
            }
            set
            {
                this.FullName = value;
                OnPropertyChanged(nameof(GetFullName));
            }
        }
       
        public string GetName
        {
            get
            {
                return this.UserName;
            }
            set
            {
                this.UserName = value;
                OnPropertyChanged(nameof(GetName));
            }
        }
        public string GetSourcePictureUser
        {
            get
            {
                return this.SourcePicture;
            }
            set
            {
                this.SourcePicture = value;
                OnPropertyChanged(nameof(GetSourcePictureUser));
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
