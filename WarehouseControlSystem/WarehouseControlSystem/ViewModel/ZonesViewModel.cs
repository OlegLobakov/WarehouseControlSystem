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
using WarehouseControlSystem.Helpers.Containers.StateContainer;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.Model.NAV;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using WarehouseControlSystem.Helpers.NAV;
using WarehouseControlSystem.Resx;
using System.Windows.Input;
using Plugin.Connectivity;
using WarehouseControlSystem.View.Pages.ZonesScheme;
using WarehouseControlSystem.View.Pages.RackScheme;
using System.Threading;

namespace WarehouseControlSystem.ViewModel
{
    public class ZonesViewModel : BaseViewModel
    {
        public Location Location { get; set; }

        public ZoneViewModel SelectedZoneViewModel { get; set; }
        public ObservableCollection<ZoneViewModel> ZoneViewModels { get; set; }
        public ObservableCollection<ZoneViewModel> SelectedViewModels { get; set; }

        public RunModeEnum RunMode
        {
            get { return runmode; }
            set
            {
                if (runmode != value)
                {
                    runmode = value;
                    OnPropertyChanged("RunMode");
                }
            }
        } RunModeEnum runmode;

        public ICommand ListZonesCommand { protected set; get; }
        public ICommand NewZoneCommand { protected set; get; }
        public ICommand EditZoneCommand { protected set; get; }
        public ICommand DeleteZoneCommand { protected set; get; }
        public ICommand ParamsCommand { protected set; get; }

        public int PlanHeight
        {
            get
            {
                return Location.PlanHeight;
            }
            set
            {
                if (Location.PlanHeight != value)
                {
                    Location.PlanHeight = value;
                    OnPropertyChanged("PlanHeight");
                }
            }
        }
        public int PlanWidth
        {
            get
            {
                return Location.PlanWidth;
            }
            set
            {
                if (Location.PlanWidth != value)
                {
                    Location.PlanWidth = value;
                    OnPropertyChanged("PlanWidth");
                }
            }
        }

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

        public bool IsSelectedList { get { return SelectedViewModels.Count > 0; } }

        public double ScreenWidth { get; set; }
        public double ScreenHeight { get; set; }

        public ZonesViewModel(INavigation navigation, Location location) : base(navigation)
        {
            Location = location;
            State = State.Normal;
            ZoneViewModels = new ObservableCollection<ZoneViewModel>();
            SelectedViewModels = new ObservableCollection<ZoneViewModel>();


            ListZonesCommand = new Command(ListZones);
            NewZoneCommand = new Command(NewZone);
            EditZoneCommand = new Command(EditZone);
            DeleteZoneCommand = new Command(DeleteZone);
            ParamsCommand = new Command(Params);

            RunMode = RunModeEnum.View;
            Title = AppResources.ZoneListPage_Title + " - " + location.Code;
        }

        public void ClearAll()
        {
            SelectedZoneViewModel = null;
            SelectedViewModels.Clear();
            foreach (ZoneViewModel zvm in ZoneViewModels)
            {
                zvm.Dispose();
            }
            ZoneViewModels.Clear();
        }

