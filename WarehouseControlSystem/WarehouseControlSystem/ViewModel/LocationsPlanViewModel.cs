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
using WarehouseControlSystem.View.Pages.Locations;
using WarehouseControlSystem.View.Pages.Zones;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using WarehouseControlSystem.Resx;
using System.Windows.Input;
using System.Threading;

namespace WarehouseControlSystem.ViewModel
{
    public class LocationsPlanViewModel : PlanBaseViewModel
    {
        public LocationViewModel SelectedLocationViewModel { get; set; }
        public ObservableCollection<LocationViewModel> LocationViewModels { get; set; } = new ObservableCollection<LocationViewModel>();

        public ICommand ListLocationsCommand { protected set; get; }
        public ICommand NewLocationCommand { protected set; get; }
        public ICommand EditLocationCommand { protected set; get; }
        public ICommand DeleteLocationCommand { protected set; get; }

        public LocationsPlanViewModel(INavigation navigation) : base(navigation)
        {
            ListLocationsCommand = new Command(async () => await ListLocations().ConfigureAwait(false));
            NewLocationCommand = new Command(async () => await NewLocation().ConfigureAwait(false));
            EditLocationCommand = new Command(async (x) => await EditLocation(x).ConfigureAwait(false));
            DeleteLocationCommand = new Command(async (x) => await DeleteLocation(x).ConfigureAwait(false));

            IsEditMode = true;
            Title = AppResources.LocationsSchemePage_Title;
            if (Global.CurrentConnection != null)
            {
                Title = Global.CurrentConnection.Name + " | " + AppResources.LocationsSchemePage_Title;
            }
            State = ModelState.Undefined;
            IsEditMode = false;
        }

        public void ClearAll()
        {
            foreach (LocationViewModel lvm in LocationViewModels)
            {
                lvm.DisposeModel();
            }
            LocationViewModels.Clear();
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
                PlanWidth = await NAV.GetPlanWidth(ACD.Default).ConfigureAwait(true);
                PlanHeight = await NAV.GetPlanHeight(ACD.Default).ConfigureAwait(true);
                CheckPlanSizes();
                List<Location> list = await NAV.GetLocationList("", true, 1, int.MaxValue, ACD.Default).ConfigureAwait(true);
                if ((list is List<Location>) && (!IsDisposed))
                {
                    FillModel(list);
                }
            }
            catch (OperationCanceledException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                ErrorText = e.Message;
            }
            catch (NAVErrorException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                State = ModelState.Error;
                ErrorText = AppResources.Error_LoadLocation + Environment.NewLine + e.Message;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                State = ModelState.Error;
                ErrorText = AppResources.Error_LoadLocation + Environment.NewLine + e.Message;
            }
        }

