// ----------------------------------------------------------------------------------
// Copyright © 2017, Oleg Lobakov, Contacts: <oleg.lobakov@gmail.com>
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
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.ViewModel.Base;
using Xamarin.Forms;
using System.Windows.Input;

namespace WarehouseControlSystem.ViewModel
{
    public class UserDefinedSelectionViewModel : BaseViewModel
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
        } int id;
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
        } string name;
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
        } string detail;

        public int Value
        {
            get { return value1; }
            set
            {
                if (value1 != value)
                {
                    value1 = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        } int value1;

        public Color Color
        {
            get { return color; }
            set
            {
                if (color != value)
                {
                    color = value;
                    OnPropertyChanged(nameof(Color));
                }
            }
        } Color color;

        public bool UDSIsRan
        {
            get { return udsisran; }
            set
            {
                if (udsisran != value)
                {
                    udsisran = value;
                    OnPropertyChanged(nameof(UDSIsRan));
                }
            }
        } bool udsisran;
        public bool UDSIsBeingRan
        {
            get { return udsisbeingran; }
            set
            {
                if (udsisbeingran != value)
                {
                    udsisbeingran = value;
                    OnPropertyChanged(nameof(UDSIsBeingRan));
                }
            }
        } bool udsisbeingran;
        public int UDSWidth
        {
            get { return udswidth; }
            set
            {
                if (udswidth != value)
                {
                    udswidth = value;
                    OnPropertyChanged(nameof(UDSWidth));
                }
            }
        } int udswidth;

        public event Action<UserDefinedSelectionViewModel> OnTap;

        public ICommand TapCommand { protected set; get; }

        public UserDefinedSelectionViewModel(INavigation navigation, UserDefinedSelection uds) : base(navigation)
        { 
            FillFields(uds);
            TapCommand = new Command(Tap);
            State = Helpers.Containers.StateContainer.State.Normal;
        }

        public void FillFields(UserDefinedSelection uds)
        {
            ID = uds.ID;
            Name = uds.Name;
            Detail = uds.Detail;       
            Color = Color.FromHex(uds.HexColor);
            Value = uds.Value;
        }

        public void SaveFields(UserDefinedSelection uds)
        {
            uds.ID = ID;
            uds.Name = Name;
            uds.Detail = Detail;
            uds.HexColor = ColorToHex(Color);
            uds.Value = Value;
        }

        public void Tap()
        {
            if (OnTap is Action<UserDefinedSelectionViewModel>)
            {
                OnTap(this);
            }
        }
    }
}
