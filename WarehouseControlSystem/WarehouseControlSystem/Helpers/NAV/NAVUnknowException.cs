using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseControlSystem.Helpers.NAV
{
    public class NAVUnknowException : Exception
    {
        public NAVUnknowException(string message) : base(message)
        {
        }
    }
}
