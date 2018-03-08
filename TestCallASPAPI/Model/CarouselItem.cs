using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TestCallASPAPI.Model
{
    public class CarouselItem : INotifyPropertyChanged
    {
        public string SourceUrl;
        public string GetSourceUrl
        {
            get
            {
                return this.SourceUrl;
            }
            set
            {
                this.SourceUrl = value;
                OnPropertyChanged(nameof(GetSourceUrl));
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
