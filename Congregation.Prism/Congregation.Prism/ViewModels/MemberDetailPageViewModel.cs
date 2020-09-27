using Congregation.Common.Responses;
using Congregation.Prism.Helpers;
using Prism.Navigation;

namespace Congregation.Prism.ViewModels
{
    public class MemberDetailPageViewModel : ViewModelBase
    {
        private MemberResponse _product;
//        private ObservableCollection<ProductImage> _images;

        public MemberDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = Languages.Members;
        }

        public MemberResponse Member
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        //public ObservableCollection<ProductImage> Images
        //{
        //    get => _images;
        //    set => SetProperty(ref _images, value);
        //}


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("member"))
            {
                Member = parameters.GetValue<MemberResponse>("member");
                Title = Member.FullName;
//                Images = new ObservableCollection<ProductImage>(Product.ProductImages);
            }
        }
    }
}
