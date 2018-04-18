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
using WarehouseControlSystem.Resx;
using WarehouseControlSystem.ViewModel;
using WarehouseControlSystem.Model.NAV;

namespace WarehouseControlSystem.View.Pages.RackScheme
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RackListPage : ContentPage
    {
        Zone Zone;
        private readonly RacksViewModel model;
        public RackListPage(Zone zone)
        {
            Zone = zone;
            model = new RacksViewModel(Navigation, Zone);
            BindingContext = model;
            InitializeComponent();
            Title = AppResources.ZoneSchemePage_Title + " - " + Zone.LocationCode + " | " + AppResources.RackSchemePage_Title + " - " + Zone.Code;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            model.LoadAll();
        }

        protected override async void OnDisappearing()
        {
            await model.SaveRacksVisible();
            MessagingCenter.Send(model, "ReLoad");
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            BindingContext = null;
            //model.Dispose();
            base.OnBackButtonPressed();
            return false;
        }
    }
}