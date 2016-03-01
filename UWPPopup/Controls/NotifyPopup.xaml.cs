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
            this.Width = Window.Current.Bounds.Width;
            this.Height = Window.Current.Bounds.Height;
            m_Popup.Child = this;
            this.Loaded += NotifyPopup_Loaded; ;
            this.Unloaded += NotifyPopup_Unloaded; ;
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

        private void NotifyPopup_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbNotify.Text = m_TextBlockContent;
            this.sbOut.BeginTime = this.m_ShowTime;
            this.sbOut.Begin();
            this.sbOut.Completed += SbOut_Completed;
            Window.Current.SizeChanged += Current_SizeChanged; ;
        }

        private void SbOut_Completed(object sender, object e)
        {
            this.m_Popup.IsOpen = false;
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            this.Width = e.Size.Width;
            this.Height = e.Size.Height;
        }

        private void NotifyPopup_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
        }
    }
}
