// ----------------------------------------------------------------------------------
// Copyright © 2017, Oleg Lobakov, Contacts: <oleg.lobakov@gmail.com>
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

namespace WarehouseControlSystem.Model.NAV
{
    /// <summary>
    /// Microsoft Dynamcis NAV - Bin Record
    /// </summary>
    public class Bin
    {            
        public string LocationCode { get; set; } = "";
        public string ZoneCode { get; set; } = "";
        public string Code { get; set; } = "";
        public string Description { get; set; } = "";
        public string BinType { get; set; } = "";
        public bool Empty { get; set; }
        public bool NoSize { get; set; }
        public int BlockMovement { get; set; }
        public string RackNo { get; set; } = "";
        public int Section { get; set; }
        public int Level { get; set; }
        public int Depth { get; set; }
        public int LevelSpan { get; set; } = 1;
        public int SectionSpan { get; set; } = 1;
        public int DepthSpan { get; set; } = 1;
        public int BinRanking { get; set; }
        public decimal MaximumCubage { get; set; }
        public decimal MaximumWeight { get; set; }
        public bool AdjustmentBin { get; set; }
        public string WarehouseClassCode { get; set; } = "";
        public string SpecialEquipmentCode { get; set; } = "";
        public bool SchemeVisible { get; set; } = true;
        public bool Default { get; set; }
        public string PrevCode { get; set; } = "";
        public bool Dedicated { get; set; }

        public void CopyTo(Bin tobin)
        {
            tobin.LocationCode = LocationCode;
            tobin.ZoneCode = ZoneCode;
            tobin.Code = Code;
            tobin.Description = Description;
            tobin.BinType = BinType;
            tobin.Empty = Empty;
            tobin.NoSize = NoSize;
            tobin.BlockMovement = BlockMovement;
            tobin.RackNo = RackNo;
            tobin.Section = Section;
            tobin.Level = Level;
            tobin.Depth = Depth;
            tobin.LevelSpan = LevelSpan;
            tobin.SectionSpan = SectionSpan;
            tobin.DepthSpan = DepthSpan;
            tobin.BinRanking = BinRanking;
            tobin.MaximumCubage = MaximumCubage;
            tobin.MaximumWeight = MaximumWeight;
            tobin.AdjustmentBin = AdjustmentBin;
            tobin.WarehouseClassCode = WarehouseClassCode;
            tobin.SpecialEquipmentCode = SpecialEquipmentCode;
            tobin.SchemeVisible = SchemeVisible;
            tobin.PrevCode = PrevCode;
            tobin.Default = Default;
            tobin.Dedicated = Dedicated;
        }
    }
}
