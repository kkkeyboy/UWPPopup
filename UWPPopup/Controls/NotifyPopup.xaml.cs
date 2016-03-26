using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UWPPopup.Controls
{
    public sealed partial class NotifyPopup : UserControl
    {
        private Popup m_Popup;

        private string m_TextBlockContent;
        private TimeSpan m_ShowTime;

        private NotifyPopup()
        {
            this.InitializeComponent();
            m_Popup = new Popup();
            MeasurePopupSize();
            m_Popup.Child = this;
            this.Loaded += NotifyPopup_Loaded;
            this.Unloaded += NotifyPopup_Unloaded;
        }

        public NotifyPopup(string content, TimeSpan showTime) : this()
        {
            this.m_TextBlockContent = content;
            this.m_ShowTime = showTime;
        }

        public NotifyPopup(string content) : this(content, TimeSpan.FromSeconds(2))
        {
        }

        public void Show()
        {
            this.m_Popup.IsOpen = true;
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

        private void NotifyPopup_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbNotify.Text = m_TextBlockContent;
            this.sbOut.BeginTime = this.m_ShowTime;
            this.sbOut.Begin();
            this.sbOut.Completed += SbOut_Completed;
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBoundsChanged += NotifyPopup_VisibleBoundsChanged;
        }

        private void NotifyPopup_VisibleBoundsChanged(Windows.UI.ViewManagement.ApplicationView sender, object args)
        {
            MeasurePopupSize();
        }

        private void SbOut_Completed(object sender, object e)
        {
            this.m_Popup.IsOpen = false;
        }


        private void NotifyPopup_Unloaded(object sender, RoutedEventArgs e)
        {
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBoundsChanged -= NotifyPopup_VisibleBoundsChanged;
        }
    }
}
