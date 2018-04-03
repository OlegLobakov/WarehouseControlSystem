using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseControlSystem.Helpers.NAV
{
    /// <summary>
    /// NAV ERROR
    /// </summary>
    public class NAVErrorException : Exception
    {
        public string Fault { get; set; }
        public string FaultString { get; set; }
        public string Detail { get; set; }

        public NAVErrorException(string fault, string faultstring, string detail) : base(faultstring)
        {
            Fault = fault;
            FaultString = faultstring;
            Detail = detail;
        }
    }
}
