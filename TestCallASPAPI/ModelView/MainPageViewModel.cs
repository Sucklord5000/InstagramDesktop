using InstaSharper.Classes;
using InstaSharper.Classes.Models;
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
using System.Windows.Controls;
using System.Runtime.Caching;
using System.IO;
using System.Windows.Threading;

namespace TestCallASPAPI.ModelView
{
    public class MainPageViewModel : INotifyPropertyChanged, HelperClass.IPageViewModel, IDisposable
    {
        private ObservableCollection<Model.MediaInfo> MyPhoto;
        private HelperClass.RelayCommand loadedCommand;
        private HelperClass.RelayCommand MouseEnter;
        private HelperClass.RelayCommand MouseLeave;
        private HelperClass.RelayCommand Followers;
        private HelperClass.RelayCommand SelectionChanged;
        private Model.MediaInfo SelectedItem;
        private Model.UserInfo UserInfo;
        private IDialogCoordinator coordinator;
        private bool Completed = false;
        private HelperClass.AllInformationAboutTheUser InformationToCache;
        private HelperClass.RelayCommand Home;
        private int AllPublichCount { get; set; }
        public HelperClass.RelayCommand GetHome
        {
            get
            {
                return this.Home ?? (this.Home = new HelperClass.RelayCommand(async obj =>
                {
                    if (!App.User.ToString().Contains(App.session.UserName))
                    {
                        App.User.Clear();
                        App.User.Append(App.session.UserName);
                        await Start();
                    }
                    
                }));
            }
        }

        public Model.UserInfo GetUserInfo
        {
            get
            {
                return this.UserInfo;
            }
        }
        public HelperClass.RelayCommand GetSelectionChanged
        {
            get
            {
                return this.SelectionChanged ?? (this.SelectionChanged = new HelperClass.RelayCommand(async obj =>
                {
                    if (Completed && GetMyPhoto.Count < this.AllPublichCount)
                        await GetUserMedia(App.User.ToString(), GetMyPhoto[GetMyPhoto.Count - 1].GetPictureKey);
                }));
            }
        }
        
        private async Task GetPhoto(string UserName, string LastImageSource = null)
        {
            PaginationParameters p;
            if (LastImageSource != null)
                p = PaginationParameters.MaxPagesToLoad(1).StartFromId(LastImageSource);
            else
                p = PaginationParameters.MaxPagesToLoad(1);

            var user = await App.api.GetUserMediaAsync(UserName, p);

            InstaMediaList Collection = user.Value;

            MessageBox.Show(p.NextId);
            for(int a = 0; a < Collection.Count;a++)
            {
                if (!Collection[a].IsMultiPost)
                {
                    if (Collection[a].Images.Count > 0)
                    {
                        Model.MediaInfo Item = new Model.MediaInfo();
                        Item.GetCountLikes = Collection[a].LikesCount;
                        Item.GetCountComments = Convert.ToInt32(Collection[a].CommentsCount);
                        Item.GetPictureKey = Collection[a].Pk;
                        Item.IsLike = Collection[a].HasLiked;
                        Item.GetBitmapImage = await Item.GetImage(Collection[a].Images[0].URI);
                        Item.GetSourceImage.Add(Collection[a].Images[0].URI);
                        Item.GetDate = Collection[a].DeviceTimeStap.ToLongDateString();
                        Item.GetLocation = Collection[a]?.Location?.Address + " " + Collection[a]?.Location?.City;
                        GetMyPhoto.Add(Item);
                    }
                }
                else
                {
                    InstaCarousel carousel = Collection[a].Carousel;
                    Model.MediaInfo Item = new Model.MediaInfo();
                    Item.GetCountLikes = Collection[a].LikesCount;
                    Item.IsLike = Collection[a].HasLiked;
                    Item.GetCountComments = Convert.ToInt32(Collection[a].CommentsCount);
                    Item.GetPictureKey = Collection[a].Pk;
                    Item.GetBitmapImage = await Item.GetImage(carousel[0].Images[0].URI);
                    Item.GetDate = Collection[a].DeviceTimeStap.ToLongDateString();
                    Item.GetLocation = Collection[a]?.Location?.Address + " " + Collection[a]?.Location?.City;
                    GetMyPhoto.Add(Item);
                    Collection[a].Carousel.ForEach(x =>
                    {
                        Item.GetSourceImage.Add(x.Images[0].URI);
                    });
                }
            }
        }

        public HelperClass.RelayCommand FollowersCommand
        {
            get
            {
                return this.Followers ?? (this.Followers = new HelperClass.RelayCommand(obj =>
                {
                    HelperClass.Mediator.Notify("ChangePage", 3);
                }));
            }
        }

        public HelperClass.RelayCommand GetMouseEnter
        {
            get
            {
                return this.MouseEnter ?? (this.MouseEnter = new HelperClass.RelayCommand(obj =>
                {
                    Model.MediaInfo info = obj as Model.MediaInfo;
                    info.GetVisibility = true;
                }));
            }
        }

        public HelperClass.RelayCommand GetMouseLeave
        {
            get
            {
                return this.MouseLeave ?? (this.MouseLeave = new HelperClass.RelayCommand(obj =>
                {
                    Model.MediaInfo media = obj as Model.MediaInfo;
                    media.GetVisibility = false;
                }));
            }
        }

        public Model.MediaInfo GetSelectedItem
        {
            get
            {
                return this.SelectedItem;
            }
            set
            {
                if (value != null)
                {
                    this.SelectedItem = value;
                    HelperClass.Mediator.Notify("SendValue", value);
                    OnPropertyChanged(nameof(GetSelectedItem));
                }
            }
        }

