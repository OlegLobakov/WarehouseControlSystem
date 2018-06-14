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
using WarehouseControlSystem.Model;
using WarehouseControlSystem.Model.NAV;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using WarehouseControlSystem.Helpers.NAV;
using WarehouseControlSystem.Resx;
using System.Windows.Input;
using System.Threading;

namespace WarehouseControlSystem.ViewModel
{
    public class LocationViewModel : NAVBaseViewModel
    {
        public Location Location { get; set; }

        public string Debug
        {
            get { return debug; }
            set
            {
                if (debug != value)
                {
                    debug = value;
                    Changed = true;
                    OnPropertyChanged(nameof(Debug));
                }
            }
        } string debug;
        public string Address
        {
            get { return address1; }
            set
            {
                if (address1 != value)
                {
                    address1 = value;
                    Changed = true;
                    OnPropertyChanged(nameof(Address));
                }
            }
        } string address1;
        public bool SchemeVisible
        {
            get { return schemevisible; }
            set
            {
                if (schemevisible != value)
                {
                    schemevisible = value;
                    Changed = true;
                    SaveToNAVSchemeVisible();
                    OnPropertyChanged(nameof(SchemeVisible));
                }
            }
        } bool schemevisible;

        public int ZoneQuantity
        {
            get { return zonequantity; }
            set
            {
                if (zonequantity != value)
                {
                    zonequantity = value;
                    Changed = true;
                    OnPropertyChanged(nameof(ZoneQuantity));
                }
            }
        } int zonequantity;

        public int BinQuantity
        {
            get { return binquantity; }
            set
            {
                if (binquantity != value)
                {
                    binquantity = value;
                    Changed = true;
                    OnPropertyChanged(nameof(BinQuantity));
                }
            }
        } int binquantity;

        public bool BinMandatory
        {
            get { return binmandatory; }
            set
            {
                if (binmandatory != value)
                {
                    binmandatory = value;
                    Changed = true;
                    OnPropertyChanged(nameof(BinMandatory));
                }
            }
        } bool binmandatory;

        public bool RequireReceive
        {
            get { return requirereceive; }
            set
            {
                if (requirereceive != value)
                {
                    requirereceive = value;
                    Changed = true;
                    OnPropertyChanged(nameof(RequireReceive));
                }
            }
        } bool requirereceive;

        public bool RequireShipment
        {
            get { return requireshipment; }
            set
            {
                if (requireshipment != value)
                {
                    requireshipment = value;
                    Changed = true;
                    OnPropertyChanged(nameof(RequireShipment));
                }
            }
        } bool requireshipment;

        public bool RequirePick
        {
            get { return requirepick; }
            set
            {
                if (requirepick != value)
                {
                    requirepick = value;
                    Changed = true;
                    OnPropertyChanged(nameof(RequirePick));
                }
            }
        } bool requirepick;

        public bool RequirePutaway
        {
            get { return requireputaway; }
            set
            {
                if (requireputaway != value)
                {
                    requireputaway = value;
                    Changed = true;
                    OnPropertyChanged(nameof(RequirePutaway));
                }
            }
        } bool requireputaway;

        public ICommand TapCommand { protected set; get; }
        public event Action<LocationViewModel> OnTap;

        public List<SubSchemeElement> SubSchemeElements { get; set; } = new List<SubSchemeElement>();

        public bool IsEditMode
        {
            get { return iseditmode; }
            set
            {
                if (iseditmode != value)
                {
                    iseditmode = value;
                    OnPropertyChanged("IsEditMode");
                }
            }
        } bool iseditmode;

        public bool ZonesIsLoaded
        {
            get { return zonesisloaded; }
            set
            {
                if (zonesisloaded != value)
                {
                    zonesisloaded = value;
                    OnPropertyChanged(nameof(ZonesIsLoaded));
                }
            }
        } bool zonesisloaded;

        public bool ZonesIsBeingLoaded
        {
            get { return zonesisbeingloaded; }
            set
            {
                if (zonesisbeingloaded != value)
                {
                    zonesisbeingloaded = value;
                    OnPropertyChanged(nameof(ZonesIsBeingLoaded));
                }
            }
        } bool zonesisbeingloaded;

        public LocationViewModel(INavigation navigation, Location location) : base(navigation)
        {
            State = ModelState.Undefined;
            IsSaveToNAVEnabled = false;
            Location = location;
            FillFields(location);
            Color = Color.FromHex(location.HexColor);
            EditMode = SchemeElementEditMode.None;
            TapCommand = new Command<object>(Tap);

            OKCommand = new Command(OK);
            CancelCommand = new Command(Cancel);
            CancelChangesCommand = new Command(CancelChanges);

            Changed = false;
            IsSaveToNAVEnabled = true;
        }

