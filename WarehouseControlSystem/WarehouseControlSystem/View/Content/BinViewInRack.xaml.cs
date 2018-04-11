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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.ViewModel;

namespace WarehouseControlSystem.View.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BinViewInRack : ContentView
    {
        BinViewModel model;
        public BinViewInRack(BinViewModel bvm)
        {
            model = bvm;
            BindingContext = model;
            InitializeComponent();

            //tap
            TapGestureRecognizer tc = new TapGestureRecognizer()
            {
                Command = model.TapCommand
            };

            GestureRecognizers.Add(tc);
            codelabel.GestureRecognizers.Add(tc);
            grid.GestureRecognizers.Add(tc);

            ////pan
            //PanGestureRecognizer panGesture = new PanGestureRecognizer();
            //panGesture.PanUpdated += (s, e) =>
            //{
            //    OnPaned(s, e);
            //};
            //GestureRecognizers.Add(panGesture);
            //codelabel.GestureRecognizers.Add(panGesture);
            //grid.GestureRecognizers.Add(panGesture);
        }

        //private void OnTapped()
        //{
        //    if (Taped is Action<BinView>)
        //    {
        //        Taped(this);
        //    }
        //}

        //private void OnPaned(object sender, PanUpdatedEventArgs e)
        //{
        //    if (Paned is Action<BinElementView, PanUpdatedEventArgs>)
        //    {
        //        Paned(this, e);
        //    }
        //}
    }
}