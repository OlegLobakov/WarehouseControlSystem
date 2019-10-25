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
using WarehouseControlSystem.Resx;

namespace WarehouseControlSystem.View.Pages.Locations
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
            Menu();
        }

        protected override async void OnAppearing()
        { 
            base.OnAppearing();
            PanGesture.PanUpdated += OnPaned;
            TapGesture.Tapped += GridTapped;
            MessagingCenter.Subscribe<LocationsPlanViewModel>(this, "Rebuild", Rebuild);
            MessagingCenter.Subscribe<LocationsPlanViewModel>(this, "Reshape", Reshape);
            MessagingCenter.Subscribe<LocationsPlanViewModel>(this, "BringToFront", BringToFront);
            await Model.Load();
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<LocationsPlanViewModel>(this, "Rebuild");
            MessagingCenter.Unsubscribe<LocationsPlanViewModel>(this, "Reshape");
            MessagingCenter.Unsubscribe<LocationsPlanViewModel>(this, "BringToFront");
            PanGesture.PanUpdated -= OnPaned;
            TapGesture.Tapped -= GridTapped;
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            Model.DisposeModel();
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
                lvm1.LoadIndicators();
            }
        }

        private void Reshape(LocationsPlanViewModel lmv)
        {
            Reshape();
        }

        public void BringToFront(LocationsPlanViewModel obj)
        {
            foreach (SchemeBaseView lv in Views)
            {
                if (lv.Model.IsSelected)
                {
                    abslayout.RaiseChild(lv);
                }
            }
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
            Menu();
        }

        ToolbarItem listbutton;
        ToolbarItem addbutton;
        ToolbarItem removebutton;
        ToolbarItem editbutton;
        ToolbarItem indicatorsbutton;

        private void Menu()
        {
            if (Model.IsEditMode)
            {
                if (indicatorsbutton is ToolbarItem)
                {
                    ToolbarItems.Remove(indicatorsbutton);
                }

                if (listbutton is null)
                {
                    listbutton = new ToolbarItem()
                    {
                        Order = ToolbarItemOrder.Primary,
                        Priority = 30,
                        Text = AppResources.LocationsSchemePage_Toolbar_List,
                        IconImageSource = new FileImageSource()
                        {
                            File = Global.GetPlatformPath("ic_action_dehaze.png")
                        }
                    };
                    listbutton.SetBinding(MenuItem.CommandProperty, new Binding("ListLocationsCommand"));
                }
                ToolbarItems.Add(listbutton);

                if (addbutton is null)
                {
                    addbutton = new ToolbarItem()
                    {
                        Order = ToolbarItemOrder.Primary,
                        Priority = 10,
                        Text = AppResources.LocationsSchemePage_Toolbar_New,
                        IconImageSource = new FileImageSource()
                        {
                            File = Global.GetPlatformPath("ic_action_add_circle.png")
                        }
                    };
                    addbutton.SetBinding(MenuItem.CommandProperty, new Binding("NewLocationCommand"));
                }
                ToolbarItems.Add(addbutton);

                if (removebutton is null)
                {
                    removebutton = new ToolbarItem()
                    {
                        Order = ToolbarItemOrder.Primary,
                        Priority = 20,
                        Text = AppResources.LocationsSchemePage_Toolbar_Delete,
                        IconImageSource = new FileImageSource()
                        {
                            File = Global.GetPlatformPath("ic_action_remove_circle.png")
                        }
                    };
                    removebutton.SetBinding(MenuItem.CommandProperty, new Binding("DeleteLocationCommand"));
                    removebutton.SetBinding(MenuItem.CommandParameterProperty, new Binding("SelectedLocationViewModel"));
                }
                ToolbarItems.Add(removebutton);


                if (editbutton is null)
                {
                    editbutton = new ToolbarItem()
                    {
                        Order = ToolbarItemOrder.Primary,
                        Priority = 20,
                        Text = AppResources.LocationsSchemePage_Toolbar_Edit,
                        IconImageSource = new FileImageSource()
                        {
                            File = Global.GetPlatformPath("ic_action_create.png")
                        }
                    };
                    editbutton.SetBinding(MenuItem.CommandProperty, new Binding("EditLocationCommand"));
                    editbutton.SetBinding(MenuItem.CommandParameterProperty, new Binding("SelectedLocationViewModel"));
                }
                ToolbarItems.Add(editbutton);
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

                if (indicatorsbutton is null)
                {
                    indicatorsbutton = new ToolbarItem()
                    {
                        Order = ToolbarItemOrder.Primary,
                        Priority = 20,
                        Text = AppResources.LocationsSchemePage_Toolbar_Indicators,
                        IconImageSource = new FileImageSource()
                        {
                            File = Global.GetPlatformPath("ic_action_dashboard.png")
                        }
                    };
                    indicatorsbutton.SetBinding(MenuItem.CommandProperty, new Binding("IndicatorsViewCommand"));
                }
                ToolbarItems.Add(indicatorsbutton);
            }
        }
    }
}