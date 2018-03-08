using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using InstaSharper.API.Builder;
using InstaSharper.Logger;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;

namespace TestCallASPAPI.ModelView
{
    public class MainWindowViewModel : INotifyPropertyChanged, HelperClass.IPageViewModel
    {
        private List<HelperClass.IPageViewModel> NavigationStack;
        private HelperClass.IPageViewModel Page;
        private ModelView.DialogViewModel DialogVm;
        private HelperClass.RelayCommand CloseWindow;

        public HelperClass.RelayCommand WindowClosing
        {
            get
            {
                return this.CloseWindow ?? (this.CloseWindow = new HelperClass.RelayCommand(obj =>
                {
                    var temp = this.NavigationStack[1] as ModelView.MainPageViewModel;
                    temp.Dispose();
                }));
            }
        }
        public MainWindowViewModel()
        {
            this.NavigationStack = new List<HelperClass.IPageViewModel>();
            this.DialogVm = new DialogViewModel(DialogCoordinator.Instance);
            this.NavigationStack.Add(new StartUpPageViewModel(DialogCoordinator.Instance));
            this.NavigationStack.Add(new MainPageViewModel(DialogCoordinator.Instance));
            this.NavigationStack.Add(this.DialogVm);
            this.NavigationStack.Add(new ModelView.PageFollowersOrFollowingViewModel(DialogCoordinator.Instance));
            this.NavigationStack.Add(new ModelView.FeedViewModel());
            HelperClass.Mediator.Registry("ChangePage", Change);
            HelperClass.Mediator.Registry("SendValue", Send);
            StartNewPage = this.NavigationStack[4];
        }

        private void Change(object PageIndex)
        {
            StartNewPage = this.NavigationStack[Convert.ToInt32(PageIndex)];
        }
        private void Send(object sender)
        {
            this.DialogVm.SelectedItem = sender as Model.MediaInfo;
            Change(2);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public HelperClass.IPageViewModel StartNewPage
        {
            get
            {
                return this.Page;
            }
            set
            {
                if (this.Page != value)
                {
                    this.Page = value;
                    OnPropertyChanged("StartNewPage");
                }
            }
        }
    }

}
