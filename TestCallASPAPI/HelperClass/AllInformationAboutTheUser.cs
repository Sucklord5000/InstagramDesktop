using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCallASPAPI.HelperClass
{
    public class AllInformationAboutTheUser
    {
        public ObservableCollection<Model.MediaInfo> GetPhotoUser { get; set; }
        public int CountFollowers { get; set; }
        public int CountFollowing { get; set; }
        public int CountPublish { get; set; }
        public string SourceImage { get; set; }
        public string UserName { get; set; } 
        public string FullName { get; set; }
    }
}
