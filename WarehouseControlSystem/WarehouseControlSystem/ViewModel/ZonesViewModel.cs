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
    public class ZonesViewModel : PlanBaseViewModel
    {
        public Location Location { get; set; }

        public ZoneViewModel SelectedZoneViewModel { get; set; }
        public ObservableCollection<ZoneViewModel> ZoneViewModels { get; set; }
        public ObservableCollection<ZoneViewModel> SelectedViewModels { get; set; }

        public ICommand ListZonesCommand { protected set; get; }
        public ICommand NewZoneCommand { protected set; get; }
        public ICommand EditZoneCommand { protected set; get; }
        public ICommand DeleteZoneCommand { protected set; get; }

        public bool IsSelectedList { get { return SelectedViewModels.Count > 0; } }

        public ZonesViewModel(INavigation navigation, Location location) : base(navigation)
        {
            Location = location;

            ZoneViewModels = new ObservableCollection<ZoneViewModel>();
            SelectedViewModels = new ObservableCollection<ZoneViewModel>();

            ListZonesCommand = new Command(ListZones);
            NewZoneCommand = new Command(NewZone);
            EditZoneCommand = new Command(EditZone);
            DeleteZoneCommand = new Command(DeleteZone);

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
            SelectedViewModels.Clear();
            foreach (ZoneViewModel zvm in ZoneViewModels)
            {
                zvm.DisposeModel();
            }
            ZoneViewModels.Clear();
        }

        public async void Load()
        {
            if (NotNetOrConnection)
            {
                return;
            }

            try
            {
                State = ModelState.Loading;
                List<Zone> zones = await NAV.GetZoneList(Location.Code, "", true, 1, int.MaxValue, ACD.Default).ConfigureAwait(true);

                
                if (zones is List<Zone>)
                {
                    if (zones.Count > 0)
                    {
                        ClearAll();

                        int deftop = 1;
                        int defleft = 1;
                        int defwidth = Math.Max(1, (PlanWidth - 6) / 5);
                        int defheight = Math.Max(1, (PlanHeight - 5) / 4);

                        foreach (Zone zone in zones)
                        {
                            if (zone.Width == 0)
                            {
                                zone.Left = defleft;
                                zone.Width = defwidth;
                                zone.Height = defheight;
                                zone.Top = deftop;

                                defleft = defleft + defwidth + 1;
                                if (defleft > (PlanWidth - defwidth))
                                {
                                    defleft = 1;
                                    deftop = deftop + defheight + 1;
                                }

                                if (deftop > (PlanHeight - defheight))
                                {
                                    deftop = 1;
                                }
                            }

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
                        State = ModelState.Error;
                        ErrorText = "No Data";
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

        private async void Zvm_OnTap(ZoneViewModel zvm)
        {
            if (!IsEditMode)
            {
                await Navigation.PushAsync(new RacksSchemePage(zvm.Zone));
            }
            else
            {
                foreach (ZoneViewModel zv in ZoneViewModels)
                {
                    if (zv != zvm)
                    {
                        zv.Selected = false;
                        zv.EditMode = SchemeElementEditMode.None;
                    }
                }
                if (zvm.Selected)
                {
                    switch (zvm.EditMode)
                    {
                        case SchemeElementEditMode.None:
                            break;
                        case SchemeElementEditMode.Move:
                            zvm.EditMode = SchemeElementEditMode.Resize;
                            break;
                        case SchemeElementEditMode.Resize:
                            zvm.Selected = false;
                            zvm.EditMode = SchemeElementEditMode.None;
                            break;
                        default:
                            throw new InvalidOperationException("ZonesViewModel.Zvm_OnTap Impossible value");
                    }
                }
                else
                {
                    zvm.Selected = true;
                    zvm.EditMode = SchemeElementEditMode.Move;
                }

                SelectedViewModels = new ObservableCollection<ZoneViewModel>(ZoneViewModels.ToList().FindAll(x => x.Selected == true));
            }
        }

        public override void Rebuild(bool recreate)
        {
            double widthstep = (ScreenWidth / PlanWidth);
            double heightstep = (ScreenHeight / PlanHeight);
            foreach (ZoneViewModel zvm in ZoneViewModels)
            {
                zvm.Left = zvm.Zone.Left * widthstep;
                zvm.Top = zvm.Zone.Top * heightstep;
                zvm.Width = zvm.Zone.Width * widthstep;
                zvm.Height = zvm.Zone.Height * heightstep;
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
                zvm.Selected = false;
            }
            SelectedViewModels.Clear();
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
            NewZonePage nzp = new NewZonePage(zvm);
            await Navigation.PushAsync(nzp);
        }

        public async void EditZone(object obj)
        {
            if (obj is ZoneViewModel)
            {
                ZoneViewModel zvm = (ZoneViewModel)obj;
                zvm.CreateMode = false;
                NewZonePage nzp = new NewZonePage(zvm);
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

        public async void SaveLocationParams()
        {
            if (NotNetOrConnection)
            {
                return;
            }

            Location.PlanWidth = PlanWidth;
            Location.PlanHeight = PlanHeight;
            await NAV.ModifyLocation(Location, ACD.Default);
        }

        public async void SaveZonesChangesAsync()
        {
            if (NotNetOrConnection)
            {
                return;
            }

            List<ZoneViewModel> list = ZoneViewModels.ToList().FindAll(x => x.Selected == true);
            foreach (ZoneViewModel zvm in list)
            {
                try
                {
                    await NAV.ModifyZone(zvm.Zone, ACD.Default);
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
                if (zvm.Zone.Left + zvm.Zone.Width > newminplanwidth)
                {
                    newminplanwidth = zvm.Zone.Left + zvm.Zone.Width;
                }

                if (zvm.Zone.Top + zvm.Zone.Height > newminplanheight)
                {
                    newminplanheight = zvm.Zone.Top + zvm.Zone.Height;
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
