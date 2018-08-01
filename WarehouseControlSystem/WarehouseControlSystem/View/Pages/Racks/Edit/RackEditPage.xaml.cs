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

            //infopanel.BindingContext = model.BinsViewModel;
            //locationpicker.ItemsSource = model.Locations;
            //zonepicker.ItemsSource = model.Zones;
            orientationpicker.ItemsSource = Global.OrientationList;
            orientationpicker.SelectedItem = Global.OrientationList.Find(x => x.RackOrientation == rvm.RackOrientation);
            //bintemplatepicker.ItemsSource = model.BinTemplates;

            MessagingCenter.Subscribe<RackViewModel>(this, "ZonesIsLoaded", ZonesIsLoaded);
            MessagingCenter.Subscribe<RackViewModel>(this, "LocationsIsLoaded", LocationsIsLoaded);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            model.State = ViewModel.Base.ModelState.Normal;
            //model.FillEmptyFields();
            rackview.Update(model);
        }
      
        protected override bool OnBackButtonPressed()
        {
            MessagingCenter.Unsubscribe<RackViewModel>(this, "ZonesIsLoaded");
            MessagingCenter.Unsubscribe<RackViewModel>(this, "LocationsIsLoaded");
            base.OnBackButtonPressed();
            return false;
        }

        private void ZonesIsLoaded(object obj)
        {
            //if (!string.IsNullOrEmpty(model.ZoneCode))
            //{
            //    List<Zone> list = new List<Zone>(model.Zones);
            //    Zone zone1 = list.Find(x => x.Code == model.ZoneCode);
            //    if (zone1 is Zone)
            //    {
            //        zonepicker.SelectedItem = zone1;
            //    }
            //}
        }

        private void LocationsIsLoaded(object obj)
        {
            //if (!string.IsNullOrEmpty(model.LocationCode))
            //{
            //    List<Location> list = new List<Location>(model.Locations);
            //    Location loc1 = list.Find(x => x.Code == model.LocationCode);
            //    if (loc1 is Location)
            //    {
            //        locationpicker.SelectedItem = loc1;
            //    }
            //}

            //if (!string.IsNullOrEmpty(model.ZoneCode))
            //{
            //    List<Zone> zoneslist = new List<Zone>(model.Zones);
            //    Zone zone1 = zoneslist.Find(x => x.Code == model.ZoneCode);
            //    if (zone1 is Zone)
            //    {
            //        zonepicker.SelectedItem = zone1;
            //    }
            //}
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


        //private async void PickerLocation(object sender, EventArgs e)
        //{
        //    var picker = (Picker)sender;
        //    int selectedIndex = picker.SelectedIndex;
        //    //await model.SetLocation((Location)picker.SelectedItem);
        //}

        //private async void PickerZone(object sender, EventArgs e)
        //{
        //    var picker = (Picker)sender;
        //    //await model.SetZone((Zone)picker.SelectedItem);
        //}
    }
}