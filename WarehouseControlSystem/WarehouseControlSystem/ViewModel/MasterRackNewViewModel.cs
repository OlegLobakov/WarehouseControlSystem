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

        public MasterRackNewViewModel(RackViewModel rvm) : base(rvm.Navigation)
        {
            NewModel = rvm;          
            Step1Command = new Command(Step1);
            Step2Command = new Command(Step2);
            Step3Command = new Command(async () => await Step3().ConfigureAwait(true));
            State = ModelState.Undefined;
        }

        public async Task Load()
        {
            try
            {
                await LoadBinTemplates().ConfigureAwait(true);
                await LoadBinTypesList().ConfigureAwait(true);
                await LoadWarehouseClassesList().ConfigureAwait(true);
                await LoadSpecialEquipmentsList().ConfigureAwait(true);
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
                if (NotDisposed)
                {
                    BinTemplates.Clear();
                    foreach (BinTemplate bt in bintemplates)
                    {
                        bool selected = true;
                        if (!string.IsNullOrEmpty(NewModel.LocationCode))
                        {
                            if (bt.LocationCode != NewModel.LocationCode)
                            {
                                selected = false;
                            }
                        }

                        if (!string.IsNullOrEmpty(NewModel.ZoneCode))
                        {
                            if (bt.ZoneCode != NewModel.ZoneCode)
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
            if (NotDisposed)
            {
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
            if (NotDisposed)
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
            if (NotDisposed)
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

            if (NewModel.RackOrientation == RackOrientationEnum.Undefined)
            {
                InfoText = AppResources.MasterNewRack_ControlRackOrientation;
                check = false;
            }

            if (string.IsNullOrEmpty(NewModel.No))
            {
                InfoText = AppResources.MasterNewRack_ControlNo;
                check = false;
            }

            if (check)
            {
                NewModel.BinsViewModel.BinTemplate = SelectedBinTemplate;
                NewModel.BinTemplateCode = SelectedBinTemplate.Code;
                NewModel.CreateBins();
                NewModel.NumberingPrefix = NewModel.No;
                NewModel.IsNumberingEnabled = true;
                NewModel.Renumbering();
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
            if ((NewModel.LocationCode != "") &&
                (NewModel.ZoneCode != "") &&
                (NewModel.No != ""))
            {
                int exist = await NAV.GetRackCount(NewModel.LocationCode, NewModel.ZoneCode, NewModel.No, false, ACD.Default).ConfigureAwait(true);
                if (NotDisposed)
                {
                    if (exist > 0)
                    {
                        InfoText = AppResources.RackNewPage_CodeAlreadyExist;
                    }
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
                int rackid = await NAV.CreateRack(newrack, ACD.Default).ConfigureAwait(true);
                NewModel.ID = rackid;
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

                int binexist = await NAV.GetBinCount(NewModel.LocationCode, "", "", bmv.Bin.Code, ACD.Default).ConfigureAwait(true);
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



        public override void DisposeModel()
        {
            NewModel.DisposeModel();
            BinTemplates.Clear();
            base.DisposeModel();
        }
    }
}