        public void FillFields(Location location)
        {
            Code = location.Code;
            Name = location.Name;
            Address = location.Address;
            SchemeVisible = location.SchemeVisible;
            ZoneQuantity = location.ZoneQuantity;
            BinQuantity = location.BinQuantity;
            Color = Color.FromHex(location.HexColor);
            BinMandatory = location.BinMandatory;
            RequireReceive = location.RequireReceive;
            RequireShipment = location.RequireShipment;
            RequirePick = location.RequirePick;
            RequirePutaway = location.RequirePutaway;
            PlanWidth = location.PlanWidth;
            PlanHeight = location.PlanHeight;
        }

        public void SaveFields(Location location)
        {
            location.Code = Code;
            location.Name = Name;
            location.Address = Address;
            location.SchemeVisible = SchemeVisible;
            location.ZoneQuantity = ZoneQuantity;
            location.BinQuantity = BinQuantity;
            location.HexColor = ColorToHex(Color);
            location.BinMandatory = BinMandatory;
            location.RequireReceive = RequireReceive;
            location.RequireShipment = RequireShipment;
            location.RequirePick = RequirePick;
            location.RequirePutaway = RequirePutaway;
            location.PlanWidth = PlanWidth;
            location.PlanHeight = PlanHeight;
        }

        public void SavePrevSize(double width, double height)
        {
            PrevWidth = width;
            PrevHeight = height;
        }

        public void Tap(object sender)
        {
            if (OnTap is Action<LocationViewModel>)
            {
                OnTap(this);
            }
        }

        public async void OK()
        {
            State = ModelState.Loading;
            LoadAnimation = true;
            SaveFields(Location);
            if (CreateMode)
            {
                try
                {
                    await NAV.CreateLocation(Location, ACD.Default);
                    await Navigation.PopAsync();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    State = ModelState.Error;
                    ErrorText = e.Message+" "+e.StackTrace;
                }
            }
            else
            {
                try
                {
                    await NAV.ModifyLocation(Location, ACD.Default);
                    await Navigation.PopAsync();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    State = ModelState.Error;
                    ErrorText = e.Message + " " + e.StackTrace;
                }
            }
            LoadAnimation = false;
        }

        public async void Cancel()
        {
            await Navigation.PopAsync();
        }

        public void CancelChanges()
        {
            FillFields(Location);
        }

        public async void SaveToNAVSchemeVisible()
        {
            if (IsSaveToNAVEnabled)
            {
                if (NotNetOrConnection)
                {
                    return;
                }
                try
                {
                    IsBeenSavingToNAV = true;
                    Location location = new Location();
                    SaveFields(location);
                    await NAV.SetLocationVisible(location, ACD.Default);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    State = ModelState.Error;
                    ErrorText = e.Message;
                }
                finally
                {
                    IsBeenSavingToNAV = false;
                }
            }
        }

        public async void CheckLocationCode()
        {
            if (CreateMode)
            {
                CodeWarningText = "";
                if (Code != "")
                {
                    int exist = await NAV.GetLocationCount(Code, false, ACD.Default);
                    if (exist > 0)
                    {
                        CodeWarningText = AppResources.LocationNewPage_CodeAlreadyExist;
                    }
                }
            }
        }

        public Task<string> CheckLocationCode(string code)
        {
            var tcs = new TaskCompletionSource<string>();
            string rv = "";
            Task.Run(async () =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(code))
                    {
                        code = code.Trim().ToUpper();
                        int exist = await NAV.GetLocationCount(code, false, ACD.Default);
                        if (exist > 0)
                        {
                            rv = AppResources.LocationNewPage_CodeAlreadyExist;
                        }

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

        public async void LoadZones()
        {
            if (IsEditMode)
            {
                return;
            }

            ZonesIsLoaded = false;
            ZonesIsBeingLoaded = true;
            try
            {
                List<Zone> zones = await NAV.GetZoneList(Code, "", true, 1, int.MaxValue, ACD.Default);
                if (!IsDisposed)
                {
                    SubSchemeElements.Clear();
                    foreach (Zone zone in zones)
                    {
                        SubSchemeElement sse = new SubSchemeElement
                        {
                            Left = zone.Left,
                            Top = zone.Top,
                            Height = zone.Height,
                            Width = zone.Width,
                            HexColor = zone.HexColor
                        };
                        SubSchemeElements.Add(sse);
                    }
                    ZonesIsLoaded = SubSchemeElements.Count > 0;
                }
            }
            catch (OperationCanceledException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                ErrorText = e.Message;
            }
            finally
            {
                ZonesIsBeingLoaded = false;
            }
        }

        public override void DisposeModel()
        {
            base.DisposeModel();
            SubSchemeElements.Clear();

            if (OnTap is Action<LocationViewModel>)
            {
                Delegate[] clientList = OnTap.GetInvocationList();
                foreach (var d in clientList)
                {
                    OnTap -= (d as Action<LocationViewModel>);
                }
            }
        }
    }
}
