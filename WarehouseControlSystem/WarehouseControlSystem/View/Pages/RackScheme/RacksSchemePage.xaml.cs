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


namespace WarehouseControlSystem.View.Pages.RackScheme
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RacksSchemePage : ContentPage
    {
        List<RackSchemeView> Views { get; set; }
        List<RackSchemeView> SelectedViews { get; set; }

        MovingActionTypeEnum MovingAction = MovingActionTypeEnum.None;
 
        TapGestureRecognizer TapGesture;
        PanGestureRecognizer PanGesture;

        private readonly RacksPlanViewModel model;

        public RacksSchemePage(Zone zone)
        {
            model = new RacksPlanViewModel(Navigation, zone);
            BindingContext = model;
            InitializeComponent();

            Views = new List<RackSchemeView>();
            SelectedViews = new List<RackSchemeView>(); 

            TapGesture = new TapGestureRecognizer();
            abslayout.GestureRecognizers.Add(TapGesture);

            PanGesture = new PanGestureRecognizer();
            abslayout.GestureRecognizers.Add(PanGesture);

            Title = AppResources.ZoneSchemePage_Title +" "+ Global.CurrentLocationName+" | " + AppResources.RackSchemePage_Title + " - " + zone.Description;

            MessagingCenter.Subscribe<RacksPlanViewModel>(this, "Rebuild", Rebuild);
            MessagingCenter.Subscribe<RacksPlanViewModel>(this, "Reshape", Reshape);
            MessagingCenter.Subscribe<RacksPlanViewModel>(this, "UDSRunIsDone", UDSRunIsDone);
            MessagingCenter.Subscribe<RacksPlanViewModel>(this, "UDSListIsLoaded", UDSListIsLoaded);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PanGesture.PanUpdated += OnPaned;
            TapGesture.Tapped += GridTapped;
            model.Load();
            model.LoadUDS();
        }

        protected override void OnDisappearing()
        {
            model.State = ViewModel.Base.ModelState.Undefined;
            PanGesture.PanUpdated -= OnPaned;
            TapGesture.Tapped -= GridTapped;
            SelectedViews.Clear();
            abslayout.Children.Clear();
            Views.Clear();
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            model.DisposeModel();
            MessagingCenter.Unsubscribe<RacksPlanViewModel>(this, "Rebuild");
            MessagingCenter.Unsubscribe<RacksPlanViewModel>(this, "Reshape");
            MessagingCenter.Unsubscribe<RacksPlanViewModel>(this, "UDSRunIsDone");
            MessagingCenter.Unsubscribe<RacksPlanViewModel>(this, "UDSListIsLoaded");
            base.OnBackButtonPressed();
            return false;
        }

        private void StackLayout_SizeChanged(object sender, EventArgs e)
        {
            StackLayout sl = (StackLayout)sender;
            model.SetScreenSizes(sl.Width, sl.Height, false);
            model.UDSPanelHeight = (int)Math.Round(sl.Height / 5.5);
        }

        private void Abslayout_SizeChanged(object sender, EventArgs e)
        {
            AbsoluteLayout al = (AbsoluteLayout)sender;
            model.UDSPanelHeight = (int)Math.Round(al.Height / 5.5);
            model.SetScreenSizes(al.Width, al.Height, true);
        }

        private void Rebuild(RacksPlanViewModel rsmv)
        {
            SelectedViews.Clear();
            abslayout.Children.Clear();
            Views.Clear();
            foreach (RackViewModel rvm in model.RackViewModels)
            {
                RackSchemeView rsv = new RackSchemeView(rvm);
                AbsoluteLayout.SetLayoutBounds(rsv,
                    new Rectangle(rvm.Left, rvm.Top, rvm.Width, rvm.Height));
                abslayout.Children.Add(rsv);
                Views.Add(rsv);
            }
        }

        private void Reshape(RacksPlanViewModel rsmv)
        {
            foreach (RackSchemeView rsv in Views)
            {
                AbsoluteLayout.SetLayoutBounds(rsv, new Rectangle(rsv.Model.Left, rsv.Model.Top, rsv.Model.Width, rsv.Model.Height));
            }
        }

        private void UDSRunIsDone(RacksPlanViewModel zmv)
        {
            foreach (RackSchemeView lv in Views)
            {
                lv.UpdateUDS();
            }
        }

        private void UDSListIsLoaded(RacksPlanViewModel rvm)
        {
            hlv.ItemsSource = model.UserDefinedSelectionViewModels;
        }

        private void OnSearch(SearchViewModel svm)
        {
            System.Diagnostics.Debug.WriteLine(svm.ToString());
        }

        private void GridTapped(object sender, EventArgs e)
        {
            foreach (RackSchemeView rsv in Views)
            {
                rsv.Opacity = 1;
            }
            model.UnSelectAll();
        }

        readonly Easing easing1 = Easing.Linear;
        readonly Easing easingParcking = Easing.CubicInOut;

        double x = 0;
        double y = 0;
        double widthstep = 0;
        double heightstep = 0;

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

                        foreach (RackSchemeView lv in SelectedViews)
                        {
                            leftborder = Math.Min(lv.X, leftborder);
                            topborder = Math.Min(lv.Y, topborder);
                            rightborder = Math.Max(lv.X + lv.Width, rightborder);
                            bottomborder = Math.Max(lv.Y + lv.Height, bottomborder);
                            lv.Opacity = 0.5;
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

                        if (dy + bottomborder > model.ScreenHeight)
                        {
                            dy = model.ScreenHeight - bottomborder;
                        }

                        foreach (RackSchemeView lv in SelectedViews)
                        {
                            await lv.TranslateTo(dx, dy, 250, easing1);
                        }
                        break;
                    }
                case GestureStatus.Completed:
                    {
                        x = 0;
                        y = 0;
                        oldeTotalX = 0;
                        oldeTotalY = 0;
                        foreach (RackSchemeView rv in SelectedViews)
                        {
                            double newX = rv.X + rv.TranslationX;
                            double newY = rv.Y + rv.TranslationY;

                            rv.Model.Rack.Left = (int)Math.Round(newX / widthstep);
                            rv.Model.Rack.Top = (int)Math.Round(newY / heightstep);

                            //выравнивание по сетке
                            double dX = rv.Model.Rack.Left * widthstep - rv.X;
                            double dY = rv.Model.Rack.Top * heightstep - rv.Y;

                            await rv.TranslateTo(dX, dY, 500, easingParcking);
                            rv.Opacity = 1;
                            //lv.Layout(new Rectangle(lv.X + dX, lv.Y + dY, lv.Width, lv.Height)); //в таком варианте почемуто есть глюк при переключении режима редактирования
                            AbsoluteLayout.SetLayoutBounds(rv, new Rectangle(rv.X + dX, rv.Y + dY, rv.Width, rv.Height));
                            rv.TranslationX = 0;
                            rv.TranslationY = 0;
                        }

                        model.SaveRacksChangesAsync();
                        MovingAction = MovingActionTypeEnum.None;
                        break;
                    }
                case GestureStatus.Canceled:
                    {
                        MovingAction = MovingActionTypeEnum.None;
                        break;
                    }
                default:
                    throw new InvalidOperationException("RacksSchemePage OnPaned Impossible Value ");
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            GridTapped(null, new EventArgs());

            if (model.IsEditMode)
            {
                model.IsEditMode = false;
                abslayout.BackgroundColor = Color.White;
                model.SaveZoneParams();
            }
            else
            {
                model.UnSelectAll();
                abslayout.BackgroundColor = Color.LightGray;
                model.IsEditMode = true;
                model.Rebuild(false);
            }
        }

        private void ToolbarItem_UDS(object sender, EventArgs e)
        {
            model.IsVisibleUDS = !model.IsVisibleUDS;
        }

        private async void ToolbarItem_Search(object sender, EventArgs e)
        {
            SearchViewModel svm = new SearchViewModel(Navigation);
            FindPage fp = new FindPage(svm);
            await Navigation.PushAsync(fp);
        }
   }
}