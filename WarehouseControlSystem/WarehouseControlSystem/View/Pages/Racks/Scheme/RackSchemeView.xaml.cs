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
using WarehouseControlSystem.ViewModel;
using WarehouseControlSystem.View.Pages.Base;

namespace WarehouseControlSystem.View.Pages.Racks.Scheme
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RackSchemeView : SchemeBaseView
    {
        private List<Label> udslabels = new List<Label>();
        private RackViewModel model;
        public RackSchemeView(RackViewModel rvm) : base(rvm)
        {
            model = rvm;
            InitializeComponent();

            CreateSections();
            CreateLabels();
            FillSections();
        }

        public void UpdateUDS()
        {
            RackViewModel model = (RackViewModel)Model;

            foreach (Label lbl1 in udslabels)
            {
                if (grid.Children.Contains(lbl1))
                {
                    grid.Children.Remove(lbl1);
                }
            }
            udslabels.Clear();

            for (int i = 1; i <= model.Sections; i++)
            {
                Label label1 = NewLabel();

                label1.TextColor = Color.White;

                if (model.UDSSelects is List<SubSchemeSelect>)
                {
                    List<SubSchemeSelect> list = model.UDSSelects.FindAll(x => x.Section == i);

                    if (list is List<SubSchemeSelect>)
                    {
                        int quantity = 0;
                        foreach (SubSchemeSelect sss in list)
                        {
                            quantity += sss.Value;
                            label1.BackgroundColor = Color.FromHex(sss.HexColor);
                        }

                        if (quantity != 0)
                        {
                            label1.SetBinding(Label.FontSizeProperty, new Binding("SchemeFontSize"));
                            label1.Text = quantity.ToString();
                        }
                    }
                }

                AddToGrid(model, label1, i);
                udslabels.Add(label1);
            }

        }

        private void CreateSections()
        {
            RackViewModel model = (RackViewModel)Model;

            if ((model.RackOrientation == RackOrientationEnum.HorizontalLeft || model.RackOrientation == RackOrientationEnum.HorizontalRight))
            {
                for (int i = 1; i <= model.Sections + 1; i++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                }
            }

            if ((model.RackOrientation == RackOrientationEnum.VerticalUp || model.RackOrientation == RackOrientationEnum.VerticalDown))
            {
                for (int i = 1; i <= model.Sections + 1; i++)
                {
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                }
            }
        }

        private void CreateLabels()
        {
            RackViewModel model = (RackViewModel)Model;

            Label lb = new Label
            {
                BackgroundColor = Color.FromHex("#3d567c"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White,
            };

            lb.Text = model.No;
            lb.SetBinding(Label.FontSizeProperty, new Binding("SchemeFontSize"));

            if (model.RackOrientation == RackOrientationEnum.HorizontalLeft || model.RackOrientation == RackOrientationEnum.VerticalUp)
            {
                grid.Children.Add(lb, 0, 0);
            }

            if (model.RackOrientation == RackOrientationEnum.HorizontalRight)
            {
                grid.Children.Add(lb, model.Sections, 0);
            }

            if (model.RackOrientation == RackOrientationEnum.VerticalDown)
            {
                grid.Children.Add(lb, 0, model.Sections);
            }
        }

        private void FillSections()
        {
            RackViewModel model = (RackViewModel)Model;

            for (int i = 1; i <= model.Sections; i++)
            {
                Label label1 = NewLabel();

                if (model.SubSchemeSelects is List<SubSchemeSelect>)
                {
                    List<SubSchemeSelect> list = model.SubSchemeSelects.FindAll(x => x.Section == i);

                    if (list is List<SubSchemeSelect>)
                    {
                        if (list.Count > 0)
                        {
                            label1.BackgroundColor = Color.Red;
                            label1.TextColor = Color.White;
                        }

                        int quantity = 0;

                        foreach (SubSchemeSelect sss in list)
                        {
                            quantity += sss.Value;
                        }

                        if (quantity != 0)
                        {
                            label1.SetBinding(Label.FontSizeProperty, new Binding("SchemeFontSize"));
                            label1.Text = quantity.ToString();
                        }
                    }
                }

                AddToGrid(model, label1, i);
            }
        }
        
        private void AddToGrid(RackViewModel model, Label label1, int i)
        {
            switch (model.RackOrientation)
            {
                case RackOrientationEnum.HorizontalLeft:
                    grid.Children.Add(label1, i, 0);
                    break;
                case RackOrientationEnum.HorizontalRight:
                    grid.Children.Add(label1, i - 1, 0);
                    break;
                case RackOrientationEnum.VerticalUp:
                    grid.Children.Add(label1, 0, i);
                    break;
                case RackOrientationEnum.VerticalDown:
                    grid.Children.Add(label1, 0, i - 1);
                    break;
            }
        }

        private Label NewLabel()
        {
            return new Label
            {
                BackgroundColor = Color.FromHex("#dbe1eb"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
            };
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            RackViewModel model = (RackViewModel)Model;
            model.SchemeWidth = width;
            model.SchemeHeight = height;

            double fs = width;
            if (width > height)
            {
                fs = width / (model.Sections + 1);
            }
            model.SchemeFontSize = fs * 0.6;
        }
    }
}