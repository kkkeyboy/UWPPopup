using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPPopup.Controls;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace UWPPopup
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.tb.Text = "";
            Button btn = sender as Button;
            switch (btn.Content.ToString())
            {
                case "MessageDialog":
                    ShowMessageDialog();
                    break;

                case "Custom":
                    ShowMessagePopupWindow();
                    break;

                case "Toast":
                    ShowToast();
                    break;

                case "Notify":
                    ShowNotify();
                    break;
            }
        }



        private async void ShowMessageDialog()
        {
            var msgDialog = new Windows.UI.Popups.MessageDialog("我是一个提示内容") { Title = "提示标题" };
            msgDialog.Commands.Add(new Windows.UI.Popups.UICommand("确定", uiCommand => { this.tb.Text = $"您点击了：{uiCommand.Label}"; }));
            msgDialog.Commands.Add(new Windows.UI.Popups.UICommand("取消", uiCommand => { this.tb.Text = $"您点击了：{uiCommand.Label}"; }));
            await msgDialog.ShowAsync();
        }

        private void ShowMessagePopupWindow()
        {
            var msgPopup = new Controls.MessagePopupWindow("我是一个提示内容");
            msgPopup.LeftClick += (s, e) => { this.tb.Text = "您点击了：确定"; };
            msgPopup.RightClick += (s, e) => { this.tb.Text = "您点击了：取消"; };
            msgPopup.ShowWIndow();
        }


        private void ShowToast()
        {
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode("这里是提示内容"));

            var toastNode = toastXml.SelectSingleNode("/toast") as XmlElement;

            toastNode.SetAttribute("duration", "short");
            XmlElement audio = toastXml.CreateElement("audio");
            audio.SetAttribute("silent", "true");
            toastNode.AppendChild(audio);

            ToastNotification toast = new ToastNotification(toastXml);
            var toastTag = "Toast:test";
            toast.Tag = toastTag;
            toast.Dismissed += (s, e) =>
            {
                ToastNotificationManager.History.Remove(toastTag);
            };

            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }


        private void ShowNotify()
        {
            NotifyPopup notifyPopup = new NotifyPopup("提示点东西吧!");
            notifyPopup.Show();
        }
    }
}
