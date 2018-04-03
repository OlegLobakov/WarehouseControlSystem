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

namespace WarehouseControlSystem.View.Pages.Contacts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactPage : ContentPage
    {
        ContactViewModel model;

        public ContactPage()
        {
            model = new ContactViewModel(Navigation);
            BindingContext = model;
            InitializeComponent();
            Title = AppResources.ContactPage_Title;

            messagetypepicker.ItemsSource = model.MessageTypes;
        }

        protected override bool OnBackButtonPressed()
        {
            BindingContext = null;
            model.Dispose();
            base.OnBackButtonPressed();
            return false;
        }

        private void ButtonSendClicked(object sender, EventArgs e)
        {
            model.Send();
        }

        private async void ButtonCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex >= 0)
            {
                model.MessageType = (string)picker.SelectedItem;
            }
        }
    }
}