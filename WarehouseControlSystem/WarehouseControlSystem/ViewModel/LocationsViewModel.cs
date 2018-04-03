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
using WarehouseControlSystem.Helpers.NAV;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.View.Pages.LocationsScheme;
using WarehouseControlSystem.View.Pages.ZonesScheme;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using WarehouseControlSystem.Resx;
using System.Windows.Input;
using Plugin.Connectivity;
using System.Threading;

namespace WarehouseControlSystem.ViewModel
{
    public class LocationsViewModel : BaseViewModel
    {
        public LocationViewModel SelectedLocationViewModel { get; set; }
        public ObservableCollection<LocationViewModel> LocationViewModels { get; set; }
        public ObservableCollection<LocationViewModel> SelectedViewModels { get; set; }

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

        public ICommand ListLocationsCommand { protected set; get; }
        public ICommand NewLocationCommand { protected set; get; }
        public ICommand EditLocationCommand { protected set; get; }
        public ICommand DeleteLocationCommand { protected set; get; }
        public ICommand ParamsCommand { protected set; get; }

        public double ScreenWidth { get; set; }
        public double ScreenHeight { get; set; }

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

      
        public LocationsViewModel(INavigation navigation) : base(navigation)
        {
            State = State.Normal;
            LocationViewModels = new ObservableCollection<LocationViewModel>();
            SelectedViewModels = new ObservableCollection<LocationViewModel>();
    

            ListLocationsCommand = new Command(ListLocations);
            NewLocationCommand = new Command(NewLocation);
            EditLocationCommand = new Command(EditLocation);
            DeleteLocationCommand = new Command(DeleteLocation);
            ParamsCommand = new Command(Params);

            RunMode = RunModeEnum.View;
            Title = AppResources.LocationsSchemePage_Title;
            if (Global.CurrentConnection != null)
            {
                Title = Global.CurrentConnection.Name + " | " + AppResources.LocationsSchemePage_Title;
            }
        }

