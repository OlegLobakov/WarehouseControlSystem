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
using WarehouseControlSystem.ViewModel.Base;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.Model.NAV;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using WarehouseControlSystem.Helpers.NAV;
using WarehouseControlSystem.Resx;
using System.Windows.Input;
using WarehouseControlSystem.View.Pages.ZonesScheme;
using WarehouseControlSystem.View.Pages.RackScheme;

namespace WarehouseControlSystem.ViewModel
{
    public class ZonesViewModel : ZonesPlanViewModel
    {   
        public ZonesViewModel(INavigation navigation, Location location) : base(navigation, location)
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
                List<Zone> zones = await NAV.GetZoneList(Location.Code, "", false, 1, int.MaxValue, ACD.Default);
                if (zones is List<Zone>)
                {
                    if (zones.Count > 0)
                    {
                        FillModel(zones);
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
            }
            catch
            {
                State = ModelState.Error;
                ErrorText = AppResources.Error_LoadZoneList;
            }
        }

        private void FillModel(List<Zone> zones)
        {
            ClearAll();
            foreach (Zone zone in zones)
            {
                ZoneViewModel zvm = new ZoneViewModel(Navigation, zone);
                ZoneViewModels.Add(zvm);
            }
            State = ModelState.Normal;
        }
    }
}
