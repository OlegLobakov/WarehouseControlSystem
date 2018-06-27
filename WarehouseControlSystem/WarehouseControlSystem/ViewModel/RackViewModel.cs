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
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows.Input;
using WarehouseControlSystem.Helpers.NAV;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.Resx;
using WarehouseControlSystem.ViewModel.Base;
using Xamarin.Forms;
using WarehouseControlSystem.Model;
using System.Threading.Tasks;

namespace WarehouseControlSystem.ViewModel
{
    public class RackViewModel : NAVBaseViewModel
    {
        private Rack Rack { get; set; }

        public string No
        {
            get { return no; }
            set
            {
                if (no != value)
                {
                    no = value;
                    Renumbering();
                    BinsViewModel.RackNo = no;

                    Changed = true;
                    OnPropertyChanged(nameof(No));
                }
            }
        } string no;

        public string NoWarningText
        {
            get { return nowarningtext; }
            set
            {
                if (nowarningtext != value)
                {
                    nowarningtext = value;
                    OnPropertyChanged(nameof(NoWarningText));
                }
            }
        } string nowarningtext;

        public bool CanChangeLocationAndZone
        {
            get { return canchangelocationAndzone; }
            set
            {
                if (canchangelocationAndzone != value)
                {
                    canchangelocationAndzone = value;
                    OnPropertyChanged(nameof(CanChangeLocationAndZone));
                }
            }
        } bool canchangelocationAndzone;

        public int Sections
        {
            get { return sections; }
            set
            {
                if (sections != value)
                {
                    RecreateBins(depth, depth, levels, levels, sections, value);
                    sections = value;
                    if (CreateMode)
                    {
                        MessagingCenter.Send<RackViewModel>(this, "Update");
                    }
                    Renumbering();
                    Changed = true;
                    OnPropertyChanged(nameof(Sections));
                }
            }
        } int sections;
        public int Levels
        {
            get { return levels; }
            set
            {
                if (levels != value)
                {
                    RecreateBins(depth, depth, levels, value, sections, sections);
                    levels = value;
                    if (CreateMode)
                    {
                        MessagingCenter.Send<RackViewModel>(this, "Update");
                    }
                    Renumbering();
                    Changed = true;
                    OnPropertyChanged(nameof(Levels));
                }
            }
        } int levels;
        public int Depth
        {
            get { return depth; }
            set
            {
                if (depth != value)
                {
                    RecreateBins(depth, value, levels, levels, sections, sections);
                    depth = value;
                    if (CreateMode)
                    {
                        MessagingCenter.Send<RackViewModel>(this, "Update");
                    }
                    Renumbering();
                    Changed = true;
                    OnPropertyChanged(nameof(Depth));
                }
            }
        } int depth;

        public RackOrientationEnum RackOrientation
        {
            get { return rackorientation; }
            set
            {
                if (rackorientation != value)
                {
                    rackorientation = value;
                    Changed = true;
                    OnPropertyChanged(nameof(RackOrientation));
                }
            }
        } RackOrientationEnum rackorientation;

        public string RackSectionSeparator
        {
            get { return racksectionseparator; }
            set
            {
                if (racksectionseparator != value)
                {
                    racksectionseparator = value;
                    Renumbering();
                    OnPropertyChanged("RackSectionSeparator");
                }
            }
        } string racksectionseparator;

        public string SectionLevelSeparator
        {
            get { return sectionlevelseparator; }
            set
            {
                if (sectionlevelseparator != value)
                {
                    sectionlevelseparator = value;
                    Renumbering();
                    OnPropertyChanged("SectionLevelSeparator");
                }
            }
        } string sectionlevelseparator;

        public string LevelDepthSeparator
        {
            get { return leveldepthseparator; }
            set
            {
                if (leveldepthseparator != value)
                {
                    leveldepthseparator = value;
                    Renumbering();
                    OnPropertyChanged("LevelDepthSeparator");
                }
            }
        } string leveldepthseparator;

