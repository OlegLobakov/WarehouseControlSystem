using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Windows.Input;

namespace WarehouseControlSystem.ViewModel.Base
{
    public class PlanBaseViewModel : BaseViewModel
    {
        public double ScreenWidth { get; set; }
        public double ScreenHeight { get; set; }

        public int PlanHeight
        {
            get { return planheight; }
            set
            {
                if (planheight != value)
                {
                    planheight = value;
                    OnPropertyChanged(nameof(PlanHeight));
                }
            }
        }
        int planheight;
        public int PlanWidth
        {
            get { return planwidth; }
            set
            {
                if (planwidth != value)
                {
                    planwidth = value;
                    OnPropertyChanged(nameof(PlanWidth));
                }
            }
        }
        int planwidth;

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
        }
        int minheight;
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
        }
        int minwidth;

        public PlanBaseViewModel(INavigation navigation) : base(navigation)
        {

        }
    }
}
