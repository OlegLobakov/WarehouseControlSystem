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
    public class MasterRackNewViewModel : NAVBaseViewModel
    {
        public RackViewModel NewModel { get; set; }

        public string No
        {
            get { return no; }
            set
            {
                if (no != value)
                {
                    no = value;
                    Changed = true;
                    OnPropertyChanged(nameof(No));
                }
            }
        } string no;

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
                    sections = value;
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
                    levels = value;
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
                    depth = value;
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

        public int MasterStep
        {
            get { return masterstep; }
            set
            {
                if (masterstep != value)
                {
                    masterstep = value; ;
                    IsMasterStep1 = false;
                    IsMasterStep2 = false;
                    IsMasterStep3 = false;
                    IsMasterStep1 = masterstep == 1;
                    IsMasterStep2 = masterstep == 2;
                    IsMasterStep3 = masterstep == 3;
                    OnPropertyChanged(nameof(MasterStep));
                }
            }
        } int masterstep;
        public bool IsMasterStep1
        {
            get { return ismasterstep1; }
            set
            {
                if (ismasterstep1 != value)
                {
                    ismasterstep1 = value;
                    OnPropertyChanged("IsMasterStep1");
                }
            }
        } bool ismasterstep1;
        public bool IsMasterStep2
        {
            get { return ismasterstep2; }
            set
            {
                if (ismasterstep2 != value)
                {
                    ismasterstep2 = value;
                    OnPropertyChanged("IsMasterStep2");
                }
            }
        } bool ismasterstep2;
        public bool IsMasterStep3
        {
            get { return ismasterstep3; }
            set
            {
                if (ismasterstep3 != value)
                {
                    ismasterstep3 = value;
                    OnPropertyChanged("IsMasterStep3");
                }
            }
        } bool ismasterstep3;

        public ICommand Step1Command { protected set; get; }
        public ICommand Step2Command { protected set; get; }
        public ICommand Step3Command { protected set; get; }

        public string NumberingPrefix
        {
            get { return numberingprefix; }
            set
            {
                if (numberingprefix != value)
                {
                    numberingprefix = value;
                    Renumbering();
                    OnPropertyChanged(nameof(NumberingPrefix));
                }
            }
        } string numberingprefix;

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

        public int NumberingSectionDigitsQuantity
        {
            get { return numberingsectiondigitsquantity; }
            set
            {
                int newvalue = 1;
                if ((value >= 1) && (value < 10))
                {
                    newvalue = value;
                }

                if (numberingsectiondigitsquantity != newvalue)
                {
                    numberingsectiondigitsquantity = newvalue;
                    Renumbering();
                    OnPropertyChanged(nameof(NumberingSectionDigitsQuantity));
                }
            }
        } int numberingsectiondigitsquantity = 2;
        public int NumberingLevelDigitsQuantity
        {
            get { return numberingleveldigitsquantity; }
            set
            {
                int newvalue = 1;
                if ((value >= 1) && (value < 10))
                {
                    newvalue = value;
                }
                if (numberingleveldigitsquantity != newvalue)
                {
                    numberingleveldigitsquantity = newvalue;
                    Renumbering();
                    OnPropertyChanged(nameof(NumberingLevelDigitsQuantity));
                }
            }
        } int numberingleveldigitsquantity = 1;
        public int NumberingDepthDigitsQuantity
        {
            get { return numberingdepthdigitsquantity; }
            set
            {
                int newvalue = 1;
                if ((value >= 1) && (value < 10))
                {
                    newvalue = value;
                }
                if (numberingdepthdigitsquantity != newvalue)
                {
                    numberingdepthdigitsquantity = newvalue;
                    Renumbering();
                    OnPropertyChanged(nameof(NumberingDepthDigitsQuantity));
                }
            }
        } int numberingdepthdigitsquantity = 1;

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
        public int StepNumberingLevel
        {
            get { return stepnumberinglevel; }
            set
            {
                if (stepnumberinglevel != value)
                {
                    stepnumberinglevel = value;
                    Renumbering();
                    OnPropertyChanged("StepNumberingLevel");
                }
            }
        } int stepnumberinglevel = 1;

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
        //public bool ConflictRackChange
        //{
        //    get { return conflictrackchange; }
        //    set
        //    {
        //        if (conflictrackchange != value)
        //        {
        //            conflictrackchange = value;
        //            OnPropertyChanged(nameof(ConflictRackChange));
        //        }
        //    }
        //} bool conflictrackchange;

        public MasterRackNewViewModel(RackViewModel rvm, bool createmode1) : base(rvm.Navigation)
        {
            NewModel = rvm;
            IsSaveToNAVEnabled = false;
            RackSectionSeparator = Settings.DefaultRackSectionSeparator;
            SectionLevelSeparator = Settings.DefaultSectionLevelSeparator;
            LevelDepthSeparator = Settings.DefaultLevelDepthSeparator;

            Sections = rvm.Sections;
            Levels = rvm.Levels;
            Depth = rvm.Depth;
            RackOrientation = rvm.RackOrientation;
            LocationCode = rvm.LocationCode;
            ZoneCode = rvm.ZoneCode;

            CreateMode = createmode1;

            Step1Command = new Command(Step1);
            Step2Command = new Command(Step2);
            Step3Command = new Command(async () => await Step3().ConfigureAwait(true));

            State = ModelState.Undefined;
            Changed = false;
            IsSaveToNAVEnabled = true;
        }

        public void RecreateBins(int prevdepth, int newdepth, int prevlevels, int newlevels, int prevsections, int newsections)
        {
            if (CreateMode)
            {
                NewModel.BinsViewModel.RecreateBins(prevdepth, newdepth, prevlevels, newlevels, prevsections, newsections);
            }
        }

        public async void Renumbering()
        {
            if (CreateMode)
            {
                NewModel.BinsViewModel.UnSelect();
                foreach (BinViewModel bvm in NewModel.BinsViewModel.BinViewModels)
                {
                    SetNumber(bvm);
                }

                await NewModel.BinsViewModel.CheckBins(ACD).ConfigureAwait(true);
            }
        }

        private void SetNumber(BinViewModel bvm)
        {
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-us");

            int sectionname = numberingsectionbegin + bvm.Section * StepNumberingSection;
            if (ReversSectionNumbering)
            {
                sectionname = numberingsectionbegin + (Sections - bvm.Section - 1) * StepNumberingSection; ;
            }

            int levelname = NumberingLevelBegin + (Levels - bvm.Level - 1) * StepNumberingLevel;
            if (ReversLevelNumbering)
            {
                levelname = NumberingLevelBegin + bvm.Level * StepNumberingLevel; ;
            }

            string sectionlabel = sectionname.ToString("D" + NumberingSectionDigitsQuantity.ToString(), ci);
            string lavellabel = levelname.ToString("D" + NumberingLevelDigitsQuantity.ToString(), ci);

            bvm.Code = NumberingPrefix + racksectionseparator + sectionlabel + sectionlevelseparator + lavellabel;
        }

        public async Task Load()
        {
            IsBusy = true;
            try
            {
                await LoadBinTemplates().ConfigureAwait(true);
                await LoadBinTypesList().ConfigureAwait(true);
                await LoadWarehouseClassesList().ConfigureAwait(true);
                await LoadSpecialEquipmentsList().ConfigureAwait(true);
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

        private async Task LoadBinTemplates()
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
                    if (BinTemplatesIsLoaded)
                    {
                        MessagingCenter.Send<MasterRackNewViewModel>(this, "BinTemplatesIsLoaded");
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
            }
        }
        private async Task LoadBinTypesList()
        {
            List<BinType> bintypes = await NAV.GetBinTypeList(1, int.MaxValue, ACD.Default).ConfigureAwait(true);
            if (!IsDisposed)
            {
                //ObservableCollection<string> bts = new ObservableCollection<string>();
                NewModel.BinsViewModel.BinTypes.Clear();
                foreach (BinType bt in bintypes)
                {
                    NewModel.BinsViewModel.BinTypes.Add(bt.Code);
                }
                NewModel.BinsViewModel.BinTypesIsEnabled = bintypes.Count > 0;
            }
        }
        private async Task LoadWarehouseClassesList()
        {
            List<WarehouseClass> warehouseclasses = await NAV.GetWarehouseClassList(1, int.MaxValue, ACD.Default).ConfigureAwait(true);
            if (!IsDisposed)
            {
                NewModel.BinsViewModel.WarehouseClasses.Clear();
                foreach (WarehouseClass wc in warehouseclasses)
                {
                    NewModel.BinsViewModel.WarehouseClasses.Add(wc.Code);
                }
                NewModel.BinsViewModel.WarehouseClassesIsEnabled = warehouseclasses.Count > 0;
            }
        }
        private async Task LoadSpecialEquipmentsList()
        {
            List<SpecialEquipment> specialequipments = await NAV.GetSpecialEquipmentList(1, int.MaxValue, ACD.Default).ConfigureAwait(true);
            if (!IsDisposed)
            {
                NewModel.BinsViewModel.SpecialEquipments.Clear();
                foreach (SpecialEquipment se in specialequipments)
                {
                    NewModel.BinsViewModel.SpecialEquipments.Add(se.Code);
                }
                NewModel.BinsViewModel.SpecialEquipmentsIsEnabled = specialequipments.Count > 0;
            }
        }

        public void ChangeBinTemplate(BinTemplate bt)
        {
            NewModel.BinsViewModel.BinTemplate = bt;
        }

        private void Step1()
        {
            MasterStep = 1;
        }

        private void Step2()
        {
            
            bool check = true;
            if (!(SelectedBinTemplate is BinTemplate))
            {
                InfoText = AppResources.MasterNewRack_ControlBinTemplate;
                check = false;
            }

            if (RackOrientation == RackOrientationEnum.Undefined)
            {
                InfoText = AppResources.MasterNewRack_ControlRackOrientation;
                check = false;
            }

            if (string.IsNullOrEmpty(No))
            {
                InfoText = AppResources.MasterNewRack_ControlNo;
                check = false;
            }

            if (check)
            {
                NewModel.No = No;
                NewModel.Sections = Sections;
                NewModel.Levels = Levels;
                NewModel.Depth = Depth;
                NewModel.RackOrientation = RackOrientation;
                NewModel.BinsViewModel.BinTemplate = SelectedBinTemplate;
                NewModel.BinsViewModel.CreateBins(Depth, Levels, Sections);
                NumberingPrefix = No;
                MessagingCenter.Send<MasterRackNewViewModel>(this, "UpdateRackView");
                MasterStep = 2;
            }
        }

        private async Task Step3()
        {
            MasterStep = 3;
            await CreateRackInNAV().ConfigureAwait(true);
        }


        public async Task CheckNo()
        {
            InfoText = "";
            if ((LocationCode != "") && (ZoneCode != "") && (No != ""))
            {
                int exist = await NAV.GetRackCount(LocationCode, ZoneCode, No, false, ACD.Default).ConfigureAwait(true);
                if (exist > 0)
                {
                    InfoText = AppResources.RackNewPage_CodeAlreadyExist;
                }
            }
        }

        public async Task CreateRackInNAV()
        {
            State = ModelState.Loading;
            LoadAnimation = true;
            Rack newrack = new Rack();
            NewModel.SaveFields(newrack);
            try
            {
                LoadingText = AppResources.RackNewPage_LoadingProgressRack + " " + newrack.No;
                //int rackexist = await NAV.GetRackCount(LocationCode, ZoneCode, No, false, ACD.Default).ConfigureAwait(true);
                await NAV.CreateRack(newrack, ACD.Default).ConfigureAwait(true); ;
                foreach (BinViewModel bvm in NewModel.BinsViewModel.BinViewModels)
                {
                    await SaveBin(bvm).ConfigureAwait(true);
                }
                LoadAnimation = false;
                State = ModelState.Normal;
                await Navigation.PopAsync();
            }
            catch (Exception e)
            {
                LoadAnimation = false;
                State = ModelState.Error;
                ErrorText = e.Message;
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
                    await NAV.CreateBin(NewModel.BinsViewModel.BinTemplate, bmv.Bin, ACD.Default).ConfigureAwait(true);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public void CancelAsync()
        {
            ACD.CancelAll();
            NewModel.CancelAsync(); 
        }

        public override void DisposeModel()
        {
            BinTemplates.Clear();
            base.DisposeModel();
        }
    }
}
