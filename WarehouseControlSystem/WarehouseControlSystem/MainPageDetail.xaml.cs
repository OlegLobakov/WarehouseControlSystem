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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WarehouseControlSystem.View.Pages.LocationsScheme;
using WarehouseControlSystem.View.Pages.Contacts;
using WarehouseControlSystem.View.Pages.Connections;
using WarehouseControlSystem.View.Pages.Parameters;
using WarehouseControlSystem.View.Pages.RackScheme;
using WarehouseControlSystem.View.Pages.ZonesScheme;
using WarehouseControlSystem.ViewModel;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.View.Pages.Find;
using System;
using WarehouseControlSystem.Helpers.NAV;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Collections.Generic;

namespace WarehouseControlSystem
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageDetail : ContentPage
    {

        public MainPageDetail()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void LocationsTaped()
        {
            await Navigation.PushAsync(new LocationsSchemePage());
        }

        private async void AboutTaped()
        {
            await Navigation.PushAsync(new AboutPage());
        }

        private async void ConnectionsTaped()
        {
            ConnectionsPage cp = new ConnectionsPage();
            await Navigation.PushAsync(cp);
        }

        private async void OptionsTaped()
        {
            await Navigation.PushAsync(new ParametersPage());
        }

        private async void AddLocation()
        {
            Location location = new Location();
            LocationViewModel lvm = new LocationViewModel(Navigation, location);
            lvm.CreateMode = true;
            await Navigation.PushAsync(new LocationCardPage(lvm));
        }

        private async void AddZoneTaped()
        {
            Zone zone = new Zone();
            ZoneViewModel zvm = new ZoneViewModel(Navigation, zone);
            zvm.CreateMode = true;
            zvm.CanChangeLocationCode = true;
            await Navigation.PushAsync(new ZoneCardPage(zvm));
        }

        private async void AddRackTaped()
        {
            Rack newrack = new Rack();
            newrack.Sections = Settings.DefaultRackSections;
            newrack.Levels = Settings.DefaultRackLevels;
            newrack.Depth = Settings.DefaultRackDepth;
            newrack.SchemeVisible = true;
            RackViewModel rvm = new RackViewModel(Navigation, newrack, true);
            rvm.CanChangeLocationAndZone = true;
            await Navigation.PushAsync(new RackNewPage(rvm));
        }
    }
}