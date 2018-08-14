using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseControlSystem.Model.NAV
{
    public class Indicator
    {
        public string Header { get; set; } = "";
        public string Description { get; set; } = "";
        public string Value { get; set; } = "";
        public string ValueColor { get; set; } = "";
        public int Position { get; set; }
        public string URL { get; set; } = "";
    }
}
