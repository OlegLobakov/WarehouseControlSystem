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
using WarehouseControlSystem.Resx;

namespace WarehouseControlSystem.View.Pages.Contacts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public string Version { get; set; }

        public AboutPage()
        {
            Version = "Version: "+Plugin.DeviceInfo.CrossDeviceInfo.Current.AppBuild;
            BindingContext = this;
            InitializeComponent();
            Title = AppResources.AboutPage_Title;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MainParallax.DestroyParallaxView();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ContactPage());
        }

        private void Button_ProjectSiteClicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://github.com/OlegLobakov/WarehouseControlSystem"));
        }
    }
}