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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.Resx;
using WarehouseControlSystem.ViewModel;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.Helpers.NAV;

namespace WarehouseControlSystem.View.Pages.Racks.Edit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RackEditPage : ContentPage
    {
        private readonly RackViewModel model;
        public RackEditPage(RackViewModel rvm)
        {
            model = rvm;
            BindingContext = model;
            InitializeComponent();
            Title = AppResources.RackEditPage_Title + " " + rvm.No;
            orientationpicker.ItemsSource = Global.OrientationList;
            orientationpicker.SelectedItem = Global.OrientationList.Find(x => x.RackOrientation == rvm.RackOrientation);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            model.State = ViewModel.Base.ModelState.Normal;
            MessagingCenter.Subscribe<BinsViewModel>(this, "BinsIsLoaded", BinsIsLoaded);
            await model.LoadBins();
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<BinsViewModel>(this, "BinsIsLoaded");
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            model.IsEditMode = false;
            model.BinsViewModel.IsEditMode = false;
            base.OnBackButtonPressed();
            return false;
        }

        private void BinsIsLoaded(BinsViewModel bvm)
        {
            model.FillEmptyPositions();
            model.NumberingUnNamedBins();
            rackview.Update(model);
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {

        }

        private void RackOrientationPickerChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex != -1)
            {
                RackOrientationPick rop = (RackOrientationPick)picker.SelectedItem;
                model.RackOrientation = rop.RackOrientation;
            }
        }

        private void CommentChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private void rackview_LevelSelected(int levelcoord)
        {
            model.SelectLevelBins(levelcoord);
        }

        private void rackview_SectionSelected(int sectioncoord)
        {
            model.SelectSectionBins(sectioncoord);
        }
    }
}