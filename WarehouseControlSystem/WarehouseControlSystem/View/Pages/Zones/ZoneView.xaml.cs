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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WarehouseControlSystem.ViewModel;
using WarehouseControlSystem.View.Pages.Base;
namespace WarehouseControlSystem.View.Pages.Zones
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZoneView : SchemeBaseView
    {   
        public ZoneView(ZoneViewModel zvm) : base(zvm)
        {
            InitializeComponent();
        }
    }
}