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
using System.Text;
using WarehouseControlSystem.ViewModel.Base;
using Xamarin.Forms;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.Helpers.NAV;
using WarehouseControlSystem.Resx;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace WarehouseControlSystem.ViewModel
{
    public class IndicatorViewModel :  BaseViewModel
    {
        public string Header
        {
            get { return header; }
            set
            {
                if (header != value)
                {
                    header = value;
                    OnPropertyChanged(nameof(Header));
                }
            }
        } string header;
        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        } string description;
        public Color ValueColor
        {
            get { return color; }
            set
            {
                if (color != value)
                {
                    color = value;
                    OnPropertyChanged(nameof(Color));
                }
            }
        } Color color;
        public string Value
        {
            get { return value1; }
            set
            {
                if (value1 != value)
                {
                    value1 = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        } string value1;
        public int Position
        {
            get { return position; }
            set
            {
                if (position != value)
                {
                    position = value;
                    OnPropertyChanged(nameof(Position));
                }
            }
        } int position;
        public bool IsURLExist
        {
            get { return isurlexist; }
            set
            {
                if (isurlexist != value)
                {
                    isurlexist = value;
                    OnPropertyChanged(nameof(IsURLExist));
                }
            }
        } bool isurlexist;
        public string URL
        {
            get { return url; }
            set
            {
                if (url != value)
                {
                    url = value;
                    OnPropertyChanged(nameof(URL));
                }
            }
        } string url;

        public string ID
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(ID));
                }
            }
        }
        string id;

        public string Parameters
        {
            get { return parameters; }
            set
            {
                if (parameters != value)
                {
                    parameters = value;
                    OnPropertyChanged(nameof(Parameters));
                }
            }
        }
        string parameters;

        public string LocationCode
        {
            get { return locationcode; }
            set
            {
                if (locationcode != value)
                {
                    locationcode = value;
                    OnPropertyChanged(nameof(LocationCode));
                }
            }
        }
        string locationcode = "";
        public string ZoneCode
        {
            get { return zondecode; }
            set
            {
                if (zondecode != value)
                {
                    zondecode = value;
                    OnPropertyChanged(nameof(ZoneCode));
                }
            }
        }
        string zondecode = "";

        public string BinCode
        {
            get { return bincode; }
            set
            {
                if (bincode != value)
                {
                    bincode = value;
                    OnPropertyChanged(nameof(BinCode));
                }
            }
        }
        string bincode = "";

        public ObservableCollection<IndicatorContentViewModel> Content
        {
            get { return indicatorcontent; }
            set
            {
                if (indicatorcontent != value)
                {
                    indicatorcontent = value;
                    OnPropertyChanged(nameof(Content));
                }
            }
        }
        ObservableCollection<IndicatorContentViewModel> indicatorcontent;

        public ICommand TapCommand { protected set; get; }
        public event Action<IndicatorViewModel> OnTap;

        public IndicatorViewModel(INavigation navigation, Indicator indicator) : base(navigation)
        {
            Title = "Content";
            FillFields(indicator);
            TapCommand = new Command<object>(Tap);
            Content = new ObservableCollection<IndicatorContentViewModel>();
        }

        public void FillFields(Indicator indicator)
        {
            Header = indicator.Header;
            Description = indicator.Description;
            Value = indicator.Value;
            ValueColor = Color.FromHex(indicator.ValueColor);
            URL = indicator.URL;
            IsURLExist = !string.IsNullOrEmpty(indicator.URL);
            ID = indicator.ID;
            Parameters = indicator.Parameters;
        }

        /// <summary>
        /// Load Indicator Content
        /// </summary>
        /// <returns></returns>
        public async Task LoadContent()
        {
            if (NotNetOrConnection)
            {
                return;
            }

            try
            {
                State = ModelState.Loading;
                List<IndicatorContent> indicatorcontents = await NAV.GetIndicatorContent(LocationCode, ZoneCode, BinCode, ID, Parameters, ACD.Default).ConfigureAwait(true);
                if ((NotDisposed) && (indicatorcontents is List<IndicatorContent>))
                {
                    FillContent(indicatorcontents);
                }
            }
            catch (Exception e)
            {
                State = ModelState.Error;
                //ErrorText = AppResources.Error_LoadRacks;
                ErrorText = e.Message; // AppResources.Error_LoadRacks;
            }
        }
        private void FillContent(List<IndicatorContent> indicatorcontents)
        {
            if (indicatorcontents.Count > 0)
            {
                Content.Clear();
                State = ModelState.Normal;

                List<IndicatorContentViewModel> list1 = new List<IndicatorContentViewModel> ();
                foreach (IndicatorContent ic in indicatorcontents)
                {
                    IndicatorContentViewModel icm = new IndicatorContentViewModel(Navigation, ic);
                    icm.IsShowDetail = Settings.ShowIndicatorDetailDescription;
                    list1.Add(icm);
                }
                list1.Sort((icm1, icm2) => icm1.SortOrder.CompareTo(icm2.SortOrder));

                ObservableCollection<IndicatorContentViewModel> nlist = new ObservableCollection<IndicatorContentViewModel>();
                foreach (IndicatorContentViewModel icvm in list1)
                {
                    nlist.Add(icvm);
                }
                 
                Content = nlist;
                State = ModelState.Normal;
                MessagingCenter.Send(this, "ContentIsLoaded");
            }
            else
            {
                State = ModelState.NoData;
            }
        }

        public void Tap(object sender)
        {
            if (!string.IsNullOrEmpty(URL))
            {
                try
                {
                    Uri uri = new Uri(URL);
                    Device.OpenUri(uri);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            if (OnTap is Action<IndicatorViewModel>)
            {
                OnTap(this);
            }
        }
    }
}
