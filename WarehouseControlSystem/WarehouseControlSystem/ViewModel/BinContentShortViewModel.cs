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
    public class BinContentShortViewModel : NAVEntryBaseViewModel
    {
        BinContent BinContent;

        public bool ImageIsVisible
        {
            get { return imageisvisible; }
            set
            {
                if (imageisvisible != value)
                {
                    imageisvisible = value;
                    OnPropertyChanged(nameof(ImageIsVisible));
                }
            }
        }
        bool imageisvisible;

        public bool ImageIsError
        {
            get { return imageiserror; }
            set
            {
                if (imageiserror != value)
                {
                    imageiserror = value;
                    OnPropertyChanged(nameof(ImageIsError));
                }
            }
        }
        bool imageiserror;

        public ImageSource ImageSource
        {
            get { return imagesource; }
            set
            {
                if (imagesource != value)
                {
                    imagesource = value;
                    OnPropertyChanged(nameof(ImageSource));
                }
            }
        } ImageSource imagesource;


        public string ImageURL
        {
            get { return imageurl; }
            set
            {
                if (imageurl != value)
                {
                    imageurl = value;
                    OnPropertyChanged(nameof(ImageURL));
                }
            }
        }
        string imageurl;

        public BinContentShortViewModel(INavigation navigation, BinContent bc) : base(navigation)
        {
            BinContent = bc;
            FillFields(bc);
            State = ModelState.Undefined;
        }

        public void FillFields(BinContent bc)
        {
            ItemNo = bc.ItemNo;
            Description = bc.Description;
            Quantity = bc.Quantity;
            QuantityBase = bc.QuantityBase;
            ImageURL = bc.ImageURL;
        }

        public void SaveFields(BinContent bc)
        {
            bc.ItemNo = ItemNo;
            bc.Description = Description;
            bc.Quantity = Quantity;
            bc.QuantityBase = QuantityBase;
            bc.ImageURL = ImageURL;
        }
    }
}
