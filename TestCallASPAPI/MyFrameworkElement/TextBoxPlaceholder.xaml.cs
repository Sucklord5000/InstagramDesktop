using System;
using System.Collections.Generic;
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
    public partial class TextBoxPlaceholder : UserControl
    {
        public static DependencyProperty Text =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(TextBoxPlaceholder),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static DependencyProperty SelectionChanged =
            DependencyProperty.Register("GetEnterText", typeof(string), typeof(TextBoxPlaceholder),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PropertyChangedCallback));
        public TextBoxPlaceholder()
        {
            InitializeComponent();
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
                (d as TextBoxPlaceholder).GetEnterText = e.NewValue.ToString();
        }
        public string GetEnterText
        {
            get
            {
                return GetValue(SelectionChanged) as string;
            }
            set
            {
                if (value.Length == 0)
                    this.SecretTextBlock.Visibility = Visibility.Visible;
                else
                    this.SecretTextBlock.Visibility = Visibility.Collapsed;
                SetValue(SelectionChanged, value);
            }
        }
        public string Placeholder
        {
            get
            {
                return GetValue(Text) as string;
            }
            set
            {
                SetValue(Text, value);
            }
        }
    }
}
