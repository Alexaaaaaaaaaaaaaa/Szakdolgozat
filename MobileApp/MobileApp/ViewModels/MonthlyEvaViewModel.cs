using MobileApp.Models;
using MobileApp.Services;
using MobileApp.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class MonthlyEvaViewModel :BaseViewModel
    {
        private string mainText;
        private int bought;
        private int used;
        private int wasted;
        private double usedWidth;
        private double wastedWidth;
        private string shellColor;
        private string picture;

        SecurityService securityService = new SecurityService();

        public Command LoadCommand { get; }
        public MonthlyEvaViewModel() 
        {
            Title = "Havi összesítő";
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
        }

        async Task ExecuteLoadCommand()
        {
            IsBusy = true;
            try
            {
                string appDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    dataDirectoryPath = Path.Combine(appDirectory, "Data"),
                    dataFilePath = Path.Combine(dataDirectoryPath, "UserData.txt");
                if (!File.Exists(dataFilePath))
                {
                    await Application.Current.MainPage.DisplayAlert("Ismeretlen felhasználó", "Előbb jelentkezz be!", "OK");
                    await Shell.Current.GoToAsync(nameof(NotLoggedInProfile));
                }
                using (FileStream fileStream = new FileStream(dataFilePath, FileMode.Open))
                {
                    if (fileStream.Length == 0)
                    {
                        fileStream.Close();
                        await Application.Current.MainPage.DisplayAlert("Ismeretlen felhasználó", "Előbb jelentkezz be!", "OK");
                        await Shell.Current.GoToAsync(nameof(NotLoggedInProfile));
                    }
                    else
                        fileStream.Close();
                }
                User user = securityService.Decrypt();
                Bought = user.Bought;
                Used = user.Used;
                Wasted = user.Wasted;
                if (Wasted == 0)
                {
                    MainText = "SZÉP MUNKA!";
                    ShellColor = "MediumAquamarine";
                    Picture = "Resources/drawable/gizmo.png";
                }
                else if (Wasted < 3)
                {
                    MainText = "Nem rossz!";
                    ShellColor = "MediumAquamarine";
                    Picture = "Resources/drawable/ice_cream.png";
                }
                else
                {
                    MainText = "Van még hova fejlődni..";
                    ShellColor = "LightCoral";
                    Picture = "Resources/drawable/rotten_apple.png";
                }
                if ((Used+Wasted) < Bought)
                {
                    UsedWidth = (Double.Parse(Used.ToString()) / (Double.Parse(Bought.ToString()))) * Double.Parse((Application.Current.MainPage.Width - 40).ToString());
                    WastedWidth = (Double.Parse(Wasted.ToString()) / (Double.Parse(Bought.ToString()))) * Double.Parse((Application.Current.MainPage.Width - 40).ToString());
                }
                else
                {
                    UsedWidth = (Double.Parse(Used.ToString()) / (Double.Parse((Used+Wasted).ToString()))) * Double.Parse((Application.Current.MainPage.Width - 40).ToString());
                    WastedWidth = (Double.Parse(Wasted.ToString()) / (Double.Parse((Used + Wasted).ToString()))) * Double.Parse((Application.Current.MainPage.Width - 40).ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
                { IsBusy = false; }
        }
        public void OnAppearing()
        {
            IsBusy = true;
        }

        public string MainText
        {
            get => mainText;
            set => SetProperty(ref mainText, value);
        }
        public int Bought
        {
            get => bought;
            set => SetProperty(ref bought, value);
        }
        public int Used
        {
            get => used;
            set => SetProperty(ref used, value);
        }
        public int Wasted
        {
            get => wasted;
            set => SetProperty(ref wasted, value);
        }
        public double UsedWidth
        {
            get => usedWidth;
            set => SetProperty(ref usedWidth, value);
        }
        public double WastedWidth
        {
            get => wastedWidth;
            set => SetProperty(ref wastedWidth, value);
        }
        public string ShellColor
        {
            get => shellColor;
            set => SetProperty(ref shellColor, value);
        }
        public string Picture
        {
            get => picture;
            set => SetProperty(ref picture, value);
        }
    }
}
