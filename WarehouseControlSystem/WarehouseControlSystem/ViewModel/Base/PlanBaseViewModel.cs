using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Windows.Input;

namespace WarehouseControlSystem.ViewModel.Base
{
    public class PlanBaseViewModel : BaseViewModel
    {
        public double ScreenWidth
        {
            get { return screenwidth; }
            set
            {
                if (screenwidth != value)
                {
                    screenwidth = value;
                    CalcValues(false);
                    OnPropertyChanged(nameof(ScreenWidth));
                }
            }
        } double screenwidth;

        public double ScreenHeight
        {
            get { return screenheight; }
            set
            {
                if (screenheight != value)
                {
                    screenheight = value;
                    CalcValues(false);
                    OnPropertyChanged(nameof(ScreenHeight));
                }
            }
        } double screenheight;    

        public int PlanHeight
        {
            get { return planheight; }
            set
            {
                if (planheight != value)
                {
                    planheight = value;
                    CalcValues(true);
                    OnPropertyChanged(nameof(PlanHeight));
                }
            }
        } int planheight;

        public int PlanWidth
        {
            get { return planwidth; }
            set
            {
                if (planwidth != value)
                {
                    planwidth = value;
                    CalcValues(true);
                    OnPropertyChanged(nameof(PlanWidth));
                }
            }
        } int planwidth;

        public double WidthStep
        {
            get { return widthstep; }
            set
            {
                if (widthstep != value)
                {
                    widthstep = value;
                    OnPropertyChanged(nameof(WidthStep));
                }
            }
        }
        double widthstep;

        public double HeightStep
        {
            get { return heightstep; }
            set
            {
                if (heightstep != value)
                {
                    heightstep = value;
                    OnPropertyChanged(nameof(HeightStep));
                }
            }
        } double heightstep;

        public int MinPlanHeight
        {
            get { return minheight; }
            set
            {
                if (minheight != value)
                {
                    minheight = value;
                    OnPropertyChanged(nameof(MinPlanHeight));
                }
            }
        } int minheight;

        public int MinPlanWidth
        {
            get { return minwidth; }
            set
            {
                if (minwidth != value)
                {
                    minwidth = value;
                    OnPropertyChanged(nameof(MinPlanWidth));
                }
            }
        } int minwidth;

        public bool CanRebuildInterface
        {
            get { return canrebuildinterface; }
            set
            {
                if (canrebuildinterface != value)
                {
                    canrebuildinterface = value;
                    OnPropertyChanged(nameof(CanRebuildInterface));
                }
            }
        } bool canrebuildinterface;

        private void CalcValues(bool rebuild)
        {
            if ((PlanWidth > 0) && (PlanHeight > 0) && (ScreenWidth > 0) && (ScreenHeight > 0))
            {
                WidthStep = (ScreenWidth / PlanWidth);
                HeightStep = (ScreenHeight / PlanHeight);
                CanRebuildInterface = true;
                if (rebuild)
                {
                    Rebuild(false);
                }
            }
            else
            {
                CanRebuildInterface = false;
            }
        }

        public void SetScreenSizes(double width, double height, bool rebuild)
        {
            ScreenWidth = width;
            ScreenHeight = height;
            if (rebuild)
            {
                Rebuild(false);
            }
        }

        public virtual void Rebuild(bool recreate)
        {
        }

        public virtual void SaveChanges()
        {
        }

        public PlanBaseViewModel(INavigation navigation) : base(navigation)
        {
        }
    }
}
