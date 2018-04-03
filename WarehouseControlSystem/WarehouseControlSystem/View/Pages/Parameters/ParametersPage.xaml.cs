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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WarehouseControlSystem.Resx;
using PCLStorage;

namespace WarehouseControlSystem.View.Pages.Parameters
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ParametersPage : ContentPage
    {
        public string LocalFilePath { get; set; }

        public int DefaultRackSections
        {
            get
            {
                return Settings.DefaultRackSections;
            }
            set
            {
                Settings.DefaultRackSections = Math.Min(50, Math.Max(1, value));
                OnPropertyChanged(nameof(DefaultRackSections));
            }
        }

        public int DefaultRackLevels
        {
            get
            {
                return Settings.DefaultRackLevels;
            }
            set
            {
                Settings.DefaultRackLevels = Math.Min(10, Math.Max(1, value));
                OnPropertyChanged(nameof(DefaultRackLevels));
            }
        }
        
        public int DefaultRackDepth
        {
            get
            {
                return Settings.DefaultRackDepth;
            }
            set
            {
                Settings.DefaultRackDepth = Math.Min(5, Math.Max(1, value));
                OnPropertyChanged(nameof(DefaultRackDepth));
            }
        }

        public int DefaultZonePlanWidth
        {
            get
            {
                return Settings.DefaultZonePlanWidth;
            }
            set
            {
                Settings.DefaultZonePlanWidth = Math.Min(100, Math.Max(1, value));  
                OnPropertyChanged(nameof(DefaultZonePlanWidth));
            }
        }

        public int DefaultZonePlanHeight
        {
            get
            {
                return Settings.DefaultZonePlanHeight;
            }
            set
            {
                Settings.DefaultZonePlanHeight = Math.Min(100, Math.Max(1, value));
                OnPropertyChanged(nameof(DefaultZonePlanHeight));
            }
        }

        public string DefaultRackSectionSeparator
        {
            get
            {
                return Settings.DefaultRackSectionSeparator;
            }
            set
            {
                Settings.DefaultRackSectionSeparator = value;
                OnPropertyChanged(nameof(DefaultRackSectionSeparator));
            }
        }

        public string DefaultSectionLevelSeparator
        {
            get
            {
                return Settings.DefaultSectionLevelSeparator;
            }
            set
            {
                Settings.DefaultSectionLevelSeparator = value;
                OnPropertyChanged(nameof(DefaultSectionLevelSeparator));
            }
        }

        public string DefaultLevelDepthSeparator
        {
            get
            {
                return Settings.DefaultLevelDepthSeparator;
            }
            set
            {
                Settings.DefaultLevelDepthSeparator = value;
                OnPropertyChanged(nameof(DefaultLevelDepthSeparator));
            }
        }

        public int DefaultLocationPlanWidth
        {
            get
            {
                return Settings.DefaultLocationPlanWidth;
            }
            set
            {
                Settings.DefaultLocationPlanWidth = Math.Min(100, Math.Max(1, value));
                OnPropertyChanged(nameof(DefaultLocationPlanWidth));
            }
        }

        public int DefaultLocationPlanHeight
        {
            get
            {
                return Settings.DefaultLocationPlanHeight;
            }
            set
            {
                Settings.DefaultLocationPlanHeight = Math.Min(100, Math.Max(1, value));
                OnPropertyChanged(nameof(DefaultLocationPlanHeight));
            }
        }

        List<LocaleSelector> Locales = new List<LocaleSelector>();


        public ParametersPage()
        {
            BindingContext = this;
            LocalFilePath = FileSystem.Current.LocalStorage.Path;
            InitializeComponent();
            Title = AppResources.ParametersPage_Title;

            


            Locales.Add(new LocaleSelector()
            {
                Code = "en",
                Name = "English"
            });
            Locales.Add(new LocaleSelector()
            {
                Code = "ru",
                Name = "Русский"
            });
            localizationpicker.ItemsSource = Locales;

            //текущая локаль
            LocaleSelector locale = Locales.Find(x => x.Code == Settings.CurrentLocalization);
            if (locale is LocaleSelector)
            {
                localizationpicker.SelectedItem = locale;
            }
            
        }

        private void localizationpicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex >= 0)
            {
                LocaleSelector selected = (LocaleSelector)picker.SelectedItem;
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(selected.Code);
                Settings.CurrentLocalization = selected.Code;
                Resx.AppResources.Culture = ci;                   // set the RESX for resource localization
                DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
            }
        }
    }
}