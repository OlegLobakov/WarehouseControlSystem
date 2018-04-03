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
using System.Net;

namespace WarehouseControlSystem.Helpers.NAV
{
    /// <summary>
    /// Connection to Microsoft Dynamics NAV
    /// </summary>
    public class Connection
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public bool Https { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public string Instance { get; set; }
        public string Company { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Domen { get; set; }
        public bool Verified { get; set; }
        public bool Connected { get; set; }
        public string Codeunit { get; set; } = "Codeunit/WarehouseControlManagement";
        public ClientCredentialTypeEnum ClientCredentialType { get; set; }

        public NetworkCredential GetCreditials()
        {
            return new NetworkCredential(User, Password, Domen);
        }

        public Uri GetUri()
        {
            UriBuilder uriBuilder = new UriBuilder();
            if (Https)
            {
                uriBuilder.Scheme = "https";
            }
            else
            {
                uriBuilder.Scheme = "http";
            }
            uriBuilder.Host = Server;
            uriBuilder.Path = string.Format("{0}/WS/{1}/{2}", Instance, Company, Codeunit);
            uriBuilder.Port = Port;
            return uriBuilder.Uri;
        }

        public string GetSoapActionTxt()
        {
            UriBuilder uriBuilder = new UriBuilder();
            if (Https)
            {
                uriBuilder.Scheme = "https";
            }
            else
            {
                uriBuilder.Scheme = "http";
            }
            uriBuilder.Host = Server;
            uriBuilder.Path = string.Format("{0}/WS/{1}/{2}", Instance, SOAPActionConverter(Company), Codeunit);
            uriBuilder.Port = Port;
            return uriBuilder.Uri.ToString();
        }

        private string SOAPActionConverter(string input)
        {
            string rv = "";
            foreach (var item in input.ToCharArray())
            {
                int x = (int)item;
                if (x >= 63 && x <= 126)
                {
                    rv += item;
                }
                else if (x >= 48 && x <= 57)
                {
                    rv += item;
                }
                else
                {
                    rv += "?";
                }
            }
            return rv;
        }        

        public Connection GetCopy()
        {
            return (new Connection()
            {
                Name = Name,
                Comment = Comment,
                Https = Https,
                Server = Server,
                Port = Port,
                Instance = Instance,
                Company = Company,
                User = User,
                Password = Password,
                Domen = Domen,
                Verified = Verified,
                Connected = Connected,
                ClientCredentialType = ClientCredentialType,
                Codeunit = Codeunit
            });
        }

        public void CopyTo(Connection to)
        {
            to.Name = Name;
            to.Https = Https;
            to.Server = Server;
            to.Port = Port;
            to.Instance = Instance;
            to.Company = Company;
            to.User = User;
            to.Password = Password;
            to.Domen = Domen;
            to.Verified = Verified;
            to.Connected = Connected;
            to.ClientCredentialType = ClientCredentialType;
            to.Codeunit = Codeunit;
        }
    }
}
