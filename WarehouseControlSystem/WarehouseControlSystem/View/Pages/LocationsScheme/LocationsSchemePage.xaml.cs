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
using WarehouseControlSystem.ViewModel;
using System.Threading.Tasks;
using WarehouseControlSystem.View.Pages.Base;

namespace WarehouseControlSystem.View.Pages.LocationsScheme
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationsSchemePage : SchemeBasePlanPage
    {
        private readonly LocationsPlanViewModel Model;

        public LocationsSchemePage(LocationsPlanViewModel model) :base(model)
        {
            Model = model;
            InitializeComponent();

            abslayout.GestureRecognizers.Add(TapGesture);
            abslayout.GestureRecognizers.Add(PanGesture);

            MessagingCenter.Subscribe<LocationsPlanViewModel>(this, "Rebuild", Rebuild);
            MessagingCenter.Subscribe<LocationsPlanViewModel>(this, "Reshape", Reshape);
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
            PanGesture.PanUpdated -= OnPaned;
            TapGesture.Tapped -= GridTapped;
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            Model.DisposeModel();
            MessagingCenter.Unsubscribe<LocationsPlanViewModel>(this, "Rebuild");
            MessagingCenter.Unsubscribe<LocationsPlanViewModel>(this, "Reshape");
            base.OnBackButtonPressed();
            return false;
        }

        private void Rebuild(LocationsPlanViewModel lmv)
        {
            abslayout.Children.Clear();
            SelectedViews.Clear();
            Views.Clear();
            foreach (LocationViewModel lvm1 in Model.LocationViewModels)
            {
                LocationView lv = new LocationView(lvm1);
                AbsoluteLayout.SetLayoutBounds(lv, new Rectangle(lvm1.ViewLeft, lvm1.ViewTop, lvm1.ViewWidth, lvm1.ViewHeight));
                abslayout.Children.Add(lv);
                Views.Add(lv);
                lvm1.LoadZones();
            }
        }

        private void Reshape(LocationsPlanViewModel lmv)
        {
            Reshape();
        }

        private void GridTapped(object sender, EventArgs e)
        {
            foreach (SchemeBaseView lv in Views)
            {
                lv.Opacity = 1;
            }
            Model.UnSelectAll();
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            GridTapped(null, new EventArgs());

            if (Model.IsEditMode)
            {
                abslayout.BackgroundColor = Color.White;
                Model.IsEditMode = false;
                await Model.SaveSchemeParams();
            }
            else
            {
                abslayout.BackgroundColor = Color.LightGray;
                Model.IsEditMode = true;
            }
        }
    }
}