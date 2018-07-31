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
using WarehouseControlSystem.Model;
using WarehouseControlSystem.Model.NAV;
using Xamarin.Forms;
using WarehouseControlSystem.Helpers.NAV;
using System.Windows.Input;
using WarehouseControlSystem.Resx;
using System.Threading.Tasks;

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
            ClearCommand = new Command(Clear);
            UpdateInformation();
            State = ModelState.Undefined;
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

        public async Task Search(string request)
        {
            if (request.Length < 4)
            {
                State = ModelState.Error;
                ErrorText = AppResources.FindPage_RequestLengthError;
                return;
            }

            try
            {
                State = ModelState.Loading;
                LoadingText = AppResources.FindPage_Search;
                Global.SearchRequest = request;
                Global.SearchResponses = await NAV.Search(Global.SearchLocationCode, request, ACD.Default);
                if (NotDisposed)
                {
                    LoadAnimation = true;
                    await Navigation.PopAsync();
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                State = ModelState.Error;
                ErrorText = e.Message;
            }
        }

        public void Clear()
        {
            Global.SearchRequest = "";
            Global.SearchResponses.Clear();
            UpdateInformation();
        }
    }
}
