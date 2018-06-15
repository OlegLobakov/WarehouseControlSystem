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
using System.Windows.Input;
using WarehouseControlSystem.ViewModel.Base;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using WarehouseControlSystem.Helpers.NAV;
using WarehouseControlSystem.View.Pages.Connections;
using WarehouseControlSystem.Resx;


namespace WarehouseControlSystem.ViewModel
{
    public class ConnectionsViewModel : BaseViewModel
    {
        public ConnectionViewModel SelectedConnection { get; set; }
        public ObservableCollection<ConnectionViewModel> ConnectionViewModels { get; set; }

        public ICommand NewConnectionCommand { protected set; get; }
        public ICommand DeleteConnectionCommand { protected set; get; }
        public ICommand EditConnectionCommand { protected set; get; }
        public ICommand CopyConnectionCommand { protected set; get; }


        public ConnectionsViewModel(INavigation navigation) : base(navigation)
        {
            ConnectionViewModels = new ObservableCollection<ConnectionViewModel>();

            if (Global.Parameters.Connections is List<Connection>)
            {
                foreach (Connection connection in Global.Parameters.Connections)
                {
                    if (connection.Name == Settings.CurrentConnection)
                    {
                        connection.Connected = true;
                    }
                    ConnectionViewModel cvm = new ConnectionViewModel(Navigation, connection);
                    ConnectionViewModels.Add(cvm);
                }
            }

            NewConnectionCommand = new Command(Create);
            DeleteConnectionCommand = new Command<object>(Delete);
            EditConnectionCommand = new Command<object>(Edit);
            CopyConnectionCommand = new Command<object>(Copy);
            State = ModelState.Undefined;
        }

        public void SaveChanges()
        {
            Global.Parameters.Connections.Clear();
            foreach (ConnectionViewModel cvm in ConnectionViewModels)
            {
                Global.Parameters.Connections.Add(cvm.Connection);
            }
        }
        private async void Create()
        {
            Connection connection = new Connection();
            Global.Parameters.Connections.Add(connection);
            ConnectionViewModel cvm = new ConnectionViewModel(Navigation, connection);
            ConnectionViewModels.Add(cvm);
            NewConnectionPage nc = new NewConnectionPage(cvm, true);
            await Navigation.PushAsync(nc);
        }

        public void Copy(object sender)
        {
            ConnectionViewModel source = (ConnectionViewModel)sender;
            Connection connection = new Connection();
            source.Connection.CopyTo(connection);
            connection.Connected = false;
            connection.Name += " Copy";
            Global.Parameters.Connections.Add(connection);
            ConnectionViewModel cvm = new ConnectionViewModel(Navigation, connection);
            ConnectionViewModels.Add(cvm);
            SelectedConnection = cvm;
        }

        public async void Edit(object sender)
        {
            ConnectionViewModel source = (ConnectionViewModel)sender;
            NewConnectionPage nc = new NewConnectionPage(source, false);
            await Navigation.PushAsync(nc);
        }

        public void Delete(object sender)
        {
            ConnectionViewModel source = (ConnectionViewModel)sender;
            Global.Parameters.Connections.Remove(source.Connection);
            ConnectionViewModels.Remove(source);
        }

        public void Select(ConnectionViewModel selected)
        {
            Global.Connect(selected.Name);
            selected.Connected = true;
            selected.Connection.Connected = true;
            foreach (ConnectionViewModel cvm in ConnectionViewModels)
            {
                if (cvm != selected)
                {
                    cvm.Connected = false;
                    cvm.Connection.Connected = false;
                }
            }
        }

        public override void DisposeModel()
        {
            foreach (ConnectionViewModel cvm in ConnectionViewModels)
            {
                cvm.DisposeModel();
            }
            ConnectionViewModels.Clear();
            base.DisposeModel();
        }
    }

}
