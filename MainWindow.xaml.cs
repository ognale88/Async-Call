using BCS.CustomTaskBarNotifier;
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

namespace AsyncCall
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TaskbarNotification ObjNotify = new TaskbarNotification();
        List<string> LstNotifierIds = new List<string>();
        int NotifierId = 1;
        int taskbarHeight;
        int taskbarWidth;

        private const double topOffset = 220;
        private const double leftOffset = 300;



        public MainWindow()
        {
            InitializeComponent();

            taskbarHeight = (int)SystemParameters.PrimaryScreenHeight - (int)SystemParameters.WorkArea.Height;
            taskbarWidth = (int)SystemParameters.PrimaryScreenWidth - (int)SystemParameters.WorkArea.Width;

            AsyncMethod();

            ObjNotify.Height = SystemParameters.PrimaryScreenHeight;
            ObjNotify.Top = SystemParameters.WorkArea.Top - taskbarHeight;
            ObjNotify.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - leftOffset;
            ObjNotify.NotifierClosed += new TaskbarNotification.NotificationClose(Notifications_NotifierClosed);
            ObjNotify.MaximumHeight = (int)SystemParameters.WorkArea.Height;
            ObjNotify.MAX_NOTIFICATIONS = (int)SystemParameters.WorkArea.Height;

            ObjNotify.SetDisplayTime(60);
        }


        void Notifications_NotifierClosed(string notiferId)
        {
            try
            {
                if (LstNotifierIds.Contains(notiferId))
                    LstNotifierIds.Remove(notiferId);
            }
            catch (Exception generalException)
            {
                // Error Catching
            }
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    RTBContent.AppendText( txtInput.Text+ Environment.NewLine );
                }
            } 
            catch (Exception ex)
            {
                //Error Catching
            }
        }

        async void AsyncMethod()
        {
            try
            {
                int SizeVal = await Task.Run(() => ComputeOperation());

                ObjNotify.AddNotification(new BCS.CustomTaskBarNotifier.Notification { RefId = NotifierId.ToString() ,
                                                                                        Title = "Custom Taskbar Notifier "+ NotifierId.ToString(),
                                                                                        ImageUrl = "pack://application:,,,/Resources/notification-icon.png",
                                                                                        Message = "Result Value : " + SizeVal.ToString(),
                                                                                        TitleBackground = new BrushConverter().ConvertFromString("#007ACC") as SolidColorBrush,
                                                                                        ContentBackground = new BrushConverter().ConvertFromString("#F5F5F5") as SolidColorBrush,
                                                                                        ContentForeground = new BrushConverter().ConvertFromString("#213554") as SolidColorBrush,
                                                                                        TitleForeground = new BrushConverter().ConvertFromString("#F5F5F5") as SolidColorBrush,
                                                                                        FontBold = FontWeights.Regular });

                ++NotifierId;
                //RTBContent.AppendText("Result Value is :  " + SizeVal.ToString() + Environment.NewLine);
            }
            catch (Exception ex)
            {
                //Error Catching
            }
        }

        int ComputeOperation()
        {
            int size = 0;
            try
            {
                for (int z = 0; z < 100; z++)
                {
                    for (int i = 0; i < 1000000; i++)
                    {
                        string value = i.ToString();
                        if (value == null)
                        {
                            return 0;
                        }
                        size += value.Length;
                    }
                }
            }
            catch (Exception ex)
            {
                //Error Catching
            }
            return size;
        }

        private void btnStartAsync_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AsyncMethod();

                var sessionName = (Environment.GetEnvironmentVariable("SessionName") ?? "").ToUpper();
                //MessageBox.Show(
                if (sessionName.StartsWith("ICA") || sessionName.StartsWith("RDP"))
                {
                    MessageBox.Show(sessionName.ToString());
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}
