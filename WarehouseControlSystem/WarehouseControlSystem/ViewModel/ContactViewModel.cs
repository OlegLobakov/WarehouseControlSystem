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
using WarehouseControlSystem.ViewModel.Base;
using WarehouseControlSystem.Helpers.Containers.StateContainer;
using Xamarin.Forms;
using Plugin.Messaging;

namespace WarehouseControlSystem.ViewModel
{
    public class ContactViewModel : BaseViewModel
    {
        public string Name
        {
            get { return name1; }
            set
            {
                if (name1 != value)
                {
                    name1 = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        string name1;

        public string Email
        {
            get { return email; }
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged("Email");
                }
            }
        }
        string email;

        public string MessageType
        {
            get { return messagetype; }
            set
            {
                if (messagetype != value)
                {
                    messagetype = value;
                    OnPropertyChanged("MessageType");
                }
            }
        }
        string messagetype;

        public string Message
        {
            get { return message; }
            set
            {
                if (message != value)
                {
                    message = value;
                    OnPropertyChanged("Message");
                }
            }
        }
        string message;

        public List<string> MessageTypes { get; set; }

        public ContactViewModel(INavigation navigation) : base(navigation)
        {
            State = State.Normal;
            MessageTypes = new List<string>();
            MessageTypes.Add("Message");
            MessageTypes.Add("Bug report");
        }

        public void Send()
        {
            try
            {
                var emailTask = CrossMessaging.Current.EmailMessenger;
                if (emailTask.CanSendEmail)
                {
                    var email = new EmailMessageBuilder()
                        .To("oleg.lobakov@gmail.com")
                        .Subject(MessageType)
                        .Body(Message)
                        .Build();
                    emailTask.SendEmail(email);
                    State = State.Warning;
                    ErrorText = "Email was sent by default application";
                }
            }
            catch (Exception ex)
            {
                State = State.Error;
                ErrorText = ex.Message;
            }
        }

        public override void Dispose()
        {
            MessageTypes = null;
            base.Dispose();
        }
    }
}
