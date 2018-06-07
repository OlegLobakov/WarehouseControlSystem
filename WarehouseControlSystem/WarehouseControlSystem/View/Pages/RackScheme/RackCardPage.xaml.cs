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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.Resx;
using WarehouseControlSystem.ViewModel;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.Helpers.NAV;
using WarehouseControlSystem.Helpers.Containers.StateContainer;
using System.Threading;
using WarehouseControlSystem.View.Pages.Find;

namespace WarehouseControlSystem.View.Pages.RackScheme
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RackCardPage : ContentPage
    {
        private readonly RackViewModel model;
        public RackCardPage(RackViewModel rvm)
        {
            model = rvm;
            BindingContext = model;
            InitializeComponent();

            Title = AppResources.RackCardPage_Title + " " + model.No;

            infopanel.BindingContext = model.BinsViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<BinsViewModel>(this, "BinsIsLoaded", BinsIsLoaded);
            model.State = Helpers.Containers.StateContainer.State.Loading;
            model.LoadAnimation = true;
            model.LoadingText = AppResources.RackCardPage_LoadingText;
            model.LoadBins();         
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<BinsViewModel>(this, "BinsIsLoaded");
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            model.CancelAsync();
            base.OnBackButtonPressed();
            return false;
        }

        public void BinsIsLoaded(BinsViewModel bvm)
        {
            model.LoadAnimation = false;
            model.State = Helpers.Containers.StateContainer.State.Normal;
            model.GetSearchText();
        }

        public void AfterStateSet(StateContainer sc)
        {
            if (model.State == State.Normal)
            {
                rackview.Update(model);
                //model.LoadContent();
                model.LoadUDF();
            }
        }

        public void BinInfopanelItemTap(BinContentShortViewModel bcsvm)
        {
            Global.CompliantPlug = bcsvm.ToString();
        }

        public void UserDefinedFunctionTap(UserDefinedFunctionViewModel udfvm)
        {
            model.RunUserDefineFunction(udfvm);
        }

        private void Button_Clicked_OkMessage(object sender, EventArgs e)
        {
            model.RunUserDefineFunctionOK();
        }

        private void Button_Clicked_CancelMessage(object sender, EventArgs e)
        {
            model.RunUserDefineFunctionCancel();
        }

        private async void ToolbarItem_Search(object sender, EventArgs e)
        {
            SearchViewModel svm = new SearchViewModel(Navigation);
            FindPage fp = new FindPage(svm);
            await Navigation.PushAsync(fp);
        }
    }
}