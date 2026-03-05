using System;
using Xamarin.Forms;
using MobileApp.Models;
using MobileApp.Services;
using MobileApp.Views;
using System.ComponentModel;
using System.Reflection;

namespace MobileApp.ViewModels
{
    public class PopUpViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }
        public Command LeftPictureCommand { get; }
        public Command RightPictureCommand { get; }
        
        RestService restService = new RestService();
        UserService userService = new UserService();

        private string email;
        private string userName;
        private string password;

        private User loggedInUser;
        private int loggedInUserID;
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
            loggedInUserID = loggedInUser.UserId;
            userService.SaveUserId(loggedInUserID);
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
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
                UserId = users.Count + 1
            };
            await restService.AddUserAsync(user);
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
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
