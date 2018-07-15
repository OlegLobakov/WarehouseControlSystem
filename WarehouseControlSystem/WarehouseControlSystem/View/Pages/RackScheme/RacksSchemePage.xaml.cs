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
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.ViewModel;
using WarehouseControlSystem.View.Pages.Find;
using WarehouseControlSystem.View.Pages.Base;

namespace WarehouseControlSystem.View.Pages.RackScheme
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RacksSchemePage : SchemeBasePlanPage
    {
        private readonly RacksPlanViewModel Model;

        public RacksSchemePage(RacksPlanViewModel model) : base(model)
        {
            Model = model;
            InitializeComponent();

            abslayout.GestureRecognizers.Add(TapGesture);
            abslayout.GestureRecognizers.Add(PanGesture);

            Title = AppResources.ZoneSchemePage_Title +" "+ Global.CurrentLocationName+" | " + AppResources.RackSchemePage_Title + " - " + Model.Zone.Description;

            MessagingCenter.Subscribe<RacksPlanViewModel>(this, "Rebuild", Rebuild);
            MessagingCenter.Subscribe<RacksPlanViewModel>(this, "Reshape", Reshape);
            MessagingCenter.Subscribe<RacksPlanViewModel>(this, "UDSRunIsDone", UDSRunIsDone);
            MessagingCenter.Subscribe<RacksPlanViewModel>(this, "UDSListIsLoaded", UDSListIsLoaded);

            Model.IsEditMode = false;
            Model.SetEditModeForItems(Model.IsEditMode);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            PanGesture.PanUpdated += OnPaned;
            TapGesture.Tapped += GridTapped;
            await Model.Load();
            await Model.LoadUDS();
        }

        protected override void OnDisappearing()
        {
            Model.State = ViewModel.Base.ModelState.Undefined;
            PanGesture.PanUpdated -= OnPaned;
            TapGesture.Tapped -= GridTapped;
            SelectedViews.Clear();
            abslayout.Children.Clear();
            Views.Clear();
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            Model.DisposeModel();
            MessagingCenter.Unsubscribe<RacksPlanViewModel>(this, "Rebuild");
            MessagingCenter.Unsubscribe<RacksPlanViewModel>(this, "Reshape");
            MessagingCenter.Unsubscribe<RacksPlanViewModel>(this, "UDSRunIsDone");
            MessagingCenter.Unsubscribe<RacksPlanViewModel>(this, "UDSListIsLoaded");
            base.OnBackButtonPressed();
            return false;
        }

        private void RacksStackLayoutSizeChanged(object sender, EventArgs e)
        {
            StackLayout sl = (StackLayout)sender;
            Model.SetScreenSizes(sl.Width, sl.Height, false);
            Model.UDSPanelHeight = (int)Math.Round(sl.Height / 5.5);
        }

        private void RacksAbslayoutSizeChanged(object sender, EventArgs e)
        {
            AbsoluteLayout al = (AbsoluteLayout)sender;
            Model.SetScreenSizes(al.Width, al.Height, true);
        }

        private void Rebuild(RacksPlanViewModel rsmv)
        {
            SelectedViews.Clear();
            abslayout.Children.Clear();
            Views.Clear();
            foreach (RackViewModel rvm in Model.RackViewModels)
            {
                RackSchemeView rsv = new RackSchemeView(rvm);
                AbsoluteLayout.SetLayoutBounds(rsv, new Rectangle(rvm.ViewLeft, rvm.ViewTop, rvm.ViewWidth, rvm.ViewHeight));
                abslayout.Children.Add(rsv);
                Views.Add(rsv);
            }
        }

        private void Reshape(RacksPlanViewModel rsmv)
        {
            Reshape();
        }

        private void UDSRunIsDone(RacksPlanViewModel zmv)
        {
            foreach (SchemeBaseView lv in Views)
            {
                RackSchemeView rsv = (RackSchemeView)lv;
                rsv.UpdateUDS();
            }
        }

        private void UDSListIsLoaded(RacksPlanViewModel rvm)
        {
            hlv.ItemsSource = Model.UserDefinedSelectionViewModels;
        }

        private void OnSearch(SearchViewModel svm)
        {
            System.Diagnostics.Debug.WriteLine(svm.ToString());
        }

        private void GridTapped(object sender, EventArgs e)
        {
            foreach (SchemeBaseView rsv in Views)
            {
                rsv.Opacity = 1;
            }
            Model.UnSelectAll();
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            GridTapped(null, new EventArgs());

            if (Model.IsEditMode)
            {
                Model.IsEditMode = false;
                Model.SetEditModeForItems(Model.IsEditMode);
                abslayout.BackgroundColor = Color.White;
                await Model.SaveZoneParams();
            }
            else
            {
                Model.UnSelectAll();
                abslayout.BackgroundColor = Color.LightGray;
                Model.IsEditMode = true;
                Model.SetEditModeForItems(Model.IsEditMode);
                Model.Rebuild(false);
            }
        }

        private void ToolbarItem_UDS(object sender, EventArgs e)
        {
            Model.IsVisibleUDS = !Model.IsVisibleUDS;
        }

        private async void ToolbarItem_Search(object sender, EventArgs e)
        {
            SearchViewModel svm = new SearchViewModel(Navigation);
            FindPage fp = new FindPage(svm);
            await Navigation.PushAsync(fp);
        }
   }
}