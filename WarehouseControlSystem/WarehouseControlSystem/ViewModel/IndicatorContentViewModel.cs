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
using WarehouseControlSystem.ViewModel.Base;
using Xamarin.Forms;
using WarehouseControlSystem.Model.NAV;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace WarehouseControlSystem.ViewModel
{
    public class IndicatorContentViewModel :  BaseViewModel
    {
        public string Header
        {
            get { return header; }
            set
            {
                if (header != value)
                {
                    header = value;
                    OnPropertyChanged(nameof(Header));
                }
            }
        } string header;
        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        } string description;

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
        public bool IsShowDetail
        {
            get { return isshowdetail; }
            set
            {
                if (isshowdetail != value)
                {
                    isshowdetail = value;
                    OnPropertyChanged(nameof(IsShowDetail));
                }
            }
        }
        bool isshowdetail;

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
        public string LeftValue
        {
            get { return leftvalue1; }
            set
            {
                if (leftvalue1 != value)
                {
                    leftvalue1 = value;
                    OnPropertyChanged(nameof(LeftValue));
                }
            }
        } string leftvalue1;

        public string RightValue
        {
            get { return rightvalue1; }
            set
            {
                if (rightvalue1 != value)
                {
                    rightvalue1 = value;
                    OnPropertyChanged(nameof(RightValue));
                }
            }
        }
        string rightvalue1;

        public string ID
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
        string id;

        public string Parameters
        {
            get { return parameters; }
            set
            {
                if (parameters != value)
                {
                    parameters = value;
                    OnPropertyChanged(nameof(Parameters));
                }
            }
        }
        string parameters;
        public decimal SortOrder
        {
            get { return sortorder; }
            set
            {
                if (sortorder != value)
                {
                    sortorder = value;
                    OnPropertyChanged(nameof(SortOrder));
                }
            }
        }
        decimal sortorder;

        public ICommand TapCommand { protected set; get; }
        public event Action<IndicatorContentViewModel> OnTap;

        public IndicatorContentViewModel(INavigation navigation, IndicatorContent indicatorcontent) : base(navigation)
        {
            FillFields(indicatorcontent);
            TapCommand = new Command<object>(Tap);
        }

        public void FillFields(IndicatorContent indicatorcontent)
        {
            Header = indicatorcontent.Header;
            Description = indicatorcontent.Description;
            Detail = indicatorcontent.Detail;
            LeftValue = indicatorcontent.LeftValue;
            RightValue = indicatorcontent.RightValue;
            Color = Color.FromHex(indicatorcontent.Color);
            SortOrder = indicatorcontent.SortOrder;
            ID = indicatorcontent.ID;
            Parameters = indicatorcontent.Parameters;
        }

        public void Tap(object sender)
        {
            if (OnTap is Action<IndicatorContentViewModel>)
            {
                OnTap(this);
            }
        }
    }
}
