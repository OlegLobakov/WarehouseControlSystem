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
using WarehouseControlSystem.ViewModel.Base;
using WarehouseControlSystem.Model.NAV;
using Xamarin.Forms;
using System.Windows.Input;

namespace WarehouseControlSystem.ViewModel
{
    public class EmptySpaceViewModel : BaseViewModel
    {
        public int Section
        {
            get { return section; }
            set
            {
                if (section != value)
                {
                    section = value;
                    OnPropertyChanged(nameof(Section));
                }
            }
        }
        int section;

        public int Level
        {
            get { return level; }
            set
            {
                if (level != value)
                {
                    level = value;
                    OnPropertyChanged(nameof(Level));
                }
            }
        }
        int level;

        public int Depth
        {
            get { return depth; }
            set
            {
                if (depth != value)
                {
                    depth = value;
                    OnPropertyChanged(nameof(Depth));
                }
            }
        }
        int depth;


        public ICommand TapCommand { protected set; get; }
        public event Action<EmptySpaceViewModel> OnTap;

        public EmptySpaceViewModel(INavigation navigation) : base(navigation)
        {
            TapCommand = new Command(Tap);
            State = ModelState.Undefined;
        }

        private void Tap()
        {
            if (OnTap is Action<EmptySpaceViewModel>)
            {
                OnTap(this);
            }
        }
    }
}
