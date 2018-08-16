// ----------------------------------------------------------------------------------
// Copyright © 2018, Oleg Lobakov, Contacts: <oleg.lobakov@gmail.com>
// Licensed under the GNU GENERAL PUBLIC LICENSE, Version 3.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// https://github.com/OlegLobakov/WarehouseControlSystem/blob/master/LICENSE
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------
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

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            model.State = ViewModel.Base.ModelState.Normal;
            model.MasterStep = 1;
            MessagingCenter.Subscribe<MasterRackNewViewModel>(this, "BinTemplatesIsLoaded", BinTemplatesIsLoaded);
            MessagingCenter.Subscribe<MasterRackNewViewModel>(this, "UpdateRackView", UpdateRackView);
            MessagingCenter.Subscribe<BinsViewModel>(this, "Update", UpdateBinsViewModel);
            await model.Load();
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<MasterRackNewViewModel>(this, "BinTemplatesIsLoaded");
            MessagingCenter.Unsubscribe<MasterRackNewViewModel>(this, "UpdateRackView");
            MessagingCenter.Unsubscribe<BinsViewModel>(this, "Update");
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
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
            model.NewModel.NumberingUnNamedBins();
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

        private void ToolbarItem_UnSelect(object sender, EventArgs e)
        {
            model.NewModel.BinsViewModel.UnSelect();
        }

        /// <summary>
        /// Selec all bins in level
        /// </summary>
        /// <param name="levelcoord"></param>
        private void rackview_LevelSelected(int levelcoord)
        {
            model.NewModel.SelectLevelBins(levelcoord);
        }

        /// <summary>
        /// Select all bins in 1 section
        /// </summary>
        /// <param name="sectioncoord"></param>
        private void rackview_SectionSelected(int sectioncoord)
        {
            model.NewModel.SelectSectionBins(sectioncoord);
        }

    }
}