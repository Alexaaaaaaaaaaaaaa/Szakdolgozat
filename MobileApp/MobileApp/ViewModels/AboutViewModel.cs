using System;
using System.Net.Http;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public Command OpenFridgeCommand { get; }
        public AboutViewModel()
        {
            Title = "Főoldal";
            OpenFridgeCommand = new Command(async () => await Shell.Current.GoToAsync("//ItemsPage"));
            //OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
            
        }

        //public ICommand OpenWebCommand { get; }
    }
}