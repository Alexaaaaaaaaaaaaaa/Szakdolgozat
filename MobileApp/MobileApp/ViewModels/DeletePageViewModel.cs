using MobileApp.Models;
using MobileApp.Services;
using System;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class DeletePageViewModel :BaseViewModel
    {
        private string deleteText;
        public string FoodName;
        private int itemId;

        public Command ItemUsed { get; }
        public Command ItemWasted { get; }

        SecurityService securityService = new SecurityService();
        RestService restService = new RestService();
        public DeletePageViewModel()
        {
            Title = "Törlés";
            ItemUsed = new Command(OnItemUsed);
            ItemWasted = new Command(OnItemWasted);
        }
        public string DeleteText
        {
            get => deleteText;
            set => SetProperty(ref deleteText, value);
        }
        public int ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                GetFood(value);
            }
        }
        private async void GetFood(int itemId)
        {
            var food = await restService.GetItemAsync(itemId);
            DeleteText = $"a/az {food.Food}"; // lehetne if függvénnyel nézni hogy a vagy az legyen
        }
        private async void OnItemWasted()
        {
            User user = new User();
            user = securityService.Decrypt();
            user.Wasted = user.Wasted + 1;
            DateTime currentTime = DateTime.Now;
            DateTime newDate = DateTime.Parse(currentTime.ToShortDateString());
            user.Last_Update = newDate;
            if (await restService.UpdateUserAsync(user.Email, user.Password, user) == true)
            {
                securityService.Encrypt(user);
                await restService.DeleteItemAsync(ItemId);
                await Shell.Current.GoToAsync("..");
            }
            else
                Console.WriteLine("Hiba történt");
        }

        private async void OnItemUsed()
        {
            User user = new User();
            user = securityService.Decrypt();
            user.Used = user.Used + 1;
            DateTime currentTime = DateTime.Now;
            DateTime newDate = DateTime.Parse(currentTime.ToShortDateString());
            user.Last_Update = newDate;
            if (await restService.UpdateUserAsync(user.Email, user.Password, user) == true)
            {
                securityService.Encrypt(user);
                await restService.DeleteItemAsync(ItemId);
                await Shell.Current.GoToAsync("..");
            }
            else
                Console.WriteLine("Hiba történt");
        }
    }
}
