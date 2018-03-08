using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class InstaPanel : UserControl
    {
        public static readonly DependencyProperty HomeProperty = DependencyProperty.Register("Home",
            typeof(ICommand), typeof(InstaPanel), new PropertyMetadata(null));

        public InstaPanel()
        {
            InitializeComponent();
        }
        public ICommand Home
        {
            get
            {
                return (ICommand)GetValue(HomeProperty);
            }
            set
            {
                SetValue(HomeProperty, value);
            }
        }
    }
}
