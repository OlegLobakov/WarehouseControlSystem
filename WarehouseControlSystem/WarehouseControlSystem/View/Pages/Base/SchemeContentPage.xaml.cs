using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.Resx;
using WarehouseControlSystem.ViewModel.Base;
using WarehouseControlSystem.View.Pages.Find;

namespace WarehouseControlSystem.View.Pages.Base
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SchemeContentPage : ContentPage
	{
        //////UNDER CONSTRUCTION
        //List<SchemeView> Views { get; set; } = new List<SchemeView>();
        //List<SchemeView> SelectedViews { get; set; } = new List<SchemeView>();

        //MovingActionTypeEnum MovingAction = MovingActionTypeEnum.None;
        //TapGestureRecognizer TapGesture;
        //PanGestureRecognizer PanGesture;

        //readonly Easing easing1 = Easing.Linear;
        //readonly Easing easingParcking = Easing.CubicInOut;

        //double x = 0, y = 0, widthstep = 0, heightstep = 0;

        //double leftborder = double.MaxValue;
        //double topborder = double.MaxValue;
        //double rightborder = double.MinValue;
        //double bottomborder = double.MinValue;

        //double oldeTotalX = 0, oldeTotalY = 0;

        //public PlanBaseViewModel PlanBaseViewModel { get; private set; }



        //private void StackLayout_SizeChanged(object sender, EventArgs e)
        //{
        //    StackLayout sl = (StackLayout)sender;
        //    model.SetScreenSizes(sl.Width, sl.Height, false);
        //}

        //private void Abslayout_SizeChanged(object sender, EventArgs e)
        //{
        //    AbsoluteLayout al = (AbsoluteLayout)sender;
        //    model.SetScreenSizes(al.Width, al.Height, true);
        //}

        //public SchemeContentPage()
        //{
        //    InitializeComponent();

        //    TapGesture = new TapGestureRecognizer();
        //    PanGesture = new PanGestureRecognizer();
        //}

        //private async void OnPaned(object sender, PanUpdatedEventArgs e)
        //{
        //    if (!CheckPanConditions())
        //    {
        //        return;
        //    }

        //    switch (e.StatusType)
        //    {
        //        case GestureStatus.Started:
        //            {
        //                //SelectedViews = Views.FindAll(x => x.Model.Selected == true);
        //                //MovingAction = MovingActionTypeEnum.Pan;

        //                //widthstep = (PlanBaseViewModel.ScreenWidth / PlanBaseViewModel.PlanWidth);
        //                //heightstep = (PlanBaseViewModel.ScreenHeight / PlanBaseViewModel.PlanHeight);

        //                //leftborder = double.MaxValue;
        //                //topborder = double.MaxValue;
        //                //rightborder = double.MinValue;
        //                //bottomborder = double.MinValue;

        //                //foreach (ContentView cv in Views)
        //                //{
        //                //    leftborder = Math.Min(cv.X, leftborder);
        //                //    topborder = Math.Min(cv.Y, topborder);
        //                //    rightborder = Math.Max(cv.X + cv.Width, rightborder);
        //                //    bottomborder = Math.Max(cv.Y + cv.Height, bottomborder);
        //                //    cv.Opacity = 0.5;
        //                //    cv.Model.SavePrevSize(cv.Width, cv.Height);
        //                //}

        //                //x += oldeTotalX;
        //                //y += oldeTotalY;
        //                break;
        //            }
        //        case GestureStatus.Running:
        //            {
        //                //double dx = x + e.TotalX;
        //                //double dy = y + e.TotalY;

        //                //oldeTotalX = e.TotalX;
        //                //oldeTotalY = e.TotalY;

        //                //if (dx + leftborder < 0)
        //                //{
        //                //    dx = -leftborder;
        //                //}

        //                //if (dx + rightborder > PlanBaseViewModel.ScreenWidth)
        //                //{
        //                //    dx = PlanBaseViewModel.ScreenWidth - rightborder;
        //                //}

        //                //if (dy + topborder < 0)
        //                //{
        //                //    dy = -topborder;
        //                //}

        //                //if (dy + bottomborder > PlanBaseViewModel.ScreenHeight)
        //                //{
        //                //    dy = PlanBaseViewModel.ScreenHeight - bottomborder;
        //                //}

        //                //foreach (ContentView zv in Views)
        //                //{
        //                //    if (zv.Model.EditMode == SchemeElementEditMode.Move)
        //                //    {
        //                //        await zv.TranslateTo(dx, dy, 250, easing1);
        //                //    }
        //                //    if (zv.Model.EditMode == SchemeElementEditMode.Resize)
        //                //    {
        //                //        AbsoluteLayout.SetLayoutBounds(zv, new Rectangle(zv.X, zv.Y, zv.Model.PrevWidth + dx, zv.Model.PrevHeight + dy));
        //                //    }
        //                //}
        //                break;
        //            }
        //        case GestureStatus.Completed:
        //            {

        //                //x = 0;
        //                //y = 0;
        //                //oldeTotalX = 0;
        //                //oldeTotalY = 0;
        //                //foreach (ZoneView zv in SelectedViews)
        //                //{
        //                //    if (zv.Model.EditMode == SchemeElementEditMode.Move)
        //                //    {
        //                //        double newX = zv.X + zv.TranslationX;
        //                //        double newY = zv.Y + zv.TranslationY;

        //                //        zv.Model.Zone.Left = (int)Math.Round(newX / widthstep);
        //                //        zv.Model.Zone.Top = (int)Math.Round(newY / heightstep);

        //                //        //выравнивание по сетке
        //                //        double dX = zv.Model.Zone.Left * widthstep - zv.X;
        //                //        double dY = zv.Model.Zone.Top * heightstep - zv.Y;

        //                //        await zv.TranslateTo(dX, dY, 500, easingParcking);
        //                //        AbsoluteLayout.SetLayoutBounds(zv, new Rectangle(zv.X + dX, zv.Y + dY, zv.Width, zv.Height));
        //                //        zv.TranslationX = 0;
        //                //        zv.TranslationY = 0;
        //                //    }
        //                //    if (zv.Model.EditMode == SchemeElementEditMode.Resize)
        //                //    {
        //                //        zv.Model.Zone.Width = (int)Math.Round(zv.Width / widthstep);
        //                //        zv.Model.Zone.Height = (int)Math.Round(zv.Height / heightstep);
        //                //        double newWidth = zv.Model.Zone.Width * widthstep;
        //                //        double newheight = zv.Model.Zone.Height * heightstep;
        //                //        AbsoluteLayout.SetLayoutBounds(zv, new Rectangle(zv.X, zv.Y, newWidth, newheight));
        //                //    }
        //                //    zv.Opacity = 1;
        //                //}
        //                //model.SaveZonesChangesAsync();
        //                //MovingAction = MovingActionTypeEnum.None;
        //                break;
        //            }
        //        case GestureStatus.Canceled:
        //            {
        //                MovingAction = MovingActionTypeEnum.None;
        //                break;
        //            }
        //        default:
        //            throw new InvalidOperationException("ZonesSchemePage OnPaned Impossible Value ");
        //    }
        //}

        //private bool CheckPanConditions()
        //{
        //    //if (!model.IsEditMode)
        //    //{
        //    //    return false;
        //    //}

        //    //if ((MovingAction != MovingActionTypeEnum.None) && (MovingAction != MovingActionTypeEnum.Pan))
        //    //{
        //    //    return false;
        //    //}

        //    //if (!model.IsSelectedList)
        //    //{
        //    //    return false;
        //    //}
        //    return true;
        //}
    }
}