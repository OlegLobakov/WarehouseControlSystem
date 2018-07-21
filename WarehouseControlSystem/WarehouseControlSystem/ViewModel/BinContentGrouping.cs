using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WarehouseControlSystem.ViewModel
{
    public class BinContentGrouping : ObservableCollection<BinContentShortViewModel>
    {
        public string BinCode { get; private set; }

        public BinContentGrouping(string bincode, IEnumerable<BinContentShortViewModel> items)
        {
            BinCode = bincode;
            foreach (var item in items)
                this.Items.Add(item);
        }
    }
}
