using InstaSharper.Classes.Models;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;


namespace TestCallASPAPI.ModelView
{
    public class DialogViewModel : INotifyPropertyChanged, HelperClass.IPageViewModel
    {
        private string pictureUser;
        private string UserName;
        private int SelectedIndex = 0;
        private ObservableCollection<Model.Comment> Comments;
        private ObservableCollection<Model.Human> MyLikers;
        private Model.Human SelectedItemLike;

        private HelperClass.RelayCommand Loaded;
        private HelperClass.RelayCommand Likes;
        private HelperClass.RelayCommand ClickLike;
        private HelperClass.RelayCommand CloseLike;
        private HelperClass.RelayCommand SelectedTextBox;
        private HelperClass.RelayCommand DeleteComment;
        private HelperClass.RelayCommand SendComment;

        private ObservableCollection<Model.CarouselItem> Carousel;
        public Model.MediaInfo SelectedItem { get; set; }
        private HelperClass.RelayCommand Close;
        private bool VisibilityListLikes = false;
        private bool VisibilityComments = true;
        private bool VisibilityAddCommentText = true;
        private System.Windows.Media.Brush Foreground;
        private string TextComment = String.Empty;
        private IDialogCoordinator coordinator;
        private bool ChangeLikePhoto;
        private HelperClass.SaveLikeCollection LikeCollection { get; set; }

        public bool GetVisibilityAddCommentText
        {
            get
            {
                return this.VisibilityAddCommentText;
            }
            set
            {
                this.VisibilityAddCommentText = value;
                OnPropertyChanged(nameof(GetVisibilityAddCommentText));
            }
        }
        public HelperClass.RelayCommand GetSelectedTextBox
        {
            get
            {
                return this.SelectedTextBox ?? (this.SelectedTextBox = new HelperClass.RelayCommand(obj =>
                {
                      
                }));
            }
        }

        public HelperClass.RelayCommand GetSendComment
        {
            get
            {
                return this.SendComment ?? (this.SendComment = new HelperClass.RelayCommand(obj =>
                {
                    if (GetText.Length > 0)
                    {
                        GetComments.Add(new Model.Comment()
                        {
                            UserName = App.Me.UserName,
                            Comments = GetText,
                            Cross = true
                        });
                        GetText = "";
                        VisibilityAddCommentText = true;
                    }
                }));
            }
        }

        public string GetText
        {
            get
            {
                return this.TextComment;
            }
            set
            {
                this.TextComment = value;
                OnPropertyChanged(nameof(GetText));
            }
        }


        public System.Windows.Media.Brush GetForeground
        {
            get
            {
                return this.Foreground;
            }
            set
            {
                this.Foreground = value;
                OnPropertyChanged(nameof(GetForeground));
            }
        }


        public HelperClass.RelayCommand GetDeleteComment
        {
            get
            {
                return this.DeleteComment ?? (this.DeleteComment = new HelperClass.RelayCommand(async obj =>
                {
                    MessageDialogResult result = await this.coordinator.ShowMessageAsync(this, "", "Удалить комментарий?", MessageDialogStyle.AffirmativeAndNegative);
                    if (result == MessageDialogResult.Affirmative)
                        GetComments.Remove(obj as Model.Comment);
                }));
            }
        }

