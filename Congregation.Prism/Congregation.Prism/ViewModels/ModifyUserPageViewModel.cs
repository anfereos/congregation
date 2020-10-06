using Congregation.Prism.Helpers;
using Prism.Navigation;

namespace Congregation.Prism.ViewModels
{
    public class ModifyUserPageViewModel : ViewModelBase
    {
        public ModifyUserPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = Languages.Login;
        }
    }
}
