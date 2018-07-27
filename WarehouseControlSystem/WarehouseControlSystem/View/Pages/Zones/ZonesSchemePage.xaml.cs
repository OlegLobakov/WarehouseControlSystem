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
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.Resx;
using WarehouseControlSystem.ViewModel;
using WarehouseControlSystem.View.Pages.Find;
using WarehouseControlSystem.View.Pages.Base;

namespace WarehouseControlSystem.View.Pages.Zones
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZonesSchemePage : SchemeBasePlanPage
    {
        private readonly ZonesPlanViewModel Model;

        public ZonesSchemePage(ZonesPlanViewModel model):base(model)
        {
            Model = model;
            InitializeComponent();

            abslayout.GestureRecognizers.Add(TapGesture);
            abslayout.GestureRecognizers.Add(PanGesture);

            Title = AppResources.ZoneSchemePage_Title + " - " + Model.Location.Name;
            Global.CurrentLocationName = Model.Location.Name;

            MessagingCenter.Subscribe<ZonesPlanViewModel>(this, "Rebuild", Rebuild);
            MessagingCenter.Subscribe<ZonesPlanViewModel>(this, "Reshape", Reshape);

            Model.IsEditMode = false;
            Model.SetEditModeForItems(Model.IsEditMode);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            PanGesture.PanUpdated += OnPaned;
            TapGesture.Tapped += GridTapped;
            await Model.Load();
        }

        protected override void OnDisappearing()
        {
            Model.State = ViewModel.Base.ModelState.Undefined;
            PanGesture.PanUpdated -= OnPaned;
            TapGesture.Tapped -= GridTapped;
            SelectedViews.Clear();
            Views.Clear();
            abslayout.Children.Clear();
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            Model.DisposeModel();
            MessagingCenter.Unsubscribe<ZonesPlanViewModel>(this, "Rebuild");
            MessagingCenter.Unsubscribe<ZonesPlanViewModel>(this, "Reshape");
            base.OnBackButtonPressed();
            return false;
        }

        private void Rebuild(ZonesPlanViewModel lmv)
        {
            SelectedViews.Clear();
            abslayout.Children.Clear();
            Views.Clear();
            foreach (ZoneViewModel zvm in Model.ZoneViewModels)
            {
                ZoneView zv = new ZoneView(zvm);
                AbsoluteLayout.SetLayoutBounds(zv, new Rectangle(zvm.ViewLeft, zvm.ViewTop, zvm.ViewWidth, zvm.ViewHeight));
                abslayout.Children.Add(zv);
                Views.Add(zv);
                zvm.LoadRacks();
            }
        }

        private void Reshape(ZonesPlanViewModel rsmv)
        {
            Reshape();
        }

        private void GridTapped(object sender, EventArgs e)
        {
            foreach (SchemeBaseView zv in Views)
            {
                zv.Opacity = 1;
            }
            Model.UnSelectAll();
        }

        private async void ToolbarItem_Search(object sender, EventArgs e)
        {
            SearchViewModel svm = new SearchViewModel(Navigation);
            FindPage fp = new FindPage(svm);
            await Navigation.PushAsync(fp);
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            GridTapped(null, new EventArgs());

            if (Model.IsEditMode)
            {
                Model.IsEditMode = false;
                Model.SetEditModeForItems(Model.IsEditMode);
                abslayout.BackgroundColor = Color.White;
                await Model.SaveLocationParams();
            }
            else
            {
                abslayout.BackgroundColor = Color.LightGray;
                Model.IsEditMode = true;
                Model.SetEditModeForItems(Model.IsEditMode);
            }
        }
    }
}