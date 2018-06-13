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
using WarehouseControlSystem.Resx;
using WarehouseControlSystem.ViewModel.Base;
using Xamarin.Forms;
using System.Windows.Input;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.Helpers.NAV;
using System.Collections.ObjectModel;

namespace WarehouseControlSystem.ViewModel
{
    public class WarehouseEntryViewModel : NAVBaseViewModel
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

        public string  BinCode
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
        } string bincode;
        
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
        } string itemno;

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

        public int EntryType
        {
            get { return entrytype; }
            set
            {
                if (entrytype != value)
                {
                    entrytype = value;
                    OnPropertyChanged(nameof(EntryType));
                }
            }
        } int entrytype;

        public string WarrantyDate
        {
            get { return warrantydate; }
            set
            {
                if (warrantydate != value)
                {
                    warrantydate = value;
                    OnPropertyChanged(nameof(WarrantyDate));
                }
            }
        } string warrantydate;

        public string SourceNo
        {
            get { return sourceno; }
            set
            {
                if (sourceno != value)
                {
                    sourceno = value;
                    OnPropertyChanged(nameof(SourceNo));
                }
            }
        } string sourceno;


        public WarehouseEntryViewModel(INavigation navigation, WarehouseEntry warehouseentry) : base(navigation)
        {
            EntryNo = warehouseentry.EntryNo;
            EntryType = warehouseentry.EntryType;
            LocationCode = warehouseentry.LocationCode;
            ZoneCode = warehouseentry.ZoneCode;
            BinCode = warehouseentry.BinCode;
            ItemNo = warehouseentry.ItemNo;
            VariantCode = warehouseentry.VariantCode;
            Description = warehouseentry.Description;
            UnitofMeasureCode = warehouseentry.UnitofMeasureCode;
            RegisteringDate = warehouseentry.RegisteringDate;
            Quantity = warehouseentry.Quantity;
            QuantityBase = warehouseentry.QuantityBase;
            WarrantyDate = warehouseentry.WarrantyDate;
            SourceNo = warehouseentry.SourceNo;
        }
    }
}
