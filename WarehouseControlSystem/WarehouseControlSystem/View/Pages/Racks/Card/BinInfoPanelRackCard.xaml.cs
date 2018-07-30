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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.ViewModel;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.Helpers.NAV;

namespace WarehouseControlSystem.View.Pages.Racks.Card
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BinInfoPanelRackCard : ContentView
    {
        public event Action<BinContentShortViewModel> BinContentTap;
        public event Action<UserDefinedFunctionViewModel> UserDefinedFunctionTap;

        public BinInfoPanelRackCard()
        {
            InitializeComponent();
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            BinContentShortViewModel bcsvm = (BinContentShortViewModel)e.Item;
            BinsViewModel bvm = (BinsViewModel)BindingContext;
            if (BinContentTap is Action<BinContentShortViewModel>)
            {
                BinContentTap(bcsvm);
            }
        }

        private void ListView_UserDefinedFunctionTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is UserDefinedFunctionViewModel)
            {
                UserDefinedFunctionViewModel udfvm = (UserDefinedFunctionViewModel)e.Item;
                BinsViewModel bvm = (BinsViewModel)BindingContext;
                if (UserDefinedFunctionTap is Action<UserDefinedFunctionViewModel>)
                {
                    UserDefinedFunctionTap(udfvm);
                }
            }
        }
    }
}