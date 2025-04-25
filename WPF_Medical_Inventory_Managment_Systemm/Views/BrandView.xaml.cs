using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using WPF_Medical_Inventory_Managment_Systemm.Models;
using WPF_Medical_Inventory_Managment_Systemm.UserControls;
using WPF_Medical_Inventory_Managment_Systemm.ViewModel;

namespace WPF_Medical_Inventory_Managment_Systemm.Views
{
    public partial class BrandView : Page
    {
        private readonly BrandViewModel _viewModel;

        public BrandView()
        {
            InitializeComponent();
            _viewModel = new BrandViewModel();
            DataContext = _viewModel;

            // Initialize popup events
            DeleteConfirmationPopup.ConfirmClicked += OnDeleteConfirmed;
            DeleteConfirmationPopup.CancelClicked += OnDeleteCancelled;

            this.Loaded += BrandView_Loaded;
        }

        private async void BrandView_Loaded(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
                await _viewModel.LoadAsync();
        }
        private void BrandNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is BrandViewModel viewModel)
            {
                viewModel.MarkManualEntry();
            }
        }



        

        private void ShowDeleteConfirmation()
        {
            if (_viewModel?.SelectedBrand == null) return;

            DeleteConfirmationPopup.Title = "Delete Brand";
            DeleteConfirmationPopup.Message = $"Are you sure you want to delete the brand '{_viewModel.SelectedBrand.Name}'?";
            DeleteConfirmationPopup.ConfirmButtonText = "Delete";
            DeleteConfirmationPopup.CancelButtonText = "Cancel";

            // Animation for showing the popup
            var fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2)
            };

            PopupOverlay.BeginAnimation(OpacityProperty, fadeIn);
            PopupOverlay.Visibility = Visibility.Visible;
        }

        private async void OnDeleteConfirmed(object sender, RoutedEventArgs e)
        {
            PopupOverlay.Visibility = Visibility.Collapsed;
            if (_viewModel != null && _viewModel.SelectedBrand != null)
            {
                try
                {
                    await _viewModel.DeleteAsync();
                    ShowSuccessPopup("Success", "Brand deleted successfully!");
                }
                catch (Exception ex)
                {
                    ShowErrorPopup("Error", $"Failed to delete brand: {ex.Message}");
                }
            }
        }


        private void OnDeleteCancelled(object sender, RoutedEventArgs e)
        {
            // Animation for hiding the popup
            var fadeOut = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2)
            };

            fadeOut.Completed += (s, _) => PopupOverlay.Visibility = Visibility.Collapsed;
            PopupOverlay.BeginAnimation(OpacityProperty, fadeOut);
        }

        private void ShowSuccessPopup(string title, string message)
        {
            DeleteConfirmationPopup.Title = title;
            DeleteConfirmationPopup.Message = message;
            DeleteConfirmationPopup.SetMessageType(MessageType.Success);

            // Only show OK button for success messages
            DeleteConfirmationPopup.ConfirmClicked -= OnDeleteConfirmed;
            DeleteConfirmationPopup.ConfirmClicked += (s, e) => HidePopup();

            ShowPopup();
        }

        private void ShowErrorPopup(string title, string message)
        {
            DeleteConfirmationPopup.Title = title;
            DeleteConfirmationPopup.Message = message;
            DeleteConfirmationPopup.SetMessageType(MessageType.Error);

            // Only show OK button for error messages
            DeleteConfirmationPopup.ConfirmClicked -= OnDeleteConfirmed;
            DeleteConfirmationPopup.ConfirmClicked += (s, e) => HidePopup();

            ShowPopup();
        }

        private void ShowPopup()
        {
            var fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2)
            };
            PopupOverlay.BeginAnimation(OpacityProperty, fadeIn);
            PopupOverlay.Visibility = Visibility.Visible;
        }

        private void HidePopup()
        {
            var fadeOut = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2)
            };
            fadeOut.Completed += (s, _) => PopupOverlay.Visibility = Visibility.Collapsed;
            PopupOverlay.BeginAnimation(OpacityProperty, fadeOut);
        }






        // Modify your DataGrid's delete button to use CommandParameter
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Brand brand)
            {
                _viewModel.SelectedBrand = brand;
                ShowDeleteConfirmation();
            }
        }














        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                if (Resources["SlideDownStoryboard"] is Storyboard slideDownStoryboard)
                {
                    // Set the target to the DataGrid's TranslateTransform
                    Storyboard.SetTargetName(slideDownStoryboard, "DataGridTranslateTransform");
                    slideDownStoryboard.Begin();
                }
            }
            catch (Exception ex)
            {
                // Log or handle the error appropriately
                System.Diagnostics.Debug.WriteLine($"Error in DataGrid_Loaded: {ex.Message}");
            }
        }





    }
}