        public bool ReversSectionNumbering
        {
            get { return reverssectionnumbering; }
            set
            {
                if (reverssectionnumbering != value)
                {
                    reverssectionnumbering = value;
                    Renumbering();
                    OnPropertyChanged("ReversSectionNumbering");
                }
            }
        } bool reverssectionnumbering;

        public bool ReversLevelNumbering
        {
            get { return reverslevelbering; }
            set
            {
                if (reverslevelbering != value)
                {
                    reverslevelbering = value;
                    Renumbering();
                    OnPropertyChanged("ReversLevelNumbering");
                }
            }
        } bool reverslevelbering;
        public bool ReversDepthNumbering
        {
            get { return reversdepthnumbering; }
            set
            {
                if (reversdepthnumbering != value)
                {
                    reversdepthnumbering = value;
                    Renumbering();
                    OnPropertyChanged("ReversDepthNumbering");
                }
            }
        } bool reversdepthnumbering;

        public int NumberingSectionBegin
        {
            get { return numberingsectionbegin; }
            set
            {
                if (numberingsectionbegin != value)
                {
                    numberingsectionbegin = value;
                    Renumbering();
                    OnPropertyChanged("NumberingSectionBegin");
                }
            }
        } int numberingsectionbegin = 1;
        public int NumberingLevelBegin
        {
            get { return numberinglevelbegin; }
            set
            {
                if (numberinglevelbegin != value)
                {
                    numberinglevelbegin = value;
                    Renumbering();
                    OnPropertyChanged("NumberingLevelBegin");
                }
            }
        } int numberinglevelbegin = 1;
        public int NumberingDepthBegin
        {
            get { return numberingdepthbegin; }
            set
            {
                if (numberingdepthbegin != value)
                {
                    numberingdepthbegin = value;
                    Renumbering();
                    OnPropertyChanged("NumberingDepthBegin");
                }
            }
        } int numberingdepthbegin = 1;

        public int StepNumberingSection
        {
            get { return stepnumberingsection; }
            set
            {
                if (stepnumberingsection != value)
                {
                    stepnumberingsection = value;
                    Renumbering();
                    OnPropertyChanged("StepNumberingSection");
                }
            }
        } int stepnumberingsection = 1;

        public bool SchemeVisible
        {
            get { return schemevisible; }
            set
            {
                if (schemevisible != value)
                {
                    schemevisible = value;
                    Changed = true;
                    SaveToRackSchemeVisible();
                    OnPropertyChanged(nameof(SchemeVisible));
                }
            }
        } bool schemevisible;

        public double SchemeWidth
        {
            get { return schemewidth; }
            set
            {
                if (schemewidth != value)
                {
                    schemewidth = value;
                    OnPropertyChanged(nameof(SchemeWidth));
                }
            }
        } double schemewidth;
        public double SchemeHeight
        {
            get { return schemeheight; }
            set
            {
                if (schemeheight != value)
                {
                    schemeheight = value;
                    OnPropertyChanged(nameof(SchemeHeight));
                }
            }
        } double schemeheight;
        public double SchemeFontSize
        {
            get { return schemefontsize; }
            set
            {
                if (schemefontsize != value)
                {
                    schemefontsize = value;
                    OnPropertyChanged(nameof(SchemeFontSize));
                }
            }
        } double schemefontsize;

        public ICommand TapCommand { protected set; get; }
        public event Action<RackViewModel> OnTap;

        public BinsViewModel BinsViewModel
        {
            get { return binsviewmodel; }
            set
            {
                if (binsviewmodel != value)
                {
                    binsviewmodel = value;
                    OnPropertyChanged(nameof(BinsViewModel));
                }
            }
        } BinsViewModel binsviewmodel;

