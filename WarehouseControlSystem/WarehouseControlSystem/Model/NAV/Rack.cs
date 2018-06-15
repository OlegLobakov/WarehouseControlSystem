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
    public class Rack 
    {
        public string LocationCode { get; set; } = "";
        public string ZoneCode { get; set; } = "";
        public string No { get; set; } = "";
        public string Comment { get; set; } = "";

        public RackOrientationEnum RackOrientation {
            get {
                return rackorientation;
            }
            set {
                rackorientation = value;
                RecalculateSize();
            }
        } RackOrientationEnum rackorientation;

        public string BinTemplateCode { get; set; } = "";

        public int Sections
        {
            get
            {
                return sections;
            }
            set
            {
                sections = value;
                RecalculateSize();
            }
        } int sections;
        public int Levels
        {
            get
            {
                return levels;
            }
            set
            {
                levels = value;
                RecalculateSize();
            }
        } int levels;
        public int Depth
        {
            get
            {
                return depth;
            }
            set
            {
                depth = value;
                RecalculateSize();
            }
        } int depth;

        public int SectionSlotSize { get; set; }
        public int LevelSlotSize { get; set; }

        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public bool SchemeVisible { get; set; }
        public string PrevNo { get; set; } = "";

        public Rack()
        {
            Left = 0;
            Top = 0;
            RackOrientation = RackOrientationEnum.Undefined;
            Sections = Settings.DefaultRackSections;
            Levels = Settings.DefaultRackLevels;
            Depth = Settings.DefaultRackDepth;
        }

        public void RecalculateSize()
        {
            if ((RackOrientation == RackOrientationEnum.HorizontalLeft) || (RackOrientation == RackOrientationEnum.HorizontalRight))
            {
                Width = Sections + 1;
                Height = Depth;
            }
            else
            {
                Width = Depth;
                Height = Sections + 1;
            }
        }

        public Rack GetCopy()
        {
            return new Rack
            {
                LocationCode = LocationCode,
                ZoneCode = ZoneCode,
                No = No,
                PrevNo = PrevNo,
                RackOrientation = RackOrientation,
                BinTemplateCode = BinTemplateCode,
                Sections = Sections,
                Levels = Levels,
                Depth = Depth,
                SectionSlotSize = SectionSlotSize,
                LevelSlotSize = LevelSlotSize,
                Left = Left,
                Top = Top,
                SchemeVisible = SchemeVisible,
                Comment = Comment,
            };
        }

        public void CopyTo(Rack to)
        {
            to.LocationCode = LocationCode;
            to.ZoneCode = ZoneCode;
            to.No = No;
            to.PrevNo = PrevNo;
            to.RackOrientation = RackOrientation;
            to.BinTemplateCode = BinTemplateCode;
            to.Sections = Sections;
            to.Levels = Levels;
            to.Depth = Depth;
            to.SectionSlotSize = SectionSlotSize;
            to.LevelSlotSize = LevelSlotSize;
            to.Left = Left;
            to.Top = Top;
            to.SchemeVisible = SchemeVisible;
            to.Comment = Comment;
        }
    }
}
