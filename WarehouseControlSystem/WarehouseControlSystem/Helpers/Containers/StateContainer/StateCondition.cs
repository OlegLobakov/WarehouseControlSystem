using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WarehouseControlSystem.Helpers.Containers.StateContainer
{
    [ContentProperty("Content")]
    public class StateCondition : Xamarin.Forms.View
    {
        public object State { get; set; }
        public Xamarin.Forms.View Content { get; set; }
    }
}