        public ICommand CreateRackCommand { protected set; get; }

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
        public bool BinTemplatesIsLoaded
        {
            get { return bintemplatesisloaded; }
            set
            {
                if (bintemplatesisloaded != value)
                {
                    bintemplatesisloaded = value;
                    OnPropertyChanged(nameof(BinTemplatesIsLoaded));
                }
            }
        } bool bintemplatesisloaded;

        public bool ShowInfoPanel
        {
            get { return showinfopanel; }
            set
            {
                if (showinfopanel != value)
                {
                    showinfopanel = value;
                    OnPropertyChanged(nameof(ShowInfoPanel));
                }
            }
        } bool showinfopanel;

        public ObservableCollection<Location> Locations { get; set; } = new ObservableCollection<Location>();
        public ObservableCollection<Zone> Zones { get; set; } = new ObservableCollection<Zone>();
        public ObservableCollection<BinTemplate> BinTemplates { get; set; } = new ObservableCollection<BinTemplate>();

        public BinTemplate SelectedBinTemplate {
            get { return selecteditem; }
            set
            {
                if (selecteditem != value)
                {
                    selecteditem = value;
                    ChangeBinTemplate(selecteditem);
                    OnPropertyChanged(nameof(SelectedBinTemplate));
                }
            }
        } BinTemplate selecteditem;

        public bool IsBusy
        {
            get { return isbusy; }
            set
            {
                if (isbusy != value)
                {
                    isbusy = value;
                    OnPropertyChanged("IsBusy");
                }
            }
        } bool isbusy;

        public bool ConflictBinChange
        {
            get { return conflictbinchange; }
            set
            {
                if (conflictbinchange != value)
                {
                    conflictbinchange = value;
                    OnPropertyChanged(nameof(ConflictBinChange));
                }
            }
        } bool conflictbinchange;
        public bool ConflictRackChange
        {
            get { return conflictrackchange; }
            set
            {
                if (conflictrackchange != value)
                {
                    conflictrackchange = value;
                    OnPropertyChanged(nameof(ConflictRackChange));
                }
            }
        } bool conflictrackchange;

        public string SearchResult
        {
            get { return searchresult; }
            set
            {
                if (searchresult != value)
                {
                    searchresult = value;
                    OnPropertyChanged(nameof(SearchResult));
                }
            }
        } string searchresult;

        public List<SubSchemeSelect> SubSchemeSelects { get; set; } = new List<SubSchemeSelect>();
        public List<SubSchemeSelect> UDSSelects { get; set; } = new List<SubSchemeSelect>();

        public RackViewModel(INavigation navigation, Rack rack, bool createmode1) : base(navigation)
        {
            Rack = rack;
            IsSaveToNAVEnabled = false;
            RackSectionSeparator = Settings.DefaultRackSectionSeparator;
            SectionLevelSeparator = Settings.DefaultSectionLevelSeparator;
            LevelDepthSeparator = Settings.DefaultLevelDepthSeparator;
            CreateMode = createmode1;
            BinsViewModel = new BinsViewModel(navigation);
            BinsViewModel.LocationCode = rack.LocationCode;
            BinsViewModel.ZoneCode = rack.ZoneCode;
            TapCommand = new Command<object>(Tap);
            FillFields(Rack);
            CreateRackCommand = new Command(async () => await CreateRackInNAV().ConfigureAwait(true));
            State = ModelState.Undefined;
            Changed = false;
            GetSearchSelection();
            IsSaveToNAVEnabled = true;
        }

        public void FillFields(Rack rack)
        {
            No = rack.No;
            PrevCode = rack.PrevNo;
            Sections = rack.Sections;
            Levels = rack.Levels;
            Depth = rack.Depth;
            LocationCode = rack.LocationCode;
            ZoneCode = rack.ZoneCode;
            RackOrientation = rack.RackOrientation;
            SchemeVisible = rack.SchemeVisible;
            Left = rack.Left;
            Top = rack.Top;
            Width = rack.Width;
            Height = rack.Height;
        }

