
using InstaSharper.Logger;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using xNet;

namespace TestCallASPAPI.ModelView
{
    public class StartUpPageViewModel : INotifyPropertyChanged, HelperClass.IPageViewModel
    {
        private string Username;
        private string Password;
        private string SecurityCode;
        private HelperClass.RelayCommand Login;
        private HelperClass.RelayCommand Go;
        private bool Flag = true;
        private bool Flag2 = false;
        private IDialogCoordinator coordinator;
        private bool Active = false;
        private bool ButtonVisibility = true;
        public StartUpPageViewModel(IDialogCoordinator instanse)
        {                
            this.Username = String.Empty;
            this.Password = string.Empty;
            this.coordinator = instanse;
        }
        public bool IsVisibility
        {
            get
            {
                return this.ButtonVisibility;
            }
            set
            {
                this.ButtonVisibility = value;
                OnPropertyChanged(nameof(IsVisibility));
            }
        }

        public bool IsActive
        {
            get
            {
                return this.Active;
            }
            set
            {
                this.Active = value;
                OnPropertyChanged(nameof(IsActive));
            }
        }
        public string GetCode
        {
            get
            {
                return this.SecurityCode;
            }
            set
            {
                this.SecurityCode = value;
                OnPropertyChanged(nameof(GetCode));
            }
        }
        public HelperClass.RelayCommand GetGo
        {
            get
            {
                return this.Go ?? (this.Go = new HelperClass.RelayCommand(async obj =>
                {                  
                    var logInResult = await App.api.TwoFactorLoginAsync(GetCode);
                    if (!logInResult.Succeeded)
                        MessageBox.Show($"Неверный код подтверждения", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                        HelperClass.Mediator.Notify("ChangePage", 1);
                }));
            }
        }
       public string GetUserName
       {
            get
            {
                return this.Username;
            }
            set
            {
                this.Username = value;
                OnPropertyChanged(nameof(GetUserName));
            }
       }

        public HelperClass.RelayCommand GetLogin
        {
            get
            {
                return this.Login ?? (this.Login = new HelperClass.RelayCommand(async obj =>
                {
                    App.User.Clear();
                    App.User.Append(GetUserName);
                    App.session = new InstaSharper.Classes.UserSessionData()
                    {
                        UserName = App.User.ToString(),
                        Password = GetPassword
                    };

                    App.api = InstaSharper.API.Builder.InstaApiBuilder.CreateBuilder()
                        .SetUser(App.session)
                        .UseLogger(new DebugLogger(LogLevel.Exceptions))
                        .SetRequestDelay(TimeSpan.FromSeconds(0))
                        .Build();


                    if (!App.api.IsUserAuthenticated)
                    {
                        try
                        {
                            IsActive = true;
                            IsVisibility = false;
                            var res = await App.api.LoginAsync();
                            if (!res.Succeeded)
                            {
                                IsActive = false;
                                IsVisibility = true;
                                MessageBox.Show(res.Info.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else
                            {
                                IsActive = false;
                                IsVisibility = false;
                                HelperClass.Mediator.Notify("ChangePage", 1);
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                    }
                }));
            }
        }
        public bool GetFlag
        {
            get
            {
                return this.Flag;
            }
            set
            {
                this.Flag = value;
                OnPropertyChanged(nameof(GetFlag));
            }
        }
        public bool GetFlag2
        {
            get
            {
                return this.Flag2;
            }
            set
            {
                this.Flag2 = value;
                OnPropertyChanged(nameof(GetFlag2));
            }
        }
        public string GetPassword
        {
            get
            {
                return this.Password;
            }
            set
            {
                this.Password = value;
                OnPropertyChanged(nameof(GetPassword));
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
