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
using WarehouseControlSystem.View.Pages.Locations;
using WarehouseControlSystem.View.Pages.Contacts;
using WarehouseControlSystem.View.Pages.Connections;
using WarehouseControlSystem.View.Pages.Parameters;
using WarehouseControlSystem.ViewModel;
using System.Threading.Tasks;

namespace WarehouseControlSystem
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageDetail : ContentPage
    {
        public bool IsDevicePhone
        {
            get { return isdevicephone; }
            set
            {
                if (isdevicephone != value)
                {
                    isdevicephone = value;
                    OnPropertyChanged(nameof(IsDevicePhone));
                }
            }
        }
        bool isdevicephone;

        public MainPageDetail()
        {
            InitializeComponent();
            BindingContext = this;
            if (Device.Idiom == TargetIdiom.Phone)
            {
                IsDevicePhone = true;
            }
        }

        private async Task LocationsTaped()
        {
            LocationsPlanViewModel lpvm = new LocationsPlanViewModel(Navigation);
            LocationsSchemePage lsp = new LocationsSchemePage(lpvm);
            await Navigation.PushAsync(lsp);
        }

        private async Task AboutTaped()
        {
            await Navigation.PushAsync(new AboutPage());
        }

        private async Task ConnectionsTaped()
        {
            ConnectionsPage cp = new ConnectionsPage();
            await Navigation.PushAsync(cp);
        }

        private async Task OptionsTaped()
        {
            await Navigation.PushAsync(new ParametersPage());
        }
    }
}