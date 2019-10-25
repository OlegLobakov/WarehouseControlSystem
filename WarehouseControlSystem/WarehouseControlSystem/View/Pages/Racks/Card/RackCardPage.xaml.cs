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
using System.Collections.ObjectModel;
using WarehouseControlSystem.View.Pages.Racks.Edit;

namespace WarehouseControlSystem.View.Pages.Racks.Card
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RackCardPage : ContentPage
    {
        public ObservableCollection<RackViewModel> LinkPlanRackViewModels
        {
            get { return linkplanrackviewmodels; }
            set
            {
                if (linkplanrackviewmodels != value)
                {
                    linkplanrackviewmodels = value;
                    OnPropertyChanged(nameof(LinkPlanRackViewModels));
                }
            }
        }  ObservableCollection<RackViewModel> linkplanrackviewmodels;

        private RackViewModel model;
        private bool ScaleMode { get; set; }

        public RackCardPage(RackViewModel rvm, ObservableCollection<RackViewModel> rsvm)
        {
            model = rvm;
            model.IsSelected = true;
            BindingContext = model;
            InitializeComponent();
            LinkPlanRackViewModels = rsvm;
            ScaleMode = false;
            Title = AppResources.RackCardPage_Title + " " + model.No;
            UpdateToolbarLabels();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            model.State = ModelState.Loading;
            model.LoadingText = AppResources.RackCardPage_LoadingText;
            MessagingCenter.Subscribe<BinsViewModel>(this, "BinsIsLoaded", BinsIsLoaded);
            MessagingCenter.Subscribe<BinViewModel>(this, "BinsViewModel.BinSelected", BinSelected);

            await model.LoadBins();
            await model.LoadUDF();
            await model.LoadBinValues();
            model.State = ModelState.Normal;
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<BinsViewModel>(this, "BinsIsLoaded");
            MessagingCenter.Unsubscribe<BinViewModel>(this, "BinsViewModel.BinSelected");
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            model.DisposeModel();
            base.OnBackButtonPressed();
            return false;
        }

        private void StackLayout_SizeChanged(object sender, EventArgs e)
        {
            StackLayout sl = (StackLayout)sender;
            rackview.BinWidth = (int)sl.Width / 10;
        }

        public void BinsIsLoaded(BinsViewModel bvm)
        {
            bvm.MultiSelectBins = Settings.MultiSelectBins;
            model.State = ModelState.Normal;
            model.GetSearchText();
            rackview.Update(model);
            model.LoadBinImages();
        }

        private async void BinSelected(BinViewModel bvm)
        {
            if (!ScaleMode)
            {
                BinView bv = rackview.GetBinView(bvm);
                if (bv is BinView)
                {
                    await rackscrollview.ScrollToAsync(bv, ScrollToPosition.Center, true);
                }
            }
        }

        public async void BinInfopanelItemTap(BinContentShortViewModel bcsvm)
        {
            await model.RunBinContentTap(bcsvm);
        }

        public async void UserDefinedFunctionTap(UserDefinedFunctionViewModel udfvm)
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
                rackview.BinWidth = (int)mainsl.Width / 10;
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

        private void ToolbarItem_UnSelect(object sender, EventArgs e)
        {
            model.BinsViewModel.UnSelect();
        }

        private void ToolbarItem_Clicked_ShowHideImages(object sender, EventArgs e)
        {
            Settings.ShowImages = !Settings.ShowImages;
            model.BinsViewModel.LoadContentImages(true);
            UpdateToolbarLabels();

            model.LoadBinImages();
        }

        private async void RackList_SelectedItemChanged(object sender, EventArgs e)
        {
            RackViewModel rvm = (RackViewModel)sender;
            if (rvm != model)
            {
                RackViewModel lastrvm = model;
                model.IsSelected = false;
                model = rvm;
                BindingContext = model;
                lastrvm.BinsViewModel.BinViewModelsDispose();
                model.IsSelected = true;
                model.State = ModelState.Loading;
                model.LoadingText = AppResources.RackCardPage_LoadingText;
                Title = AppResources.RackCardPage_Title + " " + model.No;
                await model.LoadBins();
                await model.LoadUDF();
                await model.LoadBinValues();
                await model.LoadBinImages();
            }
        }

        private async void ToolbarItem_RackEdit(object sender, EventArgs e)
        {
            model.IsEditMode = true;
            model.BinsViewModel.IsEditMode = true;
            await Navigation.PushAsync(new RackEditPage(model));
        }

        private void ToolbarItem_OnOffMultiSelect(object sender, EventArgs e)
        {
            Settings.MultiSelectBins = !Settings.MultiSelectBins;
            model.BinsViewModel.MultiSelectBins = Settings.MultiSelectBins;
            UpdateToolbarLabels();
        }

        private void UpdateToolbarLabels()
        {
            if (Settings.ShowImages)
            {
                ShowHideImagesToolbar.Text = AppResources.RackCardPage_Toolbar_HideImages;
            }
            else
            {
                ShowHideImagesToolbar.Text = AppResources.RackCardPage_Toolbar_ShowImages;
            }

            if (Settings.MultiSelectBins)
            {
                OnOffMultiSelectBins.Text = AppResources.RackCardPage_Toolbar_OffMultiSelectBins;
            }
            else
            {
                OnOffMultiSelectBins.Text = AppResources.RackCardPage_Toolbar_OnMultiSelectBins;
            }

        }
    }
}