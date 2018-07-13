using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.Resx;
using WarehouseControlSystem.ViewModel;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.Helpers.NAV;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WarehouseControlSystem.View.Pages.RackScheme.MasterNewRack
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterNewRackPage : ContentPage
    {
        private readonly MasterRackNewViewModel model;
        public MasterNewRackPage(MasterRackNewViewModel mrnvm)
        {
            model = mrnvm;
            BindingContext = model;
            InitializeComponent();
            Title = AppResources.RackNewPage_Title;
            orientationpicker.ItemsSource = Global.OrientationList;
            MessagingCenter.Subscribe<MasterRackNewViewModel>(this, "BinTemplatesIsLoaded", BinTemplatesIsLoaded);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            model.State = ViewModel.Base.ModelState.Normal;
            model.MasterStep = 1;
            await model.Load();
        }

        protected override bool OnBackButtonPressed()
        {
            MessagingCenter.Unsubscribe<MasterRackNewViewModel>(this, "BinTemplatesIsLoaded");
            model.CancelAsync();
            base.OnBackButtonPressed();
            return false;
        }

        private void BinTemplatesIsLoaded(MasterRackNewViewModel mrnvm)
        {
            bintemplatepicker.ItemsSource = model.BinTemplates;
            if (model.BinTemplates.Count == 1)
            {
                bintemplatepicker.SelectedItem = model.BinTemplates.First();
            }
        }

        private void Picker_OrientationChanged(object sender, EventArgs e)
        {
        }

        private async void CodeEntryChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = (Entry)sender;
            if (entry.Text is string)
            {
                entry.Text = entry.Text.ToUpper();
            }
            await model.CheckNo();
        }
    }
}