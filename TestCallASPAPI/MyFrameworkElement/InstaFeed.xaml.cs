using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestCallASPAPI.MyFrameworkElement
{
    public partial class InstaFeed : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty PictureUser =
            DependencyProperty.Register("UserPicture", typeof(string), typeof(InstaFeed),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty Source =
            DependencyProperty.Register("PathImage", typeof(string), typeof(InstaFeed),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty TextBlockText =
            DependencyProperty.Register("UserFullName", typeof(string), typeof(InstaFeed),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty CommentText =
            DependencyProperty.Register("GetComment", typeof(string), typeof(InstaFeed),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty CollectionComment =
            DependencyProperty.Register("ItemsSource", typeof(IList), typeof(InstaFeed),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty CommentUnderPhoto =
            DependencyProperty.Register("GetUserCommentUnderPhoto", typeof(string), typeof(InstaFeed),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty CountLike =
            DependencyProperty.Register("GetCountLike", typeof(int), typeof(InstaFeed),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public static readonly DependencyProperty Send =
            DependencyProperty.Register("SendComment", typeof(ICommand), typeof(InstaFeed));

        public static readonly DependencyProperty Delete =
            DependencyProperty.Register("DeleteCommentCommand", typeof(ICommand), typeof(InstaFeed));

        public static readonly DependencyProperty Index =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(InstaFeed),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private new bool Visibility;
        private HelperClass.RelayCommand Enter;
        private HelperClass.RelayCommand Leave;
        public event PropertyChangedEventHandler PropertyChanged;



        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        public int SelectedIndex
        {
            get
            {
                return Convert.ToInt32(GetValue(Index));
            }
            set
            {
                SetValue(Index, value);
            }
        }
        public ICommand DeleteCommentCommand
        {
            get
            {
                return GetValue(Delete) as ICommand;
            }
            set
            {
                SetValue(Delete, value);
            }
        }
        public HelperClass.RelayCommand MEnter
        {
            get
            {
                return this.Enter ?? (this.Enter = new HelperClass.RelayCommand(obj =>
                {
                    IsVisibility = true;
                }));
            }
        }
        public HelperClass.RelayCommand MLeave
        {
            get
            {
                return this.Leave ?? (this.Leave = new HelperClass.RelayCommand(obj =>
                {
                    IsVisibility = false;
                }));
            }
        }
        public bool IsVisibility
        {
            get
            {
                return this.Visibility;
            }
            set
            {
                this.Visibility = value;
                OnPropertyChanged(nameof(IsVisibility));
            }
        }
        public ICommand SendComment
        {
            get
            {
                 return GetValue(Send) as ICommand;
            }
            set
            {
                SetValue(Send, value);
            }
        }

        public int GetCountLike
        {
            get
            {
               return (int)GetValue(CountLike);
            }
            set
            {
                SetValue(CountLike, value);
            }
        }
        public string GetUserCommentUnderPhoto
        {
            get
            {
                return GetValue(CommentUnderPhoto) as string;
            }
            set
            {
                SetValue(CommentUnderPhoto, value);
            }
        }

        public string UserPicture
        {
            get
            {
               return GetValue(PictureUser) as string;
            }
            set
            {
                SetValue(PictureUser, value);
            }
        }
        public IList ItemsSource
        {
            get
            {
                return GetValue(CollectionComment) as IList;
            }
            set
            {
                SetValue(CollectionComment, value);
            }
        }
        public string GetComment
        {
            get
            {
                return GetValue(CommentText) as string;
            }
            set
            {
                SetValue(CommentText, value);
            }
        }
        public string UserFullName
        {
            get
            {
                return (string)GetValue(TextBlockText);
            }
            set
            {
                SetValue(TextBlockText, value);
            }
        }

        public string PathImage
        {
            get
            {
                return (string)GetValue(Source);
            }
            set
            {
                SetValue(Source, value);
            }
        }
        public InstaFeed()
        {
            InitializeComponent();
            IsVisibility = false;
        }
    }
}
