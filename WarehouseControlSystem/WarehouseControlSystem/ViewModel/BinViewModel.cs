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
using WarehouseControlSystem.Model;
using WarehouseControlSystem.ViewModel.Base;
using WarehouseControlSystem.Model.NAV;
using Xamarin.Forms;
using System.Windows.Input;
using System.Collections.ObjectModel;


namespace WarehouseControlSystem.ViewModel
{
    public class BinViewModel : NAVBaseViewModel
    {
        public Bin Bin;
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
        } string locationcode;
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
        } string zonecode;
        public string RackNo
        {
            get { return rackno; }
            set
            {
                if (rackno != value)
                {
                    rackno = value;
                    OnPropertyChanged(nameof(RackNo));
                }
            }
        } string rackno;

        public int Section
        {
            get { return section; }
            set
            {
                if (section != value)
                {
                    section = value;
                    OnPropertyChanged(nameof(Section));
                }
            }
        } int section;
        public int Level
        {
            get { return level; }
            set
            {
                if (level != value)
                {
                    level = value;
                    OnPropertyChanged(nameof(Level));
                }
            }
        } int level;
        public int Depth
        {
            get { return depth; }
            set
            {
                if (depth != value)
                {
                    depth = value;
                    OnPropertyChanged(nameof(Depth));
                }
            }
        } int depth;
        public int SectionSpan
        {
            get { return sectionspan; }
            set
            {
                if (sectionspan != value)
                {
                    sectionspan = value;
                    OnPropertyChanged(nameof(SectionSpan));
                }
            }
        } int sectionspan;
        public int LevelSpan
        {
            get { return levelspan; }
            set
            {
                if (levelspan != value)
                {
                    levelspan = value;
                    OnPropertyChanged(nameof(LevelSpan));
                }
            }
        } int levelspan;
        public int DepthSpan
        {
            get { return depthspan; }
            set
            {
                if (depthspan != value)
                {
                    depthspan = value;
                    OnPropertyChanged(nameof(DepthSpan));
                }
            }
        } int depthspan;

        public bool Blocked
        {
            get { return blocked; }
            set
            {
                if (blocked != value)
                {
                    blocked = value;
                    OnPropertyChanged(nameof(Blocked));
                }
            }
        } bool blocked;
        public bool Empty
        {
            get { return empty; }
            set
            {
                if (empty != value)
                {
                    empty = value;
                    OnPropertyChanged(nameof(Empty));
                }
            }
        }
        bool empty;

        public string Message
        {
            get { return message1; }
            set
            {
                if (message1 != value)
                {
                    message1 = value;
                    OnPropertyChanged("Message");
                }
            }
        } string message1;

        public bool IsMessageVisibled
        {
            get { return ismessagevisibled; }
            set
            {
                if (ismessagevisibled != value)
                {
                    ismessagevisibled = value;
                    OnPropertyChanged("IsMessageVisibled");
                }
            }
        } bool ismessagevisibled;

        public int Quantity
        {
            get { return quantity; }
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        } int quantity = 0;

        public int BlockMovement
        {
            get { return blockmovement; }
            set
            {
                if (blockmovement != value)
                {
                    blockmovement = value;
                    OnPropertyChanged("BlockMovement");
                }
            }
        } int blockmovement = 0;

        public bool IsChecked
        {
            get
            {
                return ischecked;
            }
            set
            {
                if (ischecked != value)
                {
                    ischecked = value;
                    OnPropertyChanged("IsChecked");
                }
            }
        } bool ischecked = false;
        public bool IsExist
        {
            get
            {
                return isexist;
            }
            set
            {
                if (isexist != value)
                {
                    isexist = value;
                    OnPropertyChanged(nameof(IsExist));
                }
            }
        } bool isexist = false;
        public bool IsContent
        {
            get
            {
                return iscontent;
            }
            set
            {
                if (iscontent != value)
                {
                    iscontent = value;
                    OnPropertyChanged(nameof(IsContent));
                }
            }
        } bool iscontent = false;

        public bool ExcludeFromSearch
        {
            get
            {
                return excludefromsearch;
            }
            set
            {
                if (excludefromsearch != value)
                {
                    excludefromsearch = value;
                    OnPropertyChanged(nameof(ExcludeFromSearch));
                }
            }
        } bool excludefromsearch;


        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        } string description;

        public string BinType
        {
            get { return bintype; }
            set
            {
                if (bintype != value)
                {
                    bintype = value;
                    OnPropertyChanged("BinType");
                }
            }
        } string bintype;

        public bool NoSize
        {
            get { return nosize; }
            set
            {
                if (nosize != value)
                {
                    nosize = value;
                    OnPropertyChanged("NoSize");
                }
            }
        } bool nosize;

        public int BinRanking
        {
            get { return binrancking; }
            set
            {
                if (binrancking != value)
                {
                    binrancking = value;
                    OnPropertyChanged("BinRanking");
                }
            }
        } int binrancking;

        public decimal MaximumCubage
        {
            get { return maximumcubage; }
            set
            {
                if (maximumcubage != value)
                {
                    maximumcubage = value;
                    OnPropertyChanged("MaximumCubage");
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
                    OnPropertyChanged("MaximumWeight");
                }
            }
        } decimal maximumweight;

        public bool AdjustmentBin
        {
            get { return adjustmentbin; }
            set
            {
                if (adjustmentbin != value)
                {
                    adjustmentbin = value;
                    OnPropertyChanged("AdjustmentBin");
                }
            }
        } bool adjustmentbin;

        public string WarehouseClassCode
        {
            get { return warehouseclasscode; }
            set
            {
                if (warehouseclasscode != value)
                {
                    warehouseclasscode = value;
                    OnPropertyChanged("WarehouseClassCode");
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
                    OnPropertyChanged("SpecialEquipmentCode");
                }
            }
        } string specialequipmentcode;

        public bool Default
        {
            get { return default1; }
            set
            {
                if (default1 != value)
                {
                    default1 = value;
                    OnPropertyChanged("Default");
                }
            }
        } bool default1;

        public bool SchemeVisible
        {
            get { return schemevisible; }
            set
            {
                if (schemevisible != value)
                {
                    schemevisible = value;
                    OnPropertyChanged(nameof(SchemeVisible));
                }
            }
        } bool schemevisible;

        public bool Dedicated
        {
            get { return dedicated; }
            set
            {
                if (dedicated != value)
                {
                    dedicated = value;
                    OnPropertyChanged("Dedicated");
                }
            }
        } bool dedicated;

        public event Action<BinViewModel> OnTap;

        public ICommand TapCommand { protected set; get; }

        public ObservableCollection<BinContentShortViewModel> BinContent { get; set; } = new ObservableCollection<BinContentShortViewModel>();


        public BinViewModel(INavigation navigation, Bin bin) : base(navigation)
        {
            Bin = bin;
            FillFields(bin);
            TapCommand = new Command(Tap);
        }

        public void FillFields(Bin bin)
        {
            LocationCode = bin.LocationCode;
            RackNo = bin.RackNo;
            Code = bin.Code;
            Section = bin.Section;
            Level = bin.Level;
            Depth = bin.Depth;

            SectionSpan = bin.SectionSpan;
            LevelSpan = bin.LevelSpan;
            DepthSpan = bin.DepthSpan;

            Description = bin.Description;
            BinType = bin.BinType;
            NoSize = bin.NoSize;
            BinRanking = bin.BinRanking;
            MaximumCubage = bin.MaximumCubage;
            MaximumWeight = bin.MaximumWeight;
            AdjustmentBin = bin.AdjustmentBin;
            WarehouseClassCode = bin.WarehouseClassCode;
            SpecialEquipmentCode = bin.SpecialEquipmentCode;
            BlockMovement = bin.BlockMovement;
            SchemeVisible = bin.SchemeVisible;
            Default = bin.Default;
            Dedicated = bin.Dedicated;
            Empty = bin.Empty;
        }

        public void SaveFields(Bin bin)
        {
            bin.LocationCode = LocationCode;
            bin.RackNo = RackNo;
            bin.Code = Code;
            bin.Section = Section;
            bin.Level = Level;
            bin.Depth = Depth;

            bin.SectionSpan = SectionSpan;
            bin.LevelSpan = LevelSpan;
            bin.DepthSpan = DepthSpan;

            bin.Description = Description;
            bin.BinType = BinType;
            bin.NoSize = NoSize;
            bin.BinRanking = BinRanking;
            bin.MaximumCubage = MaximumCubage;
            bin.MaximumWeight = MaximumWeight;
            bin.AdjustmentBin = AdjustmentBin;
            bin.WarehouseClassCode = WarehouseClassCode;
            bin.SpecialEquipmentCode = SpecialEquipmentCode;
            bin.BlockMovement = BlockMovement;
            bin.SchemeVisible = SchemeVisible;
            bin.Default = Default;
            bin.Dedicated = Dedicated;

            bin.Empty = Empty; ;
        }

        public void SaveFields()
        {
            SaveFields(Bin);
        }

        private void Tap()
        {
            if (OnTap is Action<BinViewModel>)
            {
                OnTap(this);
            }
        }

        public override void Dispose()
        {
            if (OnTap is Action<BinViewModel>)
            {
                Delegate[] clientList = OnTap.GetInvocationList();
                foreach (var d in clientList)
                {
                    OnTap -= (d as Action<BinViewModel>);
                }
            }

            base.Dispose();
        }
    }
}
