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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.ViewModel;

namespace WarehouseControlSystem.View.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InnerSubSchemeView : ContentView
    {
        public static readonly BindableProperty PlanHeightProperty = BindableProperty.Create(nameof(PlanHeight), typeof(int), typeof(InnerSubSchemeView), 0, BindingMode.Default, null, Changed);
        public int PlanHeight
        {
            get { return (int)GetValue(PlanHeightProperty); }
            set { SetValue(PlanHeightProperty, value); }
        }

        public static readonly BindableProperty PlanWidthProperty = BindableProperty.Create(nameof(PlanWidth), typeof(int), typeof(InnerSubSchemeView), 0, BindingMode.Default, null, Changed);
        public int PlanWidth
        {
            get { return (int)GetValue(PlanWidthProperty); }
            set { SetValue(PlanWidthProperty, value); }
        }

        public static readonly BindableProperty SubSchemeElementsProperty = BindableProperty.Create(nameof(SubSchemeElements), typeof(List<SubSchemeElement>), typeof(InnerSubSchemeView), null, BindingMode.Default, null, Changed);
        public List<SubSchemeElement> SubSchemeElements
        {
            get { return (List<SubSchemeElement>)GetValue(SubSchemeElementsProperty); }
            set { SetValue(SubSchemeElementsProperty, value); }
        }

        public static readonly BindableProperty SubSchemeBackgroundColorProperty = BindableProperty.Create(nameof(SubSchemeBackgroundColor), typeof(Color), typeof(InnerSubSchemeView), Color.Transparent, BindingMode.Default, null, Changed);
        public Color SubSchemeBackgroundColor
        {
            get { return (Color)GetValue(SubSchemeBackgroundColorProperty); }
            set { SetValue(SubSchemeBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty SubSchemeElementColorProperty = BindableProperty.Create(nameof(SubSchemeElementColor), typeof(Color), typeof(InnerSubSchemeView), Color.Transparent, BindingMode.Default, null, Changed);
        public Color SubSchemeElementColor
        {
            get { return (Color)GetValue(SubSchemeElementColorProperty); }
            set { SetValue(SubSchemeElementColorProperty, value); }
        }

        public static readonly BindableProperty UpdateSchemeProperty = BindableProperty.Create(nameof(UpdateScheme), typeof(bool), typeof(InnerSubSchemeView), false, BindingMode.Default, null, Changed);
        public bool UpdateScheme
        {
            get { return (bool)GetValue(UpdateSchemeProperty); }
            set { SetValue(UpdateSchemeProperty, value); }
        }

        private static void Changed(BindableObject bindable, object oldValue, object newValue)
        {
            var instance = bindable as InnerSubSchemeView;
            instance?.Update();
        }

        public InnerSubSchemeView()
        {
            InitializeComponent();
        }

        public void Update()
        {
            maingrid.BackgroundColor = SubSchemeBackgroundColor;

            if ((PlanWidth <= 0) || (PlanHeight <= 0))
            {
                return;
            }

            try
            {
                Rebuild();
            }
            catch (Exception ex)
            {
                string t = "InnerSubSchemeView.Update Exception " + ex.ToString();
                System.Diagnostics.Debug.WriteLine(t);
            }
        }

        private void Rebuild()
        {
            maingrid.Children.Clear();
            maingrid.RowDefinitions.Clear();
            maingrid.ColumnDefinitions.Clear();

            for (int i = 1; i <= PlanWidth; i++)
            {
                maingrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            for (int i = 1; i <= PlanHeight; i++)
            {
                maingrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            if (SubSchemeElements is List<SubSchemeElement>)
            {
                foreach (SubSchemeElement element in SubSchemeElements)
                {
                    AddToScheme(element);
                    ShowSelection(element);
                }
            }
        }

        private void AddToScheme(SubSchemeElement element)
        {
            if ((element.Width <= 0) || (element.Height <= 0) || (element.Left > PlanWidth) || (element.Top > PlanHeight))
            {
                return;
            }

            Color color1 = Color.FromHex(element.HexColor);
            if (color1 == (Color)Application.Current.Resources["SchemeBlockWhiteColor"])
            {
                color1 = (Color)Application.Current.Resources["SchemeBlockWhiteColorDark"];
            }

            int left = Math.Max(0, Math.Min(PlanWidth, element.Left));
            int right = Math.Min(PlanWidth, element.Left + element.Width);
            int top = Math.Max(0, Math.Min(PlanHeight, element.Top));
            int bottom = Math.Min(PlanHeight, element.Top + element.Height);

            maingrid.Children.Add(
                new BoxView()
                {
                    Opacity = 0.7,
                    BackgroundColor = color1
                }, left, right, top, bottom);
        }

        private void ShowSelection(SubSchemeElement element)
        {
            if (element.Selection is List<SubSchemeSelect>)
            {
                foreach (SubSchemeSelect sss in element.Selection)
                {
                    int selectLeft = 0;
                    int selectTop = 0;
                    switch (element.RackOrientation)
                    {
                        case RackOrientationEnum.Undefined:
                            break;
                        case RackOrientationEnum.HorizontalLeft:
                            {
                                selectLeft = element.Left + sss.Section;
                                selectTop = element.Top + sss.Depth - 1;
                                break;
                            }
                        case RackOrientationEnum.HorizontalRight:
                            {
                                selectLeft = element.Left + element.Width - sss.Section;
                                selectTop = element.Top + sss.Depth - 1;
                                break;
                            }
                        case RackOrientationEnum.VerticalDown:
                            {
                                selectLeft = element.Left + sss.Depth - 1;
                                selectTop = element.Top + element.Height - sss.Section;
                                break;
                            }
                        case RackOrientationEnum.VerticalUp:
                            {
                                selectLeft = element.Left + sss.Depth - 1;
                                selectTop = element.Top + sss.Section - 1;
                                break;
                            }
                        default:
                            throw new InvalidOperationException("Impossible value");
                    }

                    if ((selectLeft < 0) || (selectTop < 0) || (selectLeft > PlanWidth) || (selectTop > PlanHeight))
                    {
                        break;
                    }

                    maingrid.Children.Add(
                        new BoxView()
                        {
                            BackgroundColor = Color.Red
                        }, selectLeft, selectTop);
                }
            }
        }
    }
}