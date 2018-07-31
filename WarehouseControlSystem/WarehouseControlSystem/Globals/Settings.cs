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
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace WarehouseControlSystem
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static string GeneralSettings
        {
            get => AppSettings.GetValueOrDefault(nameof(GeneralSettings), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(GeneralSettings), value);
        }

        public static string Locale
        {
            get => AppSettings.GetValueOrDefault(nameof(Locale), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(Locale), value);
        }

        public static int DefaultRackSections
        {
            get => AppSettings.GetValueOrDefault(nameof(DefaultRackSections), 15);

            set => AppSettings.AddOrUpdateValue(nameof(DefaultRackSections), value);
        }

        public static int DefaultRackLevels
        {
            get => AppSettings.GetValueOrDefault(nameof(DefaultRackLevels), 3);

            set => AppSettings.AddOrUpdateValue(nameof(DefaultRackLevels), value);
        }

        public static int DefaultRackDepth
        {
            get => AppSettings.GetValueOrDefault(nameof(DefaultRackDepth), 1);

            set => AppSettings.AddOrUpdateValue(nameof(DefaultRackDepth), value);
        }

        public static string DefaultRackSectionSeparator
        {
            get => AppSettings.GetValueOrDefault(nameof(DefaultRackSectionSeparator), "");

            set => AppSettings.AddOrUpdateValue(nameof(DefaultRackSectionSeparator), value);
        }

        public static string DefaultSectionLevelSeparator
        {
            get => AppSettings.GetValueOrDefault(nameof(DefaultSectionLevelSeparator), "");

            set => AppSettings.AddOrUpdateValue(nameof(DefaultSectionLevelSeparator), value);
        }

        public static string DefaultLevelDepthSeparator
        {
            get => AppSettings.GetValueOrDefault(nameof(DefaultLevelDepthSeparator), "");

            set => AppSettings.AddOrUpdateValue(nameof(DefaultLevelDepthSeparator), value);
        }

        public static int DefaultZonePlanWidth
        {
            get => AppSettings.GetValueOrDefault(nameof(DefaultZonePlanWidth), 50);

            set => AppSettings.AddOrUpdateValue(nameof(DefaultZonePlanWidth), value);
        }

        public static int DefaultZonePlanHeight
        {
            get => AppSettings.GetValueOrDefault(nameof(DefaultZonePlanHeight), 30);

            set => AppSettings.AddOrUpdateValue(nameof(DefaultZonePlanHeight), value);
        }

        public static int DefaultLocationPlanWidth
        {
            get => AppSettings.GetValueOrDefault(nameof(DefaultLocationPlanWidth), 50);

            set => AppSettings.AddOrUpdateValue(nameof(DefaultLocationPlanWidth), value);
        }

        public static int DefaultLocationPlanHeight
        {
            get => AppSettings.GetValueOrDefault(nameof(DefaultLocationPlanHeight), 30);

            set => AppSettings.AddOrUpdateValue(nameof(DefaultLocationPlanHeight), value);
        }

        public static string DefaultLocation
        {
            get => AppSettings.GetValueOrDefault(nameof(DefaultLocation), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(DefaultLocation), value);
        }


        public static string CurrentLocalization
        {
            get => AppSettings.GetValueOrDefault(nameof(CurrentLocalization), "en");

            set => AppSettings.AddOrUpdateValue(nameof(CurrentLocalization), value);
        }

        public static string CurrentConnection
        {
            get => AppSettings.GetValueOrDefault(nameof(CurrentConnection), "");

            set => AppSettings.AddOrUpdateValue(nameof(CurrentConnection), value);
        }

        public static bool ShowImages
        {
            get => AppSettings.GetValueOrDefault(nameof(ShowImages), true);

            set => AppSettings.AddOrUpdateValue(nameof(ShowImages), value);
        }
    }


}