        public HelperClass.RelayCommand GetCloseLike
        {
            get
            {
                return this.CloseLike ?? (this.CloseLike = new HelperClass.RelayCommand(obj =>
                {
                    GetVisibilityComments = true;
                    GetVisibilityListLikes = false;
                    this.ChangeLikePhoto = this.SelectedItem.IsLike;
                    HelperClass.CacheHelper.RemoveFromCache(App.User + "Like");
                    HelperClass.CacheHelper.SaveTocache(App.User + "Like", this.LikeCollection, DateTime.Now.Add(MemoryCache.Default.PollingInterval));
                }));
            }
        }
        public HelperClass.RelayCommand GetClickLike
        {
            get
            {
                return this.ClickLike ?? (this.ClickLike = new HelperClass.RelayCommand(obj =>
                {
                    if (this.SelectedItem.IsLike)
                    {
                        GetForeground = Brushes.Black;
                        this.SelectedItem.IsLike = false;
                        this.SelectedItem.GetCountLikes--;
                    }
                    else
                    {
                        GetForeground = new SolidColorBrush(Color.FromRgb(237, 73, 86));
                        this.SelectedItem.IsLike = true;
                        this.SelectedItem.GetCountLikes++;
                    }
                }));
            }
        }
        public Model.Human GetSelectedItemLike
        {
            get
            {
                return this.SelectedItemLike;
            }
            set
            {
                this.SelectedItemLike = value;
                App.User.Clear();
                App.User.Append(value.UserName);
                HelperClass.Mediator.Notify("ChangePage", 1);
                OnPropertyChanged(nameof(GetSelectedItemLike));
            }
        }
        public ObservableCollection<Model.Human> GetMyLikers
        {
            get
            {
                return this.MyLikers;
            }
            set
            {
                this.MyLikers = value;
                OnPropertyChanged(nameof(GetMyLikers));
            }
        }
        public bool GetVisibilityComments
        {
            get
            {
                return this.VisibilityComments;
            }
            set
            {
                this.VisibilityComments = value;
                OnPropertyChanged(nameof(GetVisibilityComments));
            }
        }

        public bool GetVisibilityListLikes
        {
            get
            {
                return this.VisibilityListLikes;
            }
            set
            {
                this.VisibilityListLikes = value;
                OnPropertyChanged(nameof(GetVisibilityListLikes));
            }
        }

        public HelperClass.RelayCommand ClosePage
        {
            get
            {
                return this.Close ?? (this.Close = new HelperClass.RelayCommand(async obj =>
                {
                    HelperClass.Mediator.Notify("ChangePage", 1);
                    if (this.ChangeLikePhoto && this.SelectedItem.IsLike == false)
                        await App.Controller.UnPostLike(this.SelectedItem.GetPictureKey);
                    else if (this.ChangeLikePhoto == false && this.SelectedItem.IsLike)
                        await App.Controller.PostLike(this.SelectedItem.GetPictureKey);

                    if (this.LikeCollection != null)
                    {
                        HelperClass.CacheHelper.RemoveFromCache(App.User + "Like");
                        HelperClass.CacheHelper.SaveTocache(App.User + "Like", this.LikeCollection, DateTime.Now.Add(MemoryCache.Default.PollingInterval));
                    }
                    this.SelectedItem = null;
                }));
            }
        }

        public HelperClass.RelayCommand GetLikes
        {
            get
            {
                return this.Likes ?? (this.Likes = new HelperClass.RelayCommand(async obj =>
                {
                    GetMyLikers?.Clear();
                    GetVisibilityComments = false;
                    GetVisibilityListLikes = true;

                    if (HelperClass.CacheHelper.GetFromCache<HelperClass.SaveLikeCollection>(App.User + "Like") == null)
                    {
                        var result = await App.Controller.GetLikes(this.SelectedItem.GetPictureKey);
                        foreach (var it in result.Value)
                        {
                            GetMyLikers.Add(new Model.Human()
                            {
                                FullName = it.FullName,
                                UserName = it.UserName,
                                Image = it.ProfilePicture
                            });
                        }
                        this.LikeCollection = new HelperClass.SaveLikeCollection();
                        this.LikeCollection.Like = new ObservableCollection<Model.Human>(GetMyLikers);
                        HelperClass.CacheHelper.SaveTocache(App.User + "Like", this.LikeCollection, DateTime.Now.Add(MemoryCache.Default.PollingInterval));
                    }
                    else
                    {
                        GetMyLikers.Clear();
                        var RestoreLikeCollection = HelperClass.CacheHelper.GetFromCache<HelperClass.SaveLikeCollection>(App.User + "Like");
                        foreach (var it in RestoreLikeCollection.Like)
                        {
                            GetMyLikers.Add(new Model.Human()
                            {
                                FullName = it.FullName,
                                UserName = it.UserName,
                                Image = it.Image
                            });
                        }
                        if (this.ChangeLikePhoto && this.SelectedItem.IsLike == false)
                        {
                            GetMyLikers.Remove(GetMyLikers.Where(x => x.UserName.Contains(App.Me.UserName)).FirstOrDefault());
                            this.ChangeLikePhoto = false;
                            HelperClass.CacheHelper.RemoveFromCache(App.User + "Like");
                            this.LikeCollection.Like = null;
                            this.LikeCollection.Like = new ObservableCollection<Model.Human>(GetMyLikers);
                            HelperClass.CacheHelper.SaveTocache(App.User + "Like", this.LikeCollection, DateTime.Now.Add(MemoryCache.Default.PollingInterval));
                        }
                        else if (this.ChangeLikePhoto == false && this.SelectedItem.IsLike)
                        {
                            GetMyLikers.Add(new Model.Human()
                            {
                                UserName = App.Me.UserName,
                                FullName = App.Me.FullName,
                                Image = App.Me.Image
                            });
                            this.ChangeLikePhoto = true;
                            HelperClass.CacheHelper.RemoveFromCache(App.User + "Like");
                            this.LikeCollection.Like = null;
                            this.LikeCollection.Like = new ObservableCollection<Model.Human>(GetMyLikers);
                            HelperClass.CacheHelper.SaveTocache(App.User + "Like", this.LikeCollection, DateTime.Now.Add(MemoryCache.Default.PollingInterval));
                        }
                    }
                }));
            }
        }

