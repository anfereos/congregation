using Congregation.Common.Helpers;
using Congregation.Common.Responses;
using Congregation.Common.Services;
using Congregation.Prism.Helpers;
using Congregation.Prism.ItemViewModels;
using Congregation.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace Congregation.Prism.ViewModels
{
    public class MeetingPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ObservableCollection<MeetingItemViewModel> _meetings;
        private List<MeetingResponse> _myMeetings;
        private bool _isRunning;
        private DelegateCommand _addMeetingCommand;

        public MeetingPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.Meetings;
            LoadMeetingsAsync();
        }

        public DelegateCommand AddMeetingCommand => _addMeetingCommand ??
            (_addMeetingCommand = new DelegateCommand(AddMeetingAsync));


        public ObservableCollection<MeetingItemViewModel> Meetings
        {
            get => _meetings;
            set => SetProperty(ref _meetings, value);
        }


        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        private async void LoadMeetingsAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            IsRunning = true;
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListMeetingsAsync<MeetingResponse>(url, "/api", "/Meetings", token.Token);
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            _myMeetings = (List<MeetingResponse>)response.Result;

            Meetings = new ObservableCollection<MeetingItemViewModel>(_myMeetings.Select(m => new MeetingItemViewModel(_navigationService)
            {
                Assistances = m.Assistances,
                Church = m.Church,
                ChurchId = m.ChurchId,
                Date = m.Date,
                Id = m.Id
            }).ToList());
        }

        private async void AddMeetingAsync()
        {
            if (Settings.IsLogin)
            {
                await _navigationService.NavigateAsync($"/{nameof(CongregationMasterDetailPage)}/NavigationPage/{nameof(AddMeetingPage)}");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.LoginFirstMessage, Languages.Accept);
                NavigationParameters parameters = new NavigationParameters
                {
                    {"pageReturn", nameof(LoginPage) }
                };

                await _navigationService.NavigateAsync($"/{nameof(CongregationMasterDetailPage)}/NavigationPage/{nameof(LoginPage)}", parameters);
            }
        }
    }
}
