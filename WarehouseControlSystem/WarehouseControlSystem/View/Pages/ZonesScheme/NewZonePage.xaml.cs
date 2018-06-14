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
using WarehouseControlSystem.ViewModel.Base;

namespace WarehouseControlSystem.View.Pages.ZonesScheme
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewZonePage : ContentPage
    {
        private readonly ZoneViewModel model;
        public NewZonePage(ZoneViewModel zvm)
        {
            model = zvm;
            BindingContext = model;
            InitializeComponent();

            colorpicker.ItemsSource = Global.Colors;
            ColorPick color = Global.Colors.Find(x => x.HexColor == zvm.Zone.HexColor);
            if (color is ColorPick)
            {
                colorpicker.SelectedItem = color;
            }
            UpdateTitle();

            MessagingCenter.Subscribe<ZoneViewModel>(this, "LocationsIsLoaded", LocationsIsLoaded);
            MessagingCenter.Subscribe<ZoneViewModel>(this, "BinTypesIsLoaded", BinTypesIsLoaded);
            model.State = ModelState.Normal;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            model.Load();
        }

        public void LocationsIsLoaded(ZoneViewModel zvm)
        {
            locationpicker.ItemsSource = model.Locations;
            if (!string.IsNullOrEmpty(model.LocationCode))
            {
                List<Location> list = new List<Location>(model.Locations);
                Location loc1 = list.Find(x => x.Code == model.LocationCode);
                if (loc1 is Location)
                {
                    locationpicker.SelectedItem = loc1;
                }
            }
        }

        public void BinTypesIsLoaded(ZoneViewModel zvm)
        {
            bintypepicker.ItemsSource = model.BinTypes;
            if (!string.IsNullOrEmpty(model.BinTypeCode))
            {
                List<BinType> list = new List<BinType>(model.BinTypes);
                BinType bt1 = list.Find(x => x.Code == model.BinTypeCode);
                if (bt1 is BinType)
                {
                    bintypepicker.SelectedItem = bt1;
                }
            }
        }
        
        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = (Entry)sender;
            if (entry.Text is string)
            {
                entry.Text = entry.Text.ToUpper();
            }
            model.CheckZoneCode();
            UpdateTitle();
        }

        private void Colorpicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex >= 0)
            {
                ColorPick selected = (ColorPick)picker.SelectedItem;
                model.Color = selected.Color;
            }
        }

        private void LocationSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex >= 0)
            {
                Location selected = (Location)picker.SelectedItem;
                model.LocationCode = selected.Code;
            }
        }

        private void BinTypepicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex >= 0)
            {
                BinType selected = (BinType)picker.SelectedItem;
                model.BinTypeCode = selected.Code;
            }
        }

        private void UpdateTitle()
        {
            if (model.CreateMode)
            {
                Title = AppResources.NewZonePage_Title + " " + model.LocationCode + " | " + model.Code;
            }
            else
            {
                Title = AppResources.NewZonePage_Title_Edit + " " + model.LocationCode + " | " + model.Code;
            }
        }
    }
}