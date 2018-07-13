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

namespace WarehouseControlSystem.View.Pages.RackScheme.MasterNewRack
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RackSimpleView : ContentView
    {
        class InnerBoxView
        {
            public int I { get; set; }
            public int J { get; set; }
            public BoxView BoxView { get; set; }
        }
        List<InnerBoxView> InnerBoxViewList { get; set; } = new List<InnerBoxView>();

        public static readonly BindableProperty NoProperty = BindableProperty.Create(nameof(No), typeof(string), typeof(RackSimpleView), "", BindingMode.Default, null, NoChanged);
        public string No
        {
            get { return (string)GetValue(NoProperty); }
            set { SetValue(NoProperty, value); }
        }

        public static readonly BindableProperty SectionsProperty = BindableProperty.Create(nameof(Sections), typeof(int), typeof(RackSimpleView), 0, BindingMode.Default, null, SectionsChanged);
        public int Sections
        {
            get { return (int)GetValue(SectionsProperty); }
            set { SetValue(SectionsProperty, value); }
        }

        public static readonly BindableProperty LevelsProperty = BindableProperty.Create(nameof(Levels), typeof(int), typeof(RackSimpleView), 0, BindingMode.Default, null, LevelsChanged);
        public int Levels
        {
            get { return (int)GetValue(LevelsProperty); }
            set { SetValue(LevelsProperty, value); }
        }

        private static void NoChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var instance = bindable as RackSimpleView;
            string oldvalue = (string)oldValue;
            string newvalue = (string)newValue;
            //instance?.NoUpdate(oldvalue, newvalue);

            instance?.NoUpdate(newvalue);
        }

        private static void SectionsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var instance = bindable as RackSimpleView;
            int oldvalue = (int)oldValue;
            int newvalue = (int)newValue;
            instance?.Update();
        }

        private static void LevelsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var instance = bindable as RackSimpleView;
            int oldvalue = (int)oldValue;
            int newvalue = (int)newValue;
            instance?.Update();
        }

        Label HeaderLabel;

        public RackSimpleView()
        {
            InitializeComponent();

            CreateGrid();
        }

        public void NoUpdate(string newvalue)
        {
            if (HeaderLabel is Label)
            {
                HeaderLabel.Text = newvalue;
            }
        }

        public void Update()
        {
            if (Sections > 0 && Levels > 0)
            {
                grid.IsVisible = false;

                CreateGrid();
                //CreateLabels();
                FillBins();
                grid.IsVisible = true;
            }
        }

        private void CreateGrid()
        {
            WidthRequest = 22 * Sections + 60 + 2;
            HeightRequest = 42 * Levels + 2;
            
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();

             for (int i = 1; i <= Levels; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40, GridUnitType.Absolute) });
            }

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60, GridUnitType.Absolute) });
            for (int i = 1; i <= Sections; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Absolute) });
            }

            HeaderLabel = new Label
            {
                BackgroundColor = Color.FromHex("#7b7670"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Text = No,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold
            };

            grid.Children.Add(HeaderLabel, 0, 1, 0, Levels + 1);
        }

        private void CreateLabels()
        {
            CreateLevelsLabels();
            CreateSectionLabels();
        }

        private void CreateLevelsLabels()
        {
            for (int i = 1; i <= Levels; i++)
            {
                grid.Children.Add(new BoxView()
                {
                    BackgroundColor = Color.FromHex("#7b7670"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                }, 1, i);
            }
        }

        private void CreateSectionLabels()
        {
            for (int j = 1; j <= Sections; j++)
            {
                grid.Children.Add(new BoxView()
                {
                    BackgroundColor = Color.FromHex("#7b7670"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                }, j, 0);
            }
        }

        private void FillBins()
        {
            for (int i = 0; i < Levels; i++)
            {
                for (int j = 0; j < Sections; j++)
                {
                    InnerBoxView find = InnerBoxViewList.Find(x => x.I == i && x.J == j);
                    if (find is InnerBoxView)
                    {
                        grid.Children.Add(find.BoxView, j + 1, i);
                    }
                    else
                    {
                        find = new InnerBoxView
                        {
                            I = i,
                            J = j,
                            BoxView = new BoxView
                            {
                                BackgroundColor = Color.FromHex("#b0aaa1"),
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                VerticalOptions = LayoutOptions.FillAndExpand
                            }
                        };
                        InnerBoxViewList.Add(find);
                    }

                    grid.Children.Add(find.BoxView, j + 1, i);
                }
            }
        }
    }
}