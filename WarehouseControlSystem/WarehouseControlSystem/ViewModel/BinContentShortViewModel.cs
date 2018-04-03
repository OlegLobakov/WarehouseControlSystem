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

namespace WarehouseControlSystem.ViewModel
{
    public class BinContentShortViewModel : BaseViewModel
    {
        BinContent BinContent;
        public string ItemNo
        {
            get { return itemno; }
            set
            {
                if (itemno != value)
                {
                    itemno = value;
                    OnPropertyChanged(nameof(ItemNo));
                }
            }
        }
        string itemno;

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
        }
        string description;

        public decimal Quantity
        {
            get { return quantity; }
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }
        decimal quantity;

        public decimal QuantityBase
        {
            get { return quantitybase; }
            set
            {
                if (quantitybase != value)
                {
                    quantitybase = value;
                    OnPropertyChanged(nameof(QuantityBase));
                }
            }
        }
        decimal quantitybase;


        public BinContentShortViewModel(INavigation navigation, BinContent bc) : base(navigation)
        {
            BinContent = bc;
            FillFields(bc);
        }
        public void FillFields(BinContent bc)
        {
            ItemNo = bc.ItemNo;
            Description = bc.Description;
            Quantity = bc.Quantity;
            QuantityBase = bc.QuantityBase;
        }

        public void SaveFields(BinContent bc)
        {
            bc.ItemNo = ItemNo;
            bc.Description = Description;
            bc.Quantity = Quantity;
            bc.QuantityBase = QuantityBase;
        }
    }
}
