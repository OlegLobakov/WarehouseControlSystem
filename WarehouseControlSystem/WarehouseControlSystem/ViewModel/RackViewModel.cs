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
using WarehouseControlSystem.Helpers.Containers.StateContainer;
using WarehouseControlSystem.Helpers.NAV;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.Resx;
using WarehouseControlSystem.ViewModel.Base;
using Xamarin.Forms;
using WarehouseControlSystem.Model;
using System.Threading;

namespace WarehouseControlSystem.ViewModel
{
    public class RackViewModel : BaseViewModel
    {
        public Rack Rack { get; set; }

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

        public string LocationCode
        {
            get { return locationcode; }
            set
            {
                if (locationcode != value)
                {
                    locationcode = value;
                    BinsViewModel.LocationCode = locationcode;
                    Changed = true;
                    OnPropertyChanged(nameof(LocationCode));
                }
            }
        } string locationcode;

        public string ZoneCode
        {
            get { return zonecode; }
            set
            {
                if (zonecode != value)
                {
                    zonecode = value;
                    BinsViewModel.ZoneCode = zonecode;
                    Changed = true;
                    OnPropertyChanged(nameof(ZoneCode));
                }
            }
        } string zonecode;

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
        }

        bool canchangelocationAndzone;
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
                    OnPropertyChanged(nameof(SchemeVisible));
                }
            }
        } bool schemevisible;
        public double PrevWidth { get; set; }
        public double PrevHeight { get; set; }

        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

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
        public bool CreateMode
        {
            get { return createmode; }
            set
            {
                if (createmode != value)
                {
                    createmode = value;
                    OnPropertyChanged(nameof(CreateMode));
                }
            }
        } bool createmode;

        public ICommand TapCommand { protected set; get; }
        public event Action<RackViewModel> OnTap;

        public BinsViewModel BinsViewModel;
      
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
                    ChangeBinTemplate();                   
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
        }  bool conflictrackchange;
        public bool Changed
        {
            get { return changed; }
            set
            {
                if (changed != value)
                {
                    changed = value;
                    OnPropertyChanged(nameof(Changed));
                }
            }
        } bool changed;

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
        public List<UserDefinedFunction> UserDefinedFunctions = new List<UserDefinedFunction>();


        public RackViewModel(INavigation navigation, Rack rack, bool createmode1) : base(navigation)
        {
            Rack = rack;

            RackSectionSeparator = Settings.DefaultRackSectionSeparator;
            SectionLevelSeparator = Settings.DefaultSectionLevelSeparator;
            LevelDepthSeparator = Settings.DefaultLevelDepthSeparator;

            CreateMode = createmode1;
            BinsViewModel = new BinsViewModel(navigation);
            TapCommand = new Command<object>(Tap);
            FillFields(Rack);
            CreateRackCommand = new Command(CreateRackInNAV);
            //SearchCommand = new Command(Search);
            State = State.Normal;
            Changed = false;
            GetSearchSelection();
        }

        public void FillFields(Rack rack)
        {
            No = rack.No;
            Sections = rack.Sections;
            Levels = rack.Levels;
            Depth = rack.Depth;
            LocationCode = rack.LocationCode;
            ZoneCode = rack.ZoneCode;
            RackOrientation = rack.RackOrientation;
            SchemeVisible = rack.SchemeVisible;
        }

        public void SaveFields(Rack rack)
        {
            rack.No = No;
            rack.Sections = Sections;
            rack.Levels = Levels;
            rack.Depth = Depth;
            rack.LocationCode = LocationCode;
            rack.ZoneCode = ZoneCode;
            rack.RackOrientation = RackOrientation;
            rack.SchemeVisible = SchemeVisible;
        }

        public void SavePrevSize(double width, double height)
        {
            PrevWidth = width;
            PrevHeight = height;
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

        public void Renumbering()
        {
            if (CreateMode)
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-us");

                for (int k = 1; k <= Depth; k++)
                {
                    int levelnumber = NumberingLevelBegin + Levels - 1; ;

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

                BinsViewModel.CheckBins(ACD);
            }
        }

        public async void Load()
        {
            IsBusy = true;
            try
            {
                List<Location> locations = await NAV.GetLocationList("", false, 1, int.MaxValue, ACD.Default);
                if (!IsDisposed)
                {
                    Locations.Clear();
                    foreach (Location location in locations)
                    {
                        Locations.Add(location);
                    }
                    LocationsIsLoaded = CanChangeLocationAndZone && locations.Count > 0;
                }
                List<BinType> bintypes = await NAV.GetBinTypeList(1, int.MaxValue, ACD.Default);
                if (!IsDisposed)
                {
                    BinsViewModel.BinTypes.Clear();
                    foreach (BinType bt in bintypes)
                    {
                        BinsViewModel.BinTypes.Add(bt.Code);
                    }
                    BinsViewModel.BinTypesIsEnabled = bintypes.Count > 0;
                }
                List<WarehouseClass> warehouseclasses = await NAV.GetWarehouseClassList(1, int.MaxValue, ACD.Default);
                if (!IsDisposed)
                {
                    BinsViewModel.WarehouseClasses.Clear();
                    foreach (WarehouseClass wc in warehouseclasses)
                    {
                        BinsViewModel.WarehouseClasses.Add(wc.Code);
                    }
                    BinsViewModel.WarehouseClassesIsEnabled = warehouseclasses.Count > 0;
                }
                List<SpecialEquipment> specialequipments = await NAV.GetSpecialEquipmentList(1, int.MaxValue, ACD.Default);
                if (!IsDisposed)
                {
                    BinsViewModel.SpecialEquipments.Clear();
                    foreach (SpecialEquipment se in specialequipments)
                    {
                        BinsViewModel.SpecialEquipments.Add(se.Code);
                    }
                    BinsViewModel.SpecialEquipmentsIsEnabled = specialequipments.Count > 0;
                }

                if (ZoneCode != "")
                {
                    List<Zone> zones = await NAV.GetZoneList(LocationCode, "", false, 1, int.MaxValue, ACD.Default);
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

                ReloadBinTemplates();
                MessagingCenter.Send<RackViewModel>(this, "LocationsIsLoaded");
                IsBusy = false;
            }
            catch (OperationCanceledException ex)
            {
                ErrorText = ex.Message;
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
        }

        public void LoadBins()
        {
            BinsViewModel.LoadBins(ACD);
        }

        public async void SetLocation(Location location)
        {
            if (CanChangeLocationAndZone)
            {
                LocationCode = location.Code;
                IsBusy = true;
                try
                {
                    ZoneCode = "";
                    ZonesIsLoaded = false;
                    List<Zone> zones = await NAV.GetZoneList(location.Code, "", false, 1, int.MaxValue, ACD.Default);
                    Zones.Clear();
                    foreach (Zone zone in zones)
                    {
                        Zones.Add(zone);
                    }
                    ZonesIsLoaded = CanChangeLocationAndZone && zones.Count > 0;
                    ReloadBinTemplates();
                    MessagingCenter.Send<RackViewModel>(this, "ZonesIsLoaded");
                    CheckNo();
                }
                catch (OperationCanceledException ex)
                {
                    ErrorText = ex.Message;
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        public void SetZone(Zone zone)
        {
            if (CanChangeLocationAndZone)
            {
                ZoneCode = zone.Code;
                CheckNo();
                ReloadBinTemplates();
            }
        }

        public async void ReloadBinTemplates()
        {
            BinTemplatesIsLoaded = false;
            try
            {
                List<BinTemplate> bintemplates = await NAV.GetBinTemplateList(1, int.MaxValue, ACD.Default);
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
            catch (OperationCanceledException ex)
            {
                ErrorText = ex.Message;
            }
            catch(Exception ex)
            {
                ErrorText = ex.Message;
            }
        }

        public void ChangeBinTemplate()
        {
            BinsViewModel.BinTemplate = SelectedBinTemplate;
        }

        //public void LoadContent()
        //{
        //    BinsViewModel.LoadContent(ACD);
        //}

        public void LoadUDF()
        {
            BinsViewModel.LoadUDF(ACD);
        }

        public void GetSearchText()
        {
            if (!string.IsNullOrEmpty(Global.SearchRequest))
            {
                SearchResult = Global.SearchRequest + " | "+AppResources.RackCardPage_Search_Finded+" "
                   +AppResources.RackCardPage_Search_Bins +": " + BinsViewModel.SearchBinsQuantity.ToString();
            }
            else
            {
                SearchResult = "";
            }
        }

        public async void CheckNo()
        {
            NoWarningText = "";
            if ((LocationCode != "") && (ZoneCode != "") && (No != ""))
            {
                int exist = await NAV.GetRackCount(LocationCode, ZoneCode, No, false, ACD.Default);
                if (exist > 0)
                {
                    NoWarningText = AppResources.RackNewPage_CodeAlreadyExist;
                }
            }
        }

       
        public async void CreateRackInNAV()
        {
            if (RackOrientation == RackOrientationEnum.Undefined)
            {
                State = State.Error;
                ErrorText = AppResources.RackNewPage_Error_RackOrientationNeeded;
                return;
            }

            if (SelectedBinTemplate == null)
            {
                State = State.Error;
                ErrorText = AppResources.RackNewPage_Error_BinTemplateIsNeeded;
                return;
            }

            if (string.IsNullOrEmpty(No))
            {
                State = State.Error;
                ErrorText = AppResources.RackNewPage_Error_NoNeeded;
                return;
            }

            State = State.Loading;
            LoadAnimation = true;
            Rack newrack = new Rack();
            SaveFields(newrack);
            try
            {
                LoadingText = AppResources.RackNewPage_LoadingProgressRack + " " + newrack.No;
                int rackexist = await NAV.GetRackCount(LocationCode, ZoneCode, No,false, ACD.Default);
                if (rackexist > 0)
                {
                    if (ConflictRackChange)
                    {
                        newrack.PrevNo = newrack.No;
                        await NAV.ModifyRack(newrack, ACD.Default);
                    }
                    else
                    {
                        State = State.Error;
                        ErrorText = AppResources.RackNewPage_Error_RackAlreadyExist;
                    }
                }
                else
                {
                    await NAV.CreateRack(newrack, ACD.Default);
                }

                bool errorsexist = false;
                foreach (BinViewModel bmv in BinsViewModel.BinViewModels)
                {
                    try
                    {
                        bmv.SaveFields();
                        LoadingText = AppResources.RackNewPage_LoadingProgressBin + " " + bmv.Bin.Code;

                        int binexist = await NAV.GetBinCount(LocationCode, "", "", bmv.Bin.Code, ACD.Default);
                        if (binexist > 0)
                        {
                            if (ConflictBinChange)
                            {                         
                                LoadingText = AppResources.RackNewPage_LoadingProgressModifyBin + " " + bmv.Bin.Code;
                                bmv.Bin.PrevCode = bmv.Bin.Code;
                                await NAV.ModifyBin(bmv.Bin, ACD.Default);
                            }
                            else
                            {
                                //skip
                            }
                        }
                        else
                        {
                            LoadingText = AppResources.RackNewPage_LoadingProgressBin + " " + bmv.Bin.Code;
                            await NAV.CreateBin(BinsViewModel.BinTemplate, bmv.Bin, ACD.Default);
                        }
                    }
                    catch (Exception ex)
                    {
                        errorsexist = true;
                        ErrorText += ex.Message;
                    }
                }
                SaveFields(Rack);
                LoadAnimation = false;
                if (errorsexist)
                {
                    State = State.Error;
                }
                else
                {
                    MessagingCenter.Send<RackViewModel>(this, "Update");
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                LoadAnimation = false;
                State = State.Error;
                ErrorText = ex.Message;
            }
        }

        //public void Search()
        //{
        //    BinsViewModel.SearchByContentItemNo(SearchRequest);
        //}

        #region User Defined Functions
        UserDefinedFunctionViewModel udfvmselected;
        public void RunUserDefineFunction(UserDefinedFunctionViewModel udfvm)
        {
            udfvmselected = udfvm;
            if (!string.IsNullOrEmpty(udfvm.Confirm))
            {
                State = State.Request;
                RequestMessageText = udfvm.Confirm;
            }
            else
            {
                RunUDF();
            }
        }
        public void RunUserDefineFunctionOK()
        {
            RunUDF();
        }
        public void RunUserDefineFunctionCancel()
        {
            State = State.Normal;
        }

        public async void RunUDF()
        {
            try
            {
                BinViewModel bvm = BinsViewModel.BinViewModels.Find(x => x.Selected == true);
                if (bvm is BinViewModel)
                {
                    State = State.Loading;
                    LoadAnimation = true;
                    string response = await NAV.RunFunction(udfvmselected.ID, bvm.LocationCode, bvm.ZoneCode, bvm.RackNo, bvm.Code, "", "", 0, ACD.Default);
                    State = State.Warning;
                    InfoText = response;
                }
                else
                {
                    State = State.Warning;
                    InfoText = AppResources.RackCardPage_Error_BinDidNotSelect;
                }
            }
            catch (OperationCanceledException ex)
            {
                ErrorText = ex.Message;
            }
            catch (Exception ex)
            {
                State = State.Error;
                ErrorText = ex.Message;
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
                        SubSchemeSelect sss = new SubSchemeSelect();
                        sss.Section = sr.Section;
                        sss.Level = sr.Level;
                        sss.Depth = sr.Depth;
                        sss.HexColor = "#e3125c";
                        sss.Value = sr.QuantityBase;
                        SubSchemeSelects.Add(sss);
                    }
                }
            }
        }

        public void CancelAsync()
        {
            ACD.CancelAll();
        }

        public override void Dispose()
        {
            Locations.Clear();
            Zones.Clear();
            BinTemplates.Clear();
            TapCommand = null;
            Rack = null;
            if (OnTap is Action<RackViewModel>)
            {
                Delegate[] clientList = OnTap.GetInvocationList();
                foreach (var d in clientList)
                {
                    OnTap -= (d as Action<RackViewModel>);
                }
            }
            BinsViewModel.Dispose();
            base.Dispose();
        }
    }
}
