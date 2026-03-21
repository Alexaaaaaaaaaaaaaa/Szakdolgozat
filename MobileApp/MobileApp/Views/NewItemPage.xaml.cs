using MobileApp.ViewModels;
using System;
using Xamarin.Forms;

namespace MobileApp.Views
{
    public partial class NewItemPage : ContentPage
    {

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
            dtPckrExpiration.Date = DateTime.Now;
        }
    }
}