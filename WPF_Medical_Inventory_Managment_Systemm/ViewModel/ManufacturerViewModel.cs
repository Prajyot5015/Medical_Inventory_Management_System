using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_Medical_Inventory_Managment_Systemm.Models;
using WPF_Medical_Inventory_Managment_Systemm.Services;
using WPF_Medical_Inventory_Managment_Systemm.Helpers;

namespace WPF_Medical_Inventory_Managment_Systemm.ViewModel
{
    public class ManufacturerViewModel : INotifyPropertyChanged
    {
        private readonly ManufacturerService _service;

        public ObservableCollection<Manufacturer> Manufacturers { get; } = new();

        private Manufacturer _selectedManufacturer = new Manufacturer();
        public Manufacturer SelectedManufacturer
        {
            get => _selectedManufacturer;
            set
            {
                if (_selectedManufacturer != value)
                {
                    _selectedManufacturer = value;
                    OnPropertyChanged();
                    ((RelayCommand)UpdateCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        public ManufacturerViewModel()
        {
            _service = new ManufacturerService(App.HttpClient);

            LoadCommand = new RelayCommand(async () => await LoadAsync());
            AddCommand = new RelayCommand(async () => await AddAsync());
            UpdateCommand = new RelayCommand(async () => await UpdateAsync(), () => SelectedManufacturer?.Id > 0);
            DeleteCommand = new RelayCommand(async () => await DeleteAsync(), () => SelectedManufacturer?.Id > 0);
        }

        public async Task LoadAsync()
        {
            Manufacturers.Clear();
            var manufacturers = await _service.GetManufacturerAsync();
            foreach (var manufacturer in manufacturers)
                Manufacturers.Add(manufacturer);
        }

        public async Task AddAsync()
        {
            if (string.IsNullOrWhiteSpace(SelectedManufacturer.Name))
            {
                MessageBox.Show("Manufacturer name cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            await _service.AddManufacturerAsync(SelectedManufacturer);
            SelectedManufacturer = new Manufacturer(); // reset
            OnPropertyChanged(nameof(SelectedManufacturer));
            await LoadAsync();
        }

        public async Task UpdateAsync()
        {
            if (string.IsNullOrWhiteSpace(SelectedManufacturer.Name))
            {
                MessageBox.Show("Manufacturer name cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            await _service.UpdateManufacturerAsync(SelectedManufacturer);
            await LoadAsync();
        }

        public async Task DeleteAsync()
        {
            var result = MessageBox.Show($"Are you sure you want to delete '{SelectedManufacturer.Name}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                await _service.DeleteManufacturerAsync(SelectedManufacturer.Id);
                SelectedManufacturer = new Manufacturer(); // reset
                OnPropertyChanged(nameof(SelectedManufacturer));
                await LoadAsync();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
