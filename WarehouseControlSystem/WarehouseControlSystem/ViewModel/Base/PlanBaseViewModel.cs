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
using System.Text;
using Xamarin.Forms;
using System.Windows.Input;
using System.Threading.Tasks;

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

        /// <summary>
        /// 
        /// </summary>
        public virtual void SaveChangesAsync()
        {
        }

        public int DefaultTop { get; set; }
        public int DefaultLeft { get; set; }
        public int DefaultWidth { get; set; }
        public int DefaultHeight { get; set; }

        public void CheckPlanSizes()
        {
            if (PlanWidth == 0)
            {
                PlanWidth = 20;
            }

            if (PlanHeight == 0)
            {
                PlanHeight = 10;
            }

            DefaultTop = 1;
            DefaultLeft = 1;
            DefaultWidth = Math.Max(1, (PlanWidth - 6) / 5);
            DefaultHeight = Math.Max(1, (PlanHeight - 5) / 4);
        }

        public PlanBaseViewModel(INavigation navigation) : base(navigation)
        {
        }
    }
}
