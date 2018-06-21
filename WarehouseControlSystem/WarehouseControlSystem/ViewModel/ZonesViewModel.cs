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
    public class ZonesViewModel : BaseViewModel
    {
        public Location Location { get; set; }

        public ObservableCollection<ZoneViewModel> ZoneViewModels { get; set; }

        public ICommand ListZonesCommand { protected set; get; }
        public ICommand NewZoneCommand { protected set; get; }
        public ICommand EditZoneCommand { protected set; get; }
        public ICommand DeleteZoneCommand { protected set; get; }

        public ZonesViewModel(INavigation navigation, Location location) : base(navigation)
        {
            Location = location;
            ZoneViewModels = new ObservableCollection<ZoneViewModel>();

            ListZonesCommand = new Command(ListZones);
            NewZoneCommand = new Command(NewZone);
            EditZoneCommand = new Command(EditZone);
            DeleteZoneCommand = new Command(DeleteZone);

            IsEditMode = true;
            Title = AppResources.ZoneListPage_Title + " - " + location.Code;
            State = ModelState.Undefined;
        }

        public void ClearAll()
        {
            foreach (ZoneViewModel zvm in ZoneViewModels)
            {
                zvm.DisposeModel();
            }
            ZoneViewModels.Clear();
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
                        ClearAll();
                        foreach (Zone zone in zones)
                        {
                            ZoneViewModel zvm = new ZoneViewModel(Navigation, zone);
                            ZoneViewModels.Add(zvm);
                        }
                        State = ModelState.Normal;
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

        public async void ListZones()
        {
            ZoneListPage zlp = new ZoneListPage(Location);
            await Navigation.PushAsync(zlp);
        }

        public async void NewZone()
        {
            Zone zone = new Zone()
            {
                LocationCode = Location.Code
            };
            ZoneViewModel zvm = new ZoneViewModel(Navigation, zone)
            {
                CreateMode = true
            };
            zvm.CanChangeLocationCode = false;
            ZoneCardPage nzp = new ZoneCardPage(zvm);
            await Navigation.PushAsync(nzp);
        }

        public async void EditZone(object obj)
        {
            if (obj is ZoneViewModel)
            {
                ZoneViewModel zvm = (ZoneViewModel)obj;
                zvm.CreateMode = false;
                ZoneCardPage nzp = new ZoneCardPage(zvm);
                await Navigation.PushAsync(nzp);
            }
        }

        public async void DeleteZone(object obj)
        {
            if (NotNetOrConnection)
            {
                return;
            }

            try
            {
                ZoneViewModel zvm = (ZoneViewModel)obj;
                State = ModelState.Loading;
                await NAV.DeleteZone(zvm.Zone, ACD.Default);
                ZoneViewModels.Remove(zvm);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                ErrorText = e.Message;
                State = ModelState.Error;
            }
            finally
            {
                State = ModelState.Normal;
                LoadAnimation = false;
            }
        }

        public override void DisposeModel()
        {
            ClearAll();
            base.DisposeModel();
        }
    }
}
