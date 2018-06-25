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
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.Resx;
using WarehouseControlSystem.ViewModel;

namespace WarehouseControlSystem.View.Pages.BinTemplate
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewBinTemplatePage : ContentPage
    {
        BinTemplateViewModel model;
        public NewBinTemplatePage(BinTemplateViewModel bvm)
        {
            model = bvm;
            BindingContext = model;
            InitializeComponent();
            Title = AppResources.NewBinTemplatePage_Title;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await model.Update();
        }

        protected override bool OnBackButtonPressed()
        {
            model.DisposeModel();
            base.OnBackButtonPressed();
            return false;
        }

        private async void PickerSelection1(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex >= 0)
            {
                Location selected = (Location)picker.SelectedItem;
                await model.UpdateLocation(selected);
            }
        }
        private void PickerSelection2(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex >= 0)
            {
                Zone selected = (Zone)picker.SelectedItem;
                model.ZoneCode = selected.Code;
            }
        }
        private void PickerSelection3(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex >= 0)
            {
                BinType selected = (BinType)picker.SelectedItem;
                model.BinTypeCode = selected.Code;
            }
        }
        private void codeentry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry)
            {
                Entry entry = (Entry)sender;
                if (entry.Text is string)
                {
                    entry.Text = entry.Text.ToUpper();
                }
            }
        }
    }

}