using Congregation.Common.Helpers;
using Congregation.Common.Request;
using Congregation.Common.Responses;
using Congregation.Common.Services;
using Congregation.Prism.Helpers;
using Congregation.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace Congregation.Prism.ViewModels
{
    public class AddMeetingPageViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private IApiService _apiService;

        private bool _isRunning;
        private bool _isEnabled;
        private bool _isEnabledSave;
        private DateTime _dateMeeting;
        private DelegateCommand _newMeetingCommand;
        private DelegateCommand _updateMeetingCommand;
        private List<AssistanceResponse> _myAssistances;
        private ObservableCollection<AssistanceResponse> _assistances;

        private ChurchResponse _church;
        private int _meetingId;
        private DelegateCommand _backMeetingCommand;

        public AddMeetingPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.AddMeeting;
            IsEnabled = true;
            IsEnabledSave = false;
            DateMeeting = DateTime.Today;
        }

        public DelegateCommand NewMeetingCommand => _newMeetingCommand ??
            (_newMeetingCommand = new DelegateCommand(NewMeetingAsync));

        public DelegateCommand UpdateMeetingCommand => _updateMeetingCommand ??
            (_updateMeetingCommand = new DelegateCommand(UpdateMeetingAsync));

        public DelegateCommand BackMeetingCommand => _backMeetingCommand ??
            (_backMeetingCommand = new DelegateCommand(BackMeetingAsync));

        public DateTime DateMeeting
        {
            get => _dateMeeting;
            set => SetProperty(ref _dateMeeting, value);
        }

        public ObservableCollection<AssistanceResponse> Assistances
        {
            get => _assistances;
            set => SetProperty(ref _assistances, value);
        }

        public ChurchResponse Church
        {
            get => _church;
            set => SetProperty(ref _church, value);
        }

        public int MeetingId
        {
            get => _meetingId;
            set => SetProperty(ref _meetingId, value);
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

        public bool IsEnabledSave
        {
            get => _isEnabledSave;
            set => SetProperty(ref _isEnabledSave, value);
        }

        private async void NewMeetingAsync()
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

            MeetingRequest request = new MeetingRequest { Date = DateMeeting };

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            string url = App.Current.Resources["UrlAPI"].ToString();

            Response response = await _apiService.AddMeetingAsync(url, "/api", "/Meetings", request, token.Token);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                return;
            }
            else
            {
                var meeting = (MeetingResponse)response.Result;
                Church = meeting.Church;
                _myAssistances = meeting.Assistances;
                MeetingId = meeting.Id;
            }
            ShowAssistance();
        }

        private void ShowAssistance()
        {
            IsEnabled = false;
            IsEnabledSave = true;
            Assistances = new ObservableCollection<AssistanceResponse>(_myAssistances);
        }

        private async void UpdateMeetingAsync()
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

            UpdateMeetingRequest meeting = new UpdateMeetingRequest
            {
                Date = DateMeeting,
                Assistances = _myAssistances,
                Church = Church,
                Id = MeetingId,
                DateLocal = DateMeeting,
                MeetingId = MeetingId
            };

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            string url = App.Current.Resources["UrlAPI"].ToString();

            Response response = await _apiService.UpdateMeetingAsync(url, "/api", "/Meetings", meeting, token.Token);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                return;
            }
            else
            {
                var newmeeting = (MeetingResponse)response.Result;
                _myAssistances = newmeeting.Assistances;
            }
            RefreshListMeeting();
        }

        private async void RefreshListMeeting()
        {
            if (Settings.IsLogin)
            {
                await _navigationService.NavigateAsync(nameof(MeetingPage));
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
        private async void BackMeetingAsync()
        {
            await _navigationService.NavigateAsync($"/{nameof(CongregationMasterDetailPage)}/NavigationPage/{nameof(MeetingPage)}");
        }
    }
}