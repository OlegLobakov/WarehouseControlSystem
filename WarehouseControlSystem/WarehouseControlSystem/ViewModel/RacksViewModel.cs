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
using WarehouseControlSystem.ViewModel.Base;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.Model;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Plugin.Connectivity;
using WarehouseControlSystem.Helpers.NAV;
using WarehouseControlSystem.Resx;
using WarehouseControlSystem.View.Pages.RackScheme;
using System.Windows.Input;
using System.Threading;

namespace WarehouseControlSystem.ViewModel
{
    public class RacksViewModel : BaseViewModel
    {
        public Zone Zone { get; set; }

        public ObservableCollection<RackViewModel> RackViewModels { get; set; }

        public ICommand NewRackCommand { protected set; get; }
        public ICommand EditRackCommand { protected set; get; }
        public ICommand DeleteRackCommand { protected set; get; }


        public RacksViewModel(INavigation navigation, Zone zone) : base(navigation)
        {
            Zone = zone;
            RackViewModels = new ObservableCollection<RackViewModel>();

            NewRackCommand = new Command(NewRack);
            EditRackCommand = new Command(EditRack);
            DeleteRackCommand = new Command(DeleteRack);

            State = ModelState.Undefined;
            IsEditMode = false;
        }

        public void ClearAll()
        {
            foreach (RackViewModel lvm in RackViewModels)
            {
                lvm.DisposeModel();
            }
            RackViewModels.Clear();
        }

        public async void NewRack()
        {
            Rack newrack = new Rack
            {
                Sections = Settings.DefaultRackSections,
                Levels = Settings.DefaultRackLevels,
                Depth = Settings.DefaultRackDepth,
                SchemeVisible = true,
            };
            RackViewModel rvm = new RackViewModel(Navigation, newrack, true)
            {
                LocationCode = Zone.LocationCode,
                ZoneCode = Zone.Code,
                CanChangeLocationAndZone = false
            };
            RackNewPage rnp = new RackNewPage(rvm);
            await Navigation.PushAsync(rnp);
        }

        public void EditRack(object obj)
        {
            System.Diagnostics.Debug.WriteLine(obj.ToString());
        }

        public void DeleteRack(object obj)
        {
            System.Diagnostics.Debug.WriteLine(obj.ToString());
        }

        public override void DisposeModel()
        {
            ClearAll();
            base.DisposeModel();
        }
    }
}
