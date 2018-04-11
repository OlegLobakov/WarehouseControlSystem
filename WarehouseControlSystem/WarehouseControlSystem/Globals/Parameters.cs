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
using PCLStorage;
using System.IO;
using System.Xml;
using WarehouseControlSystem.Helpers.NAV;
using System.Xml.Serialization;
using System.Reflection;
using WarehouseControlSystem.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WarehouseControlSystem
{
    /// <summary>
    /// Parameters file
    /// </summary>
    public class Parameters
    {
        public string WarehouseControlSystem { get; set; } = "Warehouse Control System Parameters/Settings File";
        public List<Connection> Connections { get; set; } = new List<Connection>();
    }
}
