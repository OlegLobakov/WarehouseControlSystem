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
using System.Collections.ObjectModel;
using Xamarin.Forms;
using WarehouseControlSystem.Helpers.NAV;
using WarehouseControlSystem.View.Pages.Connections;
using WarehouseControlSystem.Resx;
using WarehouseControlSystem.ViewModel.Base;
using System.Windows.Input;

namespace WarehouseControlSystem.ViewModel
{
    public class ConnectionViewModel : BaseViewModel
    {
        public Connection Connection { get; set; }

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        } string name;
        public string Comment
        {
            get { return comment; }
            set
            {
                if (comment != value)
                {
                    comment = value;
                    OnPropertyChanged(nameof(Comment));
                }
            }
        } string comment;
        public bool Https
        {
            get { return https; }
            set
            {
                if (https != value)
                {
                    https = value;
                    OnPropertyChanged(nameof(Https));
                }
            }
        } bool https;
        public string Server
        {
            get { return server; }
            set
            {
                if (server != value)
                {
                    server = value;
                    OnPropertyChanged(nameof(Server));
                }
            }
        } string server;
        public int Port
        {
            get { return port; }
            set
            {
                if (port != value)
                {
                    port = value;
                    OnPropertyChanged(nameof(Port));
                }
            }
        } int port;
        public string Instance
        {
            get { return instance; }
            set
            {
                if (instance != value)
                {
                    instance = value;
                    OnPropertyChanged(nameof(Instance));
                }
            }
        } string instance;
        public string Company
        {
            get { return company; }
            set
            {
                if (company != value)
                {
                    company = value;
                    OnPropertyChanged(nameof(Company));
                }
            }
        } string company;
        public string User
        {
            get { return user; }
            set
            {
                if (user != value)
                {
                    user = value;
                    OnPropertyChanged(nameof(User));
                }
            }
        } string user;
        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        } string password;
        public ClientCredentialTypeEnum ClientCredentialType
        {
            get { return clientcreditialtype; }
            set
            {
                if (clientcreditialtype != value)
                {
                    clientcreditialtype = value;
                    OnPropertyChanged(nameof(ClientCredentialType));
                }
            }
        } ClientCredentialTypeEnum clientcreditialtype;
        public string Domen
        {
            get { return domen; }
            set
            {
                if (domen != value)
                {
                    domen = value;
                    OnPropertyChanged(nameof(Domen));
                }
            }
        } string domen;
        public bool Verified
        {
            get { return verified; }
            set
            {
                if (verified != value)
                {
                    verified = value;
                    OnPropertyChanged(nameof(Verified));
                }
            }
        } bool verified;
        public bool Connected
        {
            get { return connected; }
            set
            {
                if (connected != value)
                {
                    connected = value;
                    OnPropertyChanged(nameof(Connected));
                }
            }
        } bool connected;

        public bool CreateMode
        {
            get { return createmode; }
            set
            {
                if (createmode != value)
                {
                    createmode = value;
                    OnPropertyChanged(nameof(CreateMode));
                }
            }
        } bool createmode;

        public List<ClientCredentialTypeEnum> CreditialList { get; set; }

        public string Codeunit
        {
            get { return codeunit; }
            set
            {
                if (codeunit != value)
                {
                    codeunit = value;
                    OnPropertyChanged(nameof(Codeunit));
                }
            }
        }  string codeunit;

        public ICommand TestConnectionCommand { protected set; get; }

        public ConnectionViewModel(INavigation navigation, Connection connection) : base(navigation)
        {
            CancelChangesCommand = new Command(CancelChanges);
            FillFields(connection);
            Connection = connection;

            CreditialList = Global.CreditialList;
            TestConnectionCommand = new Command(TestConnection);
            State = ModelState.Undefined;
        }

        public void FillFields(Connection connection)
        {
            Name = connection.Name;
            Comment = connection.Comment;
            Https = connection.Https;
            Server = connection.Server;
            Port = connection.Port;
            Instance = connection.Instance;
            Company = connection.Company;
            User = connection.User;
            Password = connection.Password;
            Domen = connection.Domen;
            Verified = connection.Verified;
            Connected = connection.Connected;
            Codeunit = connection.Codeunit;
            ClientCredentialType = connection.ClientCredentialType;
        }

        public void SaveFields(Connection connection)
        {
            connection.Name = Name;
            connection.Comment = Comment;
            connection.Https = Https;
            connection.Server = Server;
            connection.Port = Port;
            connection.ClientCredentialType = ClientCredentialType;
            connection.Instance = Instance;
            connection.Company = Company;
            connection.User = User;
            connection.Password = Password;
            connection.Domen = Domen;
            connection.Verified = Verified;
            connection.Connected = Connected;
            connection.Codeunit = Codeunit;
        }

        public void SaveChanges()
        {
            SaveFields(Connection);
        }

        public void CancelChanges()
        {
            FillFields(Connection);
        }

        public async void TestConnection()
        {
            State = ModelState.Loading;
            Title = AppResources.NewConnectionPage_Test;
            SaveFields(Global.TestConnection);
            try
            {
                await NAV.TestConnection(ACD.Default).ConfigureAwait(true);
                Verified = true;
                Connection.Verified = true;
                State = ModelState.Normal;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                Verified = false;
                Connection.Verified = false;
                State = ModelState.Error;
                ErrorText = e.Message;
            }
            Title = AppResources.NewConnectionPage_Title;
        }
    }
}
