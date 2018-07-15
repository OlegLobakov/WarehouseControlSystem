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
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.Model;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Plugin.Connectivity;
using WarehouseControlSystem.Helpers.NAV;
using WarehouseControlSystem.Resx;
using WarehouseControlSystem.View.Pages.RackScheme;
using System.Windows.Input;
using System.Threading;
using WarehouseControlSystem.View.Pages.RackScheme.MasterNewRack;

namespace WarehouseControlSystem.ViewModel
{
    public class RacksPlanViewModel : PlanBaseViewModel
    {
        public Zone Zone { get; set; }

        public RackViewModel SelectedRackViewModel { get; set; }
        public ObservableCollection<RackViewModel> RackViewModels
        {
            get { return rackviewmodels; }
            set
            {
                if (rackviewmodels != value)
                {
                    rackviewmodels = value;
                    OnPropertyChanged(nameof(RackViewModels));
                }
            }
        }  ObservableCollection<RackViewModel> rackviewmodels;

       
        public ICommand RackListCommand { protected set; get; }
        public ICommand NewRackCommand { protected set; get; }
        public ICommand EditRackCommand { protected set; get; }
        public ICommand DeleteRackCommand { protected set; get; }

     
        public ObservableCollection<UserDefinedSelectionViewModel> UserDefinedSelectionViewModels { get; set; }

        public bool IsVisibleUDS
        {
            get { return isvisibleUDS; }
            set
            {
                if (isvisibleUDS != value)
                {
                    isvisibleUDS = value;
                    OnPropertyChanged(nameof(IsVisibleUDS));
                }
            }
        } bool isvisibleUDS;

        public int UDSPanelHeight
        {
            get { return udspanelheight; }
            set
            {
                if (udspanelheight != value)
                {
                    udspanelheight = value;
                    OnPropertyChanged(nameof(UDSPanelHeight));
                }
            }
        } int udspanelheight;

        public RacksPlanViewModel(INavigation navigation, Zone zone) : base(navigation)
        {
            Zone = zone;
            RackViewModels = new ObservableCollection<RackViewModel>();
            UserDefinedSelectionViewModels = new ObservableCollection<UserDefinedSelectionViewModel>();

            RackListCommand = new Command(async () => await RackList().ConfigureAwait(false));
            NewRackCommand = new Command(async () => await NewRack().ConfigureAwait(false));
            EditRackCommand = new Command(EditRack);
            DeleteRackCommand = new Command(DeleteRack);

            PlanWidth = zone.PlanWidth;
            PlanHeight = zone.PlanHeight;

            if (PlanHeight == 0)
            {
                PlanHeight = Settings.DefaultZonePlanHeight;
            }

            if (PlanWidth == 0)
            {
                PlanWidth = Settings.DefaultZonePlanWidth;
            }

            State = ModelState.Undefined;
            IsEditMode = false;
        }

        public void ClearAll()
        {
            foreach (RackViewModel lvm in RackViewModels)
            {
                lvm.DisposeModel();
            }
            RackViewModels.Clear();
            SelectedRackViewModel = null;
        }

