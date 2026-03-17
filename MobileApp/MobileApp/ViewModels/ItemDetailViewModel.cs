using MobileApp.Services;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        RestService restService = new RestService();
        private int itemId;
        private string text;
        private string description;
        private string expDate;
        public int Id { get; set; }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
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
        public string ExpiryDate
        {
            get => expDate;
            set => SetProperty(ref expDate, value);
        }

        public async void LoadItemId(int itemId)
        {
            try
            {
                var item = await restService.GetItemAsync(itemId);
                Id = item.Id;
                Text = item.Food;
                Description = item.Quantity.ToString() + " " + item.QuantityMeasure;
                ExpiryDate = item.Date.ToString("yyyy.MM.dd.");
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
