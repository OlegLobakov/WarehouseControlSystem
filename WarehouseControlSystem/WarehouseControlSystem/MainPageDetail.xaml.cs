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
using WarehouseControlSystem.View.Pages.Locations;
using WarehouseControlSystem.View.Pages.Contacts;
using WarehouseControlSystem.View.Pages.Connections;
using WarehouseControlSystem.View.Pages.Parameters;
using WarehouseControlSystem.ViewModel;
using System.Threading.Tasks;

namespace WarehouseControlSystem
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageDetail : ContentPage
    {
        public int BlockSize
        {
            get { return blocksize; }
            set
            {
                if (blocksize != value)
                {
                    blocksize = value;
                    OnPropertyChanged(nameof(BlockSize));
                }
            }
        }
        int blocksize;

        public int HSCWidth
        {
            get { return hscwidth; }
            set
            {
                if (hscwidth != value)
                {
                    hscwidth = value;
                    OnPropertyChanged(nameof(HSCWidth));
                }
            }
        }
        int hscwidth;

        public double LargeFontSize
        {
            get { return largefontsize; }
            set
            {
                if (largefontsize != value)
                {
                    largefontsize = value;
                    OnPropertyChanged(nameof(LargeFontSize));
                }
            }
        }
        double largefontsize;

        public double SmallFontSize
        {
            get { return smallfontsize; }
            set
            {
                if (smallfontsize != value)
                {
                    smallfontsize = value;
                    OnPropertyChanged(nameof(SmallFontSize));
                }
            }
        }
        double smallfontsize;

        ConnectionsViewModel model;

        public MainPageDetail()
        {
            model = new ConnectionsViewModel(Navigation);
            BindingContext = model;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            hlv.SelectedItem = null;
        }

        private async void HorizontalListView_SelectedItemChanged(object sender, System.EventArgs e)
        {
            if (sender is ConnectionViewModel)
            {
                ConnectionViewModel selected = (ConnectionViewModel)sender;
                model.Select(selected);
                LocationsPlanViewModel lpvm = new LocationsPlanViewModel(Navigation);
                LocationsSchemePage lsp = new LocationsSchemePage(lpvm);
                await Navigation.PushAsync(lsp);
            }
        }

        private void StackLayout_SizeChanged(object sender, System.EventArgs e)
        {
            StackLayout sl = (StackLayout)sender;
            BlockSize = (int)sl.Height;
            HSCWidth = model.ConnectionViewModels.Count * BlockSize;
            LargeFontSize = sl.Width / 40;
            SmallFontSize = sl.Width / 70;
        }
    }
}