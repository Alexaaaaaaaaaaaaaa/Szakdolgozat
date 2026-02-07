using MobileApp.Models;
using MobileApp.Services;
using MobileApp.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private Item _selectedItem;

        IRestService restService = new RestService();
        public ObservableCollection<Item> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Item> ItemTapped { get; }
        public Command<Item> DeleteCommand { get; }
        public Command UpdateCommand { get; }

        public ItemsViewModel()
        {
            Title = "Hűtő";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Item>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);

            DeleteCommand = new Command<Item>(OnDeleteItem);
            UpdateCommand = new Command<Item>(OnUpdateItem);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await restService.GetItemsAsync();
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }
        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }
        async void OnDeleteItem(Item item)
        {
            if (item == null)
                return;
            
            if (await Application.Current.MainPage.DisplayAlert("Törlés", $"Biztosan törölni szeretnéd a \"{item.Food}\" nevű élelmiszert?", "Törlés", "Mégsem") == true)
            {
                await restService.DeleteItemAsync(item.Id);
                //await ExecuteLoadItemsCommand(); //enélkül nem frissíti le az oldalt automatikusan
                IsBusy = true;
                try
                {
                    Items.Clear();
                    var items = await restService.GetItemsAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }
        async void OnUpdateItem(Item item)
        {
            if (item == null)
                return;
            else
                return;
        }
    }
}