        public void SaveFields(Rack rack)
        {
            rack.No = No;
            rack.PrevNo = PrevCode;
            rack.Sections = Sections;
            rack.Levels = Levels;
            rack.Depth = Depth;
            rack.LocationCode = LocationCode;
            rack.ZoneCode = ZoneCode;
            rack.RackOrientation = RackOrientation;
            rack.SchemeVisible = SchemeVisible;
            rack.Left = Left;
            rack.Top = Top;
            rack.Width = Width;
            rack.Height = Height;
        }

        public void Tap(object sender)
        {
            if (OnTap is Action<RackViewModel>)
            {
                OnTap(this);
            }
        }

        public void RecreateBins(int prevdepth, int newdepth, int prevlevels, int newlevels, int prevsections, int newsections)
        {
            if (CreateMode)
            {
                BinsViewModel.RecreateBins(prevdepth, newdepth, prevlevels, newlevels, prevsections, newsections);
            }
        }

        public async void Renumbering()
        {
            if (CreateMode)
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-us");

                for (int k = 1; k <= Depth; k++)
                {
                    int levelnumber = NumberingLevelBegin + Levels - 1;

                    if (ReversLevelNumbering)
                    {
                        levelnumber = numberinglevelbegin;
                    }

                    for (int i = 1; i <= Levels; i++)
                    {
                        int sectionnumber = numberingsectionbegin;

                        if (ReversSectionNumbering)
                        {
                            sectionnumber = numberingsectionbegin + Sections - 1;
                        }

                        for (int j = 1; j <= Sections; j++)
                        {
                            string number = No + racksectionseparator + sectionnumber.ToString("D2", ci) + sectionlevelseparator + levelnumber.ToString();

                            BinViewModel bvm = BinsViewModel.Find(j, i, k);
                            if (bvm is BinViewModel)
                            {
                                bvm.Code = number;
                            }

                            if (ReversSectionNumbering)
                            {
                                sectionnumber = sectionnumber - StepNumberingSection;
                            }
                            else
                            {
                                sectionnumber = sectionnumber + StepNumberingSection;
                            }
                        }
                        if (ReversLevelNumbering)
                        {
                            levelnumber = levelnumber + 1;
                        }
                        else
                        {
                            levelnumber = levelnumber - 1;
                        }
                    }
                }

                await BinsViewModel.CheckBins(ACD).ConfigureAwait(true);
            }
        }

        public async Task Load()
        {
            IsBusy = true;
            try
            {
                await LoadLocationsList().ConfigureAwait(true);
                await LoadBinTypesList().ConfigureAwait(true);
                await LoadWarehouseClassesList().ConfigureAwait(true);
                await LoadSpecialEquipmentsList().ConfigureAwait(true);

                if (ZoneCode != "")
                {
                    await LoadZonesList().ConfigureAwait(true);
                }

                await ReloadBinTemplates().ConfigureAwait(true);
                MessagingCenter.Send<RackViewModel>(this, "LocationsIsLoaded");
                IsBusy = false;
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
            }
        }

