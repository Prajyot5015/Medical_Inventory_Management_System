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

namespace WPF_Medical_Inventory_Managment_Systemm.Views.Popups
{
    /// <summary>
    /// Interaction logic for DeleteConfirmationPopup.xaml
    /// </summary>
    public partial class DeleteConfirmationPopup : UserControl
    {
        public DeleteConfirmationPopup()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ConfirmCommandProperty =
            DependencyProperty.Register("ConfirmCommand", typeof(ICommand), typeof(DeleteConfirmationPopup));

        public ICommand ConfirmCommand
        {
            get => (ICommand)GetValue(ConfirmCommandProperty);
            set => SetValue(ConfirmCommandProperty, value);
        }

        public static readonly DependencyProperty CancelCommandProperty =
            DependencyProperty.Register("CancelCommand", typeof(ICommand), typeof(DeleteConfirmationPopup));

        public ICommand CancelCommand
        {
            get => (ICommand)GetValue(CancelCommandProperty);
            set => SetValue(CancelCommandProperty, value);
        }

        public static readonly DependencyProperty PopupVisibilityProperty =
            DependencyProperty.Register("PopupVisibility", typeof(Visibility), typeof(DeleteConfirmationPopup));

        public Visibility PopupVisibility
        {
            get => (Visibility)GetValue(PopupVisibilityProperty);
            set => SetValue(PopupVisibilityProperty, value);
        }
    }

}
