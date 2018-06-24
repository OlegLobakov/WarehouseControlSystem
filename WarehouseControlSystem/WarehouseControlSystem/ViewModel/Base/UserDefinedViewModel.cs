using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WarehouseControlSystem.ViewModel.Base
{
    /// <summary>
    /// User Defined Function and Selection
    /// </summary>
    public class UserDefinedViewModel : BaseViewModel
    {
        public int ID
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(ID));
                }
            }
        }
        int id;

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        string name;

        public string Detail
        {
            get { return detail; }
            set
            {
                if (detail != value)
                {
                    detail = value;
                    OnPropertyChanged(nameof(Detail));
                }
            }
        }
        string detail;

        public UserDefinedViewModel(INavigation navigation) : base(navigation)
        {
        }
    }
}
