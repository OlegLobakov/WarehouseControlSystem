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

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.Resx;
using WarehouseControlSystem.ViewModel;

namespace WarehouseControlSystem.View.Pages.BinTemplate
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BinTemplatesPage : ContentPage
    {
        BinTemplatesViewModel model;
        public BinTemplatesPage()
        {
            model = new BinTemplatesViewModel(Navigation);
            BindingContext = model;
            InitializeComponent();
            Title = AppResources.BinTemplatesPage_Title;
        }

        protected override bool OnBackButtonPressed()
        {
            model.Dispose();
            base.OnBackButtonPressed();
            return false;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            BinTemplateViewModel btvm = (BinTemplateViewModel)e.Item;
        }
    }
}