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
using WarehouseControlSystem.Resx;
using WarehouseControlSystem.ViewModel.Base;
using Xamarin.Forms;
using System.Windows.Input;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.Helpers.NAV;
using System.Threading;

namespace WarehouseControlSystem.ViewModel
{
    public class BinTemplateViewModel : NAVBaseViewModel
    {
        public BinTemplate BinTemplate { get; set; }

        public string LocationCode
        {
            get { return locationcode; }
            set
            {
                if (locationcode != value)
                {
                    locationcode = value;
                    OnPropertyChanged(nameof(LocationCode));
                }
            }
        }
        string locationcode;

        public string ZoneCode
        {
            get { return zonecode; }
            set
            {
                if (zonecode != value)
                {
                    zonecode = value;
                    OnPropertyChanged(nameof(ZoneCode));
                }
            }
        }
        string zonecode;

        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged(nameof(description));
                }
            }
        } string description;

        public string BinTypeCode
        {
            get { return bintypecode; }
            set
            {
                if (bintypecode != value)
                {
                    bintypecode = value;
                    OnPropertyChanged(nameof(BinTypeCode));
                }
            }
        } string bintypecode;

        public string BinDescription
        {
            get { return bindescription; }
            set
            {
                if (bindescription != value)
                {
                    bindescription = value;
                    OnPropertyChanged(nameof(BinDescription));
                }
            }
        } string bindescription;

        public string WarehouseClassCode
        {
            get { return warehouseclasscode; }
            set
            {
                if (warehouseclasscode != value)
                {
                    warehouseclasscode = value;
                    OnPropertyChanged(nameof(WarehouseClassCode));
                }
            }
        } string warehouseclasscode;

        public string SpecialEquipmentCode
        {
            get { return specialequipmentcode; }
            set
            {
                if (specialequipmentcode != value)
                {
                    specialequipmentcode = value;
                    OnPropertyChanged(nameof(SpecialEquipmentCode));
                }
            }
        } string specialequipmentcode;

        public int BinRanking
        {
            get { return binranking; }
            set
            {
                if (binranking != value)
                {
                    binranking = value;
                    OnPropertyChanged(nameof(BinRanking));
                }
            }
        } int binranking;

        public decimal MaximumCubage
        {
            get { return maximumcubage; }
            set
            {
                if (maximumcubage != value)
                {
                    maximumcubage = value;
                    OnPropertyChanged(nameof(MaximumCubage));
                }
            }
        } decimal maximumcubage;
        public decimal MaximumWeight
        {
            get { return maximumweight; }
            set
            {
                if (maximumweight != value)
                {
                    maximumweight = value;
                    OnPropertyChanged(nameof(MaximumWeight));
                }
            }
        } decimal maximumweight;

        public bool Dedicated
        {
            get { return dedicated; }
            set
            {
                if (dedicated != value)
                {
                    dedicated = value;
                    OnPropertyChanged(nameof(Dedicated));
                }
            }
        } bool dedicated;
      
        public List<BinType> BinTypes { get; set; }
        public List<Location> Locations { get; set; }
        public List<Zone> Zones { get; set; }

        public Location SelectedLocation
        {
            get { return selectedlocation; }
            set
            {
                if (selectedlocation != value)
                {
                    selectedlocation = value;
                    OnPropertyChanged(nameof(SelectedLocation));
                }
            }
        } Location selectedlocation;
        public Zone SelectedZone
        {
            get { return selectedzone; }
            set
            {
                if (selectedzone != value)
                {
                    selectedzone = value;
                    OnPropertyChanged(nameof(SelectedZone));
                }
            }
        } Zone selectedzone;
        public BinType SelectedBinType
        {
            get { return selectedbintype; }
            set
            {
                if (selectedbintype != value)
                {
                    selectedbintype = value;
                    OnPropertyChanged(nameof(SelectedBinType));
                }
            }
        } BinType selectedbintype;

        public BinTemplateViewModel(INavigation navigation, BinTemplate bintemplate) : base(navigation)
        {
            FillFields(bintemplate);
            OKCommand = new Command(async () => await OK().ConfigureAwait(false));
            CancelCommand = new Command(async () => await Cancel().ConfigureAwait(false));
            CancelChangesCommand = new Command(CancelChanges);

            BinTypes = new List<BinType>();
            Locations = new List<Location>();
            Zones = new List<Zone>();
            State = ModelState.Undefined;
        }

        public void FillFields(BinTemplate bintemplate)
        {
            Code = bintemplate.Code;
            Description = bintemplate.Description;
            LocationCode = bintemplate.LocationCode;
            ZoneCode = bintemplate.ZoneCode;
            BinTypeCode = bintemplate.BinTypeCode;
            BinDescription = bintemplate.BinDescription;
            WarehouseClassCode = bintemplate.WarehouseClassCode;
            SpecialEquipmentCode = bintemplate.SpecialEquipmentCode;
            BinRanking = bintemplate.BinRanking;
            MaximumCubage = bintemplate.MaximumCubage;
            MaximumWeight = bintemplate.MaximumWeight;
            Dedicated = bintemplate.Dedicated;
        }

        public void SaveFields(BinTemplate bintemplate)
        {
            bintemplate.Code = Code;
            bintemplate.Description = Description;
            bintemplate.LocationCode = LocationCode;
            bintemplate.ZoneCode = ZoneCode;
            bintemplate.BinTypeCode = BinTypeCode;
            bintemplate.BinDescription = BinDescription;
            bintemplate.WarehouseClassCode = WarehouseClassCode;
            bintemplate.SpecialEquipmentCode = SpecialEquipmentCode;
            bintemplate.BinRanking = BinRanking;
            bintemplate.MaximumCubage = MaximumCubage;
            bintemplate.MaximumWeight = MaximumWeight;
            bintemplate.Dedicated = Dedicated;
        }

        public async Task OK()
        {

            if (NotNetOrConnection)
            {
                return;
            }

            if (CheckFields())
            {
                SaveFields(BinTemplate);

                try
                {
                    if (CreateMode)
                    {
                        await NAV.CreateBinTemplate(BinTemplate, ACD.Default).ConfigureAwait(true);
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await NAV.ModifyBinTemplate(BinTemplate, ACD.Default).ConfigureAwait(true);
                        await Navigation.PopAsync();
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    State = ModelState.Error;
                    ErrorText = e.Message;
                }
            }
        }

        public async Task Cancel()
        {
            await Navigation.PopAsync();
        }

        public void CancelChanges()
        {
            FillFields(BinTemplate);
        }

        public async Task Update()
        {
            if (NotNetOrConnection)
            {
                return;
            }

            try
            {
                State = ModelState.Loading;
                await LoadBinTypesList().ConfigureAwait(true);
                await LoadLocationsList().ConfigureAwait(true);

                if (LocationCode != "")
                {
                    await LoadZonesList().ConfigureAwait(true);
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
            State = ModelState.Normal;
        }

        private async Task LoadBinTypesList()
        {
            List<BinType> loadedbintypes = await NAV.GetBinTypeList(1, int.MaxValue, ACD.Default).ConfigureAwait(true);
            if ((!IsDisposed) && (loadedbintypes is List<BinType>))
            {
                BinTypes.Clear();
                foreach (BinType bintype in loadedbintypes)
                {
                    BinTypes.Add(bintype);
                }
                BinType finded = BinTypes.Find(x => x.Code == BinTypeCode);
                if (finded is BinType)
                {
                    SelectedBinType = finded;
                }
            }
        }

        private async Task LoadLocationsList()
        {
            List<Location> loadedlocations = await NAV.GetLocationList("", false, 1, int.MaxValue, ACD.Default).ConfigureAwait(true);
            if ((!IsDisposed) && (loadedlocations is List<Location>))
            {
                Locations.Clear();
                foreach (Location location in loadedlocations)
                {
                    Locations.Add(location);
                }
                Location finded = Locations.Find(x => x.Code == LocationCode);
                if (finded is Location)
                {
                    SelectedLocation = finded;
                }
            }
        }

        private async Task LoadZonesList()
        {
            List<Zone> list = await NAV.GetZoneList(LocationCode, "", false, 1, int.MaxValue, ACD.Default).ConfigureAwait(true);
            if ((!IsDisposed) && (list is List<Zone>))
            {
                Zones.Clear();
                foreach (Zone zone in list)
                {
                    Zones.Add(zone);
                }
                Zone finded = Zones.Find(x => x.Code == ZoneCode);
                if (finded is Zone)
                {
                    SelectedZone = finded;
                }
            }
        }
        public async Task UpdateLocation(Location location)
        {
            if (NotNetOrConnection)
            {
                return;
            }

            if (LocationCode != location.Code)
            {
                LocationCode = location.Code;
                ZoneCode = "";
                try
                {
                    List<Zone> list = await NAV.GetZoneList(location.Code, "", false, 1, int.MaxValue, ACD.Default).ConfigureAwait(true);
                    if (!IsDisposed)
                    {
                        if (list is List<Zone>)
                        {
                            Zones.Clear();
                            foreach (Zone zone in list)
                            {
                                Zones.Add(zone);
                            }
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
        }

        private bool CheckFields()
        {
            string error = "";
            if (Code == "")
            {
                error = AppResources.NewBinTemplatePage_CodeIsEmpty + Environment.NewLine;
            }

            if (LocationCode == "")
            {
                error += AppResources.NewBinTemplatePage_LocationCodeIsEmpty + Environment.NewLine;
            }

            if (ZoneCode == "")
            {
                error += AppResources.NewBinTemplatePage_ZoneCodeIsEmpty + Environment.NewLine;
            }

            if (BinTypeCode == "")
            {
                error += AppResources.NewBinTemplatePage_BinTypeCodeIsEmpty + Environment.NewLine;
            }

            if (error != "")
            {
                ErrorText = error;
                State = ModelState.Error;
                return false;
            }
            else
            {
                return true;
            }
        }

        public override void DisposeModel()
        {
            BinTypes.Clear();
            Locations.Clear();
            Zones.Clear();
            base.DisposeModel();
        }
    }
}