        private async Task LoadLocationsList()
        {
            List<Location> locations = await NAV.GetLocationList("", false, 1, int.MaxValue, ACD.Default).ConfigureAwait(true);
            if (!IsDisposed)
            {
                Locations.Clear();
                foreach (Location location in locations)
                {
                    Locations.Add(location);
                }
                LocationsIsLoaded = CanChangeLocationAndZone && locations.Count > 0;
            }
        }
        private async Task LoadBinTypesList()
        {
            List<BinType> bintypes = await NAV.GetBinTypeList(1, int.MaxValue, ACD.Default).ConfigureAwait(true);
            if (!IsDisposed)
            {
                BinsViewModel.BinTypes.Clear();
                foreach (BinType bt in bintypes)
                {
                    BinsViewModel.BinTypes.Add(bt.Code);
                }
                BinsViewModel.BinTypesIsEnabled = bintypes.Count > 0;
            }
        }
        private async Task LoadWarehouseClassesList()
        {
            List<WarehouseClass> warehouseclasses = await NAV.GetWarehouseClassList(1, int.MaxValue, ACD.Default).ConfigureAwait(true);
            if (!IsDisposed)
            {
                BinsViewModel.WarehouseClasses.Clear();
                foreach (WarehouseClass wc in warehouseclasses)
                {
                    BinsViewModel.WarehouseClasses.Add(wc.Code);
                }
                BinsViewModel.WarehouseClassesIsEnabled = warehouseclasses.Count > 0;
            }
        }
        private async Task LoadSpecialEquipmentsList()
        {
            List<SpecialEquipment> specialequipments = await NAV.GetSpecialEquipmentList(1, int.MaxValue, ACD.Default).ConfigureAwait(true);
            if (!IsDisposed)
            {
                BinsViewModel.SpecialEquipments.Clear();
                foreach (SpecialEquipment se in specialequipments)
                {
                    BinsViewModel.SpecialEquipments.Add(se.Code);
                }
                BinsViewModel.SpecialEquipmentsIsEnabled = specialequipments.Count > 0;
            }
        }
        private async Task LoadZonesList()
        {
            List<Zone> zones = await NAV.GetZoneList(LocationCode, "", false, 1, int.MaxValue, ACD.Default).ConfigureAwait(true);
            if (!IsDisposed)
            {
                Zones.Clear();
                foreach (Zone zone in zones)
                {
                    Zones.Add(zone);
                }
                ZonesIsLoaded = CanChangeLocationAndZone && zones.Count > 0;
            }
        }

        public async Task LoadBins()
        {
            BinsViewModel.LinkToRackViewModel = this;
            await BinsViewModel.LoadBins(ACD).ConfigureAwait(true);
        }

