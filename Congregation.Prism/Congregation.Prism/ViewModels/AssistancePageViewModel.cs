using Congregation.Common.Responses;
using Congregation.Common.Services;
using Congregation.Prism.Helpers;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace Congregation.Prism.ViewModels
{
    public class AssistancePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ObservableCollection<MeetingResponse> _members;
        private bool _isRunning;
        private List<MeetingResponse> _myMembers;



        public AssistancePageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.Members;
            LoadMembersAsync();
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }



        public ObservableCollection<MeetingResponse> Members
        {
            get => _members;
            set => SetProperty(ref _members, value);
        }

        private async void LoadMembersAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            IsRunning = true;
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAsync<MeetingResponse>(url, "/api", "/Meetings");
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            _myMembers = (List<MeetingResponse>)response.Result;
            Members = new ObservableCollection<MeetingResponse>(_myMembers);
            //ShowMembers();

        }

        //private void ShowMembers()
        //{
        //    if (string.IsNullOrEmpty(Search))
        //    {
        //        Members = new ObservableCollection<UserResponse>(_myMembers);
        //    }
        //    else
        //    {
        //        Members = new ObservableCollection<UserResponse>(_myMembers
        //            .Where(m => m.FullName.ToLower().Contains(Search.ToLower())));//TODO: Me falta hacer el filtro de church.id
        //    }
        //}
    }
}
