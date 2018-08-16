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
using Xamarin.Forms;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.Helpers.NAV;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Threading;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.Helpers.Comparer;

namespace WarehouseControlSystem.ViewModel
{
    public class BinsViewModel : BaseViewModel
    {
        public int RackID
        {
            get { return rackid; }
            set
            {
                if (rackid != value)
                {
                    rackid = value;
                    foreach (BinViewModel bvm in BinViewModels)
                    {
                        bvm.RackID = rackid;
                    }
                    OnPropertyChanged(nameof(RackID));
                }
            }
        } int rackid;
        public string LocationCode
        {
            get { return locationcode; }
            set
            {
                if (locationcode != value)
                {
                    locationcode = value;
                    foreach (BinViewModel bvm in BinViewModels)
                    {
                        bvm.LocationCode = locationcode;
                    }
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
                    foreach (BinViewModel bvm in BinViewModels)
                    {
                        bvm.ZoneCode = zonecode;
                    }
                    OnPropertyChanged(nameof(ZoneCode));
                }
            }
        } string zonecode;

        public BinTemplate BinTemplate
        {
            get { return bintemplate; }
            set
            {
                if (bintemplate != value)
                {
                    bintemplate = value;
                    if (value is BinTemplate)
                    {
                        ChangeBinTemplate();
                    }
                    OnPropertyChanged(nameof(BinTemplate));
                }
            }
        } BinTemplate bintemplate;

        public ICommand CombineBinsCommand { protected set; get; }
        public ICommand DeleteBinsCommand { protected set; get; }
        public ICommand ShowBinOperationCommand { protected set; get; }

        public ICommand ContentViewCommand { protected set; get; }
        public ICommand FunctionsViewCommand { protected set; get; }
        public ICommand BinInfoViewCommand { protected set; get; }

        public event Action<BinsViewModel> OnBinClick;

        public List<BinViewModel> BinViewModels { get; set; } = new List<BinViewModel>();
        public List<EmptySpaceViewModel> EmptySpacesViewModels { get; set; } = new List<EmptySpaceViewModel>();
        //public BinViewModel LastSelectedBinViewModel { get; set; }
        public string LastSelectedBinCode
        {
            get { return lastselectedbincode; }
            set
            {
                if (lastselectedbincode != value)
                {
                    lastselectedbincode = value;
                    OnPropertyChanged(nameof(LastSelectedBinCode));
                }
            }
        } string lastselectedbincode;

        public ObservableCollection<BinContentGrouping> SelectedBinContent
        {
            get { return selectedbincontent; }
            set
            {
                if (selectedbincontent != value)
                {
                    selectedbincontent = value;
                    OnPropertyChanged(nameof(SelectedBinContent));
                }
            }
        } ObservableCollection<BinContentGrouping> selectedbincontent;
        public ObservableCollection<UserDefinedFunctionViewModel> UserDefinedFunctions
        {
            get { return userdefinedfunctions; }
            set
            {
                if (userdefinedfunctions != value)
                {
                    userdefinedfunctions = value;
                    OnPropertyChanged(nameof(UserDefinedFunctions));
                }
            }
        } ObservableCollection<UserDefinedFunctionViewModel> userdefinedfunctions;
        public ObservableCollection<BinInfoViewModel> BinInfo
        {
            get { return bininfo; }
            set
            {
                if (bininfo != value)
                {
                    bininfo = value;
                    OnPropertyChanged(nameof(BinInfo));
                }
            }
        } ObservableCollection<BinInfoViewModel> bininfo;

        public RackViewModel LinkToRackViewModel { get; set; }