        public void ClearAll()
        {
            SelectedLocationViewModel = null;
            SelectedViewModels.Clear();
            foreach (LocationViewModel lvm in LocationViewModels)
            {
                lvm.Dispose();
            };
            LocationViewModels.Clear();
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
                PlanWidth = await NAV.GetPlanWidth(ACD.Default);
                PlanHeight = await NAV.GetPlanHeight(ACD.Default);
                if (PlanWidth == 0)
                {
                    PlanWidth = 20;
                }
                if (PlanHeight == 0)
                {
                    PlanHeight = 10;
                }
                List<Location> list = await NAV.GetLocationList("", true, 1, int.MaxValue, ACD.Default);
                if ((list is List<Location>) && (!IsDisposed))
                {
                    if (list.Count > 0)
                    {
                        ClearAll();
                        int deftop = 1;
                        int defleft = 1;
                        int defwidth = Math.Max(1, (PlanWidth - 6) / 5);
                        int defheight = Math.Max(1, (PlanHeight - 5) / 4);

                        foreach (Location location in list)
                        {
                            if (location.Width == 0)
                            {
                                location.Left = defleft;
                                location.Width = defwidth;
                                location.Height = defheight;
                                location.Top = deftop;

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

                                if (location.Left + location.Width > PlanWidth)
                                {
                                    PlanWidth += location.Left + location.Width - PlanWidth;
                                }
                                if (location.Top + location.Height > PlanHeight)
                                {
                                    PlanHeight += location.Top + location.Height - PlanHeight;
                                }
                            }
                            LocationViewModel lvm = new LocationViewModel(Navigation, location);
                            lvm.OnTap += Lvm_OnTap;
                            LocationViewModels.Add(lvm);

                            if (location.Left + location.Width > MinPlanWidth)
                            {
                                MinPlanWidth = location.Left + location.Width;
                            }
                            if (location.Top + location.Height > MinPlanHeight)
                            {
                                MinPlanHeight = location.Top + location.Height;
                            }
                        }
                        State = State.Normal;
                        ReDesign();
                    }
                    else
                    {
                        State = State.NoData;
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                ErrorText = ex.Message;
            }
            catch (NAVErrorException ex)
            {
                string error1 = ex.Message;
                string fer = ex.InnerException.ToString();
                State = State.Error;
                ErrorText = AppResources.Error_LoadLocation;
            }
            catch (Exception ex)
            {
                string error1 = ex.Message;
                string fer = ex.InnerException.ToString();
                State = State.Error;
                ErrorText = AppResources.Error_LoadLocation;
            }
        }

        public async void LoadAll()
        {
            State = State.Loading;
            try
            {
                List<Location> list = await NAV.GetLocationList("", false, 1, int.MaxValue, ACD.Default);
                if ((!IsDisposed) && (list is List<Location>))
                {
                    if (list.Count > 0)
                    {
                        ClearAll();
                        foreach (Location location in list)
                        {
                            LocationViewModel lvm = new LocationViewModel(Navigation, location);
                            LocationViewModels.Add(lvm);
                        }
                        State = State.Normal;
                    }
                    else
                    {
                        State = State.NoData;
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                ErrorText = ex.Message;
            }
            catch
            {
                State = State.Error;
                ErrorText = AppResources.Error_LoadLocationList;
            }
        }

        public void ReDesign()
        {
            double widthstep = (ScreenWidth / PlanWidth);
            double heightstep = (ScreenHeight / PlanHeight);
            foreach (LocationViewModel lvm in LocationViewModels)
            {
                lvm.Left = lvm.Location.Left * widthstep;
                lvm.Top = lvm.Location.Top * heightstep;
                lvm.Width = lvm.Location.Width * widthstep;
                lvm.Height = lvm.Location.Height * heightstep;
            }
            MessagingCenter.Send(this, "Rebuild");
        }

        private async void Lvm_OnTap(LocationViewModel tappedlvm)
        {
            if (RunMode == RunModeEnum.View)
            {
                Global.SearchLocationCode = tappedlvm.Code;
                await Navigation.PushAsync(new ZonesSchemePage(tappedlvm.Location));
            }
            else
            {
                foreach (LocationViewModel lvm in LocationViewModels)
                {
                    if (lvm != tappedlvm)
                    {
                        lvm.Selected = false;
                        lvm.EditMode = SchemeElementEditMode.None;
                    }
                }

                if (tappedlvm.Selected)
                {
                    switch (tappedlvm.EditMode)
                    {
                        case SchemeElementEditMode.None:
                            break;

                        case SchemeElementEditMode.Move:
                            tappedlvm.EditMode = SchemeElementEditMode.Resize;
                            break;

                        case SchemeElementEditMode.Resize:
                            tappedlvm.Selected = false;
                            tappedlvm.EditMode = SchemeElementEditMode.None;
                            break;
                    }
                }
                else
                {
                    tappedlvm.Selected = true;
                    tappedlvm.EditMode = SchemeElementEditMode.Move;
                }

                SelectedViewModels = new ObservableCollection<LocationViewModel>(LocationViewModels.ToList().FindAll(x => x.Selected == true));
            }
        }

        public void UnSelectAll()
        {
            foreach (LocationViewModel lvm in LocationViewModels)
            {
                lvm.Selected = false;
            }
        }

        public async void ListLocations()
        {
            LocationListPage llp = new LocationListPage();
            await Navigation.PushAsync(llp);
        }

        public async void NewLocation()
        {
            Location newLocation = new Location();
            SelectedLocationViewModel = new LocationViewModel(Navigation, newLocation);
            SelectedLocationViewModel.CreateMode = true;
            LocationViewModels.Add(SelectedLocationViewModel);
            LocationNewPage lnp = new LocationNewPage(SelectedLocationViewModel);
            await Navigation.PushAsync(lnp);
        }

        public async void EditLocation(object obj)
        {
            LocationViewModel lvm = (LocationViewModel)obj;
            if (lvm is LocationViewModel)
            {
                lvm.CreateMode = false;
                LocationNewPage lnp = new LocationNewPage(lvm);
                await Navigation.PushAsync(lnp);
            }
        }

        public async void DeleteLocation(object obj)
        {
            LocationViewModel lvm = (LocationViewModel)obj;
            State = State.Loading;
            LoadAnimation = true;
            try
            {
                await NAV.DeleteLocation(lvm.Location.Code, ACD.Default);
                LocationViewModels.Remove(lvm);
            }
            catch(Exception ex)
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
            LocationsFieldParamsPage lgpp = new LocationsFieldParamsPage(this);
            await Navigation.PushAsync(lgpp);
        }

        public async void SaveLocationChangesAsync()
        {
            List<LocationViewModel> list = LocationViewModels.ToList().FindAll(x => x.Selected == true);
            foreach (LocationViewModel lvm in list)
            {
                try
                {
                    await NAV.ModifyLocation(lvm.Location, ACD.Default);
                }
                catch (Exception ex)
                {
                    ErrorText = ex.Message;
                }
            }
        }

        public async void SaveSchemeParams()
        {
            try
            {
                await NAV.SetPlanWidth(PlanWidth, ACD.Default);
                await NAV.SetPlanHeight(PlanHeight, ACD.Default);
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
        }

        public Task<string> SaveLocationsVisible(CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<string>();
            string rv = "";
            Task.Run(async () =>
            {
                try
                {
                    List<LocationViewModel> list = LocationViewModels.ToList().FindAll(x => x.Changed == true);
                    foreach (LocationViewModel lvm in list)
                    {
                        Location location = new Location();
                        lvm.SaveFields(location);
                        int i = await NAV.SetLocationVisible(location, cts).ConfigureAwait(false);
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

        public override void Dispose()
        {
            ListLocationsCommand = null;
            NewLocationCommand = null;
            EditLocationCommand = null;
            DeleteLocationCommand = null;
            ParamsCommand = null;

            ClearAll();
            foreach (LocationViewModel lvm in LocationViewModels)
            {          
                lvm.Dispose();
            }
            base.Dispose();
        }
    }
}
