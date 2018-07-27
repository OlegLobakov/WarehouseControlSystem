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
        public int ItemsPosition { get; set; } = 1;
        public int ItemsCount { get; set; } = int.MaxValue;
    }
}
