using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_Medical_Inventory_Managment_Systemm.Models;
using WPF_Medical_Inventory_Managment_Systemm.Services;

namespace WPF_Medical_Inventory_Managment_Systemm.ViewModels
{
    public class ManufacturersViewModel : INotifyPropertyChanged
    {
        private readonly ManufacturersApiService _manufacturerService;
        private ManufacturersDTO _selectedManufacturer = new ManufacturersDTO();
        private int _searchId;

        // Validation error properties
        public string NameError { get; set; }
        public string AddressError { get; set; }
        public string ContactError { get; set; }

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
                GetManufacturerByIdCommand?.RaiseCanExecuteChanged();
            }
        }

        // Commands
        public RelayCommand LoadManufacturersCommand { get; }
        public RelayCommand SaveManufacturerCommand { get; }
        public RelayCommand NewManufacturerCommand { get; }
        public RelayCommand DeleteManufacturerCommand { get; }
        public RelayCommand GetManufacturerByIdCommand { get; }

        public ManufacturersViewModel(ManufacturersApiService manufacturerService)
        {
            _manufacturerService = manufacturerService;

            LoadManufacturersCommand = new RelayCommand(async () => await LoadManufacturersAsync());
            SaveManufacturerCommand = new RelayCommand(async () => await SaveManufacturerAsync());
            NewManufacturerCommand = new RelayCommand(() => SelectedManufacturer = new ManufacturersDTO());
            DeleteManufacturerCommand = new RelayCommand(
                async () => await DeleteManufacturerAsync(SelectedManufacturer.Id),
                () => SelectedManufacturer?.Id > 0
            );
            GetManufacturerByIdCommand = new RelayCommand(
                async () => await GetManufacturerByIdAsync(),
                () => SearchId > 0
            );

            LoadManufacturersCommand.Execute(null);
        }

        // VALIDATION
        private void ValidateManufacturer()
        {
            NameError = string.IsNullOrWhiteSpace(SelectedManufacturer?.Name)
                ? "Please enter a manufacturer name."
                : null;

            AddressError = string.IsNullOrWhiteSpace(SelectedManufacturer?.Address)
                ? "Please enter an address."
                : null;

            if (string.IsNullOrWhiteSpace(SelectedManufacturer?.ContactDetails))
            {
                ContactError = "Please enter a contact number.";
            }
            else if (!ulong.TryParse(SelectedManufacturer.ContactDetails, out _))
            {
                ContactError = "Contact number must contain digits only.";
            }
            else
            {
                ContactError = null;
            }

            OnPropertyChanged(nameof(NameError));
            OnPropertyChanged(nameof(AddressError));
            OnPropertyChanged(nameof(ContactError));
        }

        private bool HasValidationErrors()
        {
            return !string.IsNullOrEmpty(NameError) ||
                   !string.IsNullOrEmpty(AddressError) ||
                   !string.IsNullOrEmpty(ContactError);
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
            ValidateManufacturer();

            if (HasValidationErrors())
            {
                MessageBox.Show("Some fields are invalid. Please correct the errors and try again.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (SelectedManufacturer.Id == 0)
                {
                    await _manufacturerService.AddManufacturerAsync(SelectedManufacturer);
                    MessageBox.Show("Record Added Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    await _manufacturerService.UpdateManufacturerAsync(SelectedManufacturer.Id, SelectedManufacturer);
                    MessageBox.Show("Record Updated Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                await LoadManufacturersAsync();
                SelectedManufacturer = new ManufacturersDTO();
                SearchId = 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving manufacturer: {ex.Message}");
            }
        }

        public async Task DeleteManufacturerAsync(int id)
        {
            var result = MessageBox.Show("Are you sure you want to delete this manufacturer?", "Confirm Delete",
                                         MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                await _manufacturerService.DeleteManufacturerAsync(id);
                await LoadManufacturersAsync();
                SelectedManufacturer = new ManufacturersDTO();
                MessageBox.Show("Record Deleted Successfully", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    MessageBox.Show($"Manufacturer with ID {SearchId} not found.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error fetching manufacturer by ID: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}






//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Runtime.CompilerServices;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Input;
//using WPF_Medical_Inventory_Managment_Systemm.Models;
//using WPF_Medical_Inventory_Managment_Systemm.Services;

//namespace WPF_Medical_Inventory_Managment_Systemm.ViewModels
//{
//public class ManufacturersViewModel : INotifyPropertyChanged
//{
//    private readonly ManufacturersApiService _manufacturerService;
//    private ManufacturersDTO _selectedManufacturer = new ManufacturersDTO();

//    // Command fields
//    private RelayCommand _loadManufacturersCommand;
//    private RelayCommand _saveManufacturerCommand;
//    private RelayCommand _newManufacturerCommand;
//    private RelayCommand _deleteManufacturerCommand;
//    private RelayCommand _getManufacturerByIdCommand;

//    private int _searchId;

//    public ObservableCollection<ManufacturersDTO> Manufacturers { get; } = new ObservableCollection<ManufacturersDTO>();

//    public ManufacturersDTO SelectedManufacturer
//    {
//        get => _selectedManufacturer;
//        set
//        {
//            _selectedManufacturer = value;
//            OnPropertyChanged();
//            DeleteManufacturerCommand?.RaiseCanExecuteChanged();
//        }
//    }

//    public int SearchId
//    {
//        get => _searchId;
//        set
//        {
//            _searchId = value;
//            OnPropertyChanged();
//            GetManufacturerByIdCommand?.RaiseCanExecuteChanged(); // Important to re-evaluate button state
//        }
//    }

//    // Command properties
//    public RelayCommand LoadManufacturersCommand => _loadManufacturersCommand ??=
//        new RelayCommand(async () => await LoadManufacturersAsync());

//    public RelayCommand SaveManufacturerCommand => _saveManufacturerCommand ??=
//        new RelayCommand(async () => await SaveManufacturerAsync());

//    public RelayCommand NewManufacturerCommand => _newManufacturerCommand ??=
//        new RelayCommand(() => SelectedManufacturer = new ManufacturersDTO());

//    public RelayCommand DeleteManufacturerCommand => _deleteManufacturerCommand ??=
//        new RelayCommand(async () => await DeleteManufacturerAsync(SelectedManufacturer.Id),
//                         () => SelectedManufacturer?.Id > 0);

//    public RelayCommand GetManufacturerByIdCommand => _getManufacturerByIdCommand ??=
//        new RelayCommand(async () => await GetManufacturerByIdAsync(),
//                         () => SearchId > 0);

//    public event PropertyChangedEventHandler PropertyChanged;

//    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
//    {
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//    }

//    public ManufacturersViewModel(ManufacturersApiService manufacturerService)
//    {
//        _manufacturerService = manufacturerService;
//        LoadManufacturersCommand.Execute(null);
//    }

//    public async Task LoadManufacturersAsync()
//    {
//        try
//        {
//            var manufacturers = await _manufacturerService.GetAllManufacturersAsync();
//            Manufacturers.Clear();
//            foreach (var manufacturer in manufacturers)
//            {
//                Manufacturers.Add(manufacturer);
//            }
//        }
//        catch (Exception ex)
//        {
//            System.Diagnostics.Debug.WriteLine($"Error loading manufacturers: {ex.Message}");
//        }
//    }

//    public async Task SaveManufacturerAsync()
//    {
//        try
//        {
//            if (SelectedManufacturer.Id == 0)
//            {
//                await _manufacturerService.AddManufacturerAsync(SelectedManufacturer);
//                MessageBox.Show("Record Added Succesfully");
//            }
//            else
//            {
//                await _manufacturerService.UpdateManufacturerAsync(SelectedManufacturer.Id, SelectedManufacturer);
//                MessageBox.Show("Record update Successfully");

//            }
//            await LoadManufacturersAsync();
//            SelectedManufacturer = new ManufacturersDTO();
//            SearchId = 0;
//        }
//        catch (Exception ex)
//        {
//            System.Diagnostics.Debug.WriteLine($"Error saving manufacturer: {ex.Message}");
//        }
//    }

//    public async Task DeleteManufacturerAsync(int id)
//    {
//        try
//        {
//            await _manufacturerService.DeleteManufacturerAsync(id);
//            await LoadManufacturersAsync();
//            SelectedManufacturer = new ManufacturersDTO();
//        }
//        catch (Exception ex)
//        {
//            System.Diagnostics.Debug.WriteLine($"Error deleting manufacturer: {ex.Message}");
//        }
//    }

//    private async Task GetManufacturerByIdAsync()
//    {
//        try
//        {
//            var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(SearchId);
//            if (manufacturer != null)
//            {
//                SelectedManufacturer = manufacturer;
//            }
//            else
//            {
//                System.Diagnostics.Debug.WriteLine($"Manufacturer with ID {SearchId} not found.");
//            }
//        }
//        catch (Exception ex)
//        {
//            System.Diagnostics.Debug.WriteLine($"Error fetching manufacturer by ID: {ex.Message}");
//        }
//    }
//}

//    public class ManufacturersViewModel : INotifyPropertyChanged
//    {
//        private readonly ManufacturersApiService _manufacturerService;
//        private ManufacturersDTO _selectedManufacturer = new ManufacturersDTO();
//        private int _searchId;

//        public string NameError { get; set; } // For validation

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

//        public int SearchId
//        {
//            get => _searchId;
//            set
//            {
//                _searchId = value;
//                OnPropertyChanged();
//                GetManufacturerByIdCommand?.RaiseCanExecuteChanged();
//            }
//        }

//        // Relay Commands
//        public RelayCommand LoadManufacturersCommand { get; }
//        public RelayCommand SaveManufacturerCommand { get; }
//        public RelayCommand NewManufacturerCommand { get; }
//        public RelayCommand DeleteManufacturerCommand { get; }
//        public RelayCommand GetManufacturerByIdCommand { get; }

//        public ManufacturersViewModel(ManufacturersApiService manufacturerService)
//        {
//            _manufacturerService = manufacturerService;

//            LoadManufacturersCommand = new RelayCommand(async () => await LoadManufacturersAsync());
//            SaveManufacturerCommand = new RelayCommand(async () => await SaveManufacturerAsync());
//            NewManufacturerCommand = new RelayCommand(() => SelectedManufacturer = new ManufacturersDTO());
//            DeleteManufacturerCommand = new RelayCommand(
//                async () => await DeleteManufacturerAsync(SelectedManufacturer.Id),
//                () => SelectedManufacturer?.Id > 0
//            );
//            GetManufacturerByIdCommand = new RelayCommand(
//                async () => await GetManufacturerByIdAsync(),
//                () => SearchId > 0
//            );

//            LoadManufacturersCommand.Execute(null);
//        }

//        // Validation
//        private void ValidateManufacturer()
//        {
//            NameError = string.IsNullOrWhiteSpace(SelectedManufacturer?.Name)
//                ? "Please enter a manufacturer name."
//                : null;

//            OnPropertyChanged(nameof(NameError));
//        }

//        private bool HasValidationErrors()
//        {
//            return !string.IsNullOrEmpty(NameError);
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
//            ValidateManufacturer();

//            if (HasValidationErrors())
//            {
//                MessageBox.Show(NameError, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
//                return;
//            }

//            try
//            {
//                if (SelectedManufacturer.Id == 0)
//                {
//                    await _manufacturerService.AddManufacturerAsync(SelectedManufacturer);
//                    MessageBox.Show("Record Added Successfully", "Successfuly", MessageBoxButton.OK, MessageBoxImage.Information);
//                }
//                else
//                {
//                    await _manufacturerService.UpdateManufacturerAsync(SelectedManufacturer.Id, SelectedManufacturer);
//                    MessageBox.Show("Record Updated Successfully", "Successfuly", MessageBoxButton.OK, MessageBoxImage.Information);
//                }

//                await LoadManufacturersAsync();
//                SelectedManufacturer = new ManufacturersDTO();
//                SearchId = 0;
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error saving manufacturer: {ex.Message}");
//            }
//        }

//        public async Task DeleteManufacturerAsync(int id)
//        {
//            var result = MessageBox.Show("Are you sure you want to delete this manufacturer?", "Confirm Delete",
//                                         MessageBoxButton.YesNo, MessageBoxImage.Question);
//            if (result != MessageBoxResult.Yes)
//                return;

//            try
//            {
//                await _manufacturerService.DeleteManufacturerAsync(id);
//                await LoadManufacturersAsync();
//                SelectedManufacturer = new ManufacturersDTO();
//                MessageBox.Show("Record Deleted Successfully", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error deleting manufacturer: {ex.Message}");
//            }
//        }

//        private async Task GetManufacturerByIdAsync()
//        {
//            try
//            {
//                var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(SearchId);
//                if (manufacturer != null)
//                {
//                    SelectedManufacturer = manufacturer;
//                }
//                else
//                {
//                    MessageBox.Show($"Manufacturer with ID {SearchId} not found.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
//                }
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error fetching manufacturer by ID: {ex.Message}");
//            }
//        }

//        public event PropertyChangedEventHandler PropertyChanged;
//        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
//            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//    }


//}








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
//        private RelayCommand _getManufacturerByIdCommand;

//        private int _searchId;

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

//        public int SearchId
//        {
//            get => _searchId;
//            set
//            {
//                _searchId = value;
//                OnPropertyChanged();
//                GetManufacturerByIdCommand?.RaiseCanExecuteChanged();
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

//        public RelayCommand GetManufacturerByIdCommand => _getManufacturerByIdCommand ??=
//            new RelayCommand(async () => await GetManufacturerByIdAsync(),
//                             () => SearchId > 0);

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
//                SearchId = 0;
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

//        private async Task GetManufacturerByIdAsync()
//        {
//            try
//            {
//                var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(SearchId);
//                if (manufacturer != null)
//                {
//                    SelectedManufacturer = manufacturer;
//                }
//                else
//                {
//                    System.Diagnostics.Debug.WriteLine($"Manufacturer with ID {SearchId} not found.");
//                }
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error fetching manufacturer by ID: {ex.Message}");
//            }
//        }

//        // INotifyPropertyChanged Implementation
//        public event PropertyChangedEventHandler PropertyChanged;
//        protected void OnPropertyChanged([CallerMemberName] string name = null)
//            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
//    }
//}







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
