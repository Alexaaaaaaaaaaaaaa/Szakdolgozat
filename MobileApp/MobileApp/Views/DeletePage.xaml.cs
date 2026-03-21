using Xamarin.Forms;
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