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
using WarehouseControlSystem.View.Pages.Racks.Scheme;
using System.Windows.Input;
using System.Threading;

namespace WarehouseControlSystem.ViewModel
{
    public class RacksViewModel : RacksPlanViewModel
    {
        public RacksViewModel(INavigation navigation, Zone zone) : base(navigation, zone)
        {
        }

        public async Task LoadAll()
        {
            if (NotNetOrConnection)
            {
                return;
            }

            try
            {
                State = ModelState.Loading;
                List<Rack> racks = await NAV.GetRackList(Zone.LocationCode, Zone.Code, false, 1, int.MaxValue, ACD.Default).ConfigureAwait(true);
                if ((NotDisposed) && (racks is List<Rack>))
                {
                    FillModel(racks);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                State = ModelState.Error;
                ErrorText = AppResources.Error_LoadRacksList;
            }
        }

        private void FillModel(List<Rack> racks)
        {
            if (racks.Count > 0)
            {
                ObservableCollection<RackViewModel> nlist = new ObservableCollection<RackViewModel>();
                foreach (Rack rack in racks)
                {
                    RackViewModel rvm = new RackViewModel(Navigation, rack);
                    nlist.Add(rvm);
                }
                RackViewModels = nlist;
                State = ModelState.Normal;
            }
            else
            {
                State = ModelState.NoData;
            }
        }

        public override void DisposeModel()
        {
            ClearAll();
            base.DisposeModel();
        }
    }
}
