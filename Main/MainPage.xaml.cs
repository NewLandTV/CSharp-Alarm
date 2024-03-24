using System;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.Notifications;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Main
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            SetWindowSize(800, 600);
            SetWindowTitle("Alarm");

            alarmButton.Click += OnAlarmButtonClick;
        }

        private void SetWindowSize(double width, double height)
        {
            double dpi = (double)DisplayInformation.GetForCurrentView().LogicalDpi;

            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            Size windowSize = new Size(width * 96 / dpi, height * 96 / dpi);

            ApplicationView.PreferredLaunchViewSize = windowSize;

            Window.Current.Activate();

            ApplicationView.GetForCurrentView().TryResizeView(windowSize);
        }

        private void SetWindowTitle(string title)
        {
            ApplicationView.GetForCurrentView().Title = title;
        }

        private void OnAlarmButtonClick(object sender, RoutedEventArgs e)
        {
            ShowAlarm("입력된 내용", $"Hello, World!\nIt's now {DateTime.Now:yyyy-MM-dd HH:mm:ss}.\n{inputField.Text}", 5);

            inputField.Text = string.Empty;
        }

        private void ShowAlarm(string title, string content, int duration)
        {
            ToastNotifier notifier = ToastNotificationManager.CreateToastNotifier();
            XmlDocument document = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

            XmlNodeList nodeList = document.GetElementsByTagName("text");

            nodeList.Item(0).AppendChild(document.CreateTextNode(title));
            nodeList.Item(1).AppendChild(document.CreateTextNode(content));
            document.SelectSingleNode("/toast");

            XmlElement element = document.CreateElement("audio");

            element.SetAttribute("src", "ms_winsoundevent:Notification.SMS");

            ToastNotification notification = new ToastNotification(document);

            notification.ExpirationTime = DateTime.Now.AddSeconds(duration);

            notifier.Show(notification);
        }
    }
}
