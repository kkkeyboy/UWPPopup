using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UWPPopup.Controls
{
    public sealed partial class MessagePopupWindow : UserControl
    {
        private Popup m_Popup;
        private string m_TextBlockContent;
        private MessagePopupWindow()
        {
            this.InitializeComponent();
            m_Popup = new Popup();
            MeasurePopupSize();
            m_Popup.Child = this;
            this.Loaded += MessagePopupWindow_Loaded;
            this.Unloaded += MessagePopupWindow_Unloaded;
        }

        private void MeasurePopupSize()
        {
            this.Width = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds.Width;

            double marginTop = 0;
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
                marginTop = Windows.UI.ViewManagement.StatusBar.GetForCurrentView().OccludedRect.Height;
            this.Height = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds.Height;
            this.Margin = new Thickness(0, marginTop, 0, 0);
        }
        public MessagePopupWindow(string showMsg) : this()
        {
            this.m_TextBlockContent = showMsg;
        }
        private void MessagePopupWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbContent.Text = m_TextBlockContent;
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBoundsChanged += MessagePopupWindow_VisibleBoundsChanged;
        }
        private void MessagePopupWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBoundsChanged -= MessagePopupWindow_VisibleBoundsChanged;
        }
        private void MessagePopupWindow_VisibleBoundsChanged(Windows.UI.ViewManagement.ApplicationView sender, object args)
        {
            MeasurePopupSize();
        }




        public void ShowWIndow()
        {
            m_Popup.IsOpen = true;
        }

        private void DismissWindow()
        {
            m_Popup.IsOpen = false;
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            DismissWindow();
            LeftClick?.Invoke(this, e);
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            DismissWindow();
            RightClick?.Invoke(this, e);
        }

        public event EventHandler<RoutedEventArgs> LeftClick;
        public event EventHandler<RoutedEventArgs> RightClick;
    }
}
