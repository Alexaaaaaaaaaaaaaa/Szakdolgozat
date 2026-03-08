using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using MobileApp.Views;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class NotLoggedInProfileViewModel
    {
        public Command GoToLoginPageCommand { get; }
        public NotLoggedInProfileViewModel()
        {
            GoToLoginPageCommand = new Command(OnClicked);
        }
        public async void OnClicked()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
