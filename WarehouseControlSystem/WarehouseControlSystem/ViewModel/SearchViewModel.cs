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
using System.Text;
using WarehouseControlSystem.ViewModel.Base;
using WarehouseControlSystem.Helpers.Containers.StateContainer;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.Model.NAV;
using Xamarin.Forms;
using WarehouseControlSystem.Helpers.NAV;
using System.Windows.Input;
using WarehouseControlSystem.Resx;
using System.Threading;

namespace WarehouseControlSystem.ViewModel
{
    public class SearchViewModel : BaseViewModel
    {
        public string CurrentRequest
        {
            get { return currentrequest; }
            set
            {
                if (currentrequest != value)
                {
                    currentrequest = value;
                    OnPropertyChanged(nameof(CurrentRequest));
                }
            }
        }
        string currentrequest; 

        public ICommand ClearCommand { protected set; get; }

        public SearchViewModel(INavigation navigation) : base(navigation)
        {
            State = State.Normal;
            ClearCommand = new Command(Clear);
            UpdateInformation();
        }

        public void UpdateInformation()
        {
            if (!string.IsNullOrEmpty(Global.SearchRequest))
            {
                if (Global.SearchResponses is List<SearchResponse>)
                {
                    CurrentRequest = 
                        AppResources.FindPage_Request + " " + 
                        Global.SearchRequest + " " + 
                        AppResources.FindPage_Finded + " "+
                        Global.SearchResponses.Count.ToString();
                }
                else
                {
                    CurrentRequest = 
                        AppResources.FindPage_Request + " " + 
                        Global.SearchRequest + " " + 
                        AppResources.FindPage_Finded + " 0";
                }
            }
            else
            {
                CurrentRequest = "";
            }
        }

        public async void Search(string request)
        {
            if (request.Length < 4)
            {
                State = State.Error;
                ErrorText = AppResources.FindPage_RequestLengthError;
                return;
            }
            try
            {
                State = State.Loading;
                LoadingText = AppResources.FindPage_Search;
                Global.SearchRequest = request;
                Global.SearchResponses = await NAV.Search(Global.SearchLocationCode, request, ACD.Default);
                string slc = Global.SearchLocationCode;
                List<SearchResponse> list = Global.SearchResponses;
                MessagingCenter.Send(this, "Search");
                LoadAnimation = true;
                await Navigation.PopAsync();
            }
            catch (OperationCanceledException ex)
            {
                ErrorText = ex.Message;
            }
            catch (Exception ex)
            {
                State = State.Error;
                ErrorText = ex.Message;
            }
            finally
            {
                LoadAnimation = false;
            }
        }

        public void Clear()
        {
            Global.SearchRequest = "";
            Global.SearchResponses.Clear();
            UpdateInformation();
        }

        public override void Dispose()
        {
            ClearCommand = null;
            base.Dispose();
        }

    }
}
