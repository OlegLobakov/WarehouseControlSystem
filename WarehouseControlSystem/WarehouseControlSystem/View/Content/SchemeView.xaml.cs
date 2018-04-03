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
using SkiaSharp;
using SkiaSharp.Views.Forms;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.ViewModel;

namespace WarehouseControlSystem.View.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SchemeView : ContentView
    {
        public static readonly BindableProperty PlanHeightProperty = BindableProperty.Create("PlanHeight", typeof(int), typeof(SchemeView),0);
        public int PlanHeight
        {
            get { return (int)GetValue(PlanHeightProperty); }
            set { SetValue(PlanHeightProperty, value); Update(); }
        }

        public static readonly BindableProperty PlanWidthProperty = BindableProperty.Create("PlanWidth", typeof(int), typeof(SchemeView),0);
        public int PlanWidth
        {
            get { return (int)GetValue(PlanWidthProperty); }
            set { SetValue(PlanWidthProperty, value); Update(); }
        }

        public static readonly BindableProperty ZonesProperty = BindableProperty.Create("Zones", typeof(ObservableCollection<ZoneViewModel>), typeof(SchemeView), null);
        public ObservableCollection<ZoneViewModel> Zones
        {
            get { return (ObservableCollection<ZoneViewModel>)GetValue(ZonesProperty); }
            set { SetValue(ZonesProperty, value); }
        }

        public static readonly BindableProperty LocationsProperty = BindableProperty.Create("Locations", typeof(ObservableCollection<LocationViewModel>), typeof(SchemeView), null);
        public ObservableCollection<LocationViewModel> Locations
        {
            get { return (ObservableCollection<LocationViewModel>)GetValue(LocationsProperty); }
            set { SetValue(LocationsProperty, value); }
        }

        public static readonly BindableProperty RacksProperty = BindableProperty.Create("Racks", typeof(ObservableCollection<RackViewModel>), typeof(SchemeView), null);
        public ObservableCollection<RackViewModel> Racks
        {
            get { return (ObservableCollection<RackViewModel>)GetValue(RacksProperty); }
            set { SetValue(RacksProperty, value); }
        }

        SKCanvasView canvasView;

        public SchemeView()
        {
            InitializeComponent();

            canvasView = new SKCanvasView();
            //canvasView.IgnorePixelScaling = true;
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            Content = canvasView;
        }

        public void Update()
        {
            if (canvasView is SKCanvasView)
            {
                canvasView.InvalidateSurface();
            }
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKColor color = SKColor.Parse("#f6f6f6");
            SKPaint thinLinePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = color,
                StrokeWidth = 2
            };

            SKPaint thinLinePaint2 = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = color,
                StrokeWidth = 2
            };

            SKRect rect = new SKRect(0, 0, info.Width, info.Height);
            canvas.DrawRect(rect, thinLinePaint2);

            if ((PlanWidth != 0) && (PlanHeight != 0))
            {
                //SKRect rect2 = new SKRect(0, 0, info.Width,info.Height);
                //canvas.DrawRect(rect2, thinLinePaint);

                float heightsize = (float)info.Height / PlanHeight;
                float widthsize = (float)info.Width / PlanWidth;


                for (int i = 1; i <= PlanHeight; i++)
                {
                    canvas.DrawLine(0, i * heightsize, PlanWidth * widthsize, i * heightsize, thinLinePaint);
                }
                for (int j = 1; j <= PlanWidth; j++)
                {
                    canvas.DrawLine(j * widthsize, 0, j * widthsize, PlanHeight * heightsize, thinLinePaint);
                }

                #region Занятое пространство

                SKPaint rectPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = SKColors.LightGray,
                    StrokeWidth = 2
                };

                if (Zones is ObservableCollection<ZoneViewModel>)
                {
                    foreach (ZoneViewModel zvm in Zones)
                    {
                        SKRect rect1 = new SKRect
                        {
                            Left = zvm.Zone.Left * widthsize,
                            Top = zvm.Zone.Top * heightsize,
                            Right = (zvm.Zone.Left + zvm.Zone.Width) * widthsize,
                            Bottom = (zvm.Zone.Top + zvm.Zone.Height) * heightsize
                        };
                        canvas.DrawRect(rect1, rectPaint);
                    }
                }

                if (Racks is ObservableCollection<RackViewModel>)
                {
                    foreach (RackViewModel rvm in Racks)
                    {
                        SKRect rect1 = new SKRect
                        {
                            Left = rvm.Rack.Left * widthsize,
                            Top = rvm.Rack.Top * heightsize,
                            Right = (rvm.Rack.Left + rvm.Rack.Width) * widthsize,
                            Bottom = (rvm.Rack.Top + rvm.Rack.Height) * heightsize
                        };
                        canvas.DrawRect(rect1, rectPaint);
                    }
                }

                if (Locations is ObservableCollection<LocationViewModel>)
                {
                    foreach (LocationViewModel lvm in Locations)
                    {
                        SKRect rect1 = new SKRect
                        {
                            Left = lvm.Location.Left * widthsize,
                            Top = lvm.Location.Top * heightsize,
                            Right = (lvm.Location.Left + lvm.Location.Width) * widthsize,
                            Bottom = (lvm.Location.Top + lvm.Location.Height) * heightsize
                        };
                        canvas.DrawRect(rect1, rectPaint);
                    }
                }
                #endregion
            }

        }
    }
}