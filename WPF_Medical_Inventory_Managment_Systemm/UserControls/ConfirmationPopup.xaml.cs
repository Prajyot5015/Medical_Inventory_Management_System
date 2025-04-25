using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;

namespace WPF_Medical_Inventory_Managment_Systemm.UserControls
{
    public partial class ConfirmationPopup : UserControl
    {
        public event RoutedEventHandler ConfirmClicked;
        public event RoutedEventHandler CancelClicked;

            private Action<bool> _callback;


        public ConfirmationPopup()
        {
            InitializeComponent();
            SetMessageType(MessageType.Confirmation);
        }

        public string Title
        {
            get => TitleText.Text;
            set => TitleText.Text = value;
        }

        public string Message
        {
            get => MessageText.Text;
            set => MessageText.Text = value;
        }

        public string ConfirmButtonText
        {
            get => ConfirmButton.Content.ToString();
            set => ConfirmButton.Content = value;
        }

        public string CancelButtonText
        {
            get => CancelButton.Content.ToString();
            set => CancelButton.Content = value;
        }

        public void SetMessageType(MessageType type)
        {
            switch (type)
            {
                case MessageType.Confirmation:
                    MessageIcon.Kind = PackIconKind.QuestionMark;
                    MessageIcon.Foreground = Brushes.DodgerBlue;
                    ConfirmButton.Visibility = Visibility.Visible;
                    CancelButton.Visibility = Visibility.Visible;
                    ConfirmButtonText = "Confirm";
                    CancelButtonText = "Cancel";
                    break;

                case MessageType.Success:
                    MessageIcon.Kind = PackIconKind.CheckCircle;
                    MessageIcon.Foreground = Brushes.LimeGreen;
                    ConfirmButton.Visibility = Visibility.Visible;
                    CancelButton.Visibility = Visibility.Collapsed;
                    ConfirmButtonText = "OK";
                    break;

                case MessageType.Error:
                    MessageIcon.Kind = PackIconKind.Error;
                    MessageIcon.Foreground = Brushes.OrangeRed;
                    ConfirmButton.Visibility = Visibility.Visible;
                    CancelButton.Visibility = Visibility.Collapsed;
                    ConfirmButtonText = "OK";
                    break;

                case MessageType.Warning:
                    MessageIcon.Kind = PackIconKind.Alert;
                    MessageIcon.Foreground = Brushes.Gold;
                    ConfirmButton.Visibility = Visibility.Visible;
                    CancelButton.Visibility = Visibility.Visible;
                    ConfirmButtonText = "Proceed";
                    CancelButtonText = "Cancel";
                    break;
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmClicked?.Invoke(this, e);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CancelClicked?.Invoke(this, e);
        }
    }

    public enum MessageType
    {
        Confirmation,
        Success,
        Error,
        Warning
    }
}