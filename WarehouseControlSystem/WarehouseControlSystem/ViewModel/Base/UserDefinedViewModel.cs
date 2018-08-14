// ----------------------------------------------------------------------------------
// Copyright © 2018, Oleg Lobakov, Contacts: <oleg.lobakov@gmail.com>
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
