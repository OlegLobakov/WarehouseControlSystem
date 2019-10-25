using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WarehouseControlSystem.Resx;
using WarehouseControlSystem.ViewModel;
using WarehouseControlSystem.ViewModel.Base;
using WarehouseControlSystem.View.Content;
namespace WarehouseControlSystem.View.Pages.IndicatorContent
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IndicatorPage : ContentPage
    {
        private readonly IndicatorViewModel Model;

        public IndicatorPage(IndicatorViewModel model)
        {
            Model = model;
            BindingContext = Model;
            InitializeComponent();
            Title = model.Header + "  " + model.Description + "   " + model.Value;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<IndicatorViewModel>(this, "ContentIsLoaded", ContentIsLoaded);
            await Model.LoadContent();
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<IndicatorViewModel>(this, "ContentIsLoaded");
            Model.State = ViewModel.Base.ModelState.Undefined;
            base.OnDisappearing();
        }

        private void ContentIsLoaded(IndicatorViewModel ivm)
        {
            fl.Children.Clear();
            foreach (IndicatorContentViewModel  icvm in  Model.Content)
            {
                IndicatorContentView icv = new IndicatorContentView();
                icv.BindingContext = icvm;
                fl.Children.Add(icv);
            }            
        }

        /// <summary>
        /// Show Detail Text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Settings.ShowIndicatorDetailDescription = !Settings.ShowIndicatorDetailDescription;

            foreach (IndicatorContentViewModel icvm in Model.Content)
            {
                icvm.IsShowDetail = Settings.ShowIndicatorDetailDescription;
            }
        }
    }
}