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
using WarehouseControlSystem.Helpers.NAV;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.View.Pages.LocationsScheme;
using WarehouseControlSystem.View.Pages.ZonesScheme;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using WarehouseControlSystem.Resx;
using System.Windows.Input;
using System.Threading;

namespace WarehouseControlSystem.ViewModel
{
    public class LocationsViewModel : LocationsPlanViewModel
    {
        public LocationsViewModel(INavigation navigation) : base(navigation)
        {
        }

        public async void LoadAll()
        {
            if (NotNetOrConnection)
            {
                return;
            }

            try
            {
                State = ModelState.Loading;
                List<Location> list = await NAV.GetLocationList("", false, 1, int.MaxValue, ACD.Default).ConfigureAwait(true);
                if ((!IsDisposed) && (list is List<Location>))
                {
                    if (list.Count > 0)
                    {
                        FillModel(list);
                    }
                    else
                    {
                        State = ModelState.Error;
                        ErrorText = "No Data";
                    }
                }
            }
            catch (OperationCanceledException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                ErrorText = e.Message;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                ErrorText = e.Message;
                State = ModelState.Error;
                ErrorText = AppResources.Error_LoadLocationList;
            }
        }

        private void FillModel(List<Location> list)
        {
            ClearAll();
            foreach (Location location in list)
            {
                LocationViewModel lvm = new LocationViewModel(Navigation, location);
                LocationViewModels.Add(lvm);
            }
            State = ModelState.Normal;
        }
    }
}
