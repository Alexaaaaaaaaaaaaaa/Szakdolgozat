using MobileApp.Models;
using MobileApp.Services;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class UpdateItemViewModel : BaseViewModel
    {
        private int itemId;
        private string foodName;
        private int quantity;
        private string measure;
        private bool open;
        private DateTime expiry;
        IRestService restService = new RestService();
        public int Id { get; set; }
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public UpdateItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }
        public string Text
        {
            get => foodName;
            set => SetProperty(ref foodName, value);
        }

        public int DescriptionQuantity
        {
            get => quantity;
            set => SetProperty(ref quantity, value);
        }
        public string DescriptionMeasure
        {
            get => measure;
            set => SetProperty(ref measure, value);
        }
        public bool IsOpen
        {
            get => open;
            set => SetProperty(ref open, value);
        }
        public DateTime Expiration
        {
            get => expiry;
            set => SetProperty(ref expiry, value);
        }
        public async void LoadItemId(int itemId)
        {
            try
            {
                var item =  await restService.GetItemAsync(itemId);
                Id = item.Id;
                Text = item.Food;
                DescriptionQuantity = item.Quantity;
                DescriptionMeasure = item.QuantityMeasure;
                IsOpen = item.IsOpened;
                Expiration = item.Date;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
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
                LoadItemId(value);
            }
        }
        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(foodName);
        }

        private async void OnSave()
        {
            var updatedItem = new Item()
            {
                Id = Id,
                Food = Text,
                Quantity = DescriptionQuantity,
                QuantityMeasure = DescriptionMeasure,
                IsOpened = IsOpen,
                Date = Expiration
            };
            await restService.UpdateItemAsync(updatedItem.Id, updatedItem);
            await Shell.Current.GoToAsync("..");
        }
    }
}
