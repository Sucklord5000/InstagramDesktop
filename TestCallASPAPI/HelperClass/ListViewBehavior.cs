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
    public class ListViewBehavior : Behavior<ListView>
    {
        private static ListView ListView = null;
        private static ICommand command = null;

        public static readonly DependencyProperty IsEnabledProperty;

        public static void SetIsEnabled(DependencyObject DepObject, string value)
        {
            DepObject.SetValue(IsEnabledProperty, value);
        }

        public static bool GetIsEnabled(DependencyObject DepObject)
        {
            return (bool)DepObject.GetValue(IsEnabledProperty);
        }


        public static readonly DependencyProperty CommandProperty;

        public static void SetCommand(DependencyObject DepObject, ICommand value)
        {
            DepObject.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject DepObject)
        {
            return (ICommand)DepObject.GetValue(CommandProperty);
        }

        static ListViewBehavior()
        {
            IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled",
                                                                typeof(bool),
                                                                typeof(ListViewBehavior),
                                            new UIPropertyMetadata(false, IsFrontTurn));

            CommandProperty = DependencyProperty.RegisterAttached("Command",
                                                                typeof(ICommand),
                                                                typeof(ListViewBehavior));
        }


        private static void IsFrontTurn(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ListView = sender as ListView;

            if (ListView == null)
                return;

            if (e.NewValue is bool && ((bool)e.NewValue) == true)
                ListView.Loaded += new RoutedEventHandler(listViewLoaded);
            else
                ListView.Loaded -= new RoutedEventHandler(listViewLoaded);
        }

        private static void listViewLoaded(object sender, RoutedEventArgs e)
        {
            var scrollViewer = GetFirstChildOfType<ScrollViewer>(ListView);

            if (scrollViewer != null)
                scrollViewer.ScrollChanged += new ScrollChangedEventHandler(scrollViewerScrollChanged);
        }

        private static T GetFirstChildOfType<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject == null)
                return null;

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, i);

                var result = (child as T) ?? GetFirstChildOfType<T>(child);

                if (result != null)
                    return result;
            }

            return null;
        }


        private static void scrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;

            if (scrollViewer != null)
            {
                if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
                {
                    command = GetCommand(ListView);
                    command.Execute(ListView);
                }
            }
        }
    }
}
