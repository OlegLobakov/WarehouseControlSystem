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
    /// Bin Template
    /// </summary>
    public class BinTemplate
    {
        public string Code { get; set; } = "";
        public string LocationCode { get; set; } = "";
        public string ZoneCode { get; set; } = "";
        public string Description { get; set; } = "";
        public string BinTypeCode { get; set; } = "";
        public string BinDescription { get; set; } = "";
        public string WarehouseClassCode { get; set; } = "";
        public int BlockMovement { get; set; }
        public string SpecialEquipmentCode { get; set; } = "";
        public int BinRanking { get; set; }
        public decimal MaximumCubage { get; set; }
        public decimal MaximumWeight { get; set; }
        public bool Dedicated { get; set; }
        public string PrevCode { get; set; } = "";

        public BinTemplate Copy()
        {
            BinTemplate to = new BinTemplate();
            to.Code = Code;
            to.Description = Description;
            to.LocationCode = LocationCode;
            to.ZoneCode = ZoneCode;
            to.BinTypeCode = BinTypeCode;
            to.BinDescription = BinDescription;
            to.WarehouseClassCode = WarehouseClassCode;
            to.BlockMovement = BlockMovement;
            to.SpecialEquipmentCode = SpecialEquipmentCode;
            to.BinRanking = BinRanking;
            to.MaximumCubage = MaximumCubage;
            to.MaximumWeight = MaximumWeight;
            to.Dedicated = Dedicated;
            to.PrevCode = PrevCode;
            return to;
        }
    }
}
