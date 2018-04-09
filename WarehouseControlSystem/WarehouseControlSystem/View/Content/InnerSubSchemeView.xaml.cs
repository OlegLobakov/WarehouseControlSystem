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
    public partial class InnerSubSchemeView : ContentView
    {
        public static readonly BindableProperty PlanHeightProperty = BindableProperty.Create("PlanHeight", typeof(int), typeof(InnerSubSchemeView),0);
        public int PlanHeight
        {
            get { return (int)GetValue(PlanHeightProperty); }
            set
            {
                SetValue(PlanHeightProperty, value);
                Update();
            }
        }

        public static readonly BindableProperty PlanWidthProperty = BindableProperty.Create("PlanWidth", typeof(int), typeof(InnerSubSchemeView),0);
        public int PlanWidth
        {
            get { return (int)GetValue(PlanWidthProperty); }
            set
            {
                SetValue(PlanWidthProperty, value);
                Update();
            }
        }

        public static readonly BindableProperty SubSchemeElementsProperty = BindableProperty.Create("SubSchemeElements", typeof(List<SubSchemeElement>), typeof(InnerSubSchemeView), null);
        public List<SubSchemeElement> SubSchemeElements
        {
            get { return (List<SubSchemeElement>)GetValue(SubSchemeElementsProperty); }
            set
            {
                SetValue(SubSchemeElementsProperty, value);
                Update();
            }
        }

        SKCanvasView canvasView;

        public InnerSubSchemeView()
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

            if ((PlanWidth == 0) || (PlanHeight == 0))
            {
                return;
            }

            float blocksize;
            float top = 0;
            float left = 0;

            if (info.Width > info.Height)
            {
                blocksize = (float)info.Height / PlanHeight;
                top = 0;
                left = (float)info.Width - (blocksize * PlanWidth);
            }
            else
            {
                blocksize = (float)info.Width / PlanWidth;
                top = (float)info.Height - (blocksize * PlanHeight);
                left = 0;
            }

            if (SubSchemeElements is List<SubSchemeElement>)
            {
                if (SubSchemeElements.Count > 0)
                {
                    //float heightsize = (float)info.Height / PlanHeight;
                    //float widthsize = (float)info.Width / PlanWidth;

                    SKColor colorborder = SKColors.White;
                    colorborder = colorborder.WithAlpha(50);
                    SKPaint thinLinePaint2 = new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        Color = colorborder,
                        StrokeWidth = 1
                    };

                    SKRect rect = new SKRect(left+2, top, left + (PlanWidth * blocksize) - 5, top + (PlanHeight * blocksize) - 5);
                    canvas.DrawRect(rect, thinLinePaint2);

                    foreach (SubSchemeElement element in SubSchemeElements)
                    {
                        SKColor color;
                        try
                        {
                            color = SKColor.Parse(element.HexColor);
                        }
                        catch
                        {
                            color = SKColors.White;
                        }

                        color = color.WithAlpha(120);

                        SKPaint rectPaint = new SKPaint
                        {
                            Style = SKPaintStyle.StrokeAndFill,
                            Color = color,
                            StrokeWidth = 1
                        };

                        SKRect rectelement = new SKRect
                        {
                            Left = (element.Left * blocksize) + left,
                            Top = (element.Top * blocksize) + top,
                            Right = ((element.Left + element.Width) * blocksize) + left,
                            Bottom = ((element.Top + element.Height) * blocksize) + top
                        };
                        canvas.DrawRect(rectelement, rectPaint);

                        if (element.RackOrientation != RackOrientationEnum.Undefined)
                        {
                            if (element.Selection is List<SubSchemeSelect>)
                            {
                                foreach (SubSchemeSelect sss in element.Selection)
                                {
                                    //section, level ,depth
                                    float selectionleft = rectelement.Left;
                                    float selectiontop = rectelement.Top;
                                    float selectionright = rectelement.Left;
                                    float selectionbottom = rectelement.Top;

                                    switch (element.RackOrientation)
                                    {
                                        case RackOrientationEnum.HorizontalLeft:
                                            {
                                                selectionleft += (sss.Section * blocksize);
                                                selectiontop += 0;
                                                selectionright += (sss.Section * blocksize) + blocksize;
                                                selectionbottom += blocksize;
                                                break;
                                            }
                                        case RackOrientationEnum.HorizontalRight:
                                            {
                                                selectionleft += sss.Section * blocksize;
                                                selectiontop += 0;
                                                selectionright += (sss.Section * blocksize) + blocksize;
                                                selectionbottom += blocksize;
                                                break;
                                            }
                                        case RackOrientationEnum.VerticalDown:
                                            {
                                                selectionleft += 0;
                                                selectiontop += (element.Height - sss.Section) * blocksize;
                                                selectionright += blocksize;
                                                selectionbottom += ((element.Height - sss.Section) * blocksize) + blocksize;
                                                break;
                                            }
                                        case RackOrientationEnum.VerticalUp:
                                            {
                                                selectionleft += 0;
                                                selectiontop += sss.Section * blocksize;
                                                selectionright += blocksize;
                                                selectionbottom += (sss.Section * blocksize) + blocksize;
                                                break;
                                            }
                                    }


                                    SKColor colorred = SKColors.Red;
                                    color = color.WithAlpha(100);

                                    using (var selectionPaint = new SKPaint())
                                    {
                                        selectionPaint.Style = SKPaintStyle.StrokeAndFill;
                                        selectionPaint.Color = colorred;
                                        selectionPaint.StrokeWidth = 1;
                                        SKRect selectionTect = new SKRect()
                                        {
                                            Left = selectionleft,
                                            Top = selectiontop,
                                            Right = selectionright,
                                            Bottom = selectionbottom,
                                        };
                                        canvas.DrawRect(selectionTect, selectionPaint);
                                    }
                                }
                            }
                        }
                    }
   
                }
            }
        }
    }
}