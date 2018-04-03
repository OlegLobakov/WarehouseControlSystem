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
    /// Microsoft Dynamcis NAV - Item
    /// </summary>
    public class Item 
    {
        public string No { get; set; } = "";
        public string Description { get; set; } = "";
        public string BaseUoM { get; set; } = "";
        public decimal Quantity { get; set; }
        public decimal QuantityBase { get; set; }

        public List<string> BarCodes = new List<string>();
    }
}