        public async Task SetLocation(Location location)
        {
            if (CanChangeLocationAndZone)
            {
                LocationCode = location.Code;
                IsBusy = true;
                try
                {
                    ZoneCode = "";
                    ZonesIsLoaded = false;
                    List<Zone> zones = await NAV.GetZoneList(location.Code, "", false, 1, int.MaxValue, ACD.Default).ConfigureAwait(true);
                    Zones.Clear();
                    foreach (Zone zone in zones)
                    {
                        Zones.Add(zone);
                    }
                    ZonesIsLoaded = CanChangeLocationAndZone && zones.Count > 0;
                    await ReloadBinTemplates().ConfigureAwait(true);
                    MessagingCenter.Send<RackViewModel>(this, "ZonesIsLoaded");
                    await CheckNo().ConfigureAwait(true);
                }
                catch (OperationCanceledException e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    ErrorText = e.Message;
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        public async Task SetZone(Zone zone)
        {
            if (CanChangeLocationAndZone)
            {
                ZoneCode = zone.Code;
                await CheckNo().ConfigureAwait(true);
                await ReloadBinTemplates().ConfigureAwait(true);
            }
        }

        public async Task ReloadBinTemplates()
        {
            BinTemplatesIsLoaded = false;
            try
            {
                List<BinTemplate> bintemplates = await NAV.GetBinTemplateList(1, int.MaxValue, ACD.Default).ConfigureAwait(true);
                if (!IsDisposed)
                {
                    BinTemplates.Clear();
                    foreach (BinTemplate bt in bintemplates)
                    {
                        bool selected = true;
                        if (!string.IsNullOrEmpty(LocationCode))
                        {
                            if (bt.LocationCode != LocationCode)
                            {
                                selected = false;
                            }
                        }

                        if (!string.IsNullOrEmpty(ZoneCode))
                        {
                            if (bt.ZoneCode != ZoneCode)
                            {
                                selected = false;
                            }
                        }

                        if (selected)
                        {
                            BinTemplates.Add(bt);
                        }
                    }
                    BinTemplatesIsLoaded = bintemplates.Count > 0;
                }
            }
            catch (OperationCanceledException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                ErrorText = e.Message;
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                ErrorText = e.Message;
            }
        }

        public void ChangeBinTemplate(BinTemplate bt)
        {
            BinsViewModel.BinTemplate = bt;
        }

        public async Task SaveToRackSchemeVisible()
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
                    Rack rack = new Rack();
                    SaveFields(rack);
                    await NAV.SetRackVisible(rack, ACD.Default).ConfigureAwait(true);
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

        public async Task LoadUDF()
        {
            BinsViewModel.LinkToRackViewModel = this;
            await BinsViewModel.LoadUDF(ACD).ConfigureAwait(true);
        }

        public void GetSearchText()
        {
            if (!string.IsNullOrEmpty(Global.SearchRequest))
            {
                SearchResult = Global.SearchRequest + " | " + AppResources.RackCardPage_Search_Finded + " "
                   + AppResources.RackCardPage_Search_Bins + ": " + BinsViewModel.SearchBinsQuantity.ToString();
            }
            else
            {
                SearchResult = "";
            }
        }

        public async Task CheckNo()
        {
            NoWarningText = "";
            if ((LocationCode != "") && (ZoneCode != "") && (No != ""))
            {
                int exist = await NAV.GetRackCount(LocationCode, ZoneCode, No, false, ACD.Default).ConfigureAwait(true);
                if (exist > 0)
                {
                    NoWarningText = AppResources.RackNewPage_CodeAlreadyExist;
                }
            }
        }

        public async Task CreateRackInNAV()
        {
            if (!CanSave())
            {
                return;
            }

            State = ModelState.Loading;
            LoadAnimation = true;
            Rack newrack = new Rack();
            SaveFields(newrack);
            try
            {
                LoadingText = AppResources.RackNewPage_LoadingProgressRack + " " + newrack.No;
                int rackexist = await NAV.GetRackCount(LocationCode, ZoneCode, No, false, ACD.Default).ConfigureAwait(true);
                await ModifyRack(newrack, rackexist).ConfigureAwait(true);
                bool errorsexist = false;
                foreach (BinViewModel bvm in BinsViewModel.BinViewModels)
                {
                    try
                    {
                        await SaveBin(bvm).ConfigureAwait(true);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        errorsexist = true;
                        ErrorText += e.Message;
                    }
                }
                SaveFields(Rack);
                LoadAnimation = false;
                if (errorsexist)
                {
                    State = ModelState.Error;
                }
                else
                {
                    MessagingCenter.Send<RackViewModel>(this, "Update");
                    await Navigation.PopAsync();
                }
            }
            catch (Exception e)
            {
                LoadAnimation = false;
                State = ModelState.Error;
                ErrorText = e.Message;
            }
        }

        private async Task ModifyRack(Rack newrack, int rackexist)
        {
            if (rackexist > 0)
            {
                if (ConflictRackChange)
                {
                    newrack.PrevNo = newrack.No;
                    await NAV.ModifyRack(newrack, ACD.Default).ConfigureAwait(true);
                }
                else
                {
                    State = ModelState.Error;
                    ErrorText = AppResources.RackNewPage_Error_RackAlreadyExist;
                }
            }
            else
            {
                await NAV.CreateRack(newrack, ACD.Default).ConfigureAwait(true);
            }
        }

        private async Task SaveBin(BinViewModel bmv)
        {
            try
            {
                bmv.SaveFields();
                LoadingText = AppResources.RackNewPage_LoadingProgressBin + " " + bmv.Bin.Code;

                int binexist = await NAV.GetBinCount(LocationCode, "", "", bmv.Bin.Code, ACD.Default).ConfigureAwait(true);
                if (binexist > 0)
                {
                    if (ConflictBinChange)
                    {
                        LoadingText = AppResources.RackNewPage_LoadingProgressModifyBin + " " + bmv.Bin.Code;
                        bmv.Bin.PrevCode = bmv.Bin.Code;
                        await NAV.ModifyBin(bmv.Bin, ACD.Default).ConfigureAwait(true);
                    }
                }
                else
                {
                    LoadingText = AppResources.RackNewPage_LoadingProgressBin + " " + bmv.Bin.Code;
                    await NAV.CreateBin(BinsViewModel.BinTemplate, bmv.Bin, ACD.Default).ConfigureAwait(true);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private bool CanSave()
        {
            if (RackOrientation == RackOrientationEnum.Undefined)
            {
                State = ModelState.Error;
                ErrorText = AppResources.RackNewPage_Error_RackOrientationNeeded;
                return false;
            }

            if (SelectedBinTemplate == null)
            {
                State = ModelState.Error;
                ErrorText = AppResources.RackNewPage_Error_BinTemplateIsNeeded;
                return false;
            }

            if (string.IsNullOrEmpty(No))
            {
                State = ModelState.Error;
                ErrorText = AppResources.RackNewPage_Error_NoNeeded;
                return false;
            }

            return true;
        }

        #region User Defined Functions
        UserDefinedFunctionViewModel udfvmselected;

        public async Task RunUserDefineFunction(UserDefinedFunctionViewModel udfvm)
        {
            udfvmselected = udfvm;
            if (!string.IsNullOrEmpty(udfvm.Confirm))
            {
                State = ModelState.Request;
                ErrorText = udfvm.Confirm;
            }
            else
            {
                await RunUDF().ConfigureAwait(true);
            }
        }
        public async Task RunUserDefineFunctionOK()
        {
            await RunUDF().ConfigureAwait(true);
        }

        public void RunUserDefineFunctionCancel()
        {
            State = ModelState.Normal;
        }

        public async Task RunUDF()
        {
            try
            {
                List<BinViewModel> list = BinsViewModel.BinViewModels.FindAll(x => x.Selected == true);
                if (list is List<BinViewModel>)
                {
                    State = ModelState.Loading;
                    LoadAnimation = true;
                    foreach (BinViewModel bvm in list)
                    {
                        LoadingText = bvm.Code;
                        string response = await NAV.RunFunction(udfvmselected.ID, bvm.LocationCode, bvm.ZoneCode, bvm.RackNo, bvm.Code, "", "", 0, ACD.Default).ConfigureAwait(true);
                    }
                    State = ModelState.Normal;
                }
                else
                {
                    State = ModelState.Error;
                    ErrorText = AppResources.RackCardPage_Error_BinDidNotSelect;
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
                State = ModelState.Error;
                ErrorText = e.Message;
            }
            finally
            {
                LoadAnimation = false;
            }
        }
        #endregion

        public void GetSearchSelection()
        {
            if (Global.SearchResponses is List<SearchResponse>)
            {
                List<SearchResponse> list = Global.SearchResponses.FindAll(
                    x => x.ZoneCode == ZoneCode &&
                    x.RackNo == No);
                if (list is List<SearchResponse>)
                {
                    foreach (SearchResponse sr in list)
                    {
                        SubSchemeSelect sss = new SubSchemeSelect()
                        {
                            Section = sr.Section,
                            Level = sr.Level,
                            Depth = sr.Depth,
                            HexColor = "#e3125c",
                            Value = sr.QuantityBase
                        };
                        SubSchemeSelects.Add(sss);
                    }
                }
            }
        }

        public void CancelAsync()
        {
            BinsViewModel.CancelAsync();
            ACD.CancelAll();
        }

        public override void DisposeModel()
        {
            Locations.Clear();
            Zones.Clear();
            BinTemplates.Clear();

            if (OnTap is Action<RackViewModel>)
            {
                Delegate[] clientList = OnTap.GetInvocationList();
                foreach (var d in clientList)
                {
                    OnTap -= (d as Action<RackViewModel>);
                }
            }
            BinsViewModel.DisposeModel();
            base.DisposeModel();
        }
    }
}
