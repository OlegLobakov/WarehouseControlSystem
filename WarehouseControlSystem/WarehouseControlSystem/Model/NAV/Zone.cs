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
    /// Microsoft Dynamcis NAV - Zone
    /// </summary>
    public class Zone 
    {
        public string LocationCode { get; set; } = "";
        public string Code { get; set; } = "";
        public string Description { get; set; } = "";
        public string BinTypeCode { get; set; } = "";
        public int ZoneRanking { get; set; }
        public bool CrossDockBinZone { get; set; }
        public string SpecialEquipmentCode { get; set; } = "";
        public string WarehouseClassCode { get; set; } = "";
        public string HexColor { get; set; } = "";
        public bool SchemeVisible { get; set; }
        public int RackQuantity { get; set; }
        public int BinQuantity { get; set; }

        public int PlanWidth { get; set; }
        public int PlanHeight { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public bool IsOnPlan
        {
            get { return (Width != 0) && (Height != 0); }
        }

        public string PrevCode { get; set; }

        public Zone()
        {
            PlanWidth = Settings.DefaultZonePlanWidth;
            PlanHeight = Settings.DefaultZonePlanHeight;
        }

        public Zone GetCopy()
        {
            return new Zone()
            {
                LocationCode = LocationCode,
                Code = Code,
                Description = Description,
                BinTypeCode = BinTypeCode,
                ZoneRanking = ZoneRanking,
                WarehouseClassCode = WarehouseClassCode,
                CrossDockBinZone = CrossDockBinZone,
                SpecialEquipmentCode = SpecialEquipmentCode,
                SchemeVisible = SchemeVisible,
                BinQuantity = BinQuantity,
                HexColor = HexColor,
                Left = Left,
                Top = Top,
                Width = Width,
                Height = Height,
                PlanWidth = PlanWidth,
                PlanHeight = PlanHeight,
                PrevCode = PrevCode,
            };
        }

        public void CopyTo(Zone to)
        {
            to.LocationCode = LocationCode;
            to.Code = Code;
            to.Description = Description;
            to.BinTypeCode = BinTypeCode;
            to.ZoneRanking = ZoneRanking;
            to.WarehouseClassCode = WarehouseClassCode;
            to.CrossDockBinZone = CrossDockBinZone;
            to.SpecialEquipmentCode = SpecialEquipmentCode;
            to.SchemeVisible = SchemeVisible;
            to.BinQuantity = BinQuantity;
            to.HexColor = HexColor;
            to.Left = Left;
            to.Top = Top;
            to.Width = Width;
            to.Height = Height;
            to.PlanWidth = PlanWidth;
            to.PlanHeight = PlanHeight;
            to.PrevCode = PrevCode;
        }

    }
}
