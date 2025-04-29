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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF_Medical_Inventory_Managment_Systemm.Views.Notification
{
    /// <summary>
    /// Interaction logic for ToastNotification.xaml
    /// </summary>
    public partial class ToastNotification : Window
    {
        public ToastNotification(string message)
        {
            InitializeComponent();
            MessageTextBlock.Text = message;

            Loaded += (s, e) =>
            {
                var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
                this.BeginAnimation(OpacityProperty, fadeIn);

                var timer = new System.Windows.Threading.DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(3)
                };
                timer.Tick += (s2, e2) =>
                {
                    timer.Stop();
                    var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(300));
                    fadeOut.Completed += (s3, e3) => this.Close();
                    this.BeginAnimation(OpacityProperty, fadeOut);
                };
                timer.Start();
            };
        }

        public static void ShowToast(string message)
        {
            var toast = new ToastNotification(message);

            var workingArea = SystemParameters.WorkArea;
            toast.Left = workingArea.Right - toast.Width - 20;
            toast.Top = workingArea.Bottom - toast.Height - 20;

            toast.Show();
        }
    }
}
