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

using WarehouseControlSystem.Resx;
using WarehouseControlSystem.ViewModel.Base;
using Xamarin.Forms;
using System.Windows.Input;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.Helpers.NAV;
using System.Collections.ObjectModel;
using System.Threading;

namespace WarehouseControlSystem.ViewModel
{
    public class ZoneViewModel : NAVBaseViewModel
    {
        private Location Location { get; set; }
        private Zone Zone { get; set; }

        public string LocationCode
        {
            get { return locationcode; }
            set
            {
                if (locationcode != value)
                {
                    locationcode = value;
                    OnPropertyChanged(nameof(LocationCode));
                }
            }
        } string locationcode;      
        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    Changed = true;
                    OnPropertyChanged(nameof(Description));
                }
            }
        } string description;
        public string BinTypeCode
        {
            get { return bintypecode; }
            set
            {
                if (bintypecode != value)
                {
                    bintypecode = value;
                    Changed = true;
                    OnPropertyChanged(nameof(BinTypeCode));
                }
            }
        } string bintypecode;
        public bool SchemeVisible
        {
            get { return schemevisible; }
            set
            {
                if (schemevisible != value)
                {
                    schemevisible = value;
                    Changed = true;
                    SaveToZoneSchemeVisible(value);
                    OnPropertyChanged(nameof(SchemeVisible));
                }
            }
        } bool schemevisible;
        public int RackQuantity
        {
            get { return rackquantity; }
            set
            {
                if (rackquantity != value)
                {
                    rackquantity = value;
                    OnPropertyChanged(nameof(RackQuantity));
                }
            }
        } int rackquantity;
        public int BinQuantity
        {
            get { return binquantity; }
            set
            {
                if (binquantity != value)
                {
                    binquantity = value;
                    OnPropertyChanged(nameof(BinQuantity));
                }
            }
        } int binquantity;

        public ICommand TapCommand { protected set; get; }
        public event Action<ZoneViewModel> OnTap;

        public ObservableCollection<Location> Locations { get; set; } = new ObservableCollection<Location>();
        public bool LocationsIsLoaded
        {
            get { return locationsisloaded; }
            set
            {
                if (locationsisloaded != value)
                {
                    locationsisloaded = value;
                    OnPropertyChanged(nameof(LocationsIsLoaded));
                }
            }
        } bool locationsisloaded;
        public bool LocationsIsBeingLoaded
        {
            get { return locationsisbeingloaded; }
            set
            {
                if (locationsisbeingloaded != value)
                {
                    locationsisbeingloaded = value;
                    OnPropertyChanged(nameof(LocationsIsBeingLoaded));
                }
            }
        } bool locationsisbeingloaded;

        public ObservableCollection<BinType> BinTypes { get; set; } = new ObservableCollection<BinType>();

        public bool BinTypesIsLoaded
        {
            get { return bintypesisloaded; }
            set
            {
                if (bintypesisloaded != value)
                {
                    bintypesisloaded = value;
                    OnPropertyChanged(nameof(BinTypesIsLoaded));
                }
            }
        } bool bintypesisloaded;
        public bool BinTypesIsBeingLoaded
        {
            get { return bintypesisbeingloaded; }
            set
            {
                if (bintypesisbeingloaded != value)
                {
                    bintypesisbeingloaded = value;
                    OnPropertyChanged(nameof(BinTypesIsBeingLoaded));
                }
            }
        } bool bintypesisbeingloaded;

        public bool CanChangeLocationCode
        {
            get { return canchangelocationCode; }
            set
            {
                if (canchangelocationCode != value)
                {
                    canchangelocationCode = value;
                    OnPropertyChanged(nameof(CanChangeLocationCode));
                }
            }
        } bool canchangelocationCode;

        public List<SubSchemeElement> SubSchemeElements { get; set; } = new List<SubSchemeElement>();

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
                if (isindicatorsvisible != value)
                {
                    isindicatorsvisible = value;
                    IsNotIndicatorsVisible = !value;
                    OnPropertyChanged(nameof(IsIndicatorsVisible));
                }
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

        public bool RacksIsLoaded
        {
            get { return racksisloaded; }
            set
            {
                if (racksisloaded != value)
                {
                    racksisloaded = value;
                    OnPropertyChanged(nameof(RacksIsLoaded));
                }
            }
        } bool racksisloaded;
        public bool RacksIsBeingLoaded
        {
            get { return racksisbeingloaded; }
            set
            {
                if (racksisbeingloaded != value)
                {
                    racksisbeingloaded = value;
                    OnPropertyChanged(nameof(RacksIsBeingLoaded));
                }
            }
        } bool racksisbeingloaded;

        public ZoneViewModel(INavigation navigation, Zone zone) : base(navigation)
        {
            Zone = zone;
            IsSaveToNAVEnabled = false;
            FillFields(zone);
            EditMode = SchemeElementEditMode.None;
            TapCommand = new Command<object>(Tap);

            OKCommand = new Command(async () => await OK().ConfigureAwait(false));
            CancelCommand = new Command(async () => await Cancel().ConfigureAwait(false));
            CancelChangesCommand = new Command(CancelChanges);

            State = ModelState.Undefined;
            Changed = false;
            IsSaveToNAVEnabled = true;
            IsIndicatorsVisible = Settings.ShowIndicators;
        }

        public void FillFields(Zone zone)
        {
            LocationCode = zone.LocationCode;
            Code = zone.Code;
            PrevCode = zone.PrevCode;
            Description = zone.Description;
            Color = Color.FromHex(zone.HexColor);
            BinTypeCode = zone.BinTypeCode;
            SchemeVisible = zone.SchemeVisible;
            RackQuantity = zone.RackQuantity;
            BinQuantity = zone.BinQuantity;
            Left = zone.Left;
            Top = zone.Top;
            Width = zone.Width;
            Height = zone.Height;
            PlanWidth = zone.PlanWidth;
            PlanHeight = zone.PlanHeight;
        }

        public void SaveFields(Zone zone)
        {
            zone.LocationCode = LocationCode;
            zone.Code = Code;
            zone.PrevCode = PrevCode;
            zone.Description = Description;
            zone.HexColor = ColorToHex(Color);
            zone.BinTypeCode = BinTypeCode;
            zone.SchemeVisible = SchemeVisible;
            zone.Left = Left;
            zone.Top = Top;
            zone.Width = Width;
            zone.Height = Height;
            zone.PlanWidth = PlanWidth;
            zone.PlanHeight = PlanHeight;
        }
       
        public void Tap(object sender)
        {
            if (OnTap is Action<ZoneViewModel>)
            {
                OnTap(this);
            }
        }

        public async Task OK()
        {
            if (NotNetOrConnection)
            {
                return;
            }

            SaveFields(Zone);

            if (CreateMode)
            {
                try
                {
                    IsBeenSavingToNAV = true;
                    await NAV.CreateZone(Zone, ACD.Default).ConfigureAwait(true);
                    await Navigation.PopAsync();
                }
                catch (Exception e)
                {
                    IsBeenSavingToNAV = false;
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    State = ModelState.Error;
                    ErrorText = e.Message;
                }
            }
            else
            {
                try
                {
                    IsBeenSavingToNAV = true;
                    await NAV.ModifyZone(Zone, ACD.Default).ConfigureAwait(true);
                    await Navigation.PopAsync();
                }
                catch (Exception e)
                {
                    IsBeenSavingToNAV = false;
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    State = ModelState.Error;
                    ErrorText = e.Message;
                }
            }
        }

        public async Task Cancel()
        {
            await Navigation.PopAsync();
        }

        public void CancelChanges()
        {
            FillFields(Zone);
        }

        public async Task CheckZoneCode()
        {
            if (CreateMode)
            {
                CodeWarningText = "";
                if ((LocationCode != "") && (Code != ""))
                {
                    int exist = await NAV.GetZoneCount(LocationCode, Code, false, ACD.Default).ConfigureAwait(true);
                    if (NotDisposed)
                    {
                        if (exist > 0)
                        {
                            CodeWarningText = AppResources.NewZonePage_CodeAlreadyExist;
                        }
                    }
                }
            }
        }

        public async Task Load()
        {
            if (NotNetOrConnection)
            {
                return;
            }

            try
            {
                LocationsIsBeingLoaded = true;
                BinTypesIsBeingLoaded = true;
                List<Location> locations = await NAV.GetLocationList("", false, 1, int.MaxValue, ACD.Default).ConfigureAwait(true);
                List<BinType> bintypes = await NAV.GetBinTypeList(1, int.MaxValue, ACD.Default).ConfigureAwait(true);
                if (NotDisposed)
                {
                    AddLocations(locations);
                    AddBinTypes(bintypes);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                State = ModelState.Error;
                ErrorText = e.Message;
            }
            finally
            {
                BinTypesIsBeingLoaded = false;
                LocationsIsBeingLoaded = false;
            }
        }
        private void AddLocations(List<Location> locations)
        {
            Locations.Clear();
            foreach (Location location in locations)
            {
                Locations.Add(location);
            }

            if (CanChangeLocationCode)
            {
                LocationsIsLoaded = locations.Count > 0;
            }
            else
            {
                LocationsIsLoaded = false;
            }
            MessagingCenter.Send<ZoneViewModel>(this, "LocationsIsLoaded");
        }

        private void AddBinTypes(List<BinType> bintypes)
        {
            BinTypes.Clear();
            foreach (BinType bt in bintypes)
            {
                BinTypes.Add(bt);
            }
            BinTypesIsLoaded = bintypes.Count > 0;
            MessagingCenter.Send<ZoneViewModel>(this, "BinTypesIsLoaded");
        }

        public async Task LoadRacks()
        {
            if (IsEditMode)
            {
                return;
            }

            RacksIsLoaded = false;
            RacksIsBeingLoaded = true;
            try
            {
                List<Rack> racks = await NAV.GetRackList(LocationCode, Code, true, 1, int.MaxValue, ACD.Default).ConfigureAwait(true);
                if (NotDisposed)
                {
                    FillRacks(racks);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            finally
            {
                RacksIsBeingLoaded = false;
            }
        }

        private void FillRacks(List<Rack> racks)
        {
            SubSchemeElements.Clear();
            foreach (Rack rack in racks)
            {
                SubSchemeElement sse = CreateSSE(rack);
                CheckGlobalSearch(rack, sse);
                SubSchemeElements.Add(sse);
            }
            RacksIsLoaded = SubSchemeElements.Count > 0;
        }

        public async Task LoadIndicators()
        {
            if (IsEditMode)
            {
                return;
            }

            try
            {
                List<Indicator> list = await NAV.GetIndicatorsByZone(LocationCode, Code, ACD.Default).ConfigureAwait(true);
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
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                ErrorText = e.Message;
            }
        }

        private SubSchemeElement CreateSSE(Rack rack)
        {
            return new SubSchemeElement()
            {
                Left = rack.Left,
                Top = rack.Top,
                Height = rack.Height,
                Width = rack.Width,
                HexColor = ColorToHex((Color)Application.Current.Resources["PageHeaderBarBackgoundColor"]),
                RackOrientation = rack.RackOrientation
            };
        }

        private void CheckGlobalSearch(Rack rack, SubSchemeElement sse)
        {
            if (Global.SearchResponses is List<SearchResponse>)
            {
                List<SearchResponse> list = Global.SearchResponses.FindAll(x => x.ZoneCode == Code && x.RackID == rack.ID);
                if (list is List<SearchResponse>)
                {
                    sse.Selection = new List<SubSchemeSelect>();
                    foreach (SearchResponse sr in list)
                    {
                        SubSchemeSelect sss = new SubSchemeSelect
                        {
                            Section = sr.Section,
                            Level = sr.Level,
                            Depth = sr.Depth
                        };
                        sse.Selection.Add(sss);
                    }
                }
            }
        }

        public async Task SaveToZoneSchemeVisible(bool value)
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
                    Zone zone = new Zone();
                    SaveFields(zone);
                    zone.SchemeVisible = value;
                    await NAV.SetZoneVisible(zone, ACD.Default).ConfigureAwait(true);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    ErrorText = e.Message;
                    State = ModelState.Error;
                }
                finally
                {
                    IsBeenSavingToNAV = false;
                }
            }
        }

        public override void DisposeModel()
        {
            if (OnTap is Action<ZoneViewModel>)
            {
                Delegate[] clientList = OnTap.GetInvocationList();
                foreach (var d in clientList)
                {
                    OnTap -= (d as Action<ZoneViewModel>);
                }
            }

            base.DisposeModel();
        }
    }
}
