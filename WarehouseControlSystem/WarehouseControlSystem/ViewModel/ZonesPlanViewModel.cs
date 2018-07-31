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
using WarehouseControlSystem.View.Pages.Zones;
using WarehouseControlSystem.View.Pages.Racks.Scheme;
using System.Threading.Tasks;

namespace WarehouseControlSystem.ViewModel
{
    public class ZonesPlanViewModel : PlanBaseViewModel
    {
        public Location Location { get; set; }

        public ZoneViewModel SelectedZoneViewModel { get; set; }
        public ObservableCollection<ZoneViewModel> ZoneViewModels { get; set; }
     
        public ICommand ListZonesCommand { protected set; get; }
        public ICommand NewZoneCommand { protected set; get; }
        public ICommand EditZoneCommand { protected set; get; }
        public ICommand DeleteZoneCommand { protected set; get; }

     
        public ZonesPlanViewModel(INavigation navigation, Location location) : base(navigation)
        {
            Location = location;

            ZoneViewModels = new ObservableCollection<ZoneViewModel>();
      
            ListZonesCommand = new Command(async () => await ListZones().ConfigureAwait(false));
            NewZoneCommand = new Command(async () => await NewZone().ConfigureAwait(false));
            EditZoneCommand = new Command(async (x) => await EditZone(x).ConfigureAwait(false));
            DeleteZoneCommand = new Command(async (x) => await DeleteZone(x).ConfigureAwait(false));

            IsEditMode = true;
            Title = AppResources.ZoneListPage_Title + " - " + location.Code;

            PlanWidth = location.PlanWidth;
            PlanHeight = location.PlanHeight;
            if (PlanWidth == 0)
            {
                PlanWidth = 80;
            }
            if (PlanHeight == 0)
            {
                PlanHeight = 60;
            }
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

        public async Task Load()
        {
            if (NotNetOrConnection)
            {
                return;
            }

            try
            {
                State = ModelState.Loading;
                List<Zone> zones = await NAV.GetZoneList(Location.Code, "", true, 1, int.MaxValue, ACD.Default).ConfigureAwait(true);
                if (NotDisposed)
                {
                    CheckPlanSizes();
                    if (zones is List<Zone>)
                    {
                        FillModel(zones);
                    }
                }
            }
            catch (OperationCanceledException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                State = ModelState.Error;
                ErrorText = AppResources.Error_LoadZones;
            }
        }

        private void FillModel(List<Zone> zones)
        {
            if (zones.Count > 0)
            {
                ClearAll();
                foreach (Zone zone in zones)
                {
                    SetDefaultSizes(zone);
                    ZoneViewModel zvm = new ZoneViewModel(Navigation, zone);
                    zvm.OnTap += Zvm_OnTap;
                    ZoneViewModels.Add(zvm);
                }
                State = ModelState.Normal;
                UpdateMinSizes();
                Rebuild(true);
            }
            else
            {
                State = ModelState.NoData;
            }
        }

        private void SetDefaultSizes(Zone zone)
        {
            if (zone.Width == 0)
            {
                zone.Left = DefaultLeft;
                zone.Width = DefaultWidth;
                zone.Height = DefaultHeight;
                zone.Top = DefaultTop;

                DefaultLeft = DefaultLeft + DefaultWidth + 1;
                if (DefaultLeft > (PlanWidth - DefaultWidth))
                {
                    DefaultLeft = 1;
                    DefaultTop = DefaultTop + DefaultHeight + 1;
                }

                if (DefaultTop > (PlanHeight - DefaultHeight))
                {
                    DefaultTop = 1;
                }
            }

            if (zone.Left + zone.Width > PlanWidth)
            {
                PlanWidth += zone.Left + zone.Width - PlanWidth;
            }
            if (zone.Top + zone.Height > PlanHeight)
            {
                PlanHeight += zone.Top + zone.Height - PlanHeight;
            }
        }

        private async void Zvm_OnTap(ZoneViewModel zvm)
        {
            if (!IsEditMode)
            {
                Zone zone = new Zone();
                zvm.SaveFields(zone);
                RacksViewModel rvm = new RacksViewModel(Navigation, zone);
                RacksSchemePage rsp = new RacksSchemePage(rvm);
                await Navigation.PushAsync(rsp);
            }
            else
            {
                foreach (ZoneViewModel zv in ZoneViewModels)
                {
                    if (zv != zvm)
                    {
                        zv.IsSelected = false;
                        zv.EditMode = SchemeElementEditMode.None;
                    }
                }
                if (zvm.IsSelected)
                {
                    switch (zvm.EditMode)
                    {
                        case SchemeElementEditMode.None:
                            break;
                        case SchemeElementEditMode.Move:
                            zvm.EditMode = SchemeElementEditMode.Resize;
                            break;
                        case SchemeElementEditMode.Resize:
                            zvm.IsSelected = false;
                            zvm.EditMode = SchemeElementEditMode.None;
                            break;
                        default:
                            throw new InvalidOperationException("ZonesViewModel.Zvm_OnTap Impossible value");
                    }
                }
                else
                {
                    zvm.IsSelected = true;
                    zvm.EditMode = SchemeElementEditMode.Move;
                }
            }
        }

        public override void Rebuild(bool recreate)
        {
            if ((!CanRebuildInterface) || (ZoneViewModels.Count == 0))
            {
                return;
            }

            foreach (ZoneViewModel zvm in ZoneViewModels)
            {
                zvm.ViewLeft = zvm.Left * WidthStep;
                zvm.ViewTop = zvm.Top * HeightStep;
                zvm.ViewWidth = zvm.Width * WidthStep;
                zvm.ViewHeight = zvm.Height * HeightStep;
            }

            if (recreate)
            {
                MessagingCenter.Send(this, "Rebuild");
            }
            else
            {
                MessagingCenter.Send(this, "Reshape");
            }
        }



        public void UnSelectAll()
        {
            foreach (ZoneViewModel zvm in ZoneViewModels)
            {
                zvm.IsSelected = false;
            }
        }

        public async Task ListZones()
        {
            ZoneListPage zlp = new ZoneListPage(Location);
            await Navigation.PushAsync(zlp);
        }

        public async Task NewZone()
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

        public async Task EditZone(object obj)
        {
            if (obj is ZoneViewModel)
            {
                ZoneViewModel zvm = (ZoneViewModel)obj;
                zvm.CreateMode = false;
                ZoneCardPage nzp = new ZoneCardPage(zvm);
                await Navigation.PushAsync(nzp);
            }
        }

        public async Task DeleteZone(object obj)
        {
            if (NotNetOrConnection)
            {
                return;
            }

            ZoneViewModel zvm = (ZoneViewModel)obj;

            string request = String.Format(AppResources.ZonesPlanViewModel_DeleteZone, zvm.Name);

            var action = await App.Current.MainPage.DisplayAlert(
                AppResources.ZonesPlanViewModel_DeleteQuestion,
                request,
                "OK",
                AppResources.ZonesPlanViewModel_DeleteCancel);

            if (action)
            {
                try
                {
                    State = ModelState.Loading;
                    Zone zone = new Zone();
                    zvm.SaveFields(zone);
                    await NAV.DeleteZone(zone, ACD.Default).ConfigureAwait(true);
                    ZoneViewModels.Remove(zvm);
                    State = ModelState.Normal;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    ErrorText = e.Message;
                    State = ModelState.Error;
                }
                finally
                {
                    LoadAnimation = false;
                }
            }
        }

        public async Task SaveLocationParams()
        {
            if (NotNetOrConnection)
            {
                return;
            }

            Location.PlanWidth = PlanWidth;
            Location.PlanHeight = PlanHeight;
            await NAV.ModifyLocation(Location, ACD.Default).ConfigureAwait(true);
        }

        public override async void SaveChangesAsync()
        {
            if (NotNetOrConnection)
            {
                return;
            }

            List<ZoneViewModel> list = ZoneViewModels.ToList().FindAll(x => x.IsSelected == true);
            foreach (ZoneViewModel zvm in list)
            {
                try
                {
                    Zone zone = new Zone();
                    zvm.SaveFields(zone);
                    await NAV.ModifyZone(zone, ACD.Default).ConfigureAwait(true);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    State = ModelState.Error;
                    ErrorText = e.Message;
                }
            }

            UpdateMinSizes();
        }

        public void UpdateMinSizes()
        {
            int newminplanwidth = 0; 
            int newminplanheight = 0;
            foreach (ZoneViewModel zvm in ZoneViewModels)
            {
                if (zvm.Left + zvm.Width > newminplanwidth)
                {
                    newminplanwidth = zvm.Left + zvm.Width;
                }

                if (zvm.Top + zvm.Height > newminplanheight)
                {
                    newminplanheight = zvm.Top + zvm.Height;
                }
            }
            MinPlanWidth = newminplanwidth;
            MinPlanHeight = newminplanheight;
        }

        public void SetEditModeForItems(bool iseditmode)
        {
            foreach (ZoneViewModel zvm in ZoneViewModels)
            {
                zvm.IsEditMode = iseditmode;
            }
        }

        public override void DisposeModel()
        {
            ClearAll();
            base.DisposeModel();
        }
    }
}
