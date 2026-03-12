using MobileApp.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChangePassword : ContentPage
	{
		public ChangePassword ()
		{
			InitializeComponent ();
			BindingContext = new ChangePasswordViewModel();
        }
	}
}