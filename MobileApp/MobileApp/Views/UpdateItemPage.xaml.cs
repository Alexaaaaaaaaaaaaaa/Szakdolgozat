using MobileApp.ViewModels;
using Xamarin.Forms;

namespace MobileApp.Views
{
    public partial class UpdateItemPage : ContentPage
    {
        public UpdateItemPage()
        {
            InitializeComponent();
            BindingContext = new UpdateItemViewModel();
        }
    }
}