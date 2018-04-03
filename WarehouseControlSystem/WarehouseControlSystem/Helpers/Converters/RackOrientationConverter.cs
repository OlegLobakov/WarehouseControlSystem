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
using Xamarin.Forms;
using System.Globalization;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.Resx;

namespace WarehouseControlSystem.Helpers.Converters
{
    public class RackOrientationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RackOrientationEnum roe = (RackOrientationEnum)value;
            string name1 = "";
            switch (roe)
            {
                case RackOrientationEnum.Undefined:
                    {
                        name1 = AppResources.RackNewPage_OrientationUndefined;
                        break;
                    }
                case RackOrientationEnum.HorizontalLeft:
                    {
                        name1 = AppResources.RackNewPage_OrientationRackHL;
                        break;
                    }
                case RackOrientationEnum.HorizontalRight:
                    {
                        name1 = AppResources.RackNewPage_OrientationRackHR;
                        break;
                    }
                case RackOrientationEnum.VerticalUp:
                    {
                        name1 = AppResources.RackNewPage_OrientationRackVU;
                        break;
                    }
                case RackOrientationEnum.VerticalDown:
                    {
                        name1 = AppResources.RackNewPage_OrientationRackVD;
                        break;
                    }
            }

            RackOrientationPick rop = new RackOrientationPick()
            {
                RackOrientation = roe,
                Name = name1
            };

            return rop;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RackOrientationEnum rv = RackOrientationEnum.Undefined;
            try
            {
                RackOrientationPick rop = (RackOrientationPick)value;
                rv = rop.RackOrientation;
            }
            catch {}
            return rv;
        }
    }
}
