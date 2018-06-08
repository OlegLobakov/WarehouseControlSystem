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
using WarehouseControlSystem.Helpers.Containers.StateContainer;
using Xamarin.Forms;
using System.Windows.Input;
using System.Threading;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.Resx;

namespace WarehouseControlSystem.ViewModel.Base
{
    /// <summary>
    /// Base for NAV objects
    /// </summary>
    public class NAVBaseViewModel : BaseViewModel
    {
        public string Code
        {
            get { return code; }
            set
            {
                if (code != value)
                {
                    code = value;
                    Changed = true;
                    OnPropertyChanged(nameof(Code));
                }
            }
        } string code = "";

        public string LocationCode
        {
            get { return locationcode; }
            set
            {
                if (locationcode != value)
                {
                    locationcode = value;
                    OnPropertyChanged(nameof(LocationCode));
                }
            }
        } string locationcode;

        public string ZoneCode
        {
            get { return zonecode; }
            set
            {
                if (zonecode != value)
                {
                    zonecode = value;
                    OnPropertyChanged(nameof(ZoneCode));
                }
            }
        } string zonecode;

        public string CodeWarningText
        {
            get { return codewarningtext; }
            set
            {
                if (codewarningtext != value)
                {
                    codewarningtext = value;
                    OnPropertyChanged(nameof(CodeWarningText));
                }
            }
        } string codewarningtext;

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    Changed = true;
                    OnPropertyChanged(nameof(Name));
                }
            }
        } string name;

        public Color Color
        {
            get { return color; }
            set
            {
                if (color != value)
                {
                    color = value;
                    Changed = true;
                    OnPropertyChanged(nameof(Color));
                }
            }
        } Color color;

        public bool Changed
        {
            get { return changed; }
            set
            {
                if (changed != value)
                {
                    changed = value;
                    OnPropertyChanged(nameof(Changed));
                }
            }
        } bool changed;

        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public double PrevWidth { get; set; }
        public double PrevHeight { get; set; }

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
        } int planwidth;

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
        } int planheight;

        public bool CreateMode
        {
            get { return createmode; }
            set
            {
                if (createmode != value)
                {
                    createmode = value;
                    OnPropertyChanged(nameof(CreateMode));
                }
            }
        } bool createmode;

        public string EditModeText
        {
            get { return editmodetext; }
            set
            {
                if (editmodetext != value)
                {
                    editmodetext = value;
                    Changed = true;
                    OnPropertyChanged(nameof(EditModeText));
                }
            }
        } string editmodetext = "";

        public SchemeElementEditMode EditMode
        {
            get { return editmode; }
            set
            {
                if (editmode != value)
                {
                    editmode = value;
                    switch (editmode)
                    {
                        case SchemeElementEditMode.None:
                            EditModeText = "";
                            break;

                        case SchemeElementEditMode.Move:
                            EditModeText = AppResources.ZoneView_EditMode1;
                            break;

                        case SchemeElementEditMode.Resize:
                            EditModeText = AppResources.ZoneView_EditMode2;
                            break;
                        default:
                            throw new InvalidOperationException("Impossible value");
                    }
                    Changed = true;
                    OnPropertyChanged("EditMode");
                }
            }
        }
        SchemeElementEditMode editmode;

        public NAVBaseViewModel(INavigation navigation) : base(navigation)
        {
        }
    }
}
