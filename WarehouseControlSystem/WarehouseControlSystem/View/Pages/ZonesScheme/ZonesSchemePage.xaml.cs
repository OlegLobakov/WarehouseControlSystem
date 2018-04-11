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

namespace WarehouseControlSystem.View.Pages.ZonesScheme
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZonesSchemePage : ContentPage
    {
        List<ZoneView> Views { get; set; }
        List<ZoneView> SelectedViews { get; set; }

        MovingActionTypeEnum MovingAction = MovingActionTypeEnum.None;

        TapGestureRecognizer TapGesture;
        PanGestureRecognizer PanGesture;

        ZonesViewModel model;
        public ZonesSchemePage(Location location)
        {
            model = new ZonesViewModel(Navigation, location);
            BindingContext = model;

            InitializeComponent();

            Views = new List<ZoneView>();
            SelectedViews = new List<ZoneView>();

            TapGesture = new TapGestureRecognizer();
            schemegrid.GestureRecognizers.Add(TapGesture);

            PanGesture = new PanGestureRecognizer();
            abslayout.GestureRecognizers.Add(PanGesture);

            Title = AppResources.ZoneSchemePage_Title + " - " + location.Name;

            MessagingCenter.Subscribe<ZonesViewModel>(this, "Rebuild", Rebuild);
            MessagingCenter.Subscribe<ZonesViewModel>(this, "ReLoad", Reload);
            PanGesture.PanUpdated += OnPaned;
            TapGesture.Tapped += GridTapped;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            model.Load();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            MessagingCenter.Unsubscribe<ZonesViewModel>(this, "Rebuild");
            MessagingCenter.Unsubscribe<ZonesViewModel>(this, "ReLoad");
            PanGesture.PanUpdated -= OnPaned;
            TapGesture.Tapped -= GridTapped;
            BindingContext = null;
            foreach (ZoneView lv in Views)
            {
                lv.BindingContext = null;
            }
            Views.Clear();
            model.Dispose();
            base.OnBackButtonPressed();
            return false;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            model.ScreenWidth = width - 10;
            model.ScreenHeight = height - 10;
        }

        private void Rebuild(ZonesViewModel lmv)
        {
            SelectedViews.Clear();
            foreach (ZoneView lv in Views)
            {
                abslayout.Children.Remove(lv);
            }
            Views.Clear();

            foreach (ZoneViewModel zvm in model.ZoneViewModels)
            {
                ZoneView zv = new ZoneView(zvm);
                AbsoluteLayout.SetLayoutBounds(zv,
                    new Rectangle(zvm.Left, zvm.Top, zvm.Width, zvm.Height));
                abslayout.Children.Add(zv);
                Views.Add(zv);
                zvm.LoadRacks();
            }
        }

        private void Reload(ZonesViewModel zmv)
        {
            model.Load();
        }

        private void GridTapped(object sender, EventArgs e)
        {
            model.UnSelectAll();
        }

        Easing easing1 = Easing.Linear;
        Easing easingParcking = Easing.CubicInOut;

        double x, y, widthstep, heightstep = 0;

        double leftborder = double.MaxValue;
        double topborder = double.MaxValue;
        double rightborder = double.MinValue;
        double bottomborder = double.MinValue;

        double oldeTotalX, oldeTotalY = 0;
        private async void OnPaned(object sender, PanUpdatedEventArgs e)
        {
            if (model.RunMode == RunModeEnum.View)
            {
                return;
            }

            if ((MovingAction != MovingActionTypeEnum.None) && (MovingAction != MovingActionTypeEnum.Pan))
            {
                return;
            }

            if (!model.IsSelectedList)
            {
                return;
            }

            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    {
                        SelectedViews = Views.FindAll(x => x.model.Selected == true);
                        MovingAction = MovingActionTypeEnum.Pan;

                        widthstep = (abslayout.Width / model.Location.PlanWidth);
                        heightstep = (abslayout.Height / model.Location.PlanHeight);

                        leftborder = double.MaxValue;
                        topborder = double.MaxValue;
                        rightborder = double.MinValue;
                        bottomborder = double.MinValue;

                        foreach (ZoneView zv in SelectedViews)
                        {
                            leftborder = Math.Min(zv.X, leftborder);
                            topborder = Math.Min(zv.Y, topborder);
                            rightborder = Math.Max(zv.X + zv.Width, rightborder);
                            bottomborder = Math.Max(zv.Y + zv.Height, bottomborder);
                            zv.Opacity = 0.5;
                            zv.model.SavePrevSize(zv.Width, zv.Height);
                        }

                        x += oldeTotalX;
                        y += oldeTotalY;
                        break;
                    }
                case GestureStatus.Running:
                    {
                        double dx = x + e.TotalX;
                        double dy = y + e.TotalY;

                        oldeTotalX = e.TotalX;
                        oldeTotalY = e.TotalY;

                        if (dx + leftborder < 0)
                        {
                            dx = -leftborder;
                        }

                        if (dx + rightborder > abslayout.Width)
                        {
                            dx = abslayout.Width - rightborder;
                        }

                        if (dy + topborder < 0)
                        {
                            dy = -topborder;
                        }

                        if (dy + bottomborder > abslayout.Height)
                        {
                            dy = abslayout.Height - bottomborder;
                        }

                        foreach (ZoneView zv in SelectedViews)
                        {
                            if (zv.model.EditMode == SchemeElementEditMode.Move)
                            {
                                await zv.TranslateTo(dx, dy, 250, easing1);
                            }
                            if (zv.model.EditMode == SchemeElementEditMode.Resize)
                            {
                                AbsoluteLayout.SetLayoutBounds(zv, new Rectangle(zv.X, zv.Y, zv.model.PrevWidth + dx, zv.model.PrevHeight + dy));
                            }
                        }
                        break;
                    }
                case GestureStatus.Completed:
                    {

                        x = 0;
                        y = 0;
                        oldeTotalX = 0;
                        oldeTotalY = 0;
                        foreach (ZoneView zv in SelectedViews)
                        {
                            if (zv.model.EditMode == SchemeElementEditMode.Move)
                            {
                                double newX = zv.X + zv.TranslationX;
                                double newY = zv.Y + zv.TranslationY;

                                zv.model.Zone.Left = (int)Math.Round(newX / widthstep);
                                zv.model.Zone.Top = (int)Math.Round(newY / heightstep);

                                //выравнивание по сетке
                                double dX = zv.model.Zone.Left * widthstep - zv.X;
                                double dY = zv.model.Zone.Top * heightstep - zv.Y;

                                await zv.TranslateTo(dX, dY, 500, easingParcking);
                                //lv.Layout(new Rectangle(lv.X + dX, lv.Y + dY, lv.Width, lv.Height)); //в таком варианте почемуто есть глюк при переключении режима редактирования
                                AbsoluteLayout.SetLayoutBounds(zv, new Rectangle(zv.X + dX, zv.Y + dY, zv.Width, zv.Height));
                                zv.TranslationX = 0;
                                zv.TranslationY = 0;
                            }
                            if (zv.model.EditMode == SchemeElementEditMode.Resize)
                            {
                                zv.model.Zone.Width = (int)Math.Round(zv.Width / widthstep);
                                zv.model.Zone.Height = (int)Math.Round(zv.Height / heightstep);
                                double newWidth = zv.model.Zone.Width * widthstep;
                                double newheight = zv.model.Zone.Height * heightstep;
                                AbsoluteLayout.SetLayoutBounds(zv, new Rectangle(zv.X, zv.Y, newWidth, newheight));
                            }
                            zv.Opacity = 1;
                        }
                        model.SaveZonesChangesAsync();
                        MovingAction = MovingActionTypeEnum.None;
                        break;
                    }
                case GestureStatus.Canceled:
                    {
                        MovingAction = MovingActionTypeEnum.None;
                        break;
                    }
            }
        }

        private async void ToolbarItem_Search(object sender, EventArgs e)
        {
            SearchViewModel svm = new SearchViewModel(Navigation);
            FindPage fp = new FindPage(svm);
            await Navigation.PushAsync(fp);
        }

        private async void ToolbarItem_FieldParamsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ZonesFieldParamsPage(model));
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            GridTapped(null, new EventArgs());

            if (model.RunMode == RunModeEnum.View)
            {              
                schemegrid.PlanHeight = model.Location.PlanHeight;
                schemegrid.PlanWidth = model.Location.PlanWidth;
                schemegrid.IsVisible = true;
                model.RunMode = RunModeEnum.Edit;
            }
            else
            {
                schemegrid.IsVisible = false;
                model.RunMode = RunModeEnum.View;
            }
        }
    }
}