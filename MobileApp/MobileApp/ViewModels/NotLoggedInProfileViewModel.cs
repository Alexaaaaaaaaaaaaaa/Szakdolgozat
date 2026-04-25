using MobileApp.Views;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    [QueryProperty(nameof(PageName), nameof(PageName))]
    public class NotLoggedInProfileViewModel : BaseViewModel
    {
        public Command GoToLoginPageCommand { get; }
        private string pageName;
        public string PageName
        {
            get => pageName;
            set => SetProperty(ref pageName, value);
        }
        public NotLoggedInProfileViewModel()
        {
            GoToLoginPageCommand = new Command(OnClicked);
        }
        public async void OnClicked()
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}?{nameof(LoginViewModel.PageName)}={PageName}");
        }
    }
}
