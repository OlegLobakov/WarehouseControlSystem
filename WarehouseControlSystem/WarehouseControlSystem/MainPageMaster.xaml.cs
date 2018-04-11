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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WarehouseControlSystem.Resx;
using WarehouseControlSystem.View.Pages.Parameters;
using WarehouseControlSystem.View.Pages.Find;
using WarehouseControlSystem.View.Pages.Contacts;
using WarehouseControlSystem.View.Pages.Connections;




namespace WarehouseControlSystem
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageMaster : ContentPage
    {
        public ListView ListView;

        public MainPageMaster()
        {
            InitializeComponent();

            BindingContext = new MainPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MainPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MainPageMenuItem> MenuItems { get; set; }

            public MainPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MainPageMenuItem>(new[]
                {
                    new MainPageMenuItem { Id = 0,  Title =  AppResources.MainMenuHome, TargetType=typeof(MainPageDetail)},
                    new MainPageMenuItem { Id = 10, Title =  AppResources.FindPage_Title,TargetType=typeof(FindPage)},
                    //new MainPageMenuItem { Id = 20, Title =  AppResources.MainMenuLog, TargetType=typeof(LogPage)},
                    new MainPageMenuItem { Id = 30, Title =  AppResources.MainMenuConnections, TargetType=typeof(ConnectionsPage)},
                    new MainPageMenuItem { Id = 50, Title =  AppResources.MainMenuParameters, TargetType=typeof(ParametersPage)},
                    new MainPageMenuItem { Id = 60, Title =  AppResources.MainMenuAbout, TargetType=typeof(AboutPage)},
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                {
                    return;
                }
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}