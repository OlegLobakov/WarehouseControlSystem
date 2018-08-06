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
using WarehouseControlSystem.Helpers.NAV;

namespace WarehouseControlSystem.View.Pages.Connections
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewConnectionPage : ContentPage
    {
        ConnectionViewModel model;

        public NewConnectionPage(ConnectionViewModel cvm, bool createmode)
        {
            model = cvm;
            BindingContext = model;
            InitializeComponent();
            model.CreateMode = createmode;
            creditialspicker.SelectedItem = cvm.CreditialList.Find(x => x == cvm.ClientCredentialType);
            Title = AppResources.NewConnectionPage_Title;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            model.State = ViewModel.Base.ModelState.Normal;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            model.SaveChanges();
        }

        protected override bool OnBackButtonPressed()
        {  
            base.OnBackButtonPressed();
            return false;
        }

        private void creditialspicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            if (picker.SelectedIndex == -1)
            {
                model.ClientCredentialType = ClientCredentialTypeEnum.Basic;
            }
            else
            {
                model.ClientCredentialType = (ClientCredentialTypeEnum)picker.SelectedItem;
            }
        }
    }
}