using Congregation.Common.Responses;
using Congregation.Prism.Views;
using Prism.Commands;
using Prism.Navigation;

namespace Congregation.Prism.ItemViewModels
{
    public class MeetingItemViewModel : MeetingResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectMeetingCommand;

        public MeetingItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectMeetingCommand => _selectMeetingCommand ??
            (_selectMeetingCommand = new DelegateCommand(SelectMeeting));

        private async void SelectMeeting()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { "meeting", this }
            };

            await _navigationService.NavigateAsync(nameof(AddMeetingPage), parameters);
        }
    }
}
