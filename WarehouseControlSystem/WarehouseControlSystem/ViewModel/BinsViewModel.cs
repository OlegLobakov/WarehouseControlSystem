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

namespace WarehouseControlSystem.ViewModel
{
    public class BinsViewModel : BaseViewModel
    {
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
        public string RackNo
        {
            get { return rackno; }
            set
            {
                if (rackno != value)
                {
                    rackno = value;
                    foreach (BinViewModel bvm in BinViewModels)
                    {
                        bvm.RackNo = rackno;
                    }
                    OnPropertyChanged(nameof(RackNo));
                }
            }
        } string rackno;
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

        public ICommand BlockBinsCommand { protected set; get; }
        public ICommand CombineBinsCommand { protected set; get; }
        public ICommand DeleteBinsCommand { protected set; get; }
        public ICommand ShowBinOperationCommand { protected set; get; }

        public ICommand ContentViewCommand { protected set; get; }
        public ICommand FunctionsViewCommand { protected set; get; }

        public event Action<BinsViewModel> OnBinClick;

        public List<BinViewModel> BinViewModels { get; set; } = new List<BinViewModel>();

        public ObservableCollection<BinContentShortViewModel> SelectedBinContent { get; set; } = new ObservableCollection<BinContentShortViewModel>();
        public ObservableCollection<UserDefinedFunctionViewModel> UserDefinedFunctions { get; set; } = new ObservableCollection<UserDefinedFunctionViewModel>();

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
                    SetSelectedBinsByTemplate();
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
                    SetSelectedBinsByTemplate();
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
                    SetSelectedBinsByTemplate();
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
                    SetSelectedBinsByTemplate();
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
                    SetSelectedBinsByTemplate();
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
                    SetSelectedBinsByTemplate();
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
                    SetSelectedBinsByTemplate();
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
                    SetSelectedBinsByTemplate();
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
                    SetSelectedBinsByTemplate();
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
                    SetSelectedBinsByTemplate();
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
                    SetSelectedBinsByTemplate();
                    OnPropertyChanged(nameof(TemplateDefault));
                }
            }
        } bool templatedefault1;


        public BinsViewModel(INavigation navigation) : base(navigation)
        {
            BlockBinsCommand = new Command(BlockBins);
            CombineBinsCommand = new Command(CombineBins);
            DeleteBinsCommand = new Command(DeleteBins);
            ShowBinOperationCommand = new Command(ShowBinOperations);

            ContentViewCommand = new Command(ShowContent);
            FunctionsViewCommand = new Command(ShowFunctions);

            State = ModelState.Undefined;
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

        public void CreateBin(int i, int j, int k)
        {
            Bin newbin = new Bin()
            {
                RackNo = RackNo,
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
        }

        private void DeleteBin(BinViewModel bvm)
        {
            bvm.OnTap -= Bvm_OnTap;
            BinViewModels.Remove(bvm);
        }

        public void BlockBins()
        {
            List<BinViewModel> selectedlist = BinViewModels.FindAll(x => x.Selected == true);
            foreach (BinViewModel bvm in selectedlist)
            {
                bvm.Blocked = !bvm.Blocked;
            }
            UnSelect();

            MessagingCenter.Send(this, "Update");
        }

        public void CombineBins()
        {
            string combinedrackno1 = "";
            string locationcode1 = "";
            string rackno1 = "";
            List<BinViewModel> selectedlist = BinViewModels.FindAll(x => x.Selected == true);
            if (selectedlist.Count > 1)
            {
                int leftsection = int.MaxValue;
                int leftlevel = int.MaxValue;
                int rightsection = 0;
                int rightlevel = 0;
                foreach (BinViewModel bvm1 in selectedlist)
                {
                    combinedrackno1 = bvm1.Code;
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

                List<BinViewModel> deleted =
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
                    rackno1 = firstbvm.RackNo;
                }

                foreach (BinViewModel bvm2 in deleted)
                {
                    DeleteBin(bvm2);
                }

                Bin newbin = new Bin()
                {
                    LocationCode = locationcode1,
                    RackNo = rackno1,
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

        public void DeleteBins()
        {
            List<BinViewModel> selectedlist = BinViewModels.FindAll(x => x.Selected == true);
            foreach (BinViewModel bvm in selectedlist)
            {
                DeleteBin(bvm);
            }
            MessagingCenter.Send(this, "Update");
        }

        public void ShowBinOperations()
        {
            Global.CompliantPlug = "";
        }

        public void ShowContent()
        {
            IsUserDefinedCommandsVisible = false;
        }

        public void ShowFunctions()
        {
            IsUserDefinedCommandsVisible = true;
        }

        public async void CheckBins(AsyncCancelationDispatcher acd)
        {
            try
            {
                List<BinViewModel> list = BinViewModels.ToList();
                foreach (BinViewModel bvm in list)
                {
                    bvm.IsChecked = false;
                    List<Bin> binsexist = await NAV.GetBinList(LocationCode, "", "", bvm.Code, 1, int.MaxValue, ACD.Default);
                    if (binsexist.Count > 0)
                    {
                        bvm.IsExist = true;
                        Bin bin = binsexist.First();

                        //Place in new rack
                        bin.RackNo = bvm.RackNo;
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
        public async void LoadBins(AsyncCancelationDispatcher acd)
        {
            BinViewModelsDispose();
            try
            {
                LoadedBinsQuantity = 0;
                SearchBinsQuantity = 0;
                List<Bin> bins = await NAV.GetBinList(LocationCode, ZoneCode, RackNo, "", 1, int.MaxValue, ACD.Default).ConfigureAwait(true);
                if (!IsDisposed)
                {
                    if (bins.Count > 0)
                    {
                        LoadedBinsQuantity = bins.Count;
                        foreach (Bin bin in bins)
                        {
                            BinViewModel bvm = new BinViewModel(Navigation, bin);
                            bvm.IsContent = !bin.Empty;
                            bvm.OnTap += Bvm_OnTap;
                        
                            if (!string.IsNullOrEmpty(Global.SearchRequest))
                            {
                                bvm.ExcludeFromSearch = true;

                                List<SearchResponse> list = Global.SearchResponses.FindAll(
                                    x => 
                                    x.ZoneCode == ZoneCode &&
                                    x.RackNo == RackNo &&
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
        public async void LoadUDF(AsyncCancelationDispatcher acd)
        {
            try
            {
                UserDefinedFunctions.Clear();
                List<UserDefinedFunction> list = await NAV.LoadUserDefinedFunctionList(LocationCode, ZoneCode, RackNo, ACD.Default).ConfigureAwait(true);
                if (list is List<UserDefinedFunction>)
                {
                    foreach (UserDefinedFunction udf in list)
                    {
                        UserDefinedFunctionViewModel udfvm = new UserDefinedFunctionViewModel(Navigation, udf);
                        UserDefinedFunctions.Add(udfvm);
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
            List<BinViewModel> selectedlist = BinViewModels.FindAll(x => x.Selected == true);
            foreach (BinViewModel bvm in selectedlist)
            {
                bvm.Selected = false;
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

        private async void Bvm_OnTap(BinViewModel bvm)
        {
            bvm.Selected = !bvm.Selected;

            BinViewModel selectedbvm = BinViewModels.Find(x => x.Selected == true);
            IsSelectedBins = selectedbvm is BinViewModel;
        
            if (bvm.IsContent)
            {
                bvm.LoadAnimation = true;
                try
                {
                    List<BinContent> bincontent = await NAV.GetBinContentList(LocationCode, ZoneCode, bvm.Code, "", "", 1, int.MaxValue, ACD.Default).ConfigureAwait(true);
                    if ((!IsDisposed) && (bincontent.Count > 0))
                    {
                        bvm.BinContent.Clear();
                        foreach (BinContent bc in bincontent)
                        {
                            BinContentShortViewModel bsvm = new BinContentShortViewModel(Navigation, bc);
                            bvm.BinContent.Add(bsvm);
                        }
                    }
                }
                catch (OperationCanceledException e)
                {
                    System.Diagnostics.Debug.WriteLine("Cancel LoadBinContent", e.Message);
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
                bvm.LoadAnimation = false;
            }

            SelectedBinContent.Clear();
            List<BinViewModel> list = BinViewModels.FindAll(x => x.Selected == true);
            if (list is List<BinViewModel>)
            {
                foreach (BinViewModel bvm1 in list)
                {
                    foreach (BinContentShortViewModel bcsvm in bvm1.BinContent)
                    {
                        SelectedBinContent.Add(bcsvm);
                    }
                }
                EditedBinCodeIsEnabled = list.Count == 1;
            }
            else
            {
                EditedBinCodeIsEnabled = false;
            }

            if (OnBinClick is Action<BinsViewModel>)
            {
                OnBinClick(this);
            }
        }

        private BinViewModel GetFirstSelected()
        {
            return BinViewModels.Find(x => x.Selected == true);
        }

        private void SetSelectedBinsByTemplate()
        {
            List<BinViewModel> selectedlist = BinViewModels.FindAll(x => x.Selected == true);
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
            BinViewModelsDispose();

            if (OnBinClick is Action<BinsViewModel>)
            {
                Delegate[] clientList = OnBinClick.GetInvocationList();
                foreach (var d in clientList)
                {
                    OnBinClick -= (d as Action<BinsViewModel>);
                }
            }

            base.DisposeModel();
        }
    }
}