        public async Task Load()
        {
            if (NotNetOrConnection)
            {
                return;
            }

            try
            {
                State = ModelState.Loading;
                List<Rack> racks = await NAV.GetRackList(Zone.LocationCode, Zone.Code, true, 1, int.MaxValue, ACD.Default).ConfigureAwait(true);
                if ((!IsDisposed) && (racks is List<Rack>))
                {
                    FillModel(racks);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                State = ModelState.Error;
                ErrorText = AppResources.Error_LoadRacks;
            }
        }

        private void FillModel(List<Rack> racks)
        {
            if (racks.Count > 0)
            {
                RackViewModels.Clear();
                State = ModelState.Normal;
                foreach (Rack rack in racks)
                {
                    RackViewModel rvm = new RackViewModel(Navigation, rack, false);
                    rvm.OnTap += Rvm_OnTap;
                    RackViewModels.Add(rvm);
                }
                UpdateMinSizes();
                Rebuild(true);
            }
            else
            {
                State = ModelState.Error;
                ErrorText = "No Data";
            }
        }

        public async Task LoadUDS()
        {
            if (NotNetOrConnection)
            {
                return;
            }

            try
            {
                UserDefinedSelectionViewModels.Clear();
                List<UserDefinedSelection> list = await NAV.LoadUDS(Zone.LocationCode, Zone.Code, ACD.Default).ConfigureAwait(true);
                FillUDSList(list);
            }
            catch (OperationCanceledException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                State = ModelState.Error;
                ErrorText = e.ToString();
            }
        }
        private void FillUDSList(List<UserDefinedSelection> list)
        {
            if (list is List<UserDefinedSelection>)
            {
                foreach (UserDefinedSelection uds in list)
                {
                    UserDefinedSelectionViewModel udsvm = new UserDefinedSelectionViewModel(Navigation, uds)
                    {
                        UDSWidth = UDSPanelHeight,
                    };
                    udsvm.OnTap += RunUDS;

                    UserDefinedSelectionViewModels.Add(udsvm);
                }
                MessagingCenter.Send(this, "UDSListIsLoaded");
            }
        }

        private async void Rvm_OnTap(RackViewModel rvm)
        {
            if (!IsEditMode)
            {
                await Navigation.PushAsync(new RackCardPage(rvm));
            }
            else
            {
                foreach (RackViewModel rv in RackViewModels)
                {
                    if (rv != rvm)
                    {
                        rv.Selected = false;
                    }
                }
                rvm.Selected = !rvm.Selected;
                if (rvm.Selected)
                {
                    rvm.EditMode = SchemeElementEditMode.Move;
                }
            }
        }

        public async void RunUDS(UserDefinedSelectionViewModel udsvm)
        {
            if (NotNetOrConnection)
            {
                return;
            }

            try
            {
                if (udsvm.UDSIsRan)
                {
                    foreach (RackViewModel rvm in RackViewModels)
                    {
                        rvm.UDSSelects.RemoveAll(x => x.FunctionID == udsvm.ID);
                    }
                    MessagingCenter.Send(this, "UDSRunIsDone");
                    udsvm.UDSIsRan = false;
                }
                else
                {
                    udsvm.State = ModelState.Loading;
                    List<UserDefinedSelectionResult> list = await NAV.RunUDS(Zone.LocationCode, Zone.Code, udsvm.ID, ACD.Default).ConfigureAwait(true);
                    if (list is List<UserDefinedSelectionResult>)
                    {
                        foreach (RackViewModel rvm in RackViewModels)
                        {
                            rvm.UDSSelects.RemoveAll(x => x.FunctionID == udsvm.ID);
                        }
                        FillRackUDSR(list);
                    }
                    MessagingCenter.Send(this, "UDSRunIsDone");
                    udsvm.UDSIsRan = true;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                udsvm.UDSIsRan = false;
            }
            finally
            {
                LoadAnimation = false;
            }
            udsvm.State = ModelState.Normal;
        }

        private void FillRackUDSR(List<UserDefinedSelectionResult> list)
        {
            foreach (UserDefinedSelectionResult udsr in list)
            {
                RackViewModel rvm = RackViewModels.ToList().Find(x => x.No == udsr.RackNo);
                if (rvm is RackViewModel)
                {
                    SubSchemeSelect sss = new SubSchemeSelect()
                    {
                        FunctionID = udsr.FunctionID,
                        Section = udsr.Section,
                        Level = udsr.Level,
                        Depth = udsr.Depth,
                        Value = udsr.Value,
                        HexColor = udsr.HexColor
                    };
                    rvm.UDSSelects.Add(sss);
                }
            }
        }

        public override void Rebuild(bool recreate)
        {
            if ((!CanRebuildInterface) || (RackViewModels.Count == 0))
            {
                return;
            }

            foreach (RackViewModel rvm in RackViewModels)
            {
                rvm.ViewLeft = rvm.Left * WidthStep;
                rvm.ViewTop = rvm.Top * HeightStep;
                rvm.ViewWidth = rvm.Width * WidthStep;
                rvm.ViewHeight = rvm.Height * HeightStep;
            }

            if (recreate)
            {
                MessagingCenter.Send(this, "Rebuild");
            }
            else
            {
                MessagingCenter.Send(this, "Reshape");
            }
        }

        public void UnSelectAll()
        {
            foreach (RackViewModel rvm in RackViewModels)
            {
                rvm.Selected = false;
            }
        }

        public async Task RackList()
        {
            RackListPage rlp = new RackListPage(Zone);
            await Navigation.PushAsync(rlp);
        }

        public async Task NewRack()
        {
            Rack newrack = new Rack
            {
                Sections = Settings.DefaultRackSections,
                Levels = Settings.DefaultRackLevels,
                Depth = Settings.DefaultRackDepth,
                SchemeVisible = true,
            };

            RackViewModel rvm = new RackViewModel(Navigation, newrack, true)
            {
                LocationCode = Zone.LocationCode,
                ZoneCode = Zone.Code,
            };

            MasterRackNewViewModel mrnvm = new MasterRackNewViewModel(rvm, true);
            MasterNewRackPage mnrp = new MasterNewRackPage(mrnvm);

            await Navigation.PushAsync(mnrp);
        }

        public void EditRack(object obj)
        {
            System.Diagnostics.Debug.WriteLine(obj.ToString());
        }

        public void DeleteRack(object obj)
        {
            System.Diagnostics.Debug.WriteLine(obj.ToString());
        }

        public async Task SaveZoneParams()
        {
            if (NotNetOrConnection)
            {
                return;
            }
            Zone.PlanWidth = PlanWidth;
            Zone.PlanHeight = PlanHeight;
            await NAV.ModifyZone(Zone, ACD.Default).ConfigureAwait(true);
        }

        public override async void SaveChangesAsync()
        {
            if (NotNetOrConnection)
            {
                return;
            }

            List<RackViewModel> list = RackViewModels.ToList().FindAll(x => x.Selected == true);
            foreach (RackViewModel rvm in list)
            {
                try
                {
                    Rack rack = new Rack();
                    rvm.SaveFields(rack);
                    await NAV.ModifyRack(rack, ACD.Default).ConfigureAwait(true);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    ErrorText = e.Message;
                }
            }

            UpdateMinSizes();
        }

        public void UpdateMinSizes()
        {
            int newminplanwidth = 0;
            int newminplanheight = 0;
            foreach (RackViewModel rvm in RackViewModels)
            {
                if ((rvm.Left + rvm.Width) > newminplanwidth)
                {
                    newminplanwidth = rvm.Left + rvm.Width;
                }
                if ((rvm.Top + rvm.Height) > newminplanheight)
                {
                    newminplanheight = rvm.Top + rvm.Height;
                }
            }

            MinPlanWidth = newminplanwidth;
            MinPlanHeight = newminplanheight;
            if (PlanWidth < MinPlanWidth)
            {
                PlanWidth = MinPlanWidth;
            }
            if (PlanHeight < MinPlanHeight)
            {
                PlanHeight = MinPlanHeight;
            }
        }

        public void SetEditModeForItems(bool iseditmode)
        {
            foreach (RackViewModel rvm in RackViewModels)
            {
                rvm.IsEditMode = iseditmode;
            }
        }

        public override void DisposeModel()
        {
            ClearAll();
            UserDefinedSelectionViewModels.Clear();
            base.DisposeModel();
        }
    }
}
