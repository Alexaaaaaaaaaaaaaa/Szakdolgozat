using MobileApp.Services;
using MobileApp.Views;
using MobileApp.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        public Command IsLoggedIn { get; }
        public Command EmailVisible { get; }
        public Command ChangePassword { get; }
        public Command LogOut { get; }

        SecurityService securityService = new SecurityService();

        private string userName;
        private string email;
        private string star;
        private string isEyesClosed;
        private bool visible;
        private bool invisible;
        public ProfileViewModel()
        {
            Title = "Profil";
            IsLoggedIn = new Command(async () => await OnIsLoggedIn());
            EmailVisible = new Command(EyeStage);
            ChangePassword = new Command(OnChangePassword);
            LogOut = new Command(OnLogOut);
            UserName = GetUserName();
            Email = GetUserEmail();
            Star = GetUserEmailHidden();
            if (Invisible == false && Visible == false)
            {
                Invisible = true;
                Visible = false;
            }
            IsEyesClosed = EyePicture(Invisible, Visible);
        }
        public async Task OnIsLoggedIn()
        {
            IsBusy = true;
            try
            {
                string appDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    dataPath = Path.Combine(appDirectory, "Data", "UserData.txt");
                using (FileStream fileStream = new FileStream(dataPath, FileMode.Open))
                {
                    if (fileStream.Length == 0)
                    {
                        fileStream.Close();
                        await Shell.Current.GoToAsync($"{nameof(NotLoggedInProfile)}");
                    }
                    else
                        fileStream.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }
        public string Star
        {
            get => star;
            set => SetProperty(ref star, value);
        }
        public string IsEyesClosed
        {
            get => isEyesClosed;
            set => SetProperty(ref isEyesClosed, value);
        }
        public bool Visible
        {
            get => visible;
            set => SetProperty(ref visible, value);
        }
        public bool Invisible
        {
            get => invisible;
            set => SetProperty(ref invisible, value);
        }
        public void EyeStage()
        {
            if (Invisible)
            {
                Invisible = false;
                Visible = true;
                IsEyesClosed = EyePicture(Invisible, Visible);
            }
            else
            {
                Invisible = true;
                Visible = false;
                IsEyesClosed = EyePicture(Invisible, Visible);
            }
        }
        public async void OnChangePassword()
        {
            await Shell.Current.GoToAsync($"{nameof(ChangePassword)}");
        }
        public async void OnLogOut()
        {
            string appDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    dataDirectoryPath = Path.Combine(appDirectory, "Data");
            if (!Directory.Exists(dataDirectoryPath))
                Directory.CreateDirectory(dataDirectoryPath);
            if (!File.Exists(Path.Combine(dataDirectoryPath, "UserData.txt")))
                File.Create(Path.Combine(dataDirectoryPath, "UserData.txt")).Close();
            string dataFilePath = Path.Combine(dataDirectoryPath, "UserData.txt");
            using (FileStream fileStream = new FileStream(dataFilePath, FileMode.Open))
                if (fileStream.Length == 0)
                    Console.WriteLine("Valami nagyon nem okés tesám!");
                else
                {
                    fileStream.Close();
                    bool isCleared = securityService.ClearUserInfo(dataFilePath);
                    if (isCleared)
                        Console.WriteLine("User data cleared successfully.");
                    else
                        Console.WriteLine("Something went wrong!");
                    await Shell.Current.GoToAsync($"//{nameof(Profile)}");
                }
        }
        public string GetUserName()
        {
            User user = new User();
            user = securityService.Decrypt();
            return user.User_Name;
        }
        public string GetUserEmail()
        {
            User user = new User();
            user = securityService.Decrypt();
            return user.Email;
        }
        public string GetUserEmailHidden()
        {
            string stars = "";
            User user = new User();
            user = securityService.Decrypt();
            string email = user.Email;
            int emailLength = email.Length;
            foreach (char character in email)
                stars += "*";
            return stars;
        }
        public string EyePicture(bool invisible, bool visible)
        {
            if (invisible)
                return "Resources/drawable/icon_feed.png";
            else if (visible)
                return "Resources/drawable/icon_about.png";
            else
                return " ";
        }
    }
}
