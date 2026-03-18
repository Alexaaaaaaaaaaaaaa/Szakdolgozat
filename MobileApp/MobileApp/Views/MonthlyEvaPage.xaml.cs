using MobileApp.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
	public partial class MonthlyEvaPage : ContentPage
	{
		MonthlyEvaViewModel viewModel;
		public MonthlyEvaPage ()
		{
			InitializeComponent ();
			BindingContext = viewModel = new MonthlyEvaViewModel();
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnAppearing();
        }
    }
}