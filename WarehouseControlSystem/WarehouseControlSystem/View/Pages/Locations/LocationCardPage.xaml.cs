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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.Resx;
using WarehouseControlSystem.ViewModel;

namespace WarehouseControlSystem.View.Pages.Locations
{ 
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LocationCardPage : ContentPage
	{
        private readonly LocationViewModel model;

        public LocationCardPage(LocationViewModel lvm)
        {
            model = lvm;
            BindingContext = model;
            InitializeComponent();
            Title = AppResources.LocationNewPage_Title;


            colorpicker.ItemsSource = Global.Colors;
            ColorPick color = Global.Colors.Find(x => x.Color == lvm.Color);
            if (color is ColorPick)
            {
                colorpicker.SelectedItem = color;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            model.State = ViewModel.Base.ModelState.Normal;
        }

        protected override bool OnBackButtonPressed()
        {
            model.DisposeModel();
            base.OnBackButtonPressed();
            return false;
        }

        private async void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = (Entry)sender;
            if (entry.Text is string)
            {
                entry.Text = entry.Text.ToUpper();
            }
            await model.CheckLocationCode();
        }

        private void colorpicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex >= 0)
            {
                ColorPick selected = (ColorPick)picker.SelectedItem;
                model.Color = selected.Color;
            }
        }
    }
}