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

            // Clear previous event handlers to avoid duplicates
            DeleteConfirmationPopup.ConfirmClicked -= OnDeleteConfirmed;
            DeleteConfirmationPopup.CancelClicked -= OnDeleteCancelled;

            // Add fresh handlers
            DeleteConfirmationPopup.ConfirmClicked += OnDeleteConfirmed;
            DeleteConfirmationPopup.CancelClicked += OnDeleteCancelled;

            DeleteConfirmationPopup.Title = "Delete Brand";
            DeleteConfirmationPopup.Message = $"Are you sure you want to delete '{_viewModel.SelectedBrand.Name}'?";
            DeleteConfirmationPopup.ConfirmButtonText = "Delete";
            DeleteConfirmationPopup.CancelButtonText = "Cancel";
            DeleteConfirmationPopup.SetMessageType(MessageType.Confirmation);

            ShowPopup();
        }

        private async void OnDeleteConfirmed(object sender, RoutedEventArgs e)
        {
            try
            {
                HidePopup();
                await _viewModel.DeleteAsync();
            }
            catch (Exception ex)
            {
                _viewModel.SnackBarMessageQueue.Enqueue($"Error: {ex.Message}");
            }
        }

        private void OnDeleteCancelled(object sender, RoutedEventArgs e)
        {
            HidePopup();
        }

        private void ShowPopup()
        {
            PopupOverlay.Visibility = Visibility.Visible;
            var fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2)
            };
            PopupOverlay.BeginAnimation(OpacityProperty, fadeIn);
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

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Brand brand)
            {
                // Make sure we're using the same instance from the collection
                var itemToDelete = _viewModel.Brands.FirstOrDefault(b => b.Id == brand.Id);
                if (itemToDelete != null)
                {
                    _viewModel.SelectedBrand = itemToDelete;
                    ShowDeleteConfirmation();
                }
            }
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Resources["SlideDownStoryboard"] is Storyboard slideDownStoryboard)
                {
                    Storyboard.SetTargetName(slideDownStoryboard, "DataGridTranslateTransform");
                    slideDownStoryboard.Begin();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in DataGrid_Loaded: {ex.Message}");
            }
        }
    }
}