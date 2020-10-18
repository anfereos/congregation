using Congregation.Common.Responses;
using Prism.Navigation;
using System;

namespace Congregation.Prism.ViewModels
{
    public class UpdateMeetingPageViewModel : ViewModelBase
    {
        //private readonly INavigationService _navigationService;
        private MeetingResponse _meeting;

        public UpdateMeetingPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            //_navigationService = navigationService;
            Title = "Meeting";
        }

        public MeetingResponse Meeting
        {
            get => _meeting;
            set => SetProperty(ref _meeting, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("meeting"))
            {
                Meeting = parameters.GetValue<MeetingResponse>("meeting");
                Title = Convert.ToString(Meeting.Date);
            }
        }
    }
}
