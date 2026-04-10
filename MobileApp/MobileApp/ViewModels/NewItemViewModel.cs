using MobileApp.Models;
using MobileApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        private bool uniqueDateTime;
        private bool datePickerIsEnabled;
        private string pickedItem;
        RestService restService = new RestService();
        SecurityService securityService = new SecurityService();
        
        public ObservableCollection<Expiry> ExpList { get;}
        public List<string> PickerItems { get; }
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            ExpList = new ObservableCollection<Expiry>();
            ExpList.Add(new Expiry() { Type_Name = "hús", Time = 2 });
            ExpList.Add(new Expiry() { Type_Name = "zöldség", Time = 3 });
            ExpList.Add(new Expiry() { Type_Name = "gyümölcs", Time = 3 });
            ExpList.Add(new Expiry() { Type_Name = "citrusok", Time = 14 });
            PickerItems = new List<string>();
            foreach(var item in ExpList)
            {
                PickerItems.Add(item.Type_Name + ": " + item.Time.ToString() + " nap");
            }
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(text)
                && !(DescriptionQuantity == 0 || DescriptionQuantity < 0);
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
        public bool UniqueDateTime
        {
            get => uniqueDateTime;
            set => SetProperty(ref uniqueDateTime, value);
        }
        public bool DatePickerIsEnabled
        {
            get => (!uniqueDateTime);
            set => SetProperty(ref datePickerIsEnabled, value);
        }
        public string PickedItem
        {
            get => pickedItem;
            set => SetProperty(ref pickedItem, value);
        }
        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
                var items = await restService.GetItemsAsync();

                Item newItem;
                if (DatePickerIsEnabled)
                {
                    newItem = new Item()
                    {
                        Date = Expiration,
                        Quantity = DescriptionQuantity,
                        QuantityMeasure = DescriptionMeasure,
                        Food = Text,
                        IsOpened = IsOpen,
                        Id = items.Count + 1,
                        Wasted = false,
                        UserId = securityService.Decrypt().UserId
                    };
                }
                else
                {
                    string[] pickedItemPieces = pickedItem.Split(' ');
                    DateTime currentTime = DateTime.Now;
                    DateTime newDate = DateTime.Parse(currentTime.ToShortDateString());
                    int daysToAdd = Int32.Parse(pickedItemPieces[1]);
                    newItem = new Item()
                    {
                        Date = newDate.AddDays(daysToAdd),
                        Quantity = DescriptionQuantity,
                        QuantityMeasure = DescriptionMeasure,
                        Food = Text,
                        IsOpened = IsOpen,
                        Id = items.Count + 1,
                        Wasted = false,
                        UserId = securityService.Decrypt().UserId
                    };
                }

                if (await restService.AddItemAsync(newItem) == true)
                {
                    DateTime currentTime = DateTime.Now;
                    DateTime newDate = DateTime.Parse(currentTime.ToShortDateString());
                    User oldUser = securityService.Decrypt();
                    User newUser = new User()
                    {
                        UserId = oldUser.UserId,
                        Email = oldUser.Email,
                        User_Name = oldUser.User_Name,
                        Password = oldUser.Password,
                        Bought = oldUser.Bought + 1,
                        Used = oldUser.Used,
                        Wasted = oldUser.Wasted,
                        Last_Update = newDate
                    };
                    await restService.UpdateUserAsync(newUser.Email, newUser.Password, newUser);
                    string appDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        dataDirectoryPath = Path.Combine(appDirectory, "Data"),
                        dataFilePath = Path.Combine(dataDirectoryPath, "UserData.txt");
                    securityService.ClearUserInfo(dataFilePath);
                    securityService.Encrypt(newUser);
                }

                // This will pop the current page off the navigation stack
                await Shell.Current.GoToAsync("..");
        }
    }
}
