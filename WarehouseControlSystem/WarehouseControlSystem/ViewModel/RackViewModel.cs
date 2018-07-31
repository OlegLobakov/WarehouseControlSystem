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
    public class RackViewModel : NAVBaseViewModel, ISelectable
    {
        public int ID
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    BinsViewModel.RackID = id;
                    Changed = true;
                    OnPropertyChanged(nameof(ID));
                }
            }
        } int id;
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

        public string LocationCode
        {
            get { return locationcode; }
            set
            {
                if (locationcode != value)
                {
                    locationcode = value;
                    BinsViewModel.LocationCode = locationcode;
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
                    OnPropertyChanged(nameof(ZoneCode));
                }
            }
        } string zonecode;

        public int Sections
        {
            get { return sections; }
            set
            {
                if (sections != value)
                {
                    if ((value >= 1) && (value <= 100))
                    {
                        sections = value;
                        Changed = true;
                        OnPropertyChanged(nameof(Sections));
                    }
                    else
                    {
                        OnPropertyChanged(nameof(Sections));
                    }
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
                    if ((value >= 1) && (value <= 100))
                    {
                        levels = value;
                        Changed = true;
                        OnPropertyChanged(nameof(Levels));
                    }
                    else
                    {
                        OnPropertyChanged(nameof(Levels));
                    }
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
                    if ((value >= 1) && (value <= 10))
                    {
                        depth = value;
                        Changed = true;
                        OnPropertyChanged(nameof(Depth));
                    }
                    else
                    {
                        OnPropertyChanged(nameof(Depth));
                    }
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
        public string Comment
        {
            get { return comment; }
            set
            {
                if (comment != value)
                {
                    comment = value;
                    Changed = true;
                    OnPropertyChanged(nameof(comment));
                }
            }
        } string comment;

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
                if (numberingsectiondigitsquantity != value)
                {
                    if ((value >= 1) && (value <= 10))
                    {
                        numberingsectiondigitsquantity = value;
                        Renumbering();
                        OnPropertyChanged(nameof(NumberingSectionDigitsQuantity));
                    }
                    else
                    {
                        OnPropertyChanged(nameof(NumberingSectionDigitsQuantity));
                    }
                }
            }
        } int numberingsectiondigitsquantity = 2;
        public int NumberingLevelDigitsQuantity
        {
            get { return numberingleveldigitsquantity; }
            set
            {
                if (numberingleveldigitsquantity != value)
                {
                    if ((value >= 1) && (value <= 10))
                    {
                        numberingleveldigitsquantity = value;
                        Renumbering();
                        OnPropertyChanged(nameof(NumberingLevelDigitsQuantity));
                    }
                    else
                    {
                        OnPropertyChanged(nameof(NumberingLevelDigitsQuantity));
                    }
                }
            }
        } int numberingleveldigitsquantity = 1;
        public int NumberingDepthDigitsQuantity
        {
            get { return numberingdepthdigitsquantity; }
            set
            {
                if (numberingdepthdigitsquantity != value)
                {
                    if ((value >= 1) && (value <= 10))
                    {
                        numberingdepthdigitsquantity = value;
                        Renumbering();
                        OnPropertyChanged(nameof(NumberingDepthDigitsQuantity));
                    }
                    else
                    {
                        OnPropertyChanged(nameof(NumberingDepthDigitsQuantity));
                    }
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
        public int StepNumberingDepth
        {
            get { return stepnumberingdepth; }
            set
            {
                if (stepnumberingdepth != value)
                {
                    stepnumberingdepth = value;
                    Renumbering();
                    OnPropertyChanged("StepNumberingDepth");
                }
            }
        } int stepnumberingdepth = 1;

        public string BinTemplateCode
        {
            get { return bintemplatecode; }
            set
            {
                if (bintemplatecode != value)
                {
                    bintemplatecode = value;
                    OnPropertyChanged("BinTemplateCode");
                }
            }
        } string bintemplatecode;

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

        public ICommand TapCommand { protected set; get; }
        public ICommand SelectCommand { set; get; }

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

        public List<SubSchemeSelect> SubSchemeSelects { get; set; } = new List<SubSchemeSelect>();
        public List<SubSchemeSelect> UDSSelects { get; set; } = new List<SubSchemeSelect>();

        public bool IsNumberingEnabled { get; set; }
        public bool IsCreateBinsEnabled { get; set; }

        public RackViewModel(INavigation navigation, Rack rack) : base(navigation)
        {
            IsSaveToNAVEnabled = false;
            BinsViewModel = new BinsViewModel(navigation);
            BinsViewModel.LocationCode = rack.LocationCode;
            BinsViewModel.ZoneCode = rack.ZoneCode;
            TapCommand = new Command<object>(Tap);
            FillFields(rack);
            State = ModelState.Undefined;
            Changed = false;
            GetSearchSelection();
            IsSaveToNAVEnabled = true;
        }

        public void FillFields(Rack rack)
        {

            ID = rack.ID;
            LocationCode = rack.LocationCode;
            ZoneCode = rack.ZoneCode;
            No = rack.No;
            Sections = rack.Sections;
            Levels = rack.Levels;
            Depth = rack.Depth;
            RackOrientation = rack.RackOrientation;
            SchemeVisible = rack.SchemeVisible;
            Left = rack.Left;
            Top = rack.Top;
            Width = rack.Width;
            Height = rack.Height;
            Comment = rack.Comment;

            bool savevalue = IsNumberingEnabled;
            IsNumberingEnabled = false;

            NumberingPrefix = rack.NumberingPrefix;

            RackSectionSeparator = rack.RackSectionSeparator;
            SectionLevelSeparator = rack.SectionLevelSeparator;
            LevelDepthSeparator = rack.LevelDepthSeparator;

            ReversSectionNumbering = rack.ReversSectionNumbering;
            ReversLevelNumbering = rack.ReversLevelNumbering;
            ReversDepthNumbering = rack.ReversDepthNumbering;

            NumberingSectionBegin = rack.NumberingSectionBegin;
            NumberingLevelBegin = rack.NumberingLevelBegin;
            NumberingDepthBegin = rack.NumberingDepthBegin;

            NumberingSectionDigitsQuantity = rack.NumberingSectionDigitsQuantity;
            NumberingLevelDigitsQuantity = rack.NumberingLevelDigitsQuantity;
            NumberingDepthDigitsQuantity = rack.NumberingDepthDigitsQuantity;

            StepNumberingSection = rack.StepNumberingSection;
            StepNumberingLevel = rack.StepNumberingLevel;
            StepNumberingDepth = rack.StepNumberingDepth;

            BinTemplateCode = rack.BinTemplateCode;

            IsNumberingEnabled = savevalue;
        }

        public void SaveFields(Rack rack)
        {
            rack.ID = ID;
            rack.No = No;
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
            rack.Comment = Comment;

            rack.NumberingPrefix = NumberingPrefix;

            rack.RackSectionSeparator = RackSectionSeparator;
            rack.SectionLevelSeparator = SectionLevelSeparator;
            rack.LevelDepthSeparator = LevelDepthSeparator;

            rack.ReversSectionNumbering = ReversSectionNumbering;
            rack.ReversLevelNumbering = ReversLevelNumbering;
            rack.ReversDepthNumbering = ReversDepthNumbering;

            rack.NumberingSectionBegin = NumberingSectionBegin;
            rack.NumberingLevelBegin = NumberingLevelBegin;
            rack.NumberingDepthBegin = NumberingDepthBegin;

            rack.NumberingSectionDigitsQuantity = NumberingSectionDigitsQuantity;
            rack.NumberingLevelDigitsQuantity = NumberingLevelDigitsQuantity;
            rack.NumberingDepthDigitsQuantity = NumberingDepthDigitsQuantity;

            rack.StepNumberingSection = StepNumberingSection;
            rack.StepNumberingLevel = StepNumberingLevel;
            rack.StepNumberingDepth = StepNumberingDepth;

            rack.BinTemplateCode = BinTemplateCode;
        }

        public void CreateBins()
        {
            BinsViewModel.CreateBins(Depth, Levels, Sections);
        }

        public void RecreateBins(int prevdepth, int newdepth, int prevlevels, int newlevels, int prevsections, int newsections)
        {
            if (IsCreateBinsEnabled)
            {
                BinsViewModel.RecreateBins(prevdepth, newdepth, prevlevels, newlevels, prevsections, newsections);
            }
        }

        public async void Renumbering()
        {
            if (IsNumberingEnabled)
            {
                BinsViewModel.UnSelect();
                foreach (BinViewModel bvm in BinsViewModel.BinViewModels)
                {
                    SetNumber(bvm);
                }
                await BinsViewModel.CheckBins(ACD).ConfigureAwait(true);
            }
        }

        public async void NumberingEmptyBins()
        {
            List<BinViewModel> list = BinsViewModel.BinViewModels.FindAll(x => x.Code == "");
            if (list is List<BinViewModel>)
            {
                foreach (BinViewModel bvm in list)
                {
                    SetNumber(bvm);
                    await BinsViewModel.CheckBin(bvm, ACD);
                }
            }
        }

        private void SetNumber(BinViewModel bvm)
        {
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-us");

            int sectionname = NumberingSectionBegin + (bvm.Section - 1) * StepNumberingSection;
            if (ReversSectionNumbering)
            {
                sectionname = NumberingSectionBegin + (Sections - bvm.Section) * StepNumberingSection; ;
            }

            int levelname = NumberingLevelBegin + (Levels - bvm.Level) * StepNumberingLevel;
            if (ReversLevelNumbering)
            {
                levelname = NumberingLevelBegin + (bvm.Level - 1) * StepNumberingLevel; ;
            }

            string sectionlabel = sectionname.ToString("D" + NumberingSectionDigitsQuantity.ToString(), ci);
            string lavellabel = levelname.ToString("D" + NumberingLevelDigitsQuantity.ToString(), ci);

            bvm.Code = NumberingPrefix + racksectionseparator + sectionlabel + sectionlevelseparator + lavellabel;
        }

        public void Tap(object sender)
        {
            if (OnTap is Action<RackViewModel>)
            {
                OnTap(this);
            }
        }

        public async Task LoadBins()
        {
            BinsViewModel.LinkToRackViewModel = this;
            await BinsViewModel.LoadBins(ACD).ConfigureAwait(true);
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

        public async Task LoadBinValues()
        {
            try
            {
                await BinsViewModel.LoadBinValues(ACD).ConfigureAwait(true);
            }
            catch (OperationCanceledException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
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
                List<BinViewModel> list = BinsViewModel.BinViewModels.FindAll(x => x.IsSelected == true);
                if (list is List<BinViewModel>)
                {
                    State = ModelState.Loading;
                    LoadAnimation = true;
                    foreach (BinViewModel bvm in list)
                    {
                        LoadingText = bvm.Code;
                        NAVFilter navfilter = new NAVFilter
                        {
                            LocationCodeFilter = bvm.LocationCode,
                            ZoneCodeFilter = bvm.ZoneCode,
                            RackIDFilter = bvm.RackID.ToString(),
                            BinCodeFilter = bvm.Code
                        };
                        string response = await NAV.RunFunction(udfvmselected.ID, navfilter, 0, ACD.Default).ConfigureAwait(true);
                    }
                    State = ModelState.Normal;
                }
                else
                {
                    State = ModelState.Error;
                    ErrorText = AppResources.RackCardPage_Error_BinDidNotSelect;
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
                    x.RackID == ID);
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

        public override void DisposeModel()
        {
            base.DisposeModel();
            if (OnTap is Action<RackViewModel>)
            {
                Delegate[] clientList = OnTap.GetInvocationList();
                foreach (var d in clientList)
                {
                    OnTap -= (d as Action<RackViewModel>);
                }
            }
            BinsViewModel.DisposeModel();
        }
    }
}