        private void FillModel(List<Location> list)
        {
            if (list.Count > 0)
            {
                ClearAll();
                foreach (Location location in list)
                {
                    SetDefaultSizes(location);
                    LocationViewModel lvm = new LocationViewModel(Navigation, location);
                    lvm.OnTap += Lvm_OnTap;
                    LocationViewModels.Add(lvm);
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

        private void SetDefaultSizes(Location location)
        {
            if (location.Width == 0)
            {
                location.Left = DefaultLeft;
                location.Width = DefaultWidth;
                location.Height = DefaultHeight;
                location.Top = DefaultTop;

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

            if (location.Left + location.Width > PlanWidth)
            {
                PlanWidth += location.Left + location.Width - PlanWidth;
            }
            if (location.Top + location.Height > PlanHeight)
            {
                PlanHeight += location.Top + location.Height - PlanHeight;
            }

        }

        public override void Rebuild(bool recreate)
        {
            if ((!CanRebuildInterface) || (LocationViewModels.Count == 0))
            {
                return;
            }

            foreach (LocationViewModel lvm in LocationViewModels)
            {
                lvm.ViewLeft = lvm.Left * WidthStep;
                lvm.ViewTop = lvm.Top * HeightStep;
                lvm.ViewWidth = lvm.Width * WidthStep;
                lvm.ViewHeight = lvm.Height * HeightStep;
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

        private void Lvm_OnTap(LocationViewModel tappedlvm)
        {
            if (!IsEditMode)
            {
                OpenZoneSchemePage(tappedlvm);
            }
            else
            {
                UnSelectAllOthers(tappedlvm);

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
                        default:
                            throw new InvalidOperationException("LocationsViewModel Lvm_OnTap Impossible Value ");
                    }
                }
                else
                {
                    tappedlvm.Selected = true;
                    tappedlvm.EditMode = SchemeElementEditMode.Move;
                }
             }
        }

        private async void OpenZoneSchemePage(LocationViewModel tappedlvm)
        {
            Global.SearchLocationCode = tappedlvm.Code;
            try
            {
                Location location = new Location();
                tappedlvm.SaveFields(location);
                ZonesPlanViewModel zpvm = new ZonesPlanViewModel(Navigation, location);
                ZonesSchemePage zsp = new ZonesSchemePage(zpvm);
                await Navigation.PushAsync(zsp);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }
        private void UnSelectAllOthers(LocationViewModel tappedlvm)
        {
            foreach (LocationViewModel lvm in LocationViewModels)
            {
                if (lvm != tappedlvm)
                {
                    lvm.Selected = false;
                    lvm.EditMode = SchemeElementEditMode.None;
                }
            }
        }
        public void UnSelectAll()
        {
            foreach (LocationViewModel lvm in LocationViewModels)
            {
                lvm.Selected = false;
            }
        }

        public async Task ListLocations()
        {
            LocationListPage llp = new LocationListPage();
            await Navigation.PushAsync(llp);
        }

        public async Task NewLocation()
        {
            Location newLocation = new Location();
            SelectedLocationViewModel = new LocationViewModel(Navigation, newLocation)
            {
                CreateMode = true
            };
            LocationViewModels.Add(SelectedLocationViewModel);
            LocationCardPage lnp = new LocationCardPage(SelectedLocationViewModel);
            await Navigation.PushAsync(lnp);
        }

        public async Task EditLocation(object obj)
        {
            LocationViewModel lvm = (LocationViewModel)obj;
            if (lvm is LocationViewModel)
            {
                lvm.CreateMode = false;
                LocationCardPage lnp = new LocationCardPage(lvm);
                await Navigation.PushAsync(lnp);
            }
        }

        public async Task DeleteLocation(object obj)
        {
            if (NotNetOrConnection)
            {
                return;
            }

            LocationViewModel lvm = (LocationViewModel)obj;

            string request = String.Format(AppResources.LocationsPlanViewModel_DeleteLocation, lvm.Name);

            var action = await App.Current.MainPage.DisplayAlert(
                AppResources.LocationsPlanViewModel_DeleteQuestion,
                request,
                "OK",
                AppResources.LocationsPlanViewModel_DeleteCancel);

            if (action)
            {
                try
                {
                    LoadAnimation = true;
                    State = ModelState.Loading;
                    await NAV.DeleteLocation(lvm.Code, ACD.Default).ConfigureAwait(true);
                    LocationViewModels.Remove(lvm);
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


        public override async void SaveChangesAsync()
        {
            if (NotNetOrConnection)
            {
                return;
            }

            List<LocationViewModel> list = LocationViewModels.ToList().FindAll(x => x.Selected == true);
            foreach (LocationViewModel lvm in list)
            {
                try
                {
                    Location location = new Location();
                    lvm.SaveFields(location);
                    await NAV.ModifyLocation(location, ACD.Default).ConfigureAwait(true);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    ErrorText = e.Message;
                }
            }
            UpdateMinSizes();
        }

        public async Task SaveSchemeParams()
        {
            if (NotNetOrConnection)
            {
                return;
            }

            try
            {
                int i = await NAV.SetPlanWidth(PlanWidth, ACD.Default).ConfigureAwait(true);
                int j = await NAV.SetPlanHeight(PlanHeight, ACD.Default).ConfigureAwait(true);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                ErrorText = e.Message;
            }
            UpdateMinSizes();
        }

        public void UpdateMinSizes()
        {
            int newminplanwidth = 0;
            int newminplanheight = 0;

            foreach (LocationViewModel lvm in LocationViewModels)
            {
                if (lvm.Left + lvm.Width > newminplanwidth)
                {
                    newminplanwidth = lvm.Left + lvm.Width;
                }
                if (lvm.Top + lvm.Height > newminplanheight)
                {
                    newminplanheight = lvm.Top + lvm.Height;
                }
            }
            MinPlanWidth = newminplanwidth;
            MinPlanHeight = newminplanheight;
        }

        public override void DisposeModel()
        {
            base.DisposeModel();
        }
    }
}
