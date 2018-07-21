using System;
using System.Collections.Generic;
using System.Text;
using WarehouseControlSystem.ViewModel;

namespace WarehouseControlSystem.Helpers.Comparer
{
    class BinContentShortViewModelComparer : IComparer<BinContentShortViewModel>
    {
        public int Compare(BinContentShortViewModel o1, BinContentShortViewModel o2)
        { 
            if (o1.BinCode == o2.BinCode)
            {
                return 0;
            }
            else
            {
                return o1.BinCode.CompareTo(o2.BinCode);
            }
        }
    }
}

