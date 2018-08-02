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
    public partial class EmptySpaceViewInRack : ContentView
    {
        protected EmptySpaceViewModel model;

        public int Section { get; set; }
        public int Level { get; set; }
        public bool Marked { get; set; }


        public double CodeFontSize
        {
            get { return codefontsize; }
            set
            {
                if (codefontsize != value)
                {
                    codefontsize = value;
                    OnPropertyChanged(nameof(CodeFontSize));
                }
            }
        }
        double codefontsize;

        public EmptySpaceViewInRack(EmptySpaceViewModel esvm)
        {
            model = esvm;
            BindingContext = model;
            InitializeComponent();
        }

        private void StackLayout_SizeChanged(object sender, EventArgs e)
        {
            StackLayout sl = (StackLayout)sender;
            CodeFontSize = sl.Width / 5;
        }

        public void Update(EmptySpaceViewModel esvm)
        {
            model = esvm;
            BindingContext = model;
        }
    }
}