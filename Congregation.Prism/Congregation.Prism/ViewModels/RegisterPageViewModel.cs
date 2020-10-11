using Congregation.Common.Request;
using Congregation.Common.Responses;
using Congregation.Common.Services;
using Congregation.Prism.Helpers;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Congregation.Prism.ViewModels
{
    public class RegisterPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IRegexHelper _regexHelper;
        private readonly IApiService _apiService;
        private ImageSource _image;
        private UserRequest _user;
        private ProfessionResponse _profession;
        private ObservableCollection<ProfessionResponse> _professions;
        private ChurchResponse _church;
        private ObservableCollection<ChurchResponse> _churches;
        private DistrictResponse _district;
        private ObservableCollection<DistrictResponse> _districts;
        private CountryResponse _country;
        private ObservableCollection<CountryResponse> _countries;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _registerCommand;

        public RegisterPageViewModel(
            INavigationService navigationService,
            IRegexHelper regexHelper,
            IApiService apiService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _regexHelper = regexHelper;
            _apiService = apiService;
            Title = Languages.Register;
            Image = App.Current.Resources["UrlNoImage"].ToString();
            IsEnabled = true;
            User = new UserRequest();
            LoadCountriesAsync();
            LoadProfessionsAsync();

        }

        public DelegateCommand RegisterCommand => _registerCommand ?? (_registerCommand = new DelegateCommand(RegisterAsync));

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public UserRequest User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public ProfessionResponse Profession
        {
            get => _profession;
            set => SetProperty(ref _profession, value);
        }

        public ObservableCollection<ProfessionResponse> Professions
        {
            get => _professions;
            set => SetProperty(ref _professions, value);
        }

        public CountryResponse Country
        {
            get => _country;
            set
            {
                Districts = value != null ? new ObservableCollection<DistrictResponse>(value.Districts) : null;
                Churches = new ObservableCollection<ChurchResponse>();
                District = null;
                Church = null;
                SetProperty(ref _country, value);
            }
        }

        public ObservableCollection<CountryResponse> Countries
        {
            get => _countries;
            set => SetProperty(ref _countries, value);
        }

        public DistrictResponse District
        {
            get => _district;
            set
            {
                Churches = value != null ? new ObservableCollection<ChurchResponse>(value.Churches) : null;
                Church = null;
                SetProperty(ref _district, value);
            }
        }

        public ObservableCollection<DistrictResponse> Districts
        {
            get => _districts;
            set => SetProperty(ref _districts, value);
        }

        public ChurchResponse Church
        {
            get => _church;
            set => SetProperty(ref _church, value);
        }

        public ObservableCollection<ChurchResponse> Churches
        {
            get => _churches;
            set => SetProperty(ref _churches, value);
        }
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private async void LoadCountriesAsync()
        {
            IsRunning = true;
            IsEnabled = false;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAsync<CountryResponse>(url, "/api", "/Countries");
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            List<CountryResponse> list = (List<CountryResponse>)response.Result;
            Countries = new ObservableCollection<CountryResponse>(list.OrderBy(c => c.Name));
        }

        private async void LoadProfessionsAsync()
        {
            IsRunning = true;
            IsEnabled = false;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAsync<ProfessionResponse>(url, "/api", "/Professions");
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            List<ProfessionResponse> list = (List<ProfessionResponse>)response.Result;
            Professions = new ObservableCollection<ProfessionResponse>(list.OrderBy(c => c.Name));
        }

        private async void RegisterAsync()
        {
            bool isValid = await ValidateDataAsync();
            if (!isValid)
            {
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }
            string url = App.Current.Resources["UrlAPI"].ToString();

            User.ChurchId = Church.Id;
            User.ProfessionId = Profession.Id;

            Response response = await _apiService.RegisterUserAsync(url, "/api", "/Account/Register", User);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                if (response.Message == "Error003")
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.Error003, Languages.Accept);
                }
                else if (response.Message == "Error004")
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.Error004, Languages.Accept);
                }
                else if (response.Message == "Error006")
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.Error006, Languages.Accept);
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                }

                return;
            }

            await App.Current.MainPage.DisplayAlert(Languages.Ok, Languages.RegisterMessge, Languages.Accept);
            await _navigationService.GoBackAsync();
        }


        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(User.Document))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.DocumentError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.FirstName))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.FirstNameError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.LastName))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.LastNameError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.Address))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.AddressError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.Email) || !_regexHelper.IsValidEmail(User.Email))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.EmailError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.Phone))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PhoneError, Languages.Accept);
                return false;
            }

            if (Profession == null)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ProfessionError, Languages.Accept);
                return false;
            }

            if (Country == null)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.CountryError, Languages.Accept);
                return false;
            }

            if (District == null)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.DistrictError, Languages.Accept);
                return false;
            }

            if (Church == null)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ChurchError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.Password) || User.Password?.Length < 6)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PasswordError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.PasswordConfirm))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PasswordConfirmError1, Languages.Accept);
                return false;
            }

            if (User.Password != User.PasswordConfirm)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PasswordConfirmError2, Languages.Accept);
                return false;
            }

            return true;
        }
    }

}
