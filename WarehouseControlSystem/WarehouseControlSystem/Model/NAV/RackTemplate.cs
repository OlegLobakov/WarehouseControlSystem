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
    public class RackTemplate 
    {
        public string Name { get; set; } = "";
        public RackOrientationEnum RackOrientation { get; set; }
        public string BinTemplateCode { get; set; } = "";
        public string Comment {get;set;} = "";
        public int Sections { get; set; }
        public int Levels { get; set; }
        public int Depth { get; set; }

        public int SectionSlotSize { get; set; }
        public int LevelSlotSize { get; set; }

        public string RackSectionSeparator { get; set; } = "";
        public string SectionLevelSeparator { get; set; } = "";
        public string LevelDepthSeparator { get; set; } = "";

        public bool ReversSectionNumbering { get; set; }
        public bool ReversLevelNumbering { get; set; }

        public int NumberingSectionBegin { get; set; }
        public int NumberingLevelBegin { get; set; }

        public int BinsQuanity { get; set; }
    }
}
