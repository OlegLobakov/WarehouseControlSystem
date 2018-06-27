using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.ViewModel.Base;
using System.Threading.Tasks;

namespace WarehouseControlSystem.View.Pages.Base
{
    public class SchemeBasePlanPage : ContentPage
    {
        protected List<SchemeBaseView> Views { get; set; } = new List<SchemeBaseView>();
        protected List<SchemeBaseView> SelectedViews { get; set; } = new List<SchemeBaseView>();

        public PlanBaseViewModel BaseModel { get; set; }

        MovingActionTypeEnum MovingAction = MovingActionTypeEnum.None;

        readonly Easing easing1 = Easing.Linear;
        readonly Easing easingParcking = Easing.CubicInOut;

        double x = 0, y = 0;

        double leftborder = double.MaxValue;
        double topborder = double.MaxValue;
        double rightborder = double.MinValue;
        double bottomborder = double.MinValue;

        double oldeTotalX = 0;
        double oldeTotalY = 0;

        protected TapGestureRecognizer TapGesture;
        protected PanGestureRecognizer PanGesture;

        public SchemeBasePlanPage(PlanBaseViewModel model)
        {
            BaseModel = model;
            BindingContext = BaseModel;

            TapGesture = new TapGestureRecognizer();
            PanGesture = new PanGestureRecognizer();
        }

        public void StackLayout_SizeChanged(object sender, EventArgs e)
        {
            StackLayout sl = (StackLayout)sender;
            BaseModel.SetScreenSizes(sl.Width, sl.Height, false);
        }

        public void Abslayout_SizeChanged(object sender, EventArgs e)
        {
            AbsoluteLayout al = (AbsoluteLayout)sender;
            BaseModel.SetScreenSizes(al.Width, al.Height, true);
        }

        protected void Reshape()
        {
            foreach (SchemeBaseView lv in Views)
            {
                AbsoluteLayout.SetLayoutBounds(lv, new Rectangle(lv.Model.ViewLeft, lv.Model.ViewTop, lv.Model.ViewWidth, lv.Model.ViewHeight));
            }
        }

        protected async void OnPaned(object sender, PanUpdatedEventArgs e)
        {
            if (CheckRules())
            {
                return;
            }

            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    {
                        SelectedViews = Views.FindAll(x => x.Model.Selected == true);
                        InitMovement();
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
                        dx = CorrectionDX(dx);
                        dy = CorrectionDY(dy);
                        await Move(dx, dy);
                        break;
                    }
                case GestureStatus.Completed:
                    {
                        x = 0;
                        y = 0;
                        oldeTotalX = 0;
                        oldeTotalY = 0;

                        foreach (SchemeBaseView lv in SelectedViews)
                        {
                            if (lv.Model.EditMode == SchemeElementEditMode.Move)
                            {
                                double newX = lv.X + lv.TranslationX;
                                double newY = lv.Y + lv.TranslationY;

                                lv.Model.Left = (int)Math.Round(newX / BaseModel.WidthStep);
                                lv.Model.Top = (int)Math.Round(newY / BaseModel.HeightStep);

                                double dX = lv.Model.Left * BaseModel.WidthStep - lv.X;
                                double dY = lv.Model.Top * BaseModel.HeightStep - lv.Y;

                                await lv.TranslateTo(dX, dY, 500, easingParcking);
                                AbsoluteLayout.SetLayoutBounds(lv, new Rectangle(lv.X + dX, lv.Y + dY, lv.Width, lv.Height));
                                lv.TranslationX = 0;
                                lv.TranslationY = 0;
                            }
                            if (lv.Model.EditMode == SchemeElementEditMode.Resize)
                            {
                                lv.Model.Width = (int)Math.Round(lv.Width / BaseModel.WidthStep);
                                lv.Model.Height = (int)Math.Round(lv.Height / BaseModel.HeightStep);
                                double newWidth = lv.Model.Width * BaseModel.WidthStep;
                                double newheight = lv.Model.Height * BaseModel.HeightStep;
                                AbsoluteLayout.SetLayoutBounds(lv, new Rectangle(lv.X, lv.Y, newWidth, newheight));
                            }
                            lv.Opacity = 1;
                        }

                        BaseModel.SaveChangesAsync();
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

        private void InitMovement()
        {
            MovingAction = MovingActionTypeEnum.Pan;

            leftborder = double.MaxValue;
            topborder = double.MaxValue;
            rightborder = double.MinValue;
            bottomborder = double.MinValue;
            foreach (SchemeBaseView lv in SelectedViews)
            {
                leftborder = Math.Min(lv.X, leftborder);
                topborder = Math.Min(lv.Y, topborder);
                rightborder = Math.Max(lv.X + lv.Width, rightborder);
                bottomborder = Math.Max(lv.Y + lv.Height, bottomborder);
                lv.Opacity = 0.5;
                lv.Model.SavePrevSize(lv.Width, lv.Height);
            }
        }
        private double CorrectionDX(double dx)
        {
            double dxrv = dx;

            if (dx + leftborder < 0)
            {
                dxrv = -leftborder;
            }

            if (dx + rightborder > BaseModel.ScreenWidth)
            {
                dxrv = BaseModel.ScreenWidth - rightborder;
            }

            return dxrv;
        }

        private double CorrectionDY(double dy)
        {
            double dyrv = dy;

            if (dy + topborder < 0)
            {
                dyrv = -topborder;
            }

            if (dy + bottomborder > (BaseModel.ScreenHeight))
            {
                dyrv = BaseModel.ScreenHeight - bottomborder;
            }

            return dyrv;
        }

        private async Task Move(double dx, double dy)
        {
            foreach (SchemeBaseView lv in SelectedViews)
            {
                if (lv.Model.EditMode == SchemeElementEditMode.Move)
                {
                    await lv.TranslateTo(dx, dy, 250, easing1);
                }
                if (lv.Model.EditMode == SchemeElementEditMode.Resize)
                {
                    AbsoluteLayout.SetLayoutBounds(lv, new Rectangle(lv.X, lv.Y, lv.Model.PrevViewWidth + dx, lv.Model.PrevViewHeight + dy));
                }
            }
        }

        private bool CheckRules()
        {
            if (!BaseModel.IsEditMode)
            {
                return true;
            }

            if ((MovingAction != MovingActionTypeEnum.None) && (MovingAction != MovingActionTypeEnum.Pan))
            {
                return true;
            }

            //if (!Model.IsSelectedList)
            //{
            //    return true;
            //}

            return false;
        }
    }
}
