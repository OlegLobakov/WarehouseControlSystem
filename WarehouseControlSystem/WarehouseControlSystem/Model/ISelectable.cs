using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WarehouseControlSystem.Model
{
    public interface ISelectable
    {
        bool IsSelected { get; set; }

        ICommand SelectCommand { get; set; }
    }
}