        private async Task<List<Model.Comment>> GetComments(string Pk)
        {
            IResult<InstaCommentList> col = 
                await App.api.GetMediaCommentsAsync(Pk, PaginationParameters.MaxPagesToLoad(5));
            InstaCommentList GetComments = col.Value;
            List<Model.Comment> TempList = new List<Model.Comment>();
            foreach (var com in GetComments.Comments)
            {
                TempList.Add(new Model.Comment()
                {
                    UserName = com.User.FullName,
                    Comments = com.Text
                });
            }
            return TempList;
        }
        public HelperClass.RelayCommand LoadedCommand
        {
            get
            {
                return this.loadedCommand ?? (this.loadedCommand = new HelperClass.RelayCommand(obj =>
                {
                    //await Start();
                    GetUserInfo.GetSourcePictureUser = "pack://application:,,,/Resources/rob-swire.jpeg";
                    GetUserInfo.GetName = ";k;lk;lk;lk";
                    GetUserInfo.GetFullName = ";lkl;k";

                    Model.MediaInfo l = new Model.MediaInfo();
                    l.GetBitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                    l.GetBitmapImage.BeginInit();
                    l.GetBitmapImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                    l.GetBitmapImage.UriSource = new Uri("pack://application:,,,/Resources/rob-swire.jpeg");
                    l.GetBitmapImage.EndInit();
                    GetMyPhoto.Add(l);
                    GetMyPhoto.Add(l);
                    GetMyPhoto.Add(l);
                    GetMyPhoto.Add(l);
                    GetMyPhoto.Add(l);
                }));

            }
        }

        private async Task Start()
        {
            GetSelectedItem = null;
            this.Completed = false;
            HelperClass.AllInformationAboutTheUser content = 
                HelperClass.CacheHelper.GetFromCache<HelperClass.AllInformationAboutTheUser>(App.User.ToString());

            if (content == null)
            {
                GetMyPhoto.Clear();
                var value = await this.coordinator.ShowProgressAsync(this, "Подождите...", "");
                value.SetIndeterminate();
                IResult<InstaUser> userInfo = await App.api.GetUserAsync(App.User.ToString());

                if (App.Me.UserName == null && App.Me.FullName == null && App.Me.Image == null)
                {
                    App.Me.UserName = userInfo.Value.UserName;
                    App.Me.FullName = userInfo.Value.FullName;
                    App.Me.Image = userInfo.Value.ProfilePicture;
                }
                GetUserInfo.GetName = userInfo.Value.UserName;
                GetUserInfo.GetFullName = userInfo.Value.FullName;
                GetUserInfo.GetSourcePictureUser = userInfo.Value.ProfilePicture;

                await GetUserMedia(App.User.ToString());
                this.Completed = true;
                Tuple<int, int> Count =
                (await App.Controller.GetCountFollowersAndFollowing(App.User.ToString()));
                GetUserInfo.GetCountFollowers = Count.Item1;
                GetUserInfo.GetCountFollowing = Count.Item2;
                this.InformationToCache = new HelperClass.AllInformationAboutTheUser();
                this.InformationToCache.CountFollowers = GetUserInfo.GetCountFollowers;
                this.InformationToCache.CountFollowing = GetUserInfo.GetCountFollowing;
                this.InformationToCache.GetPhotoUser = new ObservableCollection<Model.MediaInfo>(GetMyPhoto);
                this.InformationToCache.UserName = GetUserInfo.GetName;
                this.InformationToCache.FullName = GetUserInfo.GetFullName;
                this.InformationToCache.SourceImage = GetUserInfo.GetSourcePictureUser;
                IResult<InstaMediaList> list = 
                    await App.api.GetUserMediaAsync(App.User.ToString(), PaginationParameters.Empty);
                this.AllPublichCount = list.Value.Count;
                list.Value.Clear();
                GetUserInfo.GetCountPublish = this.AllPublichCount;

                this.InformationToCache.CountPublish = GetUserInfo.GetCountPublish;
                HelperClass.CacheHelper.SaveTocache(App.User.ToString(), this.InformationToCache, DateTime.Now.Add(MemoryCache.Default.PollingInterval));
                await value.CloseAsync();
            }
            else
            {
                HelperClass.AllInformationAboutTheUser Reboot =
                  HelperClass.CacheHelper.GetFromCache<HelperClass.AllInformationAboutTheUser>(App.User.ToString());
                GetMyPhoto.Clear();
                foreach (var it in Reboot.GetPhotoUser)
                    GetMyPhoto.Add(it);
                UserInfo.GetName = Reboot.UserName;
                UserInfo.GetSourcePictureUser = Reboot.SourceImage;
                UserInfo.GetCountFollowing = Reboot.CountFollowing;
                UserInfo.GetFullName = Reboot.FullName;
                UserInfo.GetCountFollowers = Reboot.CountFollowers;
                UserInfo.GetCountPublish = Reboot.CountPublish;
                this.Completed = true;
            }
        }

        private async Task GetUserMedia(string Username, string LastPk = null)
        {
            if (LastPk != null)
                await GetPhoto(Username, LastPk);
            else
                await GetPhoto(Username);
        }
        public MainPageViewModel(IDialogCoordinator instance)
        {
            this.coordinator = instance;
            this.MyPhoto = new ObservableCollection<Model.MediaInfo>();
            this.UserInfo = new Model.UserInfo();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public void Dispose()
        {
            foreach (var it in GetMyPhoto)
            {
                it.Dispose();
            }
            HelperClass.CacheHelper.Dispose();
        }

        public ObservableCollection<Model.MediaInfo> GetMyPhoto
        {
            get
            {
                return this.MyPhoto;
            }
            set                              
            {
                this.MyPhoto = value;
            }
        }
    }
}