        public int GetSelectedIndex
        {
            get
            {
                return this.SelectedIndex;
            }
            set
            {
                this.SelectedIndex = value;
                OnPropertyChanged(nameof(GetSelectedIndex));
            }
        }
        public HelperClass.RelayCommand LoadedCommand
        {
            get
            {
                return this.Loaded ?? (this.Loaded = new HelperClass.RelayCommand(async obj =>
                {
                    GetCarouselCollection.Clear();
                    this.ChangeLikePhoto = this.SelectedItem.IsLike;

                    if (this.SelectedItem.IsLike)
                        GetForeground = new SolidColorBrush(Color.FromRgb(237, 73, 86));
                    else
                        GetForeground = Brushes.Black;

                    HelperClass.AllInformationAboutTheUser Reboot =
                        HelperClass.CacheHelper.GetFromCache<HelperClass.AllInformationAboutTheUser>(App.User.ToString());
                    var res = await App.Controller.GetComments(this.SelectedItem.GetPictureKey);

                    GetUserName = Reboot.FullName;
                    GetPictureUser = Reboot.SourceImage;
                    foreach (var it in res.Value?.Comments)
                    {
                        GetComments.Add(new Model.Comment()
                        {
                             Comments = it.Text,
                             UserName = it.User.UserName
                        });
                    }
                    
                    if (this.SelectedItem.GetSourceImage.Count == 1)
                    {
                        GetCarouselCollection.Add(new Model.CarouselItem()
                        {
                            GetSourceUrl = this.SelectedItem.GetSourceImage[0],
                        });
                    }
                    else
                    {
                        foreach (var it in this.SelectedItem.GetSourceImage)
                        {
                            GetCarouselCollection.Add(new Model.CarouselItem()
                            {
                                GetSourceUrl = it
                            });
                        }
                    }
               }));
            }
        }

        public ObservableCollection<Model.CarouselItem> GetCarouselCollection
        {
            get
            {
                return this.Carousel;
            }
            set
            {
                this.Carousel = value;
                OnPropertyChanged(nameof(GetCarouselCollection));
            }
        }
        public DialogViewModel(IDialogCoordinator instanse)
        {
            this.Comments = new ObservableCollection<Model.Comment>();
            this.Carousel = new ObservableCollection<Model.CarouselItem>();
            GetMyLikers = new ObservableCollection<Model.Human>();
            GetForeground = Brushes.Black;
            this.coordinator = instanse;
        }

        public DialogViewModel()
        {

        }
        public ObservableCollection<Model.Comment> GetComments
        {
            get
            {
                return this.Comments;
            }
            set
            {
                this.Comments = value;
                OnPropertyChanged(nameof(GetComments));
            }
        }
        public string GetUserName
        {
            get
            {
                return this.UserName;
            }
            set
            {
                this.UserName = value;
                OnPropertyChanged(nameof(GetUserName));
            }
        }

        public string GetPictureUser
        {
            get
            {
                return this.pictureUser;
            }
            set
            {
                this.pictureUser = value;
                OnPropertyChanged(nameof(GetPictureUser));
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
