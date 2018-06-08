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
using System.Collections.ObjectModel;

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

        private readonly RacksViewModel model;
        public RacksSchemePage(Zone zone)
        {
            model = new RacksViewModel(Navigation, zone);
            BindingContext = model;
            InitializeComponent();

            Views = new List<RackSchemeView>();
            SelectedViews = new List<RackSchemeView>(); 

            TapGesture = new TapGestureRecognizer();
            schemegrid.GestureRecognizers.Add(TapGesture);

            PanGesture = new PanGestureRecognizer();
            abslayout.GestureRecognizers.Add(PanGesture);

            Title = AppResources.ZoneSchemePage_Title +" "+ Global.SearchLocationCode + " | " + AppResources.RackSchemePage_Title + " - " + zone.Description;
            MessagingCenter.Subscribe<RacksViewModel>(this, "Rebuild", Rebuild);
            MessagingCenter.Subscribe<SearchViewModel>(this, "Search", OnSearch);
            MessagingCenter.Subscribe<RacksViewModel>(this, "ReLoad", Reload);
            MessagingCenter.Subscribe<RacksViewModel>(this, "UDSRunIsDone", UDSRunIsDone);
            MessagingCenter.Subscribe<RacksViewModel>(this, "UDSListIsLoaded", UDSListIsLoaded);
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            model.Load();
            model.LoadUDS();
            PanGesture.PanUpdated += OnPaned;
            TapGesture.Tapped += GridTapped;
        }

        protected override void OnDisappearing()
        {     
            PanGesture.PanUpdated -= OnPaned;
            TapGesture.Tapped -= GridTapped;     
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            model.DisposeModel();
            MessagingCenter.Unsubscribe<RacksViewModel>(this, "Rebuild");
            MessagingCenter.Unsubscribe<RacksViewModel>(this, "ReLoad");
            MessagingCenter.Unsubscribe<SearchViewModel>(this, "Search");
            MessagingCenter.Unsubscribe<RacksViewModel>(this, "UDSRunIsDone");
            MessagingCenter.Unsubscribe<RacksViewModel>(this, "UDSListIsLoaded");
            base.OnBackButtonPressed();
            return false;
        }


        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            model.ScreenWidth = width - 10;
            model.ScreenHeight = height - 10;
            model.UDSPanelHeight = (int)Math.Round(height / 6);
        }

        private void Rebuild(RacksViewModel rsmv)
        {
            SelectedViews.Clear();
            foreach (RackSchemeView lv in Views)
            {
                abslayout.Children.Remove(lv);
            }
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

        private void Reload(RacksViewModel zmv)
        {
            model.Load();
        }

        private void UDSRunIsDone(RacksViewModel zmv)
        {
            foreach (RackSchemeView lv in Views)
            {
                lv.UpdateUDS();
            }
        }

        private void UDSListIsLoaded(RacksViewModel rvm)
        {
            hlv.ItemsSource = model.UserDefinedSelectionViewModels;
        }


        private void OnSearch(SearchViewModel svm)
        {
            System.Diagnostics.Debug.WriteLine(svm.ToString());
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
                        SelectedViews = Views.FindAll(x => x.Model.Selected == true);
                        MovingAction = MovingActionTypeEnum.Pan;

                        widthstep = (abslayout.Width / model.PlanWidth);
                        heightstep = (abslayout.Height / model.PlanHeight);

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
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (model.RunMode == RunModeEnum.View)
            {
                schemegrid.PlanHeight = model.PlanHeight;
                schemegrid.PlanWidth = model.PlanWidth;
                schemegrid.IsVisible = true;
                model.RunMode = RunModeEnum.Edit;
            }
            else
            {
                model.UnSelectAll();
                schemegrid.IsVisible = false;
                model.RunMode = RunModeEnum.View;
            }
        }

        private void ToolbarItem_UDS(object sender, EventArgs e)
        {
            model.IsVisibleUDS = !model.IsVisibleUDS;
            //object obj = hlv.ItemsSource;

            //DataTemplate dt = hlv.ItemTemplate;
            
            //if (obj is ObservableCollection<UserDefinedSelectionViewModel>)
            //{
            //    model.IsVisibleUDS = !model.IsVisibleUDS;
            //}
        }

        private async void ToolbarItem_Search(object sender, EventArgs e)
        {
            SearchViewModel svm = new SearchViewModel(Navigation);
            FindPage fp = new FindPage(svm);
            await Navigation.PushAsync(fp);
        }
   }
}