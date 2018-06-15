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
    /// Microsoft Dynamics NAV - Location
    /// </summary>
    public class Location 
    {
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string PhoneNo { get; set; } = "";
        public bool Transit { get; set; }
        public bool BinMandatory { get; set; }
        public bool RequireReceive { get; set; }
        public bool RequireShipment { get; set; }
        public bool RequirePick { get; set; }
        public bool RequirePutaway { get; set; }
        public bool SchemeVisible { get; set; }
        public int ZoneQuantity { get; set; }
        public int BinQuantity { get; set; }
        public string HexColor { get; set; } = "";

        public int PlanWidth { get; set; }
        public int PlanHeight { get; set; }

        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public string PrevCode { get; set; } = "";

        public Location()
        {
            PlanWidth = Settings.DefaultLocationPlanWidth;
            PlanHeight = Settings.DefaultLocationPlanHeight;
        }

        public Location GetCopy()
        {
            return new Location
            {
                Code = Code,
                Name = Name,
                Address = Address,
                PhoneNo = PhoneNo,
                PlanWidth = PlanWidth,
                PlanHeight = PlanHeight,
                Left = Left,
                Top = Top,
                Width = Width,
                Height = Height,
                SchemeVisible = SchemeVisible,
                BinMandatory = BinMandatory,
                RequireReceive = RequireReceive,
                RequireShipment = RequireShipment,
                RequirePick = RequirePick,
                RequirePutaway = RequirePutaway,
                HexColor = HexColor,
                Transit = Transit,
                PrevCode = PrevCode,
            };
        }

        public void CopyTo(Location to)
        {
            to.Code = Code;
            to.Name = Name;
            to.Address = Address;
            to.PhoneNo = PhoneNo;
            to.PlanWidth = PlanWidth;
            to.PlanHeight = PlanHeight;
            to.Left = Left;
            to.Top = Top;
            to.Width = Width;
            to.Height = Height;
            to.SchemeVisible = SchemeVisible;
            to.BinMandatory = BinMandatory;
            to.RequireReceive = RequireReceive;
            to.RequireShipment = RequireShipment;
            to.RequirePick = RequirePick;
            to.RequirePutaway = RequirePutaway;
            to.HexColor = HexColor;
            to.Transit = Transit;
            to.PrevCode = PrevCode;
        }
    }
}
