using MobileApp.Models;
using MobileApp.Services;
using MobileApp.Views;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private string text;
        private int description;
        private string measure;
        private bool open;
        private DateTime date;
        IRestService restService = new RestService();
        // public ObservableCollection<Item> Items { get; }
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(text);
               // && !String.IsNullOrWhiteSpace(measure);
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public int DescriptionQuantity
        {
            get => description;
            set => SetProperty(ref description, value);
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
            get => date;
            set => SetProperty(ref date, value);
        }
        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            var items = await restService.GetItemsAsync();
            /*foreach (var item in items)
            {
                Items.Add(item);
            }*/
            Item newItem = new Item()
            {
                Date = Expiration,
                Quantity = DescriptionQuantity,
                QuantityMeasure = DescriptionMeasure,
                Food = Text,
                IsOpened = IsOpen,
                Id = items.Count+1
            };

            await restService.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
