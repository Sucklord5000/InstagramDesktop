using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestCallASPAPI.ModelView
{
    public class PageFollowersOrFollowingViewModel : INotifyPropertyChanged, HelperClass.IPageViewModel
    {
        private ObservableCollection<Model.Human> Collection = 
            new ObservableCollection<Model.Human>();
        private HelperClass.RelayCommand Following;
        private HelperClass.RelayCommand IsVisibleChanged;
        private IDialogCoordinator coordinator;
        private Model.Human SelectedHuman;

        public Model.Human GetSelectedHuman
        {
            get
            {
                return this.SelectedHuman;
            }
            set
            {
                if (value != null)
                {
                    this.SelectedHuman = value;
                    App.User.Clear();
                    App.User.Append(value.UserName);
                    HelperClass.Mediator.Notify("ChangePage", 1);
                    OnPropertyChanged(nameof(GetSelectedHuman));
                }
            }
        }

        public HelperClass.RelayCommand LoadedCommand
        {
            get
            {
                return this.IsVisibleChanged ?? (this.IsVisibleChanged = new HelperClass.RelayCommand(async obj =>
                {                    
                    var result = await this.coordinator.ShowProgressAsync(this, "Подождите...", "", false);
                    result.SetIndeterminate();
                    GetCollection?.Clear();
                    foreach (var it in App.Controller.GetFollowers(App.User.ToString()))
                        GetCollection.Add(it);
                    await result.CloseAsync();
                }));
            }
        }

        public HelperClass.RelayCommand GetFollowing
        {
            get
            {
                return this.Following ?? (this.Following = new HelperClass.RelayCommand(async obj =>
                {
                    await App.api.FollowUserAsync((obj as Model.Human).Url);
                }));
            }
        }
        public PageFollowersOrFollowingViewModel(IDialogCoordinator instance)
        {
            this.coordinator = instance;
        }
           
        public ObservableCollection<Model.Human> GetCollection
        {
            get
            {
                return this.Collection;
            }
            set
            {
                this.Collection = value;
                OnPropertyChanged(nameof(GetCollection));
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
