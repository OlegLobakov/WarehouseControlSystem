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
using System.Text;

namespace WarehouseControlSystem.Helpers.NAV
{
    /// <summary>
    /// For NAV web-service functions
    /// </summary>
    public class NAVFilter
    {
        public string LocationCodeFilter { get; set; } = "";
        public string ZoneCodeFilter { get; set; } = "";
        public string RackIDFilter { get; set; } = "";
        public string BinCodeFilter { get; set; } = "";
        public string ItemNoFilter { get; set; } = "";
        public string VariantCodeFilter { get; set; } = "";
        public string DescriptionFilter { get; set; } = "";
        public int ItemsPosition { get; set; } = 1;
        public int ItemsCount { get; set; } = int.MaxValue;
    }
}
