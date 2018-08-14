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
    public class NAVEntryBaseViewModel : BaseViewModel
    {
        public int EntryNo
        {
            get { return entryno; }
            set
            {
                if (entryno != value)
                {
                    entryno = value;
                    OnPropertyChanged(nameof(EntryNo));
                }
            }
        } int entryno;

        public string LocationCode
        {
            get { return locationcode; }
            set
            {
                if (locationcode != value)
                {
                    locationcode = value;
                    OnPropertyChanged(nameof(LocationCode));
                }
            }
        }
        string locationcode;

        public string BinCode
        {
            get { return bincode; }
            set
            {
                if (bincode != value)
                {
                    bincode = value;
                    OnPropertyChanged(nameof(BinCode));
                }
            }
        }
        string bincode;

        public string ZoneCode
        {
            get { return zonecode; }
            set
            {
                if (zonecode != value)
                {
                    zonecode = value;
                    OnPropertyChanged(nameof(ZoneCode));
                }
            }
        }
        string zonecode;

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

        public string VariantCode
        {
            get { return variantcode; }
            set
            {
                if (variantcode != value)
                {
                    variantcode = value;
                    OnPropertyChanged(nameof(VariantCode));
                }
            }
        } string variantcode;

        public string RegisteringDate
        {
            get { return registeringdate; }
            set
            {
                if (registeringdate != value)
                {
                    registeringdate = value;
                    OnPropertyChanged(nameof(RegisteringDate));
                }
            }
        } string registeringdate;

        public string PostingDate
        {
            get { return postingdate; }
            set
            {
                if (postingdate != value)
                {
                    postingdate = value;
                    OnPropertyChanged(nameof(PostingDate));
                }
            }
        }
        string postingdate;

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
        } decimal quantity;

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
        } decimal quantitybase;

        public string UnitofMeasureCode
        {
            get { return unitofmeasurecode; }
            set
            {
                if (unitofmeasurecode != value)
                {
                    unitofmeasurecode = value;
                    OnPropertyChanged(nameof(UnitofMeasureCode));
                }
            }
        } string unitofmeasurecode;

        public NAVEntryBaseViewModel(INavigation navigation) : base(navigation)
        {

        }
    }
}
