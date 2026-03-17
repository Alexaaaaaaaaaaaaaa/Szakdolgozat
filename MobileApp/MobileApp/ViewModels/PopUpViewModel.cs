using System;
using Xamarin.Forms;
using MobileApp.Models;
using MobileApp.Services;
using MobileApp.Views;

namespace MobileApp.ViewModels
{
    public class PopUpViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }
        public Command LeftPictureCommand { get; }
        public Command RightPictureCommand { get; }
        
        RestService restService = new RestService();
        SecurityService securityService = new SecurityService();

        private string email;
        private string userName;
        private string password;

        private User loggedInUser;
        public PopUpViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(OnRegisterClicked);
            LeftPictureCommand = new Command(LeftPictureSwipe);
            RightPictureCommand = new Command(RightPictureSwipe);
        }
        public string Email { get => email; set => SetProperty(ref email, value); }
        public string UserName { get => userName; set => SetProperty(ref userName, value); }
        public string Password { get => password; set => SetProperty(ref password, value); }
        public async void OnLoginClicked(object obj)
        {
            loggedInUser = await restService.GetUserAsync(Email,Password);
            if(loggedInUser.Last_Update != null)
            {
                string userTimeString =(loggedInUser.Last_Update).ToString();
                DateTime userTime = DateTime.Parse(userTimeString);
                DateTime currentTime = DateTime.Now;
                if(userTime.Year < currentTime.Year || userTime.Month < currentTime.Month)
                {
                    loggedInUser.Last_Update = null;
                    loggedInUser.Bought = 0;
                    loggedInUser.Used = 0;
                    loggedInUser.Wasted = 0;
                    await restService.UpdateUserAsync(loggedInUser.Email, loggedInUser.Password, loggedInUser);
                }

            }
            securityService.Encrypt(loggedInUser);
            await Shell.Current.GoToAsync($"//{nameof(Profile)}");
        }
        public async void OnRegisterClicked()
        {
            var users = await restService.GetUsersAsync();
            User user = new User()
            {
                Email = Email,
                User_Name = UserName,
                Password = Password,
                Bought = 0,
                Used = 0,
                Wasted = 0,
                UserId = users.Count + 1,
                Last_Update = null
            };
            await restService.AddUserAsync(user);
            loggedInUser = await restService.GetUserAsync(Email, Password);
            securityService.Encrypt(loggedInUser);
            await Shell.Current.GoToAsync($"//{nameof(Profile)}");
        }
        public async void LeftPictureSwipe(object obj)
        {
            throw new NotImplementedException();
        }
        public async void RightPictureSwipe(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
