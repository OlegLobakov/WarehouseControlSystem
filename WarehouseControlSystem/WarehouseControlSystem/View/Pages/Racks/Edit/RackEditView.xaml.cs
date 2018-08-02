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
using WarehouseControlSystem.ViewModel;
using WarehouseControlSystem.View.Content;

namespace WarehouseControlSystem.View.Pages.Racks.Edit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RackEditView : ContentView
    {
        public event Action<int> LevelSelected;
        public event Action<int> SectionSelected;

        List<BinInEditRackView> BinInEditRackViews = new List<BinInEditRackView>();
        List<EmptySpaceViewInRack> EmptySpaceViewInRacks = new List<EmptySpaceViewInRack>();

        Label HeaderLabel;
        public RackViewModel model;

        public static readonly BindableProperty BinWidthProperty = BindableProperty.Create(nameof(BinWidth), typeof(int), typeof(RackEditView), 120, BindingMode.Default, null, Changed);
        public int BinWidth
        {
            get { return (int)GetValue(BinWidthProperty); }
            set { SetValue(BinWidthProperty, value); }
        }

        private static void Changed(BindableObject bindable, object oldValue, object newValue)
        {
            //var instance = bindable as RackView;
            //instance?.Update();
        }

        TapGestureRecognizer LevelTap;
        TapGestureRecognizer SectionTap;

        public RackEditView()
        {
            InitializeComponent();

            LevelTap = new TapGestureRecognizer();
            LevelTap.Tapped += (s, e) => {
                if (LevelSelected is Action<int>)
                {
                    if (s is Label)
                    {
                        Label label1 = (Label)s;
                        int t = int.Parse(label1.Text);
                        int i = model.Levels - t + 1;
                        LevelSelected(i);
                    }
                }
            };

            SectionTap = new TapGestureRecognizer();
            SectionTap.Tapped += (s, e) => {
                if (SectionSelected is Action<int>)
                {
                    if (s is Label)
                    {
                        Label label1 = (Label)s;
                        int t = int.Parse(label1.Text);
                        SectionSelected(t);
                    }
                }
            };
        }

        private void Update()
        {
            Update(model);
        }

        private void Update(BinsViewModel bvm)
        {
            Update(model);
        }

        int oldSections;
        int oldLevels;
        public void Update(RackViewModel rvm)
        {
            model = rvm;

            if ((oldSections != model.Sections) || (oldLevels != model.Levels))
            {
                CreateGrid();
                CreateLabels();
                oldSections = rvm.Sections;
                oldLevels = rvm.Levels;
                BinInEditRackViews.Clear();
                EmptySpaceViewInRacks.Clear();
            }
            FillBins();
        }

        private void CreateGrid()
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30, GridUnitType.Absolute) });
            for (int i = 1; i <= model.Levels; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50, GridUnitType.Absolute) });
            for (int i = 1; i <= model.Sections; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(BinWidth, GridUnitType.Absolute) });
            }

            HeaderLabel = new Label();
            HeaderLabel.BackgroundColor = Color.FromHex("#b0aaa1");
            HeaderLabel.HorizontalOptions = LayoutOptions.FillAndExpand;
            HeaderLabel.VerticalOptions = LayoutOptions.FillAndExpand;
            HeaderLabel.HorizontalTextAlignment = TextAlignment.Center;
            HeaderLabel.VerticalTextAlignment = TextAlignment.Center;
            HeaderLabel.Text = model.No;
            HeaderLabel.TextColor = Color.White;
            HeaderLabel.FontAttributes = FontAttributes.Bold;

            grid.Children.Add(HeaderLabel, 0, 0);
        }

        private void CreateLabels()
        {
            CreateLevelsLabels();
            CreateSectionLabels();
        }
        private void CreateLevelsLabels()
        {
            for (int i = 1; i <= model.Levels; i++)
            {
                Label lb = new Label()
                {
                    BackgroundColor = Color.FromHex("#b0aaa1"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    Text = (model.Levels - i + 1).ToString(),
                    TextColor = Color.White,
                    FontAttributes = FontAttributes.Bold
                };
              
                lb.GestureRecognizers.Add(LevelTap);
                grid.Children.Add(lb, 0, i);
            }
        }
        private void CreateSectionLabels()
        {
            for (int j = 1; j <= model.Sections; j++)
            {
                Label lb = new Label()
                {
                    BackgroundColor = Color.FromHex("#b0aaa1"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.White,
                    FontAttributes = FontAttributes.Bold
                };
                lb.GestureRecognizers.Add(SectionTap);
                lb.Text = j.ToString();
                grid.Children.Add(lb, j, 0);
            }
        }

        private void FillBins()
        {
            for (int i = 1; i <= model.Levels; i++)
            {
                for (int j = 1; j <= model.Sections; j++)
                {
                    BinViewModel finded = model.BinsViewModel.BinViewModels.Find(x => x.Level == i && x.Section == j);
                    if (finded is BinViewModel)
                    {
                        try
                        {
                            BinInEditRackView exist = BinInEditRackViews.Find(x => x.Section == j && x.Level == i);
                            if (exist is BinInEditRackView)
                            {
                                exist.Update(finded);
                                exist.Marked = true;
                            }
                            else
                            {
                                BinInEditRackView bierv = new BinInEditRackView(finded);
                                bierv.Section = j;
                                bierv.Level = i;
                                bierv.Marked = true;
                                grid.Children.Add(bierv, finded.Section, finded.Section + finded.SectionSpan, finded.Level, finded.Level + finded.LevelSpan);
                                BinInEditRackViews.Add(bierv);
                            }
                        }
                        catch (Exception exp)
                        {
                            System.Diagnostics.Debug.WriteLine(exp.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            EmptySpaceViewModel esvm = model.BinsViewModel.EmptySpacesViewModels.Find(x => x.Level == i && x.Section == j);
                            if (esvm is EmptySpaceViewModel)
                            {
                                EmptySpaceViewInRack exist = EmptySpaceViewInRacks.Find(x => x.Section == j && x.Level == i);
                                if (exist is EmptySpaceViewInRack)
                                {
                                    exist.Marked = true;
                                }
                                else
                                {
                                    EmptySpaceViewInRack esvir = new EmptySpaceViewInRack(esvm);
                                    esvir.Section = j;
                                    esvir.Level = i;
                                    esvir.Marked = true;
                                    grid.Children.Add(esvir, esvm.Section, esvm.Level);
                                    EmptySpaceViewInRacks.Add(esvir);
                                }
                            }
                        }
                        catch (Exception exp)
                        {
                            System.Diagnostics.Debug.WriteLine(exp.Message);
                        }
                    }
                }
            }
            BinInEditRackViews.RemoveAll(x => x.Marked == false);
            foreach (BinInEditRackView bierv in BinInEditRackViews)
            {
                bierv.Marked = false;
            }

            EmptySpaceViewInRacks.RemoveAll(x => x.Marked == false);
            foreach (EmptySpaceViewInRack esvmir in EmptySpaceViewInRacks)
            {
                esvmir.Marked = false;
            }
        }

        /// <summary>
        /// For Autoscrolling
        /// </summary>
        /// <param name="bvm"></param>
        /// <returns></returns>
        public BinInEditRackView GetBinView(BinViewModel bvm)
        {
            foreach (Xamarin.Forms.View view in grid.Children)
            {
                if (view is BinInEditRackView)
                {
                    BinInEditRackView binview = (BinInEditRackView)view;
                    if (binview.Model == bvm)
                    {
                        return binview;
                    }
                }
            }
            return null;
        }

        public void SetHeaderLabel(string text)
        {
            if (HeaderLabel is Label)
            {
                HeaderLabel.Text = text;
            }
        }
    }
}