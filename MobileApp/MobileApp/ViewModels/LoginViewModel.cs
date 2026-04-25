using MobileApp.Views;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    [QueryProperty(nameof(PageName), nameof(PageName))]
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }
        
        private string pageName;
        public string PageName
        {
            get => pageName;
            set => SetProperty(ref pageName, value);
        }
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(OnRegisterClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            // await Shell.Current.GoToAsync($"//{nameof(LoginPopUpPage)}");
            await Shell.Current.GoToAsync($"{nameof(LoginPopUpPage)}?{nameof(PopUpViewModel.PageName)}={PageName}");
        }
        private async void OnRegisterClicked(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(RegisterPopUpPage)}?{nameof(PopUpViewModel.PageName)}={PageName}");
        }
    }
}
