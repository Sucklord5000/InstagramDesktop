using InstaSharper.Classes;
using InstaSharper.Classes.Models;
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
    public class FeedViewModel : INotifyPropertyChanged, HelperClass.IPageViewModel
    {
        private ObservableCollection<Model.FeedItem> Item;
        private HelperClass.RelayCommand OpenPage;
        private HelperClass.RelayCommand SendComment;
        private HelperClass.RelayCommand DeleteCommand;
        private HelperClass.RelayCommand Got;

        public FeedViewModel()
        {
            this.Item = new ObservableCollection<Model.FeedItem>();
        }

        public HelperClass.RelayCommand GotFocusCommand
        {
            get
            {
                return this.Got ?? (this.Got = new HelperClass.RelayCommand(obj =>
                {
                    Model.FeedItem item = obj as Model.FeedItem;
                }));
            }
        }
        public HelperClass.RelayCommand DeleteCommentsCommand
        {
            get
            {
                return this.DeleteCommand ?? (this.DeleteCommand = new HelperClass.RelayCommand(obj =>
                {
                    var comment = obj as Model.Comment;

                }));
            }
        }
        public HelperClass.RelayCommand SendCommentCommand
        {
            get
            {
                return this.SendComment ?? (this.SendComment = new HelperClass.RelayCommand(obj =>
                {
                    if (!String.IsNullOrEmpty(GetTextComment))
                    {
                        GetItem[GetItem.Count - 1].Comments.Add(new Model.Comment()
                        {
                            UserName = App.session.UserName,
                            Comments = GetTextComment
                        });
                        GetTextComment = String.Empty;
                    }
                }));
            }
        }
        public ObservableCollection<Model.FeedItem> GetItem
        {
            get
            {
                return this.Item;
            }
            set
            {
                this.Item = value;
                OnPropertyChanged(nameof(GetItem));
            }
        }
        public HelperClass.RelayCommand LoadedCommand
        {
            get
            {
                return this.OpenPage ?? (this.OpenPage = new HelperClass.RelayCommand(async obj =>
                {
                    IResult<InstaFeed> Feed = await App.api.GetUserTimelineFeedAsync(PaginationParameters.MaxPagesToLoad(1));
                    foreach (var item in Feed.Value.Medias)
                    {
                        Model.FeedItem feed = new Model.FeedItem();
                        if (!item.IsMultiPost)
                            feed.GetBitmapImage = await feed.GetImage(item.Images[0].URI);
                        else
                            feed.GetBitmapImage = await feed.GetImage(item.Carousel[0].Images[0].URI);
                        feed.Human = new Model.Human()
                        {
                            FullName = item.User.FullName,
                            Image = item.User.ProfilePicture,
                            UserName = item.User.UserName
                        };
                        feed.CountLike = item.LikesCount;
                        feed.GetCommentUnderPhoto = new Model.Comment()
                        {
                            UserName = item.Caption.User.UserName,
                            Comments = item.Caption.Text
                        };
                        if (item.PreviewComments.Count > 0)
                        {
                            foreach (var str in item.PreviewComments)
                            {
                                feed.Comments.Add(new Model.Comment()
                                {
                                    UserName = str.User.UserName,
                                    Comments = str.Text
                                });
                            }
                        }
                        this.Item.Add(feed);
                    }            
                }));
            }
        }
        private string TextComment;
        public string GetTextComment
        {
            get
            {
                return this.TextComment;
            }
            set
            {
                this.TextComment = value;
                OnPropertyChanged(nameof(GetTextComment));
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
