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
    /// Interaction logic for ConfirmationDialog.xaml
    /// </summary>
    public partial class ConfirmationDialog : UserControl
    {
        public ConfirmationDialog()
        {
            InitializeComponent();
            this.DataContext = this;
        }


        #region Dependency Properties

        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register("IsVisible", typeof(bool), typeof(ConfirmationDialog),
                new PropertyMetadata(false));

        public static readonly DependencyProperty ConfirmUpdateCommandProperty =
            DependencyProperty.Register("ConfirmUpdateCommand", typeof(ICommand), typeof(ConfirmationDialog));

        public static readonly DependencyProperty CancelUpdateCommandProperty =
            DependencyProperty.Register("CancelUpdateCommand", typeof(ICommand), typeof(ConfirmationDialog));

        #endregion

        #region Public Properties

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public ICommand ConfirmUpdateCommand
        {
            get { return (ICommand)GetValue(ConfirmUpdateCommandProperty); }
            set { SetValue(ConfirmUpdateCommandProperty, value); }
        }

        public ICommand CancelUpdateCommand
        {
            get { return (ICommand)GetValue(CancelUpdateCommandProperty); }
            set { SetValue(CancelUpdateCommandProperty, value); }
        }

        #endregion
    }
}
