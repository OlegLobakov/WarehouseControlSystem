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
using WarehouseControlSystem.ViewModel;
using WarehouseControlSystem.Helpers.NAV;
using WarehouseControlSystem.Resx;

namespace WarehouseControlSystem.View.Pages.Connections
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConnectionsPage : ContentPage
	{
        ConnectionsViewModel model;
        public ConnectionsPage()
        {
            model = new ConnectionsViewModel(Navigation);
            BindingContext = model;
            InitializeComponent();
            Title = AppResources.ConnectionsPage_Title;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            Global.SaveParameters();
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return false;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ConnectionViewModel selected = (ConnectionViewModel)e.Item;
            model.Select(selected);
        }
    }
}