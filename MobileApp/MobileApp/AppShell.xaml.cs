using MobileApp.Services;
using MobileApp.ViewModels;
using MobileApp.Views;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace MobileApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(DeletePage), typeof(DeletePage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(UpdateItemPage), typeof(UpdateItemPage));
            Routing.RegisterRoute(nameof(LoginPopUpPage), typeof(LoginPopUpPage));
            Routing.RegisterRoute(nameof(RegisterPopUpPage), typeof(RegisterPopUpPage));
            Routing.RegisterRoute(nameof(Profile), typeof(Profile));
            Routing.RegisterRoute(nameof(NotLoggedInProfile), typeof(NotLoggedInProfile));
            Routing.RegisterRoute(nameof(ChangePassword), typeof(ChangePassword));
        }

        SecurityService securityService = new SecurityService();
        private async void OnMenuItemClicked(object sender, EventArgs e)
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
                {
                    fileStream.Close();
                    await Shell.Current.GoToAsync("//LoginPage");
                }
                else
                {
                    fileStream.Close();
                    bool isCleared = securityService.ClearUserInfo(dataFilePath);
                    if (isCleared)
                        Console.WriteLine("User data cleared successfully.");
                    else
                        Console.WriteLine("Something went wrong!");
                    await Shell.Current.GoToAsync("//AboutPage");
                }
            FlyoutIsPresented = false;
        }
    }
}
