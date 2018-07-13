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
        private Rack SourceRack { get; set; }

        public string No
        {
            get { return no; }
            set
            {
                if (no != value)
                {
                    no = value;
                    BinsViewModel.RackNo = no;
                    Changed = true;
                    OnPropertyChanged(nameof(No));
                }
            }
        } string no;

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
        }
        string searchresult;

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

        public List<SubSchemeSelect> SubSchemeSelects { get; set; } = new List<SubSchemeSelect>();
        public List<SubSchemeSelect> UDSSelects { get; set; } = new List<SubSchemeSelect>();

        public RackViewModel(INavigation navigation, Rack rack, bool createmode1) : base(navigation)
        {
            SourceRack = rack;
            IsSaveToNAVEnabled = false;
            CreateMode = createmode1;
            BinsViewModel = new BinsViewModel(navigation);
            BinsViewModel.LocationCode = rack.LocationCode;
            BinsViewModel.ZoneCode = rack.ZoneCode;
            TapCommand = new Command<object>(Tap);
            FillFields(SourceRack);
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

        private async Task SaveBin(BinViewModel bmv, bool changeexisting)
        {
            try
            {
                bmv.SaveFields();
                LoadingText = AppResources.RackNewPage_LoadingProgressBin + " " + bmv.Bin.Code;

                int binexist = await NAV.GetBinCount(LocationCode, "", "", bmv.Bin.Code, ACD.Default).ConfigureAwait(true);
                if (binexist > 0)
                {
                    if (changeexisting)
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
                        NAVFilter navfilter = new NAVFilter
                        {
                            LocationCodeFilter = bvm.LocationCode,
                            ZoneCodeFilter = bvm.ZoneCode,
                            RackCodeFilter = bvm.RackNo,
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
