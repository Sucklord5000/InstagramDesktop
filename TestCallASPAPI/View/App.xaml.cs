using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestCallASPAPI
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static InstaSharper.API.IInstaApi api;
        public static InstaSharper.Classes.UserSessionData session;
        public static StringBuilder User = new StringBuilder();
        public static Model.Human Me { get; set; } = new Model.Human();
        public static HelperClass.ApiController Controller { get; set; }
        = new HelperClass.ApiController();
    }
}
