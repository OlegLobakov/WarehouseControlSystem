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

namespace WarehouseControlSystem.View.Pages.LocationsScheme
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationsSchemePage : ContentPage
    {
        List<LocationView> Views { get; set; }
        List<LocationView> SelectedViews { get; set; }

        MovingActionTypeEnum MovingAction = MovingActionTypeEnum.None;

        TapGestureRecognizer TapGesture;
        PanGestureRecognizer PanGesture;

        private readonly LocationsViewModel model;
        public LocationsSchemePage()
        {
            model = new LocationsViewModel(Navigation);
            BindingContext = model;
            InitializeComponent();

            Views = new List<LocationView>();
            SelectedViews = new List<LocationView>();

            TapGesture = new TapGestureRecognizer();
            abslayout.GestureRecognizers.Add(TapGesture);

            PanGesture = new PanGestureRecognizer();
            abslayout.GestureRecognizers.Add(PanGesture);

            MessagingCenter.Subscribe<LocationsViewModel>(this, "Rebuild", Rebuild);
            MessagingCenter.Subscribe<LocationsViewModel>(this, "Reshape", Reshape);
        }

        protected override void OnAppearing()
        { 
            base.OnAppearing();
            PanGesture.PanUpdated += OnPaned;
            TapGesture.Tapped += GridTapped;
            model.Load();
        }

        protected override void OnDisappearing()
        {
            PanGesture.PanUpdated -= OnPaned;
            TapGesture.Tapped -= GridTapped;
            SelectedViews.Clear();
            Views.Clear();
            abslayout.Children.Clear();
            base.OnDisappearing();
        }

        private void StackLayout_SizeChanged(object sender, EventArgs e)
        {
            StackLayout sl = (StackLayout)sender;
            model.ScreenWidth = sl.Width;
            model.ScreenHeight = sl.Height;
        }

        private void Abslayout_SizeChanged(object sender, EventArgs e)
        {
            AbsoluteLayout al = (AbsoluteLayout)sender;
            model.ScreenWidth = al.Width;
            model.ScreenHeight = al.Height;
            model.Rebuild(false);
        }

        private void Rebuild(LocationsViewModel lmv)
        {
            abslayout.Children.Clear();
            SelectedViews.Clear();
            Views.Clear();
            foreach (LocationViewModel lvm1 in model.LocationViewModels)
            {
                LocationView lv = new LocationView(lvm1);
                AbsoluteLayout.SetLayoutBounds(lv, new Rectangle(lvm1.Left, lvm1.Top, lvm1.Width, lvm1.Height));
                abslayout.Children.Add(lv);
                Views.Add(lv);
                lvm1.LoadZones();
            }
        }

        private void Reshape(LocationsViewModel rsmv)
        {
            foreach (LocationView lv in Views)
            {
                AbsoluteLayout.SetLayoutBounds(lv, new Rectangle(lv.Model.Left, lv.Model.Top, lv.Model.Width, lv.Model.Height));
            }
        }

        private void GridTapped(object sender, EventArgs e)
        {
            foreach (LocationView lv in Views)
            {
                lv.Opacity = 1;
            }
            model.UnSelectAll();
        }

        readonly Easing easing1 = Easing.Linear;
        readonly Easing easingParcking = Easing.CubicInOut;

        double x = 0, y = 0, widthstep = 0, heightstep = 0;

        double leftborder = double.MaxValue;
        double topborder = double.MaxValue;
        double rightborder = double.MinValue;
        double bottomborder = double.MinValue;

        double oldeTotalX = 0, oldeTotalY = 0;

        private async void OnPaned(object sender, PanUpdatedEventArgs e)
        {
            if (!model.IsEditMode)
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
                        SelectedViews = Views.FindAll(x => x.Model.Selected == true);
                        MovingAction = MovingActionTypeEnum.Pan;

                        widthstep = model.ScreenWidth / model.PlanWidth;
                        heightstep = model.ScreenHeight / model.PlanHeight;
                        leftborder = double.MaxValue;
                        topborder = double.MaxValue;
                        rightborder = double.MinValue;
                        bottomborder = double.MinValue;
                        foreach (LocationView lv in SelectedViews)
                        {
                            leftborder = Math.Min(lv.X, leftborder);
                            topborder = Math.Min(lv.Y, topborder);
                            rightborder = Math.Max(lv.X + lv.Width, rightborder);
                            bottomborder = Math.Max(lv.Y + lv.Height, bottomborder);
                            lv.Opacity = 0.5;
                            lv.Model.SavePrevSize(lv.Width,lv.Height);
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

                        if (dx + rightborder > model.ScreenWidth)
                        {
                            dx = model.ScreenWidth - rightborder;
                        }

                        if (dy + topborder < 0)
                        {
                            dy = -topborder;
                        }

                        if (dy + bottomborder > (model.ScreenHeight))
                        {
                            dy = model.ScreenHeight - bottomborder;
                        }

                        foreach (LocationView lv in SelectedViews)
                        {
                            if (lv.Model.EditMode == SchemeElementEditMode.Move)
                            {
                                await lv.TranslateTo(dx, dy, 250, easing1);
                            }
                            if (lv.Model.EditMode == SchemeElementEditMode.Resize)
                            {
                                AbsoluteLayout.SetLayoutBounds(lv, new Rectangle(lv.X, lv.Y, lv.Model.PrevWidth + dx, lv.Model.PrevHeight + dy));
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
                        foreach (LocationView lv in SelectedViews)
                        {
                            if (lv.Model.EditMode == SchemeElementEditMode.Move)
                            {
                                double newX = lv.X + lv.TranslationX;
                                double newY = lv.Y + lv.TranslationY;

                                lv.Model.Location.Left = (int)Math.Round(newX / widthstep);
                                lv.Model.Location.Top = (int)Math.Round(newY / heightstep);

                                double dX = lv.Model.Location.Left * widthstep - lv.X;
                                double dY = lv.Model.Location.Top * heightstep - lv.Y;

                                await lv.TranslateTo(dX, dY, 500, easingParcking);
                                //lv.Layout(new Rectangle(lv.X + dX, lv.Y + dY, lv.Width, lv.Height)); //в таком варианте почемуто есть глюк при переключении режима редактирования
                                AbsoluteLayout.SetLayoutBounds(lv, new Rectangle(lv.X + dX, lv.Y + dY, lv.Width, lv.Height));
                                lv.TranslationX = 0;
                                lv.TranslationY = 0;
                            }
                            if (lv.Model.EditMode == SchemeElementEditMode.Resize)
                            {
                                lv.Model.Location.Width = (int)Math.Round(lv.Width / widthstep);
                                lv.Model.Location.Height = (int)Math.Round(lv.Height / heightstep);
                                double newWidth = lv.Model.Location.Width * widthstep;
                                double newheight = lv.Model.Location.Height * heightstep;
                                AbsoluteLayout.SetLayoutBounds(lv, new Rectangle(lv.X, lv.Y, newWidth, newheight));
                            }
                            lv.Opacity = 1;
                        }
                        model.SaveLocationChangesAsync();
                        MovingAction = MovingActionTypeEnum.None;
                        break;
                    }
                case GestureStatus.Canceled:
                    {
                        MovingAction = MovingActionTypeEnum.None;
                        break;
                    }
                default:
                    throw new InvalidOperationException("Impossible value");
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            GridTapped(null, new EventArgs());

            if (model.IsEditMode)
            {
                abslayout.BackgroundColor = Color.White;
                model.IsEditMode = false;
                model.SaveSchemeParams();
            }
            else
            {
                abslayout.BackgroundColor = Color.LightGray;
                model.IsEditMode = true;
            }
        }
    }
}