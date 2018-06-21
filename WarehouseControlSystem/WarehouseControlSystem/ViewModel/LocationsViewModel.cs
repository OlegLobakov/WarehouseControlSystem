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
    public class LocationsViewModel : BaseViewModel
    {
        public ObservableCollection<LocationViewModel> LocationViewModels { get; set; } = new ObservableCollection<LocationViewModel>();
        
        public ICommand ListLocationsCommand { protected set; get; }
        public ICommand NewLocationCommand { protected set; get; }
        public ICommand EditLocationCommand { protected set; get; }
        public ICommand DeleteLocationCommand { protected set; get; }

        public LocationsViewModel(INavigation navigation) : base(navigation)
        {
            ListLocationsCommand = new Command(ListLocations);
            NewLocationCommand = new Command(NewLocation);
            EditLocationCommand = new Command(EditLocation);
            DeleteLocationCommand = new Command(DeleteLocation);

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
                        ClearAll();
                        foreach (Location location in list)
                        {
                            LocationViewModel lvm = new LocationViewModel(Navigation, location);
                            LocationViewModels.Add(lvm);
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
        
        public async void ListLocations()
        {
            LocationListPage llp = new LocationListPage();
            await Navigation.PushAsync(llp);
        }

        public async void NewLocation()
        {
            Location newLocation = new Location();
            LocationViewModel lvm = new LocationViewModel(Navigation, newLocation)
            {
                CreateMode = true
            };
            LocationViewModels.Add(lvm);
            LocationCardPage lnp = new LocationCardPage(lvm);
            await Navigation.PushAsync(lnp);
        }

        public async void EditLocation(object obj)
        {
            LocationViewModel lvm = (LocationViewModel)obj;
            if (lvm is LocationViewModel)
            {
                lvm.CreateMode = false;
                LocationCardPage lnp = new LocationCardPage(lvm);
                await Navigation.PushAsync(lnp);
            }
        }

        public async void DeleteLocation(object obj)
        {
            if (NotNetOrConnection)
            {
                return;
            }

            try
            {
                LocationViewModel lvm = (LocationViewModel)obj;
                State = ModelState.Loading;
                await NAV.DeleteLocation(lvm.Location.Code, ACD.Default);
                LocationViewModels.Remove(lvm);
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
            base.DisposeModel();
        }
    }
}
