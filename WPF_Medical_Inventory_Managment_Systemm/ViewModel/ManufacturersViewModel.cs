using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_Medical_Inventory_Managment_Systemm.Models;
using WPF_Medical_Inventory_Managment_Systemm.Services;

namespace WPF_Medical_Inventory_Managment_Systemm.ViewModels
{
    public class ManufacturersViewModel : INotifyPropertyChanged
    {
        private readonly ManufacturersApiService _manufacturerService;
        private ManufacturersDTO _selectedManufacturer = new ManufacturersDTO();

        // Command fields
        private RelayCommand _loadManufacturersCommand;
        private RelayCommand _saveManufacturerCommand;
        private RelayCommand _newManufacturerCommand;
        private RelayCommand _deleteManufacturerCommand;
        private RelayCommand _getManufacturerByIdCommand;

        private int _searchId;

        public ObservableCollection<ManufacturersDTO> Manufacturers { get; } = new ObservableCollection<ManufacturersDTO>();

        public ManufacturersDTO SelectedManufacturer
        {
            get => _selectedManufacturer;
            set
            {
                _selectedManufacturer = value;
                OnPropertyChanged();
                DeleteManufacturerCommand?.RaiseCanExecuteChanged();
            }
        }

        public int SearchId
        {
            get => _searchId;
            set
            {
                _searchId = value;
                OnPropertyChanged();
                GetManufacturerByIdCommand?.RaiseCanExecuteChanged(); // Important to re-evaluate button state
            }
        }

        // Command properties
        public RelayCommand LoadManufacturersCommand => _loadManufacturersCommand ??=
            new RelayCommand(async () => await LoadManufacturersAsync());

        public RelayCommand SaveManufacturerCommand => _saveManufacturerCommand ??=
            new RelayCommand(async () => await SaveManufacturerAsync());

        public RelayCommand NewManufacturerCommand => _newManufacturerCommand ??=
            new RelayCommand(() => SelectedManufacturer = new ManufacturersDTO());

        public RelayCommand DeleteManufacturerCommand => _deleteManufacturerCommand ??=
            new RelayCommand(async () => await DeleteManufacturerAsync(SelectedManufacturer.Id),
                             () => SelectedManufacturer?.Id > 0);

        public RelayCommand GetManufacturerByIdCommand => _getManufacturerByIdCommand ??=
            new RelayCommand(async () => await GetManufacturerByIdAsync(),
                             () => SearchId > 0);

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ManufacturersViewModel(ManufacturersApiService manufacturerService)
        {
            _manufacturerService = manufacturerService;
            LoadManufacturersCommand.Execute(null);
        }

        public async Task LoadManufacturersAsync()
        {
            try
            {
                var manufacturers = await _manufacturerService.GetAllManufacturersAsync();
                Manufacturers.Clear();
                foreach (var manufacturer in manufacturers)
                {
                    Manufacturers.Add(manufacturer);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading manufacturers: {ex.Message}");
            }
        }

        public async Task SaveManufacturerAsync()
        {
            try
            {
                if (SelectedManufacturer.Id == 0)
                {
                    await _manufacturerService.AddManufacturerAsync(SelectedManufacturer);
                }
                else
                {
                    await _manufacturerService.UpdateManufacturerAsync(SelectedManufacturer.Id, SelectedManufacturer);
                }
                await LoadManufacturersAsync();
                SelectedManufacturer = new ManufacturersDTO();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving manufacturer: {ex.Message}");
            }
        }

        public async Task DeleteManufacturerAsync(int id)
        {
            try
            {
                await _manufacturerService.DeleteManufacturerAsync(id);
                await LoadManufacturersAsync();
                SelectedManufacturer = new ManufacturersDTO();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting manufacturer: {ex.Message}");
            }
        }

        private async Task GetManufacturerByIdAsync()
        {
            try
            {
                var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(SearchId);
                if (manufacturer != null)
                {
                    SelectedManufacturer = manufacturer;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Manufacturer with ID {SearchId} not found.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error fetching manufacturer by ID: {ex.Message}");
            }
        }
    }
}




//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Runtime.CompilerServices;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using WPF_Medical_Inventory_Managment_Systemm.Models;
//using WPF_Medical_Inventory_Managment_Systemm.Services;

//namespace WPF_Medical_Inventory_Managment_Systemm.ViewModels
//{
//    public class ManufacturersViewModel : INotifyPropertyChanged
//    {
//        private readonly ManufacturersApiService _manufacturerService;
//        private ManufacturersDTO _selectedManufacturer = new ManufacturersDTO();

//        // Command fields
//        private RelayCommand _loadManufacturersCommand;
//        private RelayCommand _saveManufacturerCommand;
//        private RelayCommand _newManufacturerCommand;
//        private RelayCommand _deleteManufacturerCommand;

//        public ObservableCollection<ManufacturersDTO> Manufacturers { get; } = new ObservableCollection<ManufacturersDTO>();

//        public ManufacturersDTO SelectedManufacturer
//        {
//            get => _selectedManufacturer;
//            set
//            {
//                _selectedManufacturer = value;
//                OnPropertyChanged();
//                DeleteManufacturerCommand?.RaiseCanExecuteChanged();
//            }
//        }

//        // Command properties
//        public RelayCommand LoadManufacturersCommand => _loadManufacturersCommand ??=
//            new RelayCommand(async () => await LoadManufacturersAsync());

//        public RelayCommand SaveManufacturerCommand => _saveManufacturerCommand ??=
//            new RelayCommand(async () => await SaveManufacturerAsync());

//        public RelayCommand NewManufacturerCommand => _newManufacturerCommand ??=
//            new RelayCommand(() => SelectedManufacturer = new ManufacturersDTO());

//        public RelayCommand DeleteManufacturerCommand => _deleteManufacturerCommand ??=
//            new RelayCommand(async () => await DeleteManufacturerAsync(SelectedManufacturer.Id),
//                             () => SelectedManufacturer?.Id > 0);

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }

//        public ManufacturersViewModel(ManufacturersApiService manufacturerService)
//        {
//            _manufacturerService = manufacturerService;
//            LoadManufacturersCommand.Execute(null);
//        }

//        public async Task LoadManufacturersAsync()
//        {
//            try
//            {
//                var manufacturers = await _manufacturerService.GetAllManufacturersAsync();
//                Manufacturers.Clear();
//                foreach (var manufacturer in manufacturers)
//                {
//                    Manufacturers.Add(manufacturer);
//                }
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error loading manufacturers: {ex.Message}");
//            }
//        }

//        public async Task SaveManufacturerAsync()
//        {
//            try
//            {
//                if (SelectedManufacturer.Id == 0)
//                {
//                    await _manufacturerService.AddManufacturerAsync(SelectedManufacturer);
//                }
//                else
//                {
//                    await _manufacturerService.UpdateManufacturerAsync(SelectedManufacturer.Id, SelectedManufacturer);
//                }
//                await LoadManufacturersAsync();
//                SelectedManufacturer = new ManufacturersDTO();
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error saving manufacturer: {ex.Message}");
//            }
//        }

//        public async Task DeleteManufacturerAsync(int id)
//        {
//            try
//            {
//                await _manufacturerService.DeleteManufacturerAsync(id);
//                await LoadManufacturersAsync();
//                SelectedManufacturer = new ManufacturersDTO();
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error deleting manufacturer: {ex.Message}");
//            }
//        }
//    }
//}
