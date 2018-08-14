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

namespace WarehouseControlSystem.ViewModel
{
    public class LocationViewModel : NAVBaseViewModel
    {
        private Location Location { get; set; }

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
                    SaveToLocationSchemeVisible(value);
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

        public ObservableCollection<IndicatorViewModel> Indicators
        {
            get { return indicators; }
            set
            {
                if (indicators != value)
                {
                    indicators = value;
                    OnPropertyChanged(nameof(Indicators));
                }
            }
        } ObservableCollection<IndicatorViewModel> indicators;
        public bool IsIndicatorsVisible
        {
            get { return isindicatorsvisible; }
            set
            {
                isindicatorsvisible = value;
                IsNotIndicatorsVisible = !value;
                OnPropertyChanged(nameof(IsIndicatorsVisible));
            }
        } bool isindicatorsvisible;
        public bool IsNotIndicatorsVisible
        {
            get { return isnotindicatorsvisible; }
            set
            {
                if (isnotindicatorsvisible != value)
                {
                    isnotindicatorsvisible = value;
                    OnPropertyChanged(nameof(IsNotIndicatorsVisible));
                }
            }
        } bool isnotindicatorsvisible;


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
            IsIndicatorsVisible = Settings.ShowIndicators;
        }

        public void FillFields(Location location)
        {
            Code = location.Code;
            PrevCode = location.PrevCode;
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
            Left = location.Left;
            Top = location.Top;
            Width = location.Width;
            Height = location.Height;
            PlanWidth = location.PlanWidth;
            PlanHeight = location.PlanHeight;
        }

        public void SaveFields(Location location)
        {
            location.Code = Code;
            location.PrevCode = PrevCode;
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
            location.Left = Left;
            location.Top = Top;
            location.Width = Width;
            location.Height = Height;
            location.PlanWidth = PlanWidth;
            location.PlanHeight = PlanHeight;
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
            try
            {
                if (CreateMode)
                {
                    await CreateLocation(Location).ConfigureAwait(true);
                }
                else
                {
                    await ModifyLocation(Location).ConfigureAwait(true);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                State = ModelState.Error;
                ErrorText = e.Message + " " + e.StackTrace;
            }
        }

        private async Task CreateLocation(Location location)
        {
            try
            {
                await NAV.CreateLocation(location, ACD.Default).ConfigureAwait(true);
                await Navigation.PopAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async Task ModifyLocation(Location location)
        {
            try
            {
                await NAV.ModifyLocation(location, ACD.Default).ConfigureAwait(true);
                await Navigation.PopAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async void Cancel()
        {
            await Navigation.PopAsync();
        }

        public void CancelChanges()
        {
            FillFields(Location);
        }

        public async Task SaveToLocationSchemeVisible(bool value)
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
                    location.SchemeVisible = value;
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

        public async Task CheckLocationCode()
        {
            if (CreateMode)
            {
                CodeWarningText = "";
                if (Code != "")
                {
                    int exist = await NAV.GetLocationCount(Code, false, ACD.Default);
                    if (NotDisposed)
                    {
                        if (exist > 0)
                        {
                            CodeWarningText = AppResources.LocationNewPage_CodeAlreadyExist;
                        }
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
                        string code1 = code.Trim().ToUpper();
                        int exist = await NAV.GetLocationCount(code1, false, ACD.Default);
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

        public async Task LoadZones()
        {
            if (IsEditMode)
            {
                return;
            }

            ZonesIsLoaded = false;
            ZonesIsBeingLoaded = true;
            try
            {
                List<Zone> zones = await NAV.GetZoneList(Code, "", true, 1, int.MaxValue, ACD.Default).ConfigureAwait(true);
                if (NotDisposed)
                {
                    AddSubSchemeElements(zones);
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

        private void AddSubSchemeElements(List<Zone> zones)
        {
            SubSchemeElements.Clear();
            foreach (Zone zone in zones)
            {
                SubSchemeElement sse = new SubSchemeElement
                {
                    Text = zone.Description,
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

        public async Task LoadIndicators()
        {
            if (IsEditMode)
            {
                return;
            }

            try
            {
                List<Indicator> list = await NAV.GetIndicatorsByLocation(Code, ACD.Default).ConfigureAwait(true);
                if (NotDisposed)
                {
                    ObservableCollection<IndicatorViewModel> nlist = new ObservableCollection<IndicatorViewModel>();
                    foreach (Indicator indicator in list)
                    {
                        IndicatorViewModel ivm = new IndicatorViewModel(Navigation, indicator);
                        nlist.Add(ivm);
                    }
                    Indicators = nlist;
                }
            }
            catch (OperationCanceledException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                ErrorText = e.Message;
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
