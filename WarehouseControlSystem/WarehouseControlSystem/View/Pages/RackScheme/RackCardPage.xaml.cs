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
using System.Threading;
using WarehouseControlSystem.View.Pages.Find;
using WarehouseControlSystem.ViewModel.Base;
using System.Threading.Tasks;

namespace WarehouseControlSystem.View.Pages.RackScheme
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RackCardPage : ContentPage
    {
        private readonly RackViewModel model;
        private bool ScaleMode { get; set; }

        public RackCardPage(RackViewModel rvm)
        {
            model = rvm;
            BindingContext = model;
            InitializeComponent();
            ScaleMode = false;
            Title = AppResources.RackCardPage_Title + " " + model.No;
            MessagingCenter.Subscribe<BinsViewModel>(this, "BinsIsLoaded", BinsIsLoaded);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            model.State = ModelState.Loading;
            model.LoadingText = AppResources.RackCardPage_LoadingText;           
            await model.LoadBins();
            await model.LoadUDF();
        }

        protected override bool OnBackButtonPressed()
        {
            model.CancelAsync();
            base.OnBackButtonPressed();
            return false;
        }

        private void StackLayout_SizeChanged(object sender, EventArgs e)
        {
            StackLayout sl = (StackLayout)sender;
            rackview.BinWidth = (int)sl.Width / 8;
        }

        public void BinsIsLoaded(BinsViewModel bvm)
        {
            model.State = ModelState.Normal;
            model.GetSearchText();
            rackview.Update(model);
        }

        public void BinInfopanelItemTap(BinContentShortViewModel bcsvm)
        {
            Global.CompliantPlug = bcsvm.ToString();
        }

        public async Task UserDefinedFunctionTap(UserDefinedFunctionViewModel udfvm)
        {
            await model.RunUserDefineFunction(udfvm);
        }

        private async void Button_Clicked_OkMessage(object sender, EventArgs e)
        {
            await model.RunUserDefineFunctionOK();
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

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (ScaleMode)
            {
                rackscrollview.VerticalOptions = LayoutOptions.FillAndExpand;
                rackview.BinWidth = (int)mainsl.Width / 8;
                rackview.Update(model);
                ScaleMode = false;
            }
            else
            {
                rackscrollview.VerticalOptions = LayoutOptions.CenterAndExpand;
                rackview.BinWidth = (int)(mainsl.Width / (model.Sections + 3));
                rackview.HeightRequest = (rackview.BinWidth * 1.5) * (model.Levels + 1);
                rackview.Update(model);
                ScaleMode = true;
            }
        }
    }
}