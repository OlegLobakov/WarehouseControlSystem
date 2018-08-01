// ----------------------------------------------------------------------------------
// Copyright © 2017, Oleg Lobakov, Contacts: <oleg.lobakov@gmail.com>
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

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.ViewModel;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.Helpers.NAV;

namespace WarehouseControlSystem.View.Pages.Racks.Edit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BinInfoPanelRackEdit : ContentView
    {

        public BinInfoPanelRackEdit()
        {
            InitializeComponent();
        }

        private void BinTypeSelector(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex >= 0)
            {
                //BinType selected = (BinType)picker.SelectedItem;
                //BinType = selected.Code;
            }
        }

        private void WarehouseClassSelector(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex >= 0)
            {
                WarehouseClass selected = (WarehouseClass)picker.SelectedItem;
                //WarehouseClassCode = selected.Code;
            }
        }

        private void SpecialEquipmentSelector(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex >= 0)
            {
                SpecialEquipment selected = (SpecialEquipment)picker.SelectedItem;
                //SpecialEquipmentCode = selected.Code;
            }
        }
    }
}