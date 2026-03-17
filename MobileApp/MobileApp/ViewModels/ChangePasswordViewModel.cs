using MobileApp.Models;
using MobileApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace MobileApp.ViewModels
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        private string currentPassword;
        private string newPassword1;
        private string newPassword2;
        private string errorMessage;

        public Command ChangePassword { get; }

        SecurityService securityService = new SecurityService();
        RestService restService = new RestService();
        public ChangePasswordViewModel()
        {
            Title = "Jelszó megváltoztatása";
            ChangePassword = new Command(OnChangePassword);
        }
        public string CurrentPassword
        {
            get => currentPassword;
            set => SetProperty(ref currentPassword, value);
        }
        public string NewPassword1
        {
            get => newPassword1;
            set => SetProperty(ref newPassword1, value);
        }
        public string NewPassword2
        {
            get => newPassword2;
            set => SetProperty(ref newPassword2, value);
        }
        public string ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }
        public async void OnChangePassword()
        {
            if (NewPassword1 == NewPassword2)
            {
                User user = new User();
                User newUser = new User();
                user = securityService.Decrypt();
                if (user.Password == CurrentPassword)
                {
                    newUser.UserId = user.UserId;
                    newUser.Email = user.Email;
                    newUser.User_Name = user.User_Name;
                    newUser.Password = NewPassword1;
                    newUser.Bought = user.Bought;
                    newUser.Used = user.Used;
                    newUser.Wasted = user.Wasted;
                    newUser.Last_Update = user.Last_Update;
                    if (await restService.UpdateUserAsync(user.Email, user.Password, newUser) == true)
                    {
                        string appDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                            dataDirectoryPath = Path.Combine(appDirectory, "Data");
                        if (!Directory.Exists(dataDirectoryPath))
                            Directory.CreateDirectory(dataDirectoryPath);
                        if (!File.Exists(Path.Combine(dataDirectoryPath, "UserData.txt")))
                            File.Create(Path.Combine(dataDirectoryPath, "UserData.txt")).Close();
                        string dataFilePath = Path.Combine(dataDirectoryPath, "UserData.txt");
                        bool isCleared = securityService.ClearUserInfo(dataFilePath);
                        if (isCleared)
                        {
                            Console.WriteLine("User data cleared successfully.");
                            securityService.Encrypt(newUser);
                            await Shell.Current.GoToAsync("//Profile");
                        }
                        else
                            Console.WriteLine("Something went wrong!");
                    }
                }
                else
                {
                    ErrorMessage = "A megadott jelenlegi jelszó helytelen!";
                }
            }
            else
            {
                ErrorMessage = "A két megadott új jelszó nem egyezik meg!";
            }
        }
    }
}
