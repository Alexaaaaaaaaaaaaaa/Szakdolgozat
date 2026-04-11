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
        private string profilePicture;

        private User loggedInUser;
        public PopUpViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(OnRegisterClicked);
            LeftPictureCommand = new Command(LeftPictureSwipe);
            RightPictureCommand = new Command(RightPictureSwipe);
            ProfilePicture = "Resources/drawable/apple.png";
        }
        public string Email { get => email; set => SetProperty(ref email, value); }
        public string UserName { get => userName; set => SetProperty(ref userName, value); }
        public string Password { get => password; set => SetProperty(ref password, value); }
        public string ProfilePicture { get => profilePicture; set => SetProperty(ref profilePicture, value); }
        public async void OnLoginClicked(object obj)
        {
            if (Email != "" && Password != "" && Email != null && Password != null)
            {
                loggedInUser = await restService.GetUserAsync(Email, Password);
                if (loggedInUser.Last_Update != null)
                {
                    string userTimeString = (loggedInUser.Last_Update).ToString();
                    DateTime userTime = DateTime.Parse(userTimeString);
                    DateTime currentTime = DateTime.Now;
                    if (userTime.Year < currentTime.Year || userTime.Month < currentTime.Month)
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
            else
                await Application.Current.MainPage.DisplayAlert("Üres mező", "Kérlek töltsd ki az összes mezőt!", "OK");
        }
        public async void OnRegisterClicked()
        {
            if (Email != null && Email != "" && UserName != null && UserName != "" && Password != null && Password != "")
            {
                var users = await restService.GetUsersAsync();
                int profpic = 0;
                switch (ProfilePicture)
                {
                    case "Resources/drawable/apple.png":
                        profpic = 1;
                        break;
                    case "Resources/drawable/coffee.png":
                        profpic = 2;
                        break;
                    case "Resources/drawable/carrot.png":
                        profpic = 3;
                        break;
                }
                User user = new User()
                {
                    Email = Email,
                    User_Name = UserName,
                    Password = Password,
                    Bought = 0,
                    Used = 0,
                    Wasted = 0,
                    UserId = users.Count + 1,
                    Last_Update = null,
                    Profile_Picture = profpic
                };
                await restService.AddUserAsync(user);
                loggedInUser = await restService.GetUserAsync(Email, Password);
                securityService.Encrypt(loggedInUser);
                await Shell.Current.GoToAsync($"//{nameof(Profile)}");
            }
            else
                await Application.Current.MainPage.DisplayAlert("Üres mező", "Kérlek töltsd ki az összes mezőt!", "OK");
        }
        public async void LeftPictureSwipe()
        {
            switch (ProfilePicture)
            {
                case "Resources/drawable/apple.png":
                    ProfilePicture = "Resources/drawable/carrot.png";
                    break;
                case "Resources/drawable/carrot.png":
                    ProfilePicture = "Resources/drawable/coffee.png";
                    break;
                case "Resources/drawable/coffee.png":
                    ProfilePicture = "Resources/drawable/apple.png";
                    break;
            }
        }
        public async void RightPictureSwipe()
        {
            switch (ProfilePicture)
            {
                case "Resources/drawable/apple.png":
                    ProfilePicture = "Resources/drawable/coffee.png";
                    break;
                case "Resources/drawable/coffee.png":
                    ProfilePicture = "Resources/drawable/carrot.png";
                    break;
                case "Resources/drawable/carrot.png":
                    ProfilePicture = "Resources/drawable/apple.png";
                    break;
            }
        }
    }
}
