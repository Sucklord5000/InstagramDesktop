using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace TestCallASPAPI.HelperClass
{
    public class ScrollViewerBehavior : Behavior<ScrollViewer>
    {
        public static DependencyProperty ScrollEnd = DependencyProperty.RegisterAttached("ScrollEndCommand",
            typeof(ICommand), typeof(ScrollViewerBehavior),
             new PropertyMetadata(null)); 

        public ICommand ScrollEndCommand
        {
            get
            {
                return (ICommand)GetValue(ScrollEnd);
            }
            set
            {
                SetValue(ScrollEnd, value);
            }
        }
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.ScrollChanged += ScrollChangedEvent;
            AssociatedObject.PreviewMouseWheel += PreviewMouseWheel;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.ScrollChanged -= ScrollChangedEvent;
            AssociatedObject.PreviewMouseWheel -= PreviewMouseWheel;
        }
        private void ScrollChangedEvent(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (scrollViewer != null)
                if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
                    ScrollEndCommand?.Execute(scrollViewer);
        }
        private void PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            e2.RoutedEvent = UIElement.MouseWheelEvent;
            AssociatedObject.RaiseEvent(e2);
        }
    }
}