        public async void Load()
        {
            if ((!CrossConnectivity.Current.IsConnected) || (Global.CurrentConnection == null))
            {
                State = State.NoInternet;
                return;
            }
            State = State.Loading;
            try
            {
                List<Zone> zones = await NAV.GetZoneList(Location.Code, "", true, 1, int.MaxValue, ACD.Default);
                if (zones is List<Zone>)
                {
                    if (zones.Count > 0)
                    {
                        ClearAll();
                        int deftop = 1;
                        int defleft = 1;
                        int defwidth = Math.Max(1, (Location.PlanWidth - 6) / 5);
                        int defheight = Math.Max(1, (Location.PlanHeight - 5) / 4);

                        foreach (Zone zone in zones)
                        {
                            if (zone.Width == 0)
                            {
                                zone.Left = defleft;
                                zone.Width = defwidth;
                                zone.Height = defheight;
                                zone.Top = deftop;

                                defleft = defleft + defwidth + 1;
                                if (defleft > (Location.PlanWidth - defwidth))
                                {
                                    defleft = 1;
                                    deftop = deftop + defheight + 1;
                                }

                                if (deftop > (Location.PlanHeight - defheight))
                                {
                                    deftop = 1;
                                }
                            }
                            ZoneViewModel zvm = new ZoneViewModel(Navigation, zone);
                            zvm.OnTap += Zvm_OnTap;
                            ZoneViewModels.Add(zvm);
                        }
                        State = State.Normal;
                        UpdateMinSizes();
                        ReDesign();
                    }
                    else
                    {
                        State = State.NoData;
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch
            {
                State = State.Error;
                ErrorText = AppResources.Error_LoadZones;
            }
        }

        public async void LoadAll()
        {
            if ((!CrossConnectivity.Current.IsConnected) || (Global.CurrentConnection == null))
            {
                State = State.NoInternet;
                return;
            }
            State = State.Loading;
            try
            {
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
                        State = State.Normal;
                    }
                    else
                    {
                        State = State.NoData;
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch
            {
                State = State.Error;
                ErrorText = AppResources.Error_LoadZoneList;
            }
        }

        private async void Zvm_OnTap(ZoneViewModel zvm)
        {
            if (RunMode == RunModeEnum.View)
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

        public void ReDesign()
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
            MessagingCenter.Send(this, "Rebuild");
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
            Zone zone = new Zone();
            zone.LocationCode = Location.Code;
            ZoneViewModel zvm = new ZoneViewModel(Navigation, zone);
            zvm.CreateMode = true;
            zvm.CanChangeLocationCode = false;
            NewZonePage nzp = new NewZonePage(zvm);
            await Navigation.PushAsync(nzp);
        }

        public async void EditZone(object obj)
        {
            ZoneViewModel zvm = (ZoneViewModel)obj;
            if (zvm is ZoneViewModel)
            {
                zvm.CreateMode = false;
                NewZonePage nzp = new NewZonePage(zvm);
                await Navigation.PushAsync(nzp);   
            }
        }

        public async void DeleteZone(object obj)
        {
            ZoneViewModel zvm = (ZoneViewModel)obj;
            State = State.Loading;
            LoadAnimation = true;
            try
            {
                await NAV.DeleteZone(zvm.Zone, ACD.Default);
                ZoneViewModels.Remove(zvm);
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
                State = State.Error;
            }
            finally
            {
                State = State.Normal;
                LoadAnimation = false;
            }
        }

        public async void Params()
        {
            State = State.Normal;
            ZonesFieldParamsPage zfpp = new ZonesFieldParamsPage(this);
            await Navigation.PushAsync(zfpp);
        }

        public Task<string> SaveZonesVisible()
        {
            var tcs = new TaskCompletionSource<string>();
            string rv = "";
            Task.Run(async () =>
            {
                try
                {
                    List<ZoneViewModel> list = ZoneViewModels.ToList().FindAll(x => x.Changed == true);
                    foreach (ZoneViewModel zvm in list)
                    {
                        Zone zone = new Zone();
                        zvm.SaveFields(zone);
                        await NAV.SetZoneVisible(zone, ACD.Default);
                    }
                    tcs.SetResult(rv);
                }
                catch
                {
                    tcs.SetResult(rv);
                }
            });
            return tcs.Task;
        }

        public async void SaveLocationParams()
        {
            await NAV.ModifyLocation(Location, ACD.Default);
        }

        public async void SaveZonesChangesAsync()
        {
            List<ZoneViewModel> list = ZoneViewModels.ToList().FindAll(x => x.Selected == true);
            foreach (ZoneViewModel zvm in list)
            {
                try
                {
                    await NAV.ModifyZone(zvm.Zone, ACD.Default);
                }
                catch (Exception ex)
                {
                    State = State.Error;
                    ErrorText = ex.Message;
                }
            }
        }

        public void UpdateMinSizes()
        {
            foreach (ZoneViewModel zvm in ZoneViewModels)
            {
                if (zvm.Zone.Left + zvm.Zone.Width > 10)
                {
                    MinPlanWidth = zvm.Zone.Left + zvm.Zone.Width;
                }
                if (zvm.Zone.Top + zvm.Zone.Height > 5)
                {
                    MinPlanHeight = zvm.Zone.Top + zvm.Zone.Height;
                }
            }
        }

        public override void Dispose()
        {
            ClearAll();
            ListZonesCommand = null;
            NewZoneCommand = null;
            EditZoneCommand = null;
            DeleteZoneCommand = null;
            ParamsCommand = null;
            SelectedZoneViewModel = null;
            ZoneViewModels = null;
            SelectedViewModels = null;
            base.Dispose();
        }
    }
}