        public ObservableCollection<string> BinTypes { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> SpecialEquipments { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> WarehouseClasses { get; set; } = new ObservableCollection<string>();

        public bool SpecialEquipmentsIsEnabled
        {
            get { return specialequipmentsenabled; }
            set
            {
                if (specialequipmentsenabled != value)
                {
                    specialequipmentsenabled = value;
                    OnPropertyChanged(nameof(SpecialEquipmentsIsEnabled));
                }
            }
        } bool specialequipmentsenabled;
        public bool WarehouseClassesIsEnabled
        {
            get { return warehouseclassesisenabled; }
            set
            {
                if (warehouseclassesisenabled != value)
                {
                    warehouseclassesisenabled = value;
                    OnPropertyChanged(nameof(WarehouseClassesIsEnabled));
                }
            }
        } bool warehouseclassesisenabled;
        public bool BinTypesIsEnabled
        {
            get { return bintypesisenabled; }
            set
            {
                if (bintypesisenabled != value)
                {
                    bintypesisenabled = value;
                    OnPropertyChanged(nameof(BinTypesIsEnabled));
                }
            }
        } bool bintypesisenabled;

        /// <summary>
        /// BinInfo panel
        /// </summary>
        public bool IsUserDefinedCommandsVisible
        {
            get { return isuserdefinedcommandsvisible; }
            set
            {
                if (isuserdefinedcommandsvisible != value)
                {
                    isuserdefinedcommandsvisible = value;
                    OnPropertyChanged(nameof(IsUserDefinedCommandsVisible));
                }
            }
        } bool isuserdefinedcommandsvisible;
        public bool IsContentVisible
        {
            get { return iscontentvisible; }
            set
            {
                if (iscontentvisible != value)
                {
                    iscontentvisible = value;
                    OnPropertyChanged(nameof(IsContentVisible));
                }
            }
        } bool iscontentvisible;
        public bool IsBinInfoVisible
        {
            get { return isbininfovisible; }
            set
            {
                if (isbininfovisible != value)
                {
                    isbininfovisible = value;
                    OnPropertyChanged(nameof(IsBinInfoVisible));
                }
            }
        } bool isbininfovisible;

        public bool MultiSelectBins
        {
            get { return multiselectbins; }
            set
            {
                if (multiselectbins != value)
                {
                    multiselectbins = value;
                    OnPropertyChanged(nameof(MultiSelectBins));
                }
            }
        } bool multiselectbins;

        public bool EditedBinCodeIsEnabled
        {
            get { return editedbincodeisenabled; }
            set
            {
                if (editedbincodeisenabled != value)
                {
                    editedbincodeisenabled = value;
                    OnPropertyChanged(nameof(EditedBinCodeIsEnabled));
                }
            }
        } bool editedbincodeisenabled;
        public bool IsSelectedBins
        {
            get { return isselectedbins; }
            set
            {
                if (isselectedbins != value)
                {
                    isselectedbins = value;
                    OnPropertyChanged("IsSelectedBins");
                }
            }
        } bool isselectedbins;
        public int LoadedBinsQuantity
        {
            get { return loadedbinsquantity; }
            set
            {
                if (loadedbinsquantity != value)
                {
                    loadedbinsquantity = value;
                    OnPropertyChanged(nameof(LoadedBinsQuantity));
                }
            }
        } int loadedbinsquantity;
        public int SearchBinsQuantity
        {
            get { return searchbinsquantity; }
            set
            {
                if (searchbinsquantity != value)
                {
                    searchbinsquantity = value;
                    OnPropertyChanged(nameof(SearchBinsQuantity));
                }
            }
        } int searchbinsquantity = 1;

        /// <summary>
        /// For editing bins
        /// </summary>
        public string TemplateCode
        {
            get { return templatecode; }
            set
            {
                if (templatecode != value)
                {
                    templatecode = value;
                    SetSelectedBinsByTemplate(true);
                    OnPropertyChanged(nameof(TemplateCode));
                }
            }
        } string templatecode;
        public bool TemplateBlocked
        {
            get { return templateblocked; }
            set
            {
                if (templateblocked != value)
                {
                    templateblocked = value;
                    SetSelectedBinsByTemplate(false);
                    OnPropertyChanged(nameof(TemplateBlocked));
                }
            }
        } bool templateblocked;
        public int TemplateBlockMovement
        {
            get { return templateblockmovement; }
            set
            {
                if (templateblockmovement != value)
                {
                    templateblockmovement = value;
                    SetSelectedBinsByTemplate(false);
                    OnPropertyChanged(nameof(TemplateBlockMovement));
                }
            }
        } int templateblockmovement;
        public string TemplateBinType
        {
            get { return templatebintype; }
            set
            {
                if (templatebintype != value)
                {
                    templatebintype = value;
                    SetSelectedBinsByTemplate(false);
                    OnPropertyChanged(nameof(TemplateBinType));
                }
            }
        } string templatebintype;
        public int TemplateBinRanking
        {
            get { return templatebinranking; }
            set
            {
                if (templatebinranking != value)
                {
                    templatebinranking = value;
                    SetSelectedBinsByTemplate(false);
                    OnPropertyChanged(nameof(TemplateBinRanking));
                }
            }
        } int templatebinranking;
        public decimal TemplateMaximumCubage
        {
            get { return templatemaximumcubage; }
            set
            {
                if (templatemaximumcubage != value)
                {
                    templatemaximumcubage = value;
                    SetSelectedBinsByTemplate(false);
                    OnPropertyChanged(nameof(TemplateMaximumCubage));
                }
            }
        } decimal templatemaximumcubage;
        public decimal TemplateMaximumWeight
        {
            get { return templatemaximumweight; }
            set
            {
                if (templatemaximumweight != value)
                {
                    templatemaximumweight = value;
                    SetSelectedBinsByTemplate(false);
                    OnPropertyChanged(nameof(TemplateMaximumWeight));
                }
            }
        } decimal templatemaximumweight;
        public bool TemplateAdjustmentBin
        {
            get { return templateadjustmentbin; }
            set
            {
                if (templateadjustmentbin != value)
                {
                    templateadjustmentbin = value;
                    SetSelectedBinsByTemplate(false);
                    OnPropertyChanged(nameof(TemplateAdjustmentBin));
                }
            }
        } bool templateadjustmentbin;
        public string TemplateWarehouseClassCode
        {
            get { return templatewarehouseclasscode; }
            set
            {
                if (templatewarehouseclasscode != value)
                {
                    templatewarehouseclasscode = value;
                    SetSelectedBinsByTemplate(false);
                    OnPropertyChanged(nameof(TemplateWarehouseClassCode));
                }
            }
        } string templatewarehouseclasscode;
        public string TemplateSpecialEquipmentCode
        {
            get { return templatespecialequipmentcode; }
            set
            {
                if (templatespecialequipmentcode != value)
                {
                    templatespecialequipmentcode = value;
                    SetSelectedBinsByTemplate(false);
                    OnPropertyChanged(nameof(TemplateSpecialEquipmentCode));
                }
            }
        } string templatespecialequipmentcode;
        public bool TemplateDefault
        {
            get { return templatedefault1; }
            set
            {
                if (templatedefault1 != value)
                {
                    templatedefault1 = value;
                    SetSelectedBinsByTemplate(false);
                    OnPropertyChanged(nameof(TemplateDefault));
                }
            }
        } bool templatedefault1;

        public BinsViewModel(INavigation navigation) : base(navigation)
        {
            SelectedBinContent = new ObservableCollection<BinContentGrouping>();
            UserDefinedFunctions = new ObservableCollection<UserDefinedFunctionViewModel>();

            CombineBinsCommand = new Command(CombineBins);
            DeleteBinsCommand = new Command(DeleteBins);
            ShowBinOperationCommand = new Command(ShowBinOperations);

            ContentViewCommand = new Command(ShowContent);
            FunctionsViewCommand = new Command(ShowFunctions);
            BinInfoViewCommand = new Command(ShowBinInfo);

            IsContentVisible = true;
            State = ModelState.Undefined;
            MultiSelectBins = true;
        }

        public void RecreateBins(int prevdepth, int newdepth, int prevlevels, int newlevels, int prevsections, int newsections)
        {
            RecreateBinsDepth(prevdepth, newdepth, newlevels, newsections);
            RecreateBinsLevels(prevlevels, newlevels, newdepth, newsections);
            RecreateBinsSections(prevsections, newsections, newdepth, newlevels);
        }

        private void RecreateBinsDepth(int prevdepth, int newdepth, int newlevels, int newsections)
        {
            if (prevdepth < newdepth)
            {
                for (int k = prevdepth + 1; k <= newdepth; k++)
                {
                    for (int i = 1; i <= newlevels; i++)
                    {
                        for (int j = 1; j <= newsections; j++)
                        {
                            CreateBin(i, j, k);
                        }
                    }
                }
            }
            else if (prevdepth > newdepth)
            {
                for (int k = prevdepth; k > newdepth; k--)
                {
                    List<BinViewModel> list = BinViewModels.FindAll(x => x.Bin.Depth == k);
                    foreach (BinViewModel bvm in list)
                    {
                        DeleteBin(bvm);
                    }
                }
            }
        }
        private void RecreateBinsLevels(int prevlevels, int newlevels, int newdepth, int newsections)
        {
            if (prevlevels < newlevels)
            {
                for (int i = prevlevels + 1; i <= newlevels; i++)
                {
                    for (int k = 1; k <= newdepth; k++)
                    {
                        for (int j = 1; j <= newsections; j++)
                        {
                            CreateBin(i, j, k);
                        }
                    }
                }
            }
            else if (prevlevels > newlevels)
            {
                for (int i = prevlevels; i > newlevels; i--)
                {
                    List<BinViewModel> list = BinViewModels.FindAll(x => x.Bin.Level == i);
                    foreach (BinViewModel bvm in list)
                    {
                        DeleteBin(bvm);
                    }
                }
            }
        }
        private void RecreateBinsSections(int prevsections, int newsections, int newdepth, int newlevels)
        {
            if (prevsections < newsections)
            {
                for (int j = prevsections + 1; j <= newsections; j++)
                {
                    for (int k = 1; k <= newdepth; k++)
                    {
                        for (int i = 1; i <= newlevels; i++)
                        {
                            CreateBin(i, j, k);
                        }
                    }
                }
            }
            else if (prevsections > newsections)
            {
                for (int j = prevsections; j > newsections; j--)
                {
                    List<BinViewModel> list = BinViewModels.FindAll(x => x.Bin.Section == j);
                    foreach (BinViewModel bvm in list)
                    {
                        DeleteBin(bvm);
                    }
                }
            }
        }

        public void CreateBins(int depth, int levels, int sections)
        {
            foreach (BinViewModel bvm in BinViewModels)
            {
                bvm.OnTap -= Bvm_OnTap;
            }
            BinViewModels.Clear();

            for (int k = 1; k <= depth; k++)
            {
                for (int i = 1; i <= levels; i++)
                {
                    for (int j = 1; j <= sections; j++)
                    {
                        CreateBin(i, j, k);
                    }
                }
            }
        }

        public BinViewModel CreateBin(int i, int j, int k)
        {
            Bin newbin = new Bin()
            {
                RackID = RackID,
                Section = j,
                Level = i,
                Depth = k
            };
            BinViewModel bvm = new BinViewModel(Navigation, newbin);
            if (BinTemplate is BinTemplate)
            {
                FillBinFromTemplate(bvm);
            }
            bvm.OnTap += Bvm_OnTap;
            BinViewModels.Add(bvm);
            return bvm;
        }

        private void DeleteBin(BinViewModel bvm)
        {
            bvm.OnTap -= Bvm_OnTap;
            BinViewModels.Remove(bvm);
        }

        public void CombineBins()
        {
            string combinedrackno1 = "";
            string locationcode1 = "";
            int rackno1;
            List<BinViewModel> selectedlist = BinViewModels.FindAll(x => x.IsSelected == true);
            if (selectedlist.Count > 1)
            {
                DefineBordersOfCombine(selectedlist);

                List<BinViewModel> fordelete =
                    BinViewModels.FindAll(x =>
                        x.Section >= leftsection &&
                        x.Section <= rightsection &&
                        x.Level >= leftlevel &&
                        x.Level <= rightlevel);

                BinViewModel firstbvm = BinViewModels.Find(x =>
                        x.Section >= leftsection &&
                        x.Level >= leftlevel);

                if (firstbvm is BinViewModel)
                {
                    combinedrackno1 = firstbvm.Code;
                    locationcode1 = firstbvm.LocationCode;
                    rackno1 = firstbvm.RackID;
                }

                DeleteSelected(fordelete);

                Bin newbin = new Bin()
                {
                    LocationCode = locationcode1,
                    RackID = rackid,
                    Code = combinedrackno1,
                    Section = leftsection,
                    Level = leftlevel,
                    Depth = 1,
                    SectionSpan = rightsection - leftsection + 1,
                    LevelSpan = rightlevel - leftlevel + 1,
                };

                BinViewModel bvm = new BinViewModel(Navigation, newbin);
                FillBinFromTemplate(bvm);
                bvm.OnTap += Bvm_OnTap;
                BinViewModels.Add(bvm);
            }
            UnSelect();
            MessagingCenter.Send(this, "Update");
        }

        int leftsection = int.MaxValue;
        int leftlevel = int.MaxValue;
        int rightsection = 0;
        int rightlevel = 0;
        private void DefineBordersOfCombine(List<BinViewModel> selectedlist)
        {
            leftsection = int.MaxValue;
            leftlevel = int.MaxValue;
            rightsection = 0;
            rightlevel = 0;

            foreach (BinViewModel bvm1 in selectedlist)
            {
                if (bvm1.Section < leftsection)
                {
                    leftsection = bvm1.Section;
                }
                if (bvm1.Level < leftlevel)
                {
                    leftlevel = bvm1.Level;
                }
                if (bvm1.Section > rightsection + bvm1.SectionSpan - 1)
                {
                    rightsection = bvm1.Section + bvm1.SectionSpan - 1;
                }
                if (bvm1.Level > rightlevel + bvm1.LevelSpan - 1)
                {
                    rightlevel = bvm1.Level + bvm1.LevelSpan - 1;
                }
            }
        }

        private void DeleteSelected(List<BinViewModel> list)
        {
            foreach (BinViewModel bvm2 in list)
            {
                DeleteBin(bvm2);
            }
        }

        public void DeleteBins()
        {
            List<BinViewModel> selectedlist = BinViewModels.FindAll(x => x.IsSelected == true);
            foreach (BinViewModel bvm in selectedlist)
            {
                DeleteBin(bvm);

                for(int i=1;i<=bvm.SectionSpan;i++)
                {
                    for (int j = 1; j <= bvm.LevelSpan; j++)
                    {
                        EmptySpaceViewModel esvm = new EmptySpaceViewModel(Navigation);
                        esvm.Section = bvm.Section + i - 1;
                        esvm.Level = bvm.Level + j - 1;
                        esvm.Depth = bvm.Depth;
                        esvm.OnTap += Esvm_OnTap;
                        EmptySpacesViewModels.Add(esvm);
                    }
                }
            }
            MessagingCenter.Send(this, "Update");
        }

        /// <summary>
        /// Tap on Empty Space
        /// </summary>
        /// <param name="obj"></param>
        private void Esvm_OnTap(EmptySpaceViewModel esvm)
        {
            esvm.OnTap -= Esvm_OnTap;
            if (EmptySpacesViewModels.Contains(esvm))
            {
                EmptySpacesViewModels.Remove(esvm);
            }
            CreateBin(esvm.Level, esvm.Section, esvm.Depth);

            MessagingCenter.Send(this, "Update");
        }

        public void FillEmptyPositions(int Sections, int Levels)
        {
            for (int i = 1; i <= Sections; i++)
            {
                for (int j = 1; j <= Levels; j++)
                {
                    BinViewModel exist = BinViewModels.Find(x => x.Level == j && x.Section == i);
                    if (exist is null)
                    {
                        EmptySpaceViewModel existesvm = EmptySpacesViewModels.Find(x => x.Level == j && x.Section == i);
                        if (existesvm is null)
                        {
                            EmptySpaceViewModel esvm = new EmptySpaceViewModel(Navigation);
                            esvm.Section = i;
                            esvm.Level = j;
                            esvm.Depth = 1;
                            esvm.OnTap += Esvm_OnTap;
                            EmptySpacesViewModels.Add(esvm);
                        }
                    }
                }
            }
        }

        public void ShowBinOperations()
        {
            Global.CompliantPlug = "";
        }

        public void ShowContent()
        {
            IsUserDefinedCommandsVisible = false;
            IsBinInfoVisible = false;
            IsContentVisible = true;
        }
        public void ShowFunctions()
        {
            IsContentVisible = false;
            IsBinInfoVisible = false;
            IsUserDefinedCommandsVisible = true;
        }
        public void ShowBinInfo()
        {
            IsContentVisible = false;
            IsUserDefinedCommandsVisible = false;
            IsBinInfoVisible = true;
        }

        public async Task CheckBins(AsyncCancelationDispatcher acd)
        {
            List<BinViewModel> list = BinViewModels.ToList();
            foreach (BinViewModel bvm in list)
            {
                await CheckBin(bvm, acd).ConfigureAwait(true);
            }
        }

        public async Task CheckSelectedBins(AsyncCancelationDispatcher acd)
        {
            List<BinViewModel> list = BinViewModels.FindAll(x => x.IsSelected == true);
            foreach (BinViewModel bvm in list)
            {
                await CheckBin(bvm, acd).ConfigureAwait(true);
            }
        }

        public async Task CheckBin(BinViewModel bvm, AsyncCancelationDispatcher acd)
        {
            try
            {
                bvm.IsChecked = false;
                NAVFilter navfilter = new NAVFilter
                {
                    LocationCodeFilter = LocationCode,
                    BinCodeFilter = bvm.Code
                };
                List<Bin> binsexist = await NAV.GetBinList(navfilter, acd.Default).ConfigureAwait(true);
                if ((NotDisposed) && (binsexist.Count > 0))
                {
                    bvm.IsExist = true;

                    Bin bin = binsexist.First();
                    bin.RackID = bvm.RackID;
                    bin.Section = bvm.Section;
                    bin.Level = bvm.Level;
                    bin.Depth = bvm.Depth;
                    bin.SectionSpan = bvm.SectionSpan;
                    bin.LevelSpan = bvm.LevelSpan;
                    bin.DepthSpan = bvm.DepthSpan;
                    bvm.FillFields(bin);
                }
                else
                {
                    bvm.IsExist = false;
                }
                bvm.IsChecked = true;

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
        public async Task LoadBins(AsyncCancelationDispatcher acd)
        {
            BinViewModelsDispose();
            try
            {
                LoadedBinsQuantity = 0;
                SearchBinsQuantity = 0;
                NAVFilter navfilter = new NAVFilter
                {
                    LocationCodeFilter = LocationCode,
                    ZoneCodeFilter = ZoneCode,
                    RackIDFilter = RackID.ToString()
                };
                List<Bin> bins = await NAV.GetBinList(navfilter, acd.Default).ConfigureAwait(true);
                if (NotDisposed)
                {
                    if (bins.Count > 0)
                    {
                        LoadedBinsQuantity = bins.Count;
                        foreach (Bin bin in bins)
                        {
                            BinViewModel bvm = new BinViewModel(Navigation, bin);
                            bvm.IsContent = !bin.Empty;
                            bvm.Color = (Color)Application.Current.Resources["BinViewColor"];
                            bvm.OnTap += Bvm_OnTap;
                            ExistInSearch(bvm);
                            ExistInUDS(bvm);
                            BinViewModels.Add(bvm);
                        }
                    }
                    
                    MessagingCenter.Send<BinsViewModel>(this, "BinsIsLoaded");
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
        public async Task LoadBinValues(AsyncCancelationDispatcher acd)
        {
            try
            {
                List<BinValues> binvalues = await NAV.GetBinValuesByRackID(RackID, acd.Default).ConfigureAwait(true);
                if (NotDisposed)
                {
                    foreach (BinValues bv in binvalues)
                    {
                        BinViewModel bvm = BinViewModels.Find(x => x.Code == bv.Code);
                        if (bvm is BinViewModel)
                        {
                            bvm.BottomRightValue = bv.RightValue;
                            bvm.BottomLeftValue = bv.LeftValue;
                        }
                    }
                }
            }
            catch (OperationCanceledException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                throw e;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                throw e;
            }
        }

        private void ExistInSearch(BinViewModel bvm)
        {
            if (Global.SearchResponses is List<SearchResponse>)
            {
                if (Global.SearchResponses.Count > 0)
                {
                    bvm.ExcludeFromSearch = true;

                    List<SearchResponse> list = Global.SearchResponses.FindAll(
                        x =>
                        x.ZoneCode == ZoneCode &&
                        x.RackID == RackID &&
                        x.BinCode == bvm.Code);

                    if (list is List<SearchResponse>)
                    {
                        if (list.Count > 0)
                        {
                            bvm.ExcludeFromSearch = false;
                            SearchBinsQuantity++;
                        }
                    }
                }
            }
        }
        private void ExistInUDS(BinViewModel bvm)
        {
            if (LinkToRackViewModel is RackViewModel)
            {
                if (LinkToRackViewModel.UDSSelects.Count > 0)
                {
                    SubSchemeSelect sss = LinkToRackViewModel.UDSSelects.Find(
                      x => x.Section == bvm.Section &&
                      x.Level == bvm.Level &&
                      x.Depth == bvm.Depth);

                    if (sss is SubSchemeSelect)
                    {
                        bvm.Color = Color.FromHex(sss.HexColor);
                        bvm.SearchQuantity = sss.Value;
                        bvm.IsSearchQuantityVisible = true;
                    }
                }
            }
        }
        public async Task LoadUDF(AsyncCancelationDispatcher acd)
        {
            try
            {
                List<UserDefinedFunction> list = await NAV.LoadUserDefinedFunctionList(LocationCode, ZoneCode, RackID, acd.Default).ConfigureAwait(true);
                if (NotDisposed)
                {
                    if (list is List<UserDefinedFunction>)
                    {
                        ObservableCollection<UserDefinedFunctionViewModel> nlist = new ObservableCollection<UserDefinedFunctionViewModel>();

                        foreach (UserDefinedFunction udf in list)
                        {
                            UserDefinedFunctionViewModel udfvm = new UserDefinedFunctionViewModel(Navigation, udf);
                            nlist.Add(udfvm);
                        }

                        UserDefinedFunctions = nlist;
                    }
                }
            }
            catch (OperationCanceledException e)
            {
                System.Diagnostics.Debug.WriteLine("Cancel LoadUDF", e.Message);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                State = ModelState.Error;
                ErrorText = e.ToString();
            }
        }
        public void UnSelect()
        {
            List<BinViewModel> selectedlist = BinViewModels.FindAll(x => x.IsSelected == true);
            foreach (BinViewModel bvm in selectedlist)
            {
                bvm.IsSelected = false;
            }
            IsSelectedBins = false;
        }

        public void Clear()
        {
            foreach (BinViewModel bvm in BinViewModels)
            {
                DeleteBin(bvm);
            }
            BinViewModels.Clear();
        }

        public BinViewModel Find(int j, int i, int k)
        {
            return BinViewModels.Find(x => x.Bin.Section == j && x.Bin.Level == i && x.Bin.Depth == k);
        }

        public void ChangeBinTemplate()
        {
            foreach (BinViewModel bvm in BinViewModels)
            {
                if (!bvm.IsExist)
                {
                    FillBinFromTemplate(bvm);
                }
            }
        }

        public void FillBinFromTemplate(BinViewModel bvm)
        {
            if (BinTemplate is BinTemplate)
            {
                bvm.LocationCode = BinTemplate.LocationCode;
                bvm.BinType = BinTemplate.BinTypeCode;
                bvm.Description = BinTemplate.BinDescription;
                bvm.WarehouseClassCode = BinTemplate.WarehouseClassCode;
                bvm.BlockMovement = BinTemplate.BlockMovement;
                bvm.SpecialEquipmentCode = BinTemplate.SpecialEquipmentCode;
                bvm.BinRanking = BinTemplate.BinRanking;
                bvm.MaximumCubage = BinTemplate.MaximumCubage;
                bvm.MaximumWeight = BinTemplate.MaximumWeight;
                bvm.Dedicated = BinTemplate.Dedicated;
            }
        }

        public void CancelAsync()
        {
            ACD.CancelAll();
        }

        private async void Bvm_OnTap(BinViewModel bvm)
        {
            Select(bvm);

            if (!IsEditMode)
            {
                if (bvm.IsSelected)
                {
                    LastSelectedBinCode = bvm.Code;
                    MessagingCenter.Send(bvm, "BinsViewModel.BinSelected");

                    if (bvm.IsContent)
                    {
                        bvm.LoadAnimation = true;
                        try
                        {
                            NAVFilter navfilter = new NAVFilter
                            {
                                LocationCodeFilter = LocationCode,
                                ZoneCodeFilter = ZoneCode,
                                BinCodeFilter = bvm.Code
                            };

                            List<BinContent> bincontent = await NAV.GetBinContentList(navfilter, ACD.Default).ConfigureAwait(true);
                            if ((NotDisposed) && (bincontent.Count > 0))
                            {
                                FillBinContent(bvm, bincontent);
                            }

                            List<BinInfo> bininfo = await NAV.GetBinInfo(bvm.LocationCode, bvm.Code, ACD.Default).ConfigureAwait(true);
                            if (NotDisposed)
                            {
                                FillBinInfo(bvm, bininfo);
                            }
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e.Message);
                        }
                        bvm.LoadAnimation = false;
                    }
                }

                SetSelectedBinContent();
            }

            EditedBinCodeIsEnabled = false;
            List<BinViewModel> list = BinViewModels.FindAll(x => x.IsSelected == true);
            if (list is List<BinViewModel>)
            {
                EditedBinCodeIsEnabled = list.Count == 1;
            }

            if (OnBinClick is Action<BinsViewModel>)
            {
                OnBinClick(this);
            }
        }

        private void Select(BinViewModel bvm)
        {
            if (!MultiSelectBins)
            {
                List<BinViewModel> list = BinViewModels.FindAll(x => x.IsSelected == true);
                foreach (BinViewModel bvm1 in list)
                {
                    if (bvm1 != bvm)
                    {
                        bvm1.IsSelected = false;
                    }
                }
            }
            bvm.IsSelected = !bvm.IsSelected;
            AfterSelect();
        }

        public void AfterSelect()
        {
            BinViewModel selectedbvm = BinViewModels.Find(x => x.IsSelected == true);
            IsSelectedBins = selectedbvm is BinViewModel;

            if (selectedbvm is BinViewModel)
            {
                SetTemplateBySelectedBin(selectedbvm);
            }
        }


        private void FillBinContent(BinViewModel bvm, List<BinContent> bincontent)
        {
            bvm.BinContent.Clear();
            foreach (BinContent bc in bincontent)
            {
                BinContentShortViewModel bcsvm = new BinContentShortViewModel(Navigation, bc);
                bvm.BinContent.Add(bcsvm);
            }
        }

        private void SetSelectedBinContent()
        {
            List<BinViewModel> list = BinViewModels.FindAll(x => x.IsSelected == true);
            if (list is List<BinViewModel>)
            {
                ObservableCollection<BinContentGrouping> nlist = new ObservableCollection<BinContentGrouping>();
                foreach (BinViewModel bvm1 in list)
                {
                    nlist.Add(new BinContentGrouping(bvm1.Code, bvm1.BinContent));
                }
                SelectedBinContent = nlist;
                LoadImages(false);
            }
        }

        public void LoadImages(bool update)
        {
            foreach (BinContentGrouping bcg in SelectedBinContent)
            {
                foreach (BinContentShortViewModel bcsvm in bcg)
                {
                    if (Settings.ShowImages)
                    {
                        if (!string.IsNullOrEmpty(bcsvm.ImageURL))
                        {
                            if (bcsvm.ImageSource is null)
                            {
                                try
                                {

                                    bcsvm.ImageIsError = false;
                                    bcsvm.ImageIsVisible = false;
                                    bcsvm.ImageSource = ImageSource.FromUri(new Uri(bcsvm.ImageURL));
                                    bcsvm.ImageIsVisible = true;
                                }
                                catch (Exception exp)
                                {
                                    bcsvm.ImageIsError = true;
                                    System.Diagnostics.Debug.WriteLine(exp.Message);
                                }
                            }
                        }
                    }
                    else
                    {
                        bcsvm.ImageSource = null;
                        bcsvm.ImageIsVisible = false;
                    }
                }
            }
            if (update)
            {
                ObservableCollection<BinContentGrouping> nlist = new ObservableCollection<BinContentGrouping>();
                foreach (BinContentGrouping bcg in SelectedBinContent)
                {
                    nlist.Add(bcg);
                }
                SelectedBinContent = nlist;
            }
        }

        private void FillBinInfo(BinViewModel bvm, List<BinInfo> bininfo)
        {
            if ((NotDisposed) && (bininfo.Count > 0))
            {
                ObservableCollection<BinInfoViewModel> nlist = new ObservableCollection<BinInfoViewModel>();
                foreach (BinInfo bvm1 in bininfo)
                {
                    nlist.Add(new BinInfoViewModel(bvm1, Navigation));
                }
                BinInfo = nlist;

                //LoadImages
                foreach (BinInfoViewModel bivm in BinInfo)
                {
                    if (!string.IsNullOrEmpty(bivm.ImageURL))
                    {
                        try
                        {
                            bivm.ImageSource = ImageSource.FromUri(new Uri(bivm.ImageURL));
                            bivm.ImageIsVisible = true;
                        }
                        catch (Exception exp)
                        {
                            System.Diagnostics.Debug.WriteLine(exp.Message);
                        }
                    }
                }
            }
        }

        private BinViewModel GetFirstSelected()
        {
            return BinViewModels.Find(x => x.IsSelected == true);
        }

        private bool FillTemplateIsEnabled = false;

        private void SetTemplateBySelectedBin(BinViewModel bvm)
        {
            FillTemplateIsEnabled = false;
            TemplateCode = bvm.Code;
            TemplateMaximumWeight = bvm.MaximumWeight;
            TemplateBlocked = bvm.Blocked;
            TemplateBlockMovement = bvm.BlockMovement;
            TemplateBinType = bvm.BinType;
            TemplateBinRanking = bvm.BinRanking;
            TemplateMaximumCubage = bvm.MaximumCubage;
            TemplateMaximumWeight = bvm.MaximumWeight;
            TemplateAdjustmentBin = bvm.AdjustmentBin;
            TemplateWarehouseClassCode = bvm.WarehouseClassCode;
            TemplateSpecialEquipmentCode = bvm.SpecialEquipmentCode;
            TemplateDefault = bvm.Default;
            FillTemplateIsEnabled = true;
        }

        private async void SetSelectedBinsByTemplate(bool codefieldchanged)
        {
            if (FillTemplateIsEnabled)
            {
                List<BinViewModel> selectedlist = BinViewModels.FindAll(x => x.IsSelected == true);
                foreach (BinViewModel bvm in selectedlist)
                {
                    bvm.Code = TemplateCode;
                    bvm.MaximumWeight = TemplateMaximumWeight;
                    bvm.Blocked = TemplateBlocked;
                    bvm.BlockMovement = TemplateBlockMovement;
                    bvm.BinType = TemplateBinType;
                    bvm.BinRanking = TemplateBinRanking;
                    bvm.MaximumCubage = TemplateMaximumCubage;
                    bvm.MaximumWeight = TemplateMaximumWeight;
                    bvm.AdjustmentBin = TemplateAdjustmentBin;
                    bvm.WarehouseClassCode = TemplateWarehouseClassCode;
                    bvm.SpecialEquipmentCode = TemplateSpecialEquipmentCode;
                    bvm.Default = TemplateDefault;
                }

                if (codefieldchanged)
                {
                    await CheckSelectedBins(ACD).ConfigureAwait(true);
                }
            }
        }

        public void BinViewModelsDispose()
        {
            foreach (BinViewModel bvm in BinViewModels)
            {
                bvm.DisposeModel();
            }
            BinViewModels.Clear();
        }

        public override void DisposeModel()
        {
            base.DisposeModel();
            BinViewModelsDispose();

            if (OnBinClick is Action<BinsViewModel>)
            {
                Delegate[] clientList = OnBinClick.GetInvocationList();
                foreach (var d in clientList)
                {
                    OnBinClick -= (d as Action<BinsViewModel>);
                }
            }
        }
    }
}
