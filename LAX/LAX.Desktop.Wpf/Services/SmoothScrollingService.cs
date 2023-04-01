﻿using System.Windows.Controls;
using System.Windows.Input;

namespace AiController.Desktop.Wpf.Services
{
    public class SmoothScrollingService
    {
        public void Register(ScrollViewer scrollViewer)
        {
            scrollViewer.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;
        }

        public void UnRegister(ScrollViewer scrollViewer)
        {
            scrollViewer.PreviewMouseWheel -= ScrollViewer_PreviewMouseWheel;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)sender;

            scrollViewer.ScrollToVerticalOffset(
                scrollViewer.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
