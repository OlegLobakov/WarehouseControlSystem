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

namespace WarehouseControlSystem.View.Pages.RackScheme
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RackView : ContentView
    {
        Label HeaderLabel;
        public RackViewModel model;

        public RackView()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<BinsViewModel>(this, "Update", Update);
            MessagingCenter.Subscribe<RackViewModel>(this, "Update", Update);
        }


        private void Update(BinsViewModel bvm)
        {
            Update(model);
        }

        public void Update(RackViewModel rvm)
        {            
            model = rvm;
            BindingContext = model;

            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25, GridUnitType.Absolute) });
            for (int i = 1; i <= model.Levels; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40, GridUnitType.Absolute) });
            for (int i = 1; i <= model.Sections; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120, GridUnitType.Absolute) });
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
                grid.Children.Add(lb, 0, i);
            }

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
                lb.Text = j.ToString();              
                grid.Children.Add(lb, j, 0);
            }



            for (int i = 1; i <= model.Levels; i++)
            {
                for (int j = 1; j <= model.Sections; j++)
                {
                   
                        BinViewModel finded = rvm.BinsViewModel.BinViewModels.Find(x => x.Level == i && x.Section == j);
                    if (finded is BinViewModel)
                    {
                        if (rvm.CreateMode)
                        {
                            BinView bev = new BinView(finded);
                            grid.Children.Add(bev, finded.Section, finded.Section + finded.SectionSpan, finded.Level, finded.Level + finded.LevelSpan);
                        }
                        else
                        {
                            BinViewInRack bev = new BinViewInRack(finded);
                            grid.Children.Add(bev, finded.Section, finded.Section + finded.SectionSpan, finded.Level, finded.Level + finded.LevelSpan);
                        }
                    }
                    
                }
            }
        }

        public void SetHeaderLabel(string text)
        {
            if (HeaderLabel is Label)
            {
                HeaderLabel.Text = text;
            }
        }

        public void Dispose()
        {
            MessagingCenter.Unsubscribe<BinsViewModel>(this, "Update");
            MessagingCenter.Unsubscribe<RackViewModel>(this, "Update");        
        }
    }
}