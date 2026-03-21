using MobileApp.ViewModels;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;

namespace MobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Profile : ContentPage
	{
        public Profile ()
		{
            InitializeComponent ();
			BindingContext = viewModel = new ProfileViewModel();
			Refresh();
        }
        ProfileViewModel viewModel = new ProfileViewModel();
		public async void Refresh()
        {
            await viewModel.OnIsLoggedIn();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnAppearing();
        }
    }
}