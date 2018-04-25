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
        public List<BinViewModel> BinViewModels { get; set; } = new List<BinViewModel>();

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

        public BinTemplate BinTemplate
        {
            get { return bintemplate; }
            set
            {
                if (bintemplate != value)
                {
                    bintemplate = value;
                    ChangeBinTemplate();
                    OnPropertyChanged(nameof(BinTemplate));
                }
            }
        } BinTemplate bintemplate;

        public ICommand BlockBinsCommand { protected set; get; }
        public ICommand CombineBinsCommand { protected set; get; }
        public ICommand DeleteBinsCommand { protected set; get; }
        public ICommand ShowBinOperationCommand { protected set; get; }

        public event Action<BinsViewModel> OnBinClick;

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

        /// <summary>
        /// For editing bins
        /// </summary>
        public string Code
        {
            get
            {
                string rv = "";
                BinViewModel bvm = BinViewModels.Find(x => x.Selected == true);
                if (bvm is BinViewModel)
                {
                    rv = bvm.Code;
                }
                return rv;
            }
            set
            {
                BinViewModel selectedbvm = BinViewModels.Find(x => x.Selected == true);
                if (selectedbvm is BinViewModel)
                {
                    selectedbvm.Code = value;
                }
                OnPropertyChanged(nameof(Code));
            }
        }
        public bool Blocked
        {
            get
            {
                bool rv = false;
                BinViewModel bvm = BinViewModels.Find(x => x.Selected == true);
                if (bvm is BinViewModel)
                {
                    rv = bvm.Blocked;
                }
                return rv;
            }
            set
            {
                List<BinViewModel> selectedlist = BinViewModels.FindAll(x => x.Selected == true);
                foreach (BinViewModel bvm in selectedlist)
                {
                    bvm.Blocked = value;
                }
                OnPropertyChanged(nameof(Blocked));
            }
        }
        public int BlockMovement
        {
            get
            {
                int rv = 0;
                BinViewModel bvm = BinViewModels.Find(x => x.Selected == true);
                if (bvm is BinViewModel)
                {
                    rv = bvm.BlockMovement;
                }
                return rv;
            }
            set
            {
                List<BinViewModel> selectedlist = BinViewModels.FindAll(x => x.Selected == true);
                foreach (BinViewModel bvm in selectedlist)
                {
                    bvm.BlockMovement = value;
                }
                OnPropertyChanged("BlockMovement");
            }
        }
        public string BinType
        {
            get
            {
                string rv = "";
                BinViewModel bvm = BinViewModels.Find(x => x.Selected == true);
                if (bvm is BinViewModel)
                {
                    rv = bvm.BinType;
                }
                return rv;
            }
            set
            {
                List<BinViewModel> selectedlist = BinViewModels.FindAll(x => x.Selected == true);
                foreach (BinViewModel bvm in selectedlist)
                {
                    bvm.BinType = value;
                }
                OnPropertyChanged("BinType");
            }
        }
        public int BinRanking
        {
            get
            {
                int rv = 0;
                BinViewModel bvm = BinViewModels.Find(x => x.Selected == true);
                if (bvm is BinViewModel)
                {
                    rv = bvm.BinRanking;
                }
                return rv;
            }
            set
            {
                List<BinViewModel> selectedlist = BinViewModels.FindAll(x => x.Selected == true);
                foreach (BinViewModel bvm in selectedlist)
                {
                    bvm.BinRanking = value;
                }
                OnPropertyChanged("BinRanking");
            }
        }
        public decimal MaximumCubage
        {
            get
            {
                decimal rv = 0;
                BinViewModel bvm = BinViewModels.Find(x => x.Selected == true);
                if (bvm is BinViewModel)
                {
                    rv = bvm.MaximumCubage;
                }
                return rv;
            }
            set
            {
                List<BinViewModel> selectedlist = BinViewModels.FindAll(x => x.Selected == true);
                foreach (BinViewModel bvm in selectedlist)
                {
                    bvm.MaximumCubage = value;
                }
                OnPropertyChanged("MaximumCubage");
            }
        }
        public decimal MaximumWeight
        {
            get
            {
                decimal rv = 0;
                BinViewModel bvm = BinViewModels.Find(x => x.Selected == true);
                if (bvm is BinViewModel)
                {
                    rv = bvm.MaximumWeight;
                }
                return rv;
            }
            set
            {
                List<BinViewModel> selectedlist = BinViewModels.FindAll(x => x.Selected == true);
                foreach (BinViewModel bvm in selectedlist)
                {
                    bvm.MaximumWeight = value;
                }
                OnPropertyChanged("MaximumWeight");
            }
        }
        public bool AdjustmentBin
        {
            get
            {
                bool rv = false;
                BinViewModel bvm = BinViewModels.Find(x => x.Selected == true);
                if (bvm is BinViewModel)
                {
                    rv = bvm.AdjustmentBin;
                }
                return rv;
            }
            set
            {
                List<BinViewModel> selectedlist = BinViewModels.FindAll(x => x.Selected == true);
                foreach (BinViewModel bvm in selectedlist)
                {
                    bvm.AdjustmentBin = value;
                }
                OnPropertyChanged("AdjustmentBin");
            }
        }
        public string WarehouseClassCode
        {
            get
            {
                string rv = "";
                BinViewModel bvm = BinViewModels.Find(x => x.Selected == true);
                if (bvm is BinViewModel)
                {
                    rv = bvm.WarehouseClassCode;
                }
                return rv;
            }
            set
            {
                List<BinViewModel> selectedlist = BinViewModels.FindAll(x => x.Selected == true);
                foreach (BinViewModel bvm in selectedlist)
                {
                    bvm.WarehouseClassCode = value;
                }
                OnPropertyChanged("WarehouseClassCode");
            }
        }
        public string SpecialEquipmentCode
        {
            get
            {
                string rv = "";
                BinViewModel bvm = BinViewModels.Find(x => x.Selected == true);
                if (bvm is BinViewModel)
                {
                    rv = bvm.SpecialEquipmentCode;
                }
                return rv;
            }
            set
            {
                List<BinViewModel> selectedlist = BinViewModels.FindAll(x => x.Selected == true);
                foreach (BinViewModel bvm in selectedlist)
                {
                    bvm.SpecialEquipmentCode = value;
                }
                OnPropertyChanged("SpecialEquipmentCode");
            }
        }
        public bool Default
        {
            get
            {
                bool rv = false;
                BinViewModel bvm = BinViewModels.Find(x => x.Selected == true);
                if (bvm is BinViewModel)
                {
                    rv = bvm.Default;
                }
                return rv;
            }
            set
            {
                List<BinViewModel> selectedlist = BinViewModels.FindAll(x => x.Selected == true);
                foreach (BinViewModel bvm in selectedlist)
                {
                    bvm.Default = value;
                }
                OnPropertyChanged("Default");
            }
        }

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

        public BinsViewModel(INavigation navigation) : base(navigation)
        {
            BlockBinsCommand = new Command(BlockBins);
            CombineBinsCommand = new Command(CombineBins);
            DeleteBinsCommand = new Command(DeleteBins);
            ShowBinOperationCommand = new Command(ShowBinOperations);
        }

        public void RecreateBins(int prevdepth, int newdepth, int prevlevels, int newlevels, int prevsections, int newsections)
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
            string combinedrackno = "";
            string locationcode = "";
            string rackno = "";
            List<BinViewModel> selectedlist = BinViewModels.FindAll(x => x.Selected == true);
            if (selectedlist.Count > 1)
            {
                int leftsection = int.MaxValue;
                int leftlevel = int.MaxValue;
                int rightsection = 0;
                int rightlevel = 0;
                foreach (BinViewModel bvm1 in selectedlist)
                {
                    combinedrackno = bvm1.Code;
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
                    combinedrackno = firstbvm.Code;
                    locationcode = firstbvm.LocationCode;
                    rackno = firstbvm.RackNo;
                }

                foreach (BinViewModel bvm2 in deleted)
                {

                    DeleteBin(bvm2);
                }

                Bin newbin = new Bin()
                {
                    LocationCode = locationcode,
                    RackNo = rackno,
                    Code = combinedrackno,
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
            catch (OperationCanceledException ex)
            {
                ErrorText = ex.Message;
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
        }

        public async void LoadBins(AsyncCancelationDispatcher acd)
        {
            BinViewModelsDispose();
            try
            {
                LoadedBinsQuantity = 0;
                SearchBinsQuantity = 0;
                List<Bin> bins = await NAV.GetBinList(LocationCode, ZoneCode, RackNo, "", 1, int.MaxValue, ACD.Default);
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
                                    x=>x.ZoneCode == ZoneCode &&
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
            catch (OperationCanceledException ex)
            {
                ErrorText = ex.Message;
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
        }

        public async void LoadUDF(AsyncCancelationDispatcher acd)
        {
            try
            {
                UserDefinedFunctions.Clear();
                List<UserDefinedFunction> list = await NAV.LoadUserDefinedFunctionList(LocationCode, ZoneCode,RackNo, ACD.Default);
                if (list is List<UserDefinedFunction>)
                {
                    foreach (UserDefinedFunction udf in list)
                    {
                        UserDefinedFunctionViewModel udfvm = new UserDefinedFunctionViewModel(Navigation,udf);
                        UserDefinedFunctions.Add(udfvm);
                    }
                }

            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine("Cancel LoadUDF", e.Message);
            }
            catch (Exception ex)
            {
                State = Helpers.Containers.StateContainer.State.Error;
                ErrorText = ex.ToString();
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
                    List<BinContent> bincontent = await NAV.GetBinContentList(LocationCode, ZoneCode, bvm.Code, "", "", 1, int.MaxValue, ACD.Default);
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
                    Console.WriteLine("Cancel LoadContent", e.Message);
                }
                catch
                {
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

            Code = Code;
            Blocked = Blocked;
            BlockMovement = BlockMovement;
            BinType = BinType;
            BinRanking = BinRanking;
            MaximumCubage = MaximumCubage;
            MaximumWeight = MaximumWeight;
            AdjustmentBin = AdjustmentBin;
            WarehouseClassCode = WarehouseClassCode;
            SpecialEquipmentCode = SpecialEquipmentCode;
            Default = Default;


            if (OnBinClick is Action<BinsViewModel>)
            {
                OnBinClick(this);
            }
        }

        public void BinViewModelsDispose()
        {
            foreach (BinViewModel bvm in BinViewModels)
            {
                bvm.Dispose();
            }
            BinViewModels.Clear();
        }

        public override void Dispose()
        {
            BlockBinsCommand = null;
            CombineBinsCommand = null;
            DeleteBinsCommand = null;
            ShowBinOperationCommand = null;

            if (OnBinClick is Action<BinsViewModel>)
            {
                Delegate[] clientList = OnBinClick.GetInvocationList();
                foreach (var d in clientList)
                {
                    OnBinClick -= (d as Action<BinsViewModel>);
                }
            }

            BinViewModelsDispose();
            BinViewModels = null;

            SelectedBinContent.Clear();
            UserDefinedFunctions.Clear();
            BinTypes.Clear();
            SpecialEquipments.Clear();
            WarehouseClasses.Clear();

            SelectedBinContent = null;
            UserDefinedFunctions = null;
            BinTypes = null;
            SpecialEquipments = null;
            WarehouseClasses = null;
            base.Dispose();
        }
    }
}
