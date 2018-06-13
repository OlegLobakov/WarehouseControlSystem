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
using WarehouseControlSystem.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WarehouseControlSystem.Resx;
using WarehouseControlSystem.Pages;
using WarehouseControlSystem.Helpers.NAV;
using WarehouseControlSystem.Model.NAV;
using PCLStorage;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Globalization;

namespace WarehouseControlSystem
{
    /// <summary>
    /// Globals
    /// </summary>
    public class Global
    {
        public static MasterDetailPage MainPage { get; set; }
        
        public static List<ColorPick> Colors { get; set; } = new List<ColorPick>();
        public static List<RackOrientationPick> OrientationList { get; set; } = new List<RackOrientationPick>();
        public static List<ClientCredentialTypeEnum> CreditialList { get; set; } = new List<ClientCredentialTypeEnum>();

        static string parametersfilename = "Parameters.xml";
        public static Parameters Parameters { get; set; }

        public static List<SearchResponse> SearchResponses { get; set; } = new List<SearchResponse>();
        public static string SearchLocationCode { get; set; } = "";
        public static string SearchRequest { get; set; }

        public static CultureInfo Culture { get; set; }

        public static string CompliantPlug { get; set; }

        public static void Init()
        {
            LoadParameters();

            Colors.Add(new ColorPick
            {
                HexColor = "#e5e5e5",
                Name = AppResources.NewZonePage_Color_White,
                Color = Color.FromHex("#e5e5e5")
            });

            Colors.Add(new ColorPick
            {
                HexColor = "#4775a3",
                Name = AppResources.NewZonePage_Color_Blue,
                Color = Color.FromHex("#4775a3")
            });
            Colors.Add(new ColorPick
            {
                HexColor = "#84a3c1",
                Name = AppResources.NewZonePage_Color_BlueLight,
                Color = Color.FromHex("#84a3c1")
            });
            Colors.Add(new ColorPick
            {
                HexColor = "#4d7326",
                Name = AppResources.NewZonePage_Color_Green,
                Color = Color.FromHex("#4d7326")
            });
            Colors.Add(new ColorPick
            {
                HexColor = "#73ac39",
                Name = AppResources.NewZonePage_Color_Green_Light,
                Color = Color.FromHex("#73ac39")
            });
            Colors.Add(new ColorPick
            {
                HexColor = "#b41848",
                Name = AppResources.NewZonePage_Color_Red,
                Color = Color.FromHex("#b41848")
            });
            Colors.Add(new ColorPick
            {
                HexColor = "#c3466c",
                Name = AppResources.NewZonePage_Color_RedLight,
                Color = Color.FromHex("#c3466c")
            });

            Colors.Add(new ColorPick
            {
                HexColor = "#808080",
                Name = AppResources.Colors_Gray1,
                Color = Color.FromHex("#808080")
            });
            Colors.Add(new ColorPick
            {
                HexColor = "#737373",
                Name = AppResources.Colors_Gray2,
                Color = Color.FromHex("#737373")
            });
            Colors.Add(new ColorPick
            {
                HexColor = "#666666",
                Name = AppResources.Colors_Gray3,
                Color = Color.FromHex("#666666")
            });

            OrientationList.Add(new RackOrientationPick
            {
                RackOrientation = RackOrientationEnum.Undefined,
                Name = AppResources.RackNewPage_OrientationUndefined
            });

            OrientationList.Add(new RackOrientationPick
            {
                RackOrientation = RackOrientationEnum.HorizontalLeft,
                Name = AppResources.RackNewPage_OrientationRackHL
            });

            OrientationList.Add(new RackOrientationPick
            {
                RackOrientation = RackOrientationEnum.HorizontalRight,
                Name = AppResources.RackNewPage_OrientationRackHR,
            });

            OrientationList.Add(new RackOrientationPick
            {
                RackOrientation = RackOrientationEnum.VerticalUp,
                Name = AppResources.RackNewPage_OrientationRackVU
            });

            OrientationList.Add(new RackOrientationPick
            {
                RackOrientation = RackOrientationEnum.VerticalDown,
                Name = AppResources.RackNewPage_OrientationRackVD
            });

            CreditialList.Add(
                ClientCredentialTypeEnum.Basic
            );

            CreditialList.Add(
                ClientCredentialTypeEnum.Windows
            );
        }

        public static string GetPlatformPath(string source)
        {
            string rv = "";
            switch (Device.RuntimePlatform)
            {
                case Device.UWP:
                    rv = "Assets/Images/" + source;
                    break;
                default:
                    rv = source;
                    break;
            }
            return rv;
        }

        /// <summary>
        /// Current Connection
        /// </summary>
        public static Connection CurrentConnection { get; set; }
        public static Connection TestConnection { get; set; } = new Connection();

        /// <summary>
        /// Sync version 
        /// </summary>
        public static void LoadParameters()
        {
            Parameters = new Parameters();
            var result = Task.Run(async () =>
            {
                bool exist = await Storage.IsFileExistAsync(parametersfilename, null);
                if (exist)
                {
                    IFolder folder = FileSystem.Current.LocalStorage;
                    IFile file = await folder.GetFileAsync(parametersfilename);
                    if (file is IFile)
                    {
                        XmlSerializer x = new XmlSerializer(typeof(Parameters));
                        using (System.IO.Stream stream = await file.OpenAsync(FileAccess.ReadAndWrite))
                        {
                            try
                            {
                                Parameters = (Parameters)x.Deserialize(stream);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                return 0;
            }).Result;
        }


        public static void SaveParameters()
        {
            var result = Task.Run(async () =>
            {
                if (Parameters is Parameters)
                {
                    bool exist = await Storage.IsFileExistAsync(parametersfilename, null);
                    if (exist)
                    {
                        await Storage.DeleteFile(parametersfilename, null);
                    }
                    IFile file = await Storage.CreateFile(parametersfilename, null);
                    XmlSerializer x = new XmlSerializer(typeof(Parameters));
                    using (System.IO.Stream stream = await file.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
                    {
                        x.Serialize(stream, Parameters);
                    }
                }
                return 0;
            }).Result;
        }

        public static void Connect(string connectionname)
        {
            Settings.CurrentConnection = "";
            CurrentConnection = null;
            if (Parameters is Parameters)
            {
                if (Parameters.Connections is List<Connection>)
                {
                    Connection selected = Parameters.Connections.Find(x => x.Name == connectionname);
                    if (selected is Connection)
                    {
                        Settings.CurrentConnection = selected.Name;
                        CurrentConnection = selected;
                    }
                }
            }
        }

        public static void SetCurrentConnection()
        {
            if (!String.IsNullOrEmpty(Settings.CurrentConnection))
            {
                Connect(Settings.CurrentConnection);
            }
        }
    }
}
