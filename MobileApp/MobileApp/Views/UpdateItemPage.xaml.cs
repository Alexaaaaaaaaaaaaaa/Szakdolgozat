using MobileApp.Models;
using MobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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