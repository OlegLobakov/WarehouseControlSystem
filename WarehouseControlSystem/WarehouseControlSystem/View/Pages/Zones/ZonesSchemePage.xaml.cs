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

            Model.IsEditMode = false;
            Model.SetEditModeForItems(Model.IsEditMode);
            Menu();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<ZonesPlanViewModel>(this, "Rebuild", Rebuild);
            MessagingCenter.Subscribe<ZonesPlanViewModel>(this, "Reshape", Reshape);

            PanGesture.PanUpdated += OnPaned;
            TapGesture.Tapped += GridTapped;
            await Model.Load();
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<ZonesPlanViewModel>(this, "Rebuild");
            MessagingCenter.Unsubscribe<ZonesPlanViewModel>(this, "Reshape");
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
                zvm.LoadIndicators();
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
            Menu();
        }

        ToolbarItem listbutton;
        ToolbarItem addbutton;
        ToolbarItem removebutton;
        ToolbarItem editbutton;
        ToolbarItem searchbutton;
        ToolbarItem indicatorsbutton;
        private void Menu()
        {
            if (Model.IsEditMode)
            {
                if (searchbutton is ToolbarItem)
                {
                    ToolbarItems.Remove(searchbutton);
                }

                if (indicatorsbutton is ToolbarItem)
                {
                    ToolbarItems.Remove(indicatorsbutton);
                }

                if (addbutton is null)
                {
                    addbutton = new ToolbarItem()
                    {
                        Order = ToolbarItemOrder.Primary,
                        Priority = 10,
                        Text = AppResources.ZonesSchemePage_Toolbar_New,
                        Icon = new FileImageSource()
                    };
                    addbutton.Icon.File = Global.GetPlatformPath("ic_action_add_circle.png");
                    addbutton.SetBinding(MenuItem.CommandProperty, new Binding("NewZoneCommand"));
                }
                ToolbarItems.Add(addbutton);

                if (removebutton is null)
                {
                    removebutton = new ToolbarItem()
                    {
                        Order = ToolbarItemOrder.Primary,
                        Priority = 20,
                        Text = AppResources.LocationsSchemePage_Toolbar_Delete,
                        Icon = new FileImageSource()
                    };
                    removebutton.Icon.File = Global.GetPlatformPath("ic_action_remove_circle.png");
                    removebutton.SetBinding(MenuItem.CommandProperty, new Binding("DeleteZoneCommand"));
                    removebutton.SetBinding(MenuItem.CommandParameterProperty, new Binding("SelectedZoneViewModel"));
                }
                ToolbarItems.Add(removebutton);


                if (editbutton is null)
                {
                    editbutton = new ToolbarItem()
                    {
                        Order = ToolbarItemOrder.Primary,
                        Priority = 20,
                        Text = AppResources.LocationsSchemePage_Toolbar_Edit,
                        Icon = new FileImageSource()
                    };
                    editbutton.Icon.File = Global.GetPlatformPath("ic_action_create.png");
                    editbutton.SetBinding(MenuItem.CommandProperty, new Binding("EditZoneCommand"));
                    editbutton.SetBinding(MenuItem.CommandParameterProperty, new Binding("SelectedZoneViewModel"));
                }
                ToolbarItems.Add(editbutton);

                if (listbutton is null)
                {
                    listbutton = new ToolbarItem()
                    {
                        Order = ToolbarItemOrder.Primary,
                        Priority = 30,
                        Text = AppResources.ZonesSchemePage_Toolbar_List,
                        Icon = new FileImageSource()
                    };
                    listbutton.Icon.File = Global.GetPlatformPath("ic_action_dehaze.png");
                    listbutton.SetBinding(MenuItem.CommandProperty, new Binding("ListZonesCommand"));
                }
                ToolbarItems.Add(listbutton);
            }
            else
            {
                if (listbutton is ToolbarItem)
                {
                    ToolbarItems.Remove(listbutton);
                }

                if (addbutton is ToolbarItem)
                {
                    ToolbarItems.Remove(addbutton);
                }

                if (removebutton is ToolbarItem)
                {
                    ToolbarItems.Remove(removebutton);
                }

                if (editbutton is ToolbarItem)
                {
                    ToolbarItems.Remove(editbutton);
                }

                if (searchbutton is null)
                {
                    searchbutton = new ToolbarItem()
                    {
                        Order = ToolbarItemOrder.Primary,
                        Priority = 30,
                        Text = AppResources.ZonesSchemePage_Toolbar_Search,
                        Icon = new FileImageSource()
                    };
                    searchbutton.Icon.File = Global.GetPlatformPath("ic_action_search.png");
                    searchbutton.Clicked += ToolbarItem_Search;
                }
                ToolbarItems.Add(searchbutton);

                if (indicatorsbutton is null)
                {
                    indicatorsbutton = new ToolbarItem()
                    {
                        Order = ToolbarItemOrder.Primary,
                        Priority = 40,
                        Text = AppResources.LocationsSchemePage_Toolbar_Indicators,
                        Icon = new FileImageSource()
                    };
                    indicatorsbutton.Icon.File = Global.GetPlatformPath("ic_action_dashboard.png");
                    indicatorsbutton.SetBinding(MenuItem.CommandProperty, new Binding("IndicatorsViewCommand"));
                }
                ToolbarItems.Add(indicatorsbutton);
            }
        }
    }
}