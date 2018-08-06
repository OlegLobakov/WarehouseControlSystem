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

namespace WarehouseControlSystem.View.Pages.Racks.Scheme
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

            Model.IsEditMode = false;
            Model.SetEditModeForItems(Model.IsEditMode);
            Menu();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            PanGesture.PanUpdated += OnPaned;
            TapGesture.Tapped += GridTapped;
            MessagingCenter.Subscribe<RacksPlanViewModel>(this, "Rebuild", Rebuild);
            MessagingCenter.Subscribe<RacksPlanViewModel>(this, "Reshape", Reshape);
            MessagingCenter.Subscribe<RacksPlanViewModel>(this, "UDSRunIsDone", UDSRunIsDone);
            MessagingCenter.Subscribe<RacksPlanViewModel>(this, "UDSListIsLoaded", UDSListIsLoaded);

            await Model.Load();
            await Model.LoadUDS();
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<RacksPlanViewModel>(this, "Rebuild");
            MessagingCenter.Unsubscribe<RacksPlanViewModel>(this, "Reshape");
            MessagingCenter.Unsubscribe<RacksPlanViewModel>(this, "UDSRunIsDone");
            MessagingCenter.Unsubscribe<RacksPlanViewModel>(this, "UDSListIsLoaded");
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
            Menu();
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

        ToolbarItem listbutton;
        ToolbarItem addbutton;
        ToolbarItem removebutton;
        ToolbarItem editbutton;
        ToolbarItem searchbutton;
        ToolbarItem udcsbutton;
        private void Menu()
        {
            if (Model.IsEditMode)
            {
                if (searchbutton is ToolbarItem)
                {
                    ToolbarItems.Remove(searchbutton);
                }

                if (udcsbutton is ToolbarItem)
                {
                    ToolbarItems.Remove(udcsbutton);
                }

                if (addbutton is null)
                {
                    addbutton = new ToolbarItem()
                    {
                        Order = ToolbarItemOrder.Primary,
                        Priority = 10,
                        Text = AppResources.RackSchemePage_Toolbar_New,
                        Icon = new FileImageSource()
                    };
                    addbutton.Icon.File = Global.GetPlatformPath("ic_action_add_circle.png");
                    addbutton.SetBinding(MenuItem.CommandProperty, new Binding("NewRackCommand"));
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
                    removebutton.SetBinding(MenuItem.CommandProperty, new Binding("DeleteRackCommand"));
                    removebutton.SetBinding(MenuItem.CommandParameterProperty, new Binding("SelectedRackViewModel"));
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
                    editbutton.SetBinding(MenuItem.CommandProperty, new Binding("EditRackCommand"));
                    editbutton.SetBinding(MenuItem.CommandParameterProperty, new Binding("SelectedRackViewModel"));
                }
                ToolbarItems.Add(editbutton);

                if (listbutton is null)
                {
                    listbutton = new ToolbarItem()
                    {
                        Order = ToolbarItemOrder.Primary,
                        Priority = 30,
                        Text = AppResources.RackSchemePage_Toolbar_List,
                        Icon = new FileImageSource()
                    };
                    listbutton.Icon.File = Global.GetPlatformPath("ic_action_dehaze.png");
                    listbutton.SetBinding(MenuItem.CommandProperty, new Binding("RackListCommand"));
                }
                ToolbarItems.Add(listbutton);
            }
            else
            {
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

                if (listbutton is ToolbarItem)
                {
                    ToolbarItems.Remove(listbutton);
                }

                if (searchbutton is null)
                {
                    searchbutton = new ToolbarItem()
                    {
                        Order = ToolbarItemOrder.Primary,
                        Priority = 30,
                        Text = AppResources.RackSchemePage_Toolbar_Search,
                        Icon = new FileImageSource()
                    };
                    searchbutton.Icon.File = Global.GetPlatformPath("ic_action_search.png");
                    searchbutton.Clicked += ToolbarItem_Search;
                }
                ToolbarItems.Add(searchbutton);

                if (udcsbutton is null)
                {
                    udcsbutton = new ToolbarItem()
                    {
                        Order = ToolbarItemOrder.Primary,
                        Priority = 40,
                        Text = AppResources.RackSchemePage_Toolbar_UDSF,
                        Icon = new FileImageSource()
                    };
                    udcsbutton.Icon.File = Global.GetPlatformPath("ic_action_widgets.png");
                    udcsbutton.Clicked += ToolbarItem_UDS;
                }
                ToolbarItems.Add(udcsbutton);
            }
        }
    }
}