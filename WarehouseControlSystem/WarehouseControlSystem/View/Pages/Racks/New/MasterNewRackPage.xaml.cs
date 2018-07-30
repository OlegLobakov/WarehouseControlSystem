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

namespace WarehouseControlSystem.View.Pages.Racks.New
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
            infopanel.BindingContext = model.NewModel.BinsViewModel;
            Title = AppResources.RackNewPage_Title;
            orientationpicker.ItemsSource = Global.OrientationList;
            MessagingCenter.Subscribe<MasterRackNewViewModel>(this, "BinTemplatesIsLoaded", BinTemplatesIsLoaded);
            MessagingCenter.Subscribe<MasterRackNewViewModel>(this, "UpdateRackView", UpdateRackView);
            MessagingCenter.Subscribe<BinsViewModel>(this, "Update", UpdateBinsViewModel);
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
            MessagingCenter.Unsubscribe<MasterRackNewViewModel>(this, "UpdateRackView");
            MessagingCenter.Unsubscribe<BinsViewModel>(this, "Update");
            model.DisposeModel();
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

        private void UpdateRackView(MasterRackNewViewModel mrnvm)
        {
            rackview.BinWidth = (int)mainsl.Width / 8;
            rackview.Update(model.NewModel);
        }

        private void UpdateBinsViewModel(BinsViewModel bvm)
        {
            model.NewModel.NumberingEmptyBins();
            rackview.Update(model.NewModel);
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
                Title = AppResources.RackNewPage_Title + " " + entry.Text;
            }
            await model.CheckNo();
        }


    }
}