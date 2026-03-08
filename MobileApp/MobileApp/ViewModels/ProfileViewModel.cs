using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using MobileApp.Views;

namespace MobileApp.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        public Command IsLoggedIn { get; }
        public ProfileViewModel()
        {
            Title = "Profil";
            IsLoggedIn = new Command(async () => await OnIsLoggedIn());
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
    }
}
