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
using WarehouseControlSystem.ViewModel;
using ZXing.Net.Mobile.Forms;

namespace WarehouseControlSystem.View.Pages.Find
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FindPage : ContentPage
    {
        ZXingScannerPage scanPage = null;

        SearchViewModel model;
        public FindPage(SearchViewModel svm)
        {
            model = svm;
            BindingContext = model;
            InitializeComponent();
            Title = AppResources.FindPage_Title;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            model.State = ViewModel.Base.ModelState.Normal;
            if (!string.IsNullOrEmpty(Global.SearchRequest))
            {
                entry.Text = Global.SearchRequest;
            }
            entry.Focus();
        }

        public async void ButtonScanClick(object sender, EventArgs e)
        {
            scanPage = new ZXingScannerPage();
            scanPage.AutoFocus();
            scanPage.OnScanResult += (result) =>
            {
                scanPage.IsScanning = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopAsync();
                    entry.Text = result.Text;
                    scanPage = null;
                });
            };

            await Navigation.PushAsync(scanPage);
        }


        public async void ButtonFindClick(object sender, EventArgs e)
        {
            await model.Search(entry.Text);
        }

        private async void entry_Completed(object sender, EventArgs e)
        {
            await model.Search(entry.Text);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            entry.Text = "";
            model.Clear();
        }
    }
}