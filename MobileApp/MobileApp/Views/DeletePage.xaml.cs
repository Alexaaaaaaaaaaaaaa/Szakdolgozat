using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileApp.ViewModels;

namespace MobileApp.Views
{
    public partial class DeletePage : ContentPage
    {
        public DeletePage()
        {
            InitializeComponent();
            BindingContext = new DeletePageViewModel();
        }
    }
}