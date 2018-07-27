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
        public int ID { get; set; }
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

        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public bool SchemeVisible { get; set; }

        public string NumberingPrefix { get; set; } = "";

        public string RackSectionSeparator { get; set; } 
        public string SectionLevelSeparator { get; set; } 
        public string LevelDepthSeparator { get; set; }

        public bool ReversSectionNumbering { get; set; }
        public bool ReversLevelNumbering { get; set; }
        public bool ReversDepthNumbering { get; set; }

        public int NumberingSectionBegin { get; set; } = 1;
        public int NumberingLevelBegin { get; set; } = 1;
        public int NumberingDepthBegin { get; set; } = 1;

        public int NumberingSectionDigitsQuantity { get; set; } = 2;
        public int NumberingLevelDigitsQuantity { get; set; } = 1;
        public int NumberingDepthDigitsQuantity { get; set; } = 1;

        public int StepNumberingSection { get; set; } = 1;
        public int StepNumberingLevel { get; set; } = 1;
        public int StepNumberingDepth { get; set; } = 1;

        public string BinTemplateCode { get; set; } = "";

        public Rack()
        {
            Left = 0;
            Top = 0;
            RackOrientation = RackOrientationEnum.Undefined;

            Sections = Settings.DefaultRackSections;
            Levels = Settings.DefaultRackLevels;
            Depth = Settings.DefaultRackDepth;

            RackSectionSeparator = Settings.DefaultRackSectionSeparator;
            SectionLevelSeparator = Settings.DefaultSectionLevelSeparator;
            LevelDepthSeparator = Settings.DefaultLevelDepthSeparator;
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
                ID = ID,
                LocationCode = LocationCode,
                ZoneCode = ZoneCode,
                No = No,
                RackOrientation = RackOrientation,

                Sections = Sections,
                Levels = Levels,
                Depth = Depth,

                Left = Left,
                Top = Top,

                SchemeVisible = SchemeVisible,
                Comment = Comment,

                NumberingPrefix = NumberingPrefix,

                RackSectionSeparator = RackSectionSeparator,
                SectionLevelSeparator = SectionLevelSeparator,
                LevelDepthSeparator = LevelDepthSeparator,

                ReversSectionNumbering = ReversSectionNumbering,
                ReversLevelNumbering = ReversLevelNumbering,
                ReversDepthNumbering = ReversDepthNumbering,

                NumberingSectionBegin = NumberingSectionBegin,
                NumberingLevelBegin = NumberingLevelBegin,
                NumberingDepthBegin = NumberingDepthBegin,

                NumberingSectionDigitsQuantity = NumberingSectionDigitsQuantity,
                NumberingLevelDigitsQuantity = NumberingLevelDigitsQuantity,
                NumberingDepthDigitsQuantity = NumberingDepthDigitsQuantity,

                StepNumberingSection = StepNumberingSection,
                StepNumberingLevel = StepNumberingLevel,
                StepNumberingDepth = StepNumberingDepth,

                BinTemplateCode = BinTemplateCode,
            };
        }

        public void CopyTo(Rack to)
        {
            to.ID = ID;
            to.LocationCode = LocationCode;
            to.ZoneCode = ZoneCode;
            to.No = No;
            to.RackOrientation = RackOrientation;
            
            to.Sections = Sections;
            to.Levels = Levels;
            to.Depth = Depth;

            to.Left = Left;
            to.Top = Top;

            to.SchemeVisible = SchemeVisible;
            to.Comment = Comment;

            to.NumberingPrefix = NumberingPrefix;

            to.RackSectionSeparator = RackSectionSeparator;
            to.SectionLevelSeparator = SectionLevelSeparator;
            to.LevelDepthSeparator = LevelDepthSeparator;

            to.ReversSectionNumbering = ReversSectionNumbering;
            to.ReversLevelNumbering = ReversLevelNumbering;
            to.ReversDepthNumbering = ReversDepthNumbering;

            to.NumberingSectionBegin = NumberingSectionBegin;
            to.NumberingLevelBegin = NumberingLevelBegin;
            to.NumberingDepthBegin = NumberingDepthBegin;

            to.NumberingSectionDigitsQuantity = NumberingSectionDigitsQuantity;
            to.NumberingLevelDigitsQuantity = NumberingLevelDigitsQuantity;
            to.NumberingDepthDigitsQuantity = NumberingDepthDigitsQuantity;

            to.StepNumberingSection = StepNumberingSection;
            to.StepNumberingLevel = StepNumberingLevel;
            to.StepNumberingDepth = StepNumberingDepth;

            to.BinTemplateCode = BinTemplateCode;
        }
    }
}
