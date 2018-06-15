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
using System.Xml.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.Model;
using Xamarin.Forms;

namespace WarehouseControlSystem.Helpers.NAV
{
    public class NAV
    {
        static XNamespace ns = "http://schemas.xmlsoap.org/soap/envelope/";

        private static XNamespace GetNameSpace()
        {
            XNamespace myns = "urn:microsoft-dynamics-schemas/codeunit/" + Global.CurrentConnection.Codeunit;
            return myns;
        }

        public static Task<int> GetPlanWidth(CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "GetPlanWidth";
                        XElement body = new XElement(myns + functionname);
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        int rv = StringToInt(soapbodynode.Value);
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                       tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<int> GetPlanHeight(CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "GetPlanHeight";
                        XElement body = new XElement(myns + functionname);
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        int rv = StringToInt(soapbodynode.Value);
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<int> SetPlanWidth(int value, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "SetPlanWidth";
                        XElement body = new XElement(myns + functionname,
                            new XElement(myns + "value", value));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false); 
                        int rv = StringToInt(soapbodynode.Value);
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<int> SetPlanHeight(int value, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "SetPlanHeight";
                        XElement body = new XElement(myns + functionname,
                            new XElement(myns + "value", value));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false); 
                        int rv = StringToInt(soapbodynode.Value);
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<int> APIVersion(CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "APIVersion";
                        XElement body = new XElement(myns + functionname);
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        int rv = StringToInt(soapbodynode.Value);
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }

        #region Location
        public static Task<int> CreateLocation(Location location, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                    {
                        try
                        {
                            XNamespace myns = GetNameSpace();
                            string functionname = "CreateLocation";
                            XElement body =
                                new XElement(myns + functionname,
                                            new XElement(myns + "code", location.Code),
                                            new XElement(myns + "name", location.Name),
                                            new XElement(myns + "address", location.Address),
                                            new XElement(myns + "phoneNo", location.PhoneNo),
                                            new XElement(myns + "hexColor", location.HexColor),
                                            new XElement(myns + "planWidth", location.PlanWidth),
                                            new XElement(myns + "planHeight", location.PlanHeight),
                                            new XElement(myns + "left", location.Left),
                                            new XElement(myns + "top", location.Top),
                                            new XElement(myns + "width", location.Width),
                                            new XElement(myns + "height", location.Height),
                                            new XElement(myns + "schemeVisible", location.SchemeVisible),
                                            new XElement(myns + "binMandatory", location.BinMandatory),
                                            new XElement(myns + "requireReceive", location.RequireReceive),
                                            new XElement(myns + "requireShipment", location.RequireShipment),
                                            new XElement(myns + "requirePick", location.RequirePick),
                                            new XElement(myns + "requirePutaway", location.RequirePutaway));
                            XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                            tcs.SetResult(0);
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e.Message);
                            tcs.SetException(e);
                        }
                    });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<int> ModifyLocation(Location location, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "ModifyLocation";
                        XElement body =
                            new XElement(myns + functionname,
                                        new XElement(myns + "code", location.Code),
                                        new XElement(myns + "prevCode", location.PrevCode),
                                        new XElement(myns + "name", location.Name),
                                        new XElement(myns + "address", location.Address),
                                        new XElement(myns + "phoneNo", location.PhoneNo),
                                        new XElement(myns + "hexColor", location.HexColor),
                                        new XElement(myns + "planWidth", location.PlanWidth),
                                        new XElement(myns + "planHeight", location.PlanHeight),
                                        new XElement(myns + "left", location.Left),
                                        new XElement(myns + "top", location.Top),
                                        new XElement(myns + "width", location.Width),
                                        new XElement(myns + "height", location.Height),
                                        new XElement(myns + "schemeVisible", location.SchemeVisible),
                                        new XElement(myns + "binMandatory", location.BinMandatory),
                                        new XElement(myns + "requireReceive", location.RequireReceive),
                                        new XElement(myns + "requireShipment", location.RequireShipment),
                                        new XElement(myns + "requirePick", location.RequirePick),
                                        new XElement(myns + "requirePutaway", location.RequirePutaway));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        tcs.SetResult(0);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<int> SetLocationVisible(Location location, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "SetLocationVisible";
                        XElement body =
                        new XElement(myns + functionname,
                                    new XElement(myns + "code", location.Code),
                                    new XElement(myns + "visible", location.SchemeVisible.ToString()));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        tcs.SetResult(0);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<int> DeleteLocation(string locationcode, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                int rv = 0;
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "DeleteLocation";
                        XElement body =
                            new XElement(myns + functionname,
                                        new XElement(myns + "name", locationcode));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        rv = StringToInt(soapbodynode.Value);
                        tcs.SetResult(0);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<int> GetLocationCount(string codefilter, bool onlyvisibled, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                int rv = 0;
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "GetLocationCount";
                        XElement body =
                            new XElement(myns + functionname,
                                        new XElement(myns + "codeFilter", codefilter),
                                        new XElement(myns + "onlyVisibled", onlyvisibled.ToString()));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        rv = StringToInt(soapbodynode.Value);
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<List<Location>> GetLocationList(string codefilter, bool onlyvisibled, int position, int count, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<Location>>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "GetLocationList";
                        XElement body =
                        new XElement(myns + functionname,
                        new XElement(myns + "codeFilter", codefilter),
                        new XElement(myns + "onlyVisibled", onlyvisibled.ToString()),
                        new XElement(myns + "entriesPosition", position),
                        new XElement(myns + "entriesCount", count),
                        new XElement(myns + "responseDocument", ""));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false); 
                        List<Location> rv = new List<Location>();
                        string response = soapbodynode.Value;
                        XDocument document = GetDoc(response);
                        foreach (XElement currentnode in document.Root.Elements())
                        {
                            Location location = GetLocationFromXML(currentnode);
                            rv.Add(location);
                        }
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(null);
            }
            return tcs.Task;
        }
        private static Location GetLocationFromXML(XElement currentnode)
        {
            Location location = new Location();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "Code":
                        {
                            location.Code = currentatribute.Value;
                            location.PrevCode = location.Code;
                            break;
                        }
                    case "Name":
                        {
                            location.Name = currentatribute.Value;
                            break;
                        }
                    case "Address":
                        {
                            location.Address = currentatribute.Value;
                            break;
                        }
                    case "PhoneNo":
                        {
                            location.PhoneNo = currentatribute.Value;
                            break;
                        }
                    case "HexColor":
                        {
                            location.HexColor = currentatribute.Value;
                            break;
                        }
                    case "Left":
                        {
                            location.Left = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "Top":
                        {
                            location.Top = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "Width":
                        {
                            location.Width = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "Height":
                        {
                            location.Height = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "PlanWidth":
                        {
                            location.PlanWidth = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "PlanHeight":
                        {
                            location.PlanHeight = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "BinMandatory":
                        {
                            location.BinMandatory = StringToBool(currentatribute.Value);
                            break;
                        }
                    case "RequireReceive":
                        {
                            location.RequireReceive = StringToBool(currentatribute.Value);
                            break;
                        }
                    case "RequireShipment":
                        {
                            location.RequireShipment = StringToBool(currentatribute.Value);
                            break;
                        }
                    case "RequirePick":
                        {
                            location.RequirePick = StringToBool(currentatribute.Value);
                            break;
                        }
                    case "RequirePutaway":
                        {
                            location.RequirePutaway = StringToBool(currentatribute.Value);
                            break;
                        }
                    case "UseAsInTransit":
                        {
                            location.Transit = StringToBool(currentatribute.Value);
                            break;
                        }
                    case "ZoneQuantity":
                        {
                            location.ZoneQuantity = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "BinQuantity":
                        {
                            location.BinQuantity = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "SchemeVisible":
                        {
                            location.SchemeVisible = StringToBool(currentatribute.Value);
                            break;
                        }
                    default:
                        throw new InvalidOperationException("Impossible value GetLocationFromXML > Name: " + currentatribute.Name.LocalName);
                }
            }
            return location;
        }
        #endregion

        #region Zone
        public static Task<int> CreateZone(Zone zone, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "CreateZone";
                        XElement body =
                        new XElement(myns + functionname,
                        new XElement(myns + "locationCode", zone.LocationCode),
                        new XElement(myns + "code", zone.Code),
                        new XElement(myns + "description", zone.Description),
                        new XElement(myns + "binTypeCode", zone.BinTypeCode),
                        new XElement(myns + "zoneRanking", zone.ZoneRanking),
                        new XElement(myns + "crossDockBinZone", zone.CrossDockBinZone),
                        new XElement(myns + "specialEquipmentCode", zone.SpecialEquipmentCode),
                        new XElement(myns + "warehouseClassCode", zone.WarehouseClassCode),
                        new XElement(myns + "hexColor", zone.HexColor),
                        new XElement(myns + "planWidth", zone.PlanWidth),
                        new XElement(myns + "planHeight", zone.PlanHeight),
                        new XElement(myns + "left", zone.Left),
                        new XElement(myns + "top", zone.Top),
                        new XElement(myns + "width", zone.Width),
                        new XElement(myns + "height", zone.Height),
                        new XElement(myns + "schemeVisible", zone.SchemeVisible));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        tcs.SetResult(0);
                    }
                    catch (Exception e)
                    {
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<int> ModifyZone(Zone zone, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "ModifyZone";
                        XElement body =
                            new XElement(myns + functionname,
                            new XElement(myns + "locationCode", zone.LocationCode),
                            new XElement(myns + "code", zone.Code),
                            new XElement(myns + "prevCode", zone.PrevCode),
                            new XElement(myns + "description", zone.Description),
                            new XElement(myns + "binTypeCode", zone.BinTypeCode),
                            new XElement(myns + "zoneRanking", zone.ZoneRanking),
                            new XElement(myns + "crossDockBinZone", zone.CrossDockBinZone),
                            new XElement(myns + "specialEquipmentCode", zone.SpecialEquipmentCode),
                            new XElement(myns + "warehouseClassCode", zone.WarehouseClassCode),
                            new XElement(myns + "hexColor", zone.HexColor),
                            new XElement(myns + "planWidth", zone.PlanWidth),
                            new XElement(myns + "planHeight", zone.PlanHeight),
                            new XElement(myns + "left", zone.Left),
                            new XElement(myns + "top", zone.Top),
                            new XElement(myns + "width", zone.Width),
                            new XElement(myns + "height", zone.Height),
                            new XElement(myns + "schemeVisible", zone.SchemeVisible));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        tcs.SetResult(0);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<int> SetZoneVisible(Zone zone, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "SetZoneVisible";
                        XElement body =
                            new XElement(myns + functionname,
                            new XElement(myns + "locationCode", zone.LocationCode),
                            new XElement(myns + "code", zone.Code),
                            new XElement(myns + "visible", zone.SchemeVisible));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        tcs.SetResult(0);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<int> DeleteZone(Zone zone, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "DeleteZone";
                        XElement body =
                        new XElement(myns + functionname,
                        new XElement(myns + "locationCode", zone.LocationCode),
                        new XElement(myns + "code", zone.Code));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        tcs.SetResult(0);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<int> GetZoneCount(string locationfilter, string codefilter, bool onlyvisibled, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                int rv = 0;
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "GetZoneCount";
                        XElement body =
                            new XElement(myns + functionname,
                                        new XElement(myns + "locationFilter", locationfilter),
                                        new XElement(myns + "codeFilter", codefilter),
                                        new XElement(myns + "onlyVisibled", onlyvisibled));

                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        rv = StringToInt(soapbodynode.Value);
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<List<Zone>> GetZoneList(string locationfilter, string codefilter, bool onlyvisibled, int position, int count, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<Zone>>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "GetZoneList";
                        XElement body =
                        new XElement(myns + functionname,
                        new XElement(myns + "locationFilter", locationfilter),
                        new XElement(myns + "codeFilter", codefilter),
                        new XElement(myns + "onlyVisibled", onlyvisibled),
                        new XElement(myns + "entriesPosition", position),
                        new XElement(myns + "entriesCount", count),
                        new XElement(myns + "responseDocument", ""));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        string response = soapbodynode.Value;
                        XDocument document = GetDoc(response);
                        List<Zone> rv = new List<Zone>();
                        foreach (XElement currentnode in document.Root.Elements())
                        {
                            Zone zone = GetZoneFromXML(currentnode);
                            rv.Add(zone);
                        }
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(null);
            }
            return tcs.Task;
        }
        private static Zone GetZoneFromXML(XElement currentnode)
        {
            Zone zone = new Zone();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "LocationCode":
                        {
                            zone.LocationCode = currentatribute.Value;
                            break;
                        }
                    case "Code":
                        {
                            zone.Code = currentatribute.Value;
                            zone.PrevCode = zone.Code;
                            break;
                        }
                    case "Description":
                        {
                            zone.Description = currentatribute.Value;
                            break;
                        }
                    case "BinTypeCode":
                        {
                            zone.BinTypeCode = currentatribute.Value;
                            break;
                        }
                    case "CrossDockBinZone":
                        {
                            zone.CrossDockBinZone = StringToBool(currentatribute.Value);
                            break;
                        }
                    case "HexColor":
                        {
                            zone.HexColor = currentatribute.Value;
                            break;
                        }
                    case "Left":
                        {
                            zone.Left = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "Top":
                        {
                            zone.Top = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "Width":
                        {
                            zone.Width = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "Height":
                        {
                            zone.Height = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "PlanWidth":
                        {
                            zone.PlanWidth = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "PlanHeight":
                        {
                            zone.PlanHeight = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "SpecialEquipmentCode":
                        {
                            zone.SpecialEquipmentCode = currentatribute.Value;
                            break;
                        }
                    case "WarehouseClassCode":
                        {
                            zone.WarehouseClassCode = currentatribute.Value;
                            break;
                        }
                    case "BinQuantity":
                        {
                            zone.BinQuantity = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "RackQuantity":
                        {
                            zone.RackQuantity = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "SchemeVisible":
                        {
                            zone.SchemeVisible = StringToBool(currentatribute.Value);
                            break;
                        }
                    default:
                        throw new InvalidOperationException("Impossible value GetZoneFromXML > Name: " + currentatribute.Name.LocalName);
                }
            }
            return zone;
        }
        #endregion

        #region Rack
        public static Task<int> CreateRack(Rack rack, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "CreateRack";
                        XElement body =
                            new XElement(myns + functionname,
                            new XElement(myns + "locationCode", rack.LocationCode),
                            new XElement(myns + "zoneCode", rack.ZoneCode),
                            new XElement(myns + "no", rack.No),
                            new XElement(myns + "sections", rack.Sections),
                            new XElement(myns + "levels", rack.Levels),
                            new XElement(myns + "depth", rack.Depth),
                            new XElement(myns + "left", rack.Left),
                            new XElement(myns + "top", rack.Top),
                            new XElement(myns + "width", rack.Width),
                            new XElement(myns + "height", rack.Height),
                            new XElement(myns + "rackOrientation", (int)rack.RackOrientation),
                            new XElement(myns + "schemeVisible", rack.SchemeVisible));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        tcs.SetResult(0);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<int> ModifyRack(Rack rack, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "ModifyRack";
                        XElement body =
                            new XElement(myns + functionname,
                            new XElement(myns + "locationCode", rack.LocationCode),
                            new XElement(myns + "zoneCode", rack.ZoneCode),
                            new XElement(myns + "no", rack.No),
                            new XElement(myns + "prevNo", rack.PrevNo),
                            new XElement(myns + "sections", rack.Sections),
                            new XElement(myns + "levels", rack.Levels),
                            new XElement(myns + "depth", rack.Depth),
                            new XElement(myns + "left", rack.Left),
                            new XElement(myns + "top", rack.Top),
                            new XElement(myns + "width", rack.Width),
                            new XElement(myns + "height", rack.Height),
                            new XElement(myns + "rackOrientation", (int)rack.RackOrientation),
                            new XElement(myns + "schemeVisible", rack.SchemeVisible));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        tcs.SetResult(0);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<int> DeleteRack(Rack rack, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "DeleteRack";
                        XElement body =
                            new XElement(myns + functionname,
                            new XElement(myns + "locationCode", rack.LocationCode),
                            new XElement(myns + "zoneCode", rack.ZoneCode),
                            new XElement(myns + "no", rack.No));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        tcs.SetResult(0);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<int> SetRackVisible(Rack rack, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "SetRackVisible";
                        XElement body =
                        new XElement(myns + functionname,
                        new XElement(myns + "locationCode", rack.LocationCode),
                        new XElement(myns + "zoneCode", rack.ZoneCode),
                        new XElement(myns + "no", rack.No),
                        new XElement(myns + "visible", rack.SchemeVisible));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        tcs.SetResult(0);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<int> GetRackCount(string locationfilter, string codefilter, string nofilter, bool onlyvisibled, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "GetRackCount";
                        XElement body =
                        new XElement(myns + functionname,
                        new XElement(myns + "locationCodeFilter", locationfilter),
                        new XElement(myns + "zoneCodeFilter", codefilter),
                        new XElement(myns + "nOFilter", nofilter),
                        new XElement(myns + "onlyVisibled", onlyvisibled));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        int rv = StringToInt(soapbodynode.Value);
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<List<Rack>> GetRackList(string locationfilter, string zonefilter, bool onlyvisibled, int position, int count, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<Rack>>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "GetRackList";
                        XElement body =
                        new XElement(myns + functionname,
                        new XElement(myns + "locationCodeFilter", locationfilter),
                        new XElement(myns + "zoneCodeFilter", zonefilter),
                        new XElement(myns + "onlyVisibled", onlyvisibled),
                        new XElement(myns + "entriesPosition", position),
                        new XElement(myns + "entriesCount", count),
                        new XElement(myns + "responseDocument", ""));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        string response = soapbodynode.Value;
                        XDocument document = GetDoc(response);
                        List<Rack> rv = new List<Rack>();
                        foreach (XElement currentnode in document.Root.Elements())
                        {
                            Rack rack = GetRackFromXML(currentnode);
                            rv.Add(rack);
                        }
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(null);
            }
            return tcs.Task;
        }
        private static Rack GetRackFromXML(XElement currentnode)
        {
            Rack rack = new Rack();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "LocationCode":
                        {
                            rack.LocationCode = currentatribute.Value;
                            break;
                        }
                    case "ZoneCode":
                        {
                            rack.ZoneCode = currentatribute.Value;
                            break;
                        }
                    case "No":
                        {
                            rack.No = currentatribute.Value;
                            rack.PrevNo = rack.No;
                            break;
                        }
                    case "Sections":
                        {
                            rack.Sections = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "Levels":
                        {
                            rack.Levels = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "Depth":
                        {
                            rack.Depth = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "Left":
                        {
                            rack.Left = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "Top":
                        {
                            rack.Top = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "SchemeVisible":
                        {
                            rack.SchemeVisible = StringToBool(currentatribute.Value);
                            break;
                        }
                    case "RackOrientation":
                        {
                            int i = StringToInt(currentatribute.Value);
                            switch (i)
                            {
                                case 0:
                                    rack.RackOrientation = RackOrientationEnum.HorizontalLeft;
                                    break;
                                case 1:
                                    rack.RackOrientation = RackOrientationEnum.HorizontalRight;
                                    break;
                                case 2:
                                    rack.RackOrientation = RackOrientationEnum.VerticalUp;
                                    break;
                                case 3:
                                    rack.RackOrientation = RackOrientationEnum.VerticalDown;
                                    break;
                                default:
                                    throw new InvalidOperationException("Impossible value");
                            }
                            break;
                        }
                    default:
                        throw new InvalidOperationException("Impossible value GetRackFromXML > Name: " + currentatribute.Name.LocalName);
                }
            }
            return rack;
        }
        #endregion

        #region Bin
        public static Task<int> CreateBin(BinTemplate bintemplate, Bin bin, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "CreateBin";
                        XElement body =
                            new XElement(myns + functionname,
                            new XElement(myns + "binTemplateCode", bintemplate.Code),
                            new XElement(myns + "binCode", bin.Code),
                            new XElement(myns + "rackNo", bin.RackNo),
                            new XElement(myns + "section", bin.Section),
                            new XElement(myns + "level", bin.Level),
                            new XElement(myns + "depth", bin.Depth),
                            new XElement(myns + "sectionSpan", bin.SectionSpan),
                            new XElement(myns + "levelSpan", bin.LevelSpan),
                            new XElement(myns + "depthSpan", bin.DepthSpan));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        tcs.SetResult(0);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<int> ModifyBin(Bin bin, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "ModifyBin";
                        XElement body =
                            new XElement(myns + functionname,
                            new XElement(myns + "locationCode", bin.LocationCode),
                            new XElement(myns + "binCode", bin.Code),
                            new XElement(myns + "prevBinCode", bin.PrevCode),
                            new XElement(myns + "rackNo", bin.RackNo),
                            new XElement(myns + "section", bin.Section),
                            new XElement(myns + "level", bin.Level),
                            new XElement(myns + "depth", bin.Depth),
                            new XElement(myns + "sectionSpan", bin.SectionSpan),
                            new XElement(myns + "depthSpan", bin.LevelSpan),
                            new XElement(myns + "binRanking", bin.BinRanking),
                            new XElement(myns + "maximumCubage", bin.MaximumCubage),
                            new XElement(myns + "maximumWeight", bin.MaximumWeight),
                            new XElement(myns + "blockMovement", bin.BlockMovement),
                            new XElement(myns + "binTypeCode", bin.BinType),
                            new XElement(myns + "warehouseClassCode", bin.WarehouseClassCode),
                            new XElement(myns + "specialEquipmentCode", bin.SpecialEquipmentCode));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        tcs.SetResult(0);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<int> DeleteBin(Bin bin, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "DeleteBin";
                        XElement body =
                            new XElement(myns + functionname,
                            new XElement(myns + "locationCodeFilter", bin.LocationCode),
                            new XElement(myns + "zoneCodeFilter", bin.Code),
                            new XElement(myns + "rackCodeFilter", bin.PrevCode),
                            new XElement(myns + "code", bin.RackNo));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        tcs.SetResult(0);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<int> GetBinCount(string locationfilter, string codefilter, string rackcodefilter, string bincodefilter, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "GetBinCount";
                        XElement body =
                            new XElement(myns + functionname,
                            new XElement(myns + "locationCodeFilter", locationfilter),
                            new XElement(myns + "zoneCodeFilter", codefilter),
                            new XElement(myns + "rackCodeFilter", rackcodefilter),
                            new XElement(myns + "binCodeFilter", bincodefilter));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false); 
                        int rv = StringToInt(soapbodynode.Value);
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<List<Bin>> GetBinList(string locationfilter, string codefilter, string rackcodefilter, string bincodefilter, int position, int count, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<Bin>>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "GetBinList";
                        XElement body =
                            new XElement(myns + functionname,
                            new XElement(myns + "locationCodeFilter", locationfilter),
                            new XElement(myns + "zoneCodeFilter", codefilter),
                            new XElement(myns + "rackCodeFilter", rackcodefilter),
                            new XElement(myns + "binCodeFilter", bincodefilter),
                            new XElement(myns + "entriesPosition", position),
                            new XElement(myns + "entriesCount", count),
                            new XElement(myns + "responseDocument", ""));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        string response = soapbodynode.Value;
                        XDocument document = GetDoc(response);
                        List<Bin> rv = new List<Bin>();
                        foreach (XElement currentnode in document.Root.Elements())
                        {
                            Bin bin = GetBinFromXML(currentnode);
                            rv.Add(bin);
                        }
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(null);
            }
            return tcs.Task;
        }
        private static Bin GetBinFromXML(XElement currentnode)
        {
            Bin bin = new Bin();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "LocationCode":
                        {
                            bin.LocationCode = currentatribute.Value;
                            break;
                        }
                    case "ZoneCode":
                        {
                            bin.ZoneCode = currentatribute.Value;
                            break;
                        }
                    case "Code":
                        {
                            bin.Code = currentatribute.Value;
                            break;
                        }
                    case "Description":
                        {
                            bin.Description = currentatribute.Value;
                            break;
                        }
                    case "BinType":
                        {
                            bin.BinType = currentatribute.Value;
                            break;
                        }
                    case "Empty":
                        {
                            bin.Empty = StringToBool(currentatribute.Value);
                            break;
                        }
                    case "BlockMovement":
                        {
                            bin.BlockMovement = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "RackNo":
                        {
                            bin.RackNo = currentatribute.Value;
                            break;
                        }
                    case "Section":
                        {
                            bin.Section = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "Level":
                        {
                            bin.Level = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "Depth":
                        {
                            bin.Depth = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "LevelSpan":
                        {
                            bin.LevelSpan = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "SectionSpan":
                        {
                            bin.SectionSpan = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "DepthSpan":
                        {
                            bin.DepthSpan = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "BinRanking":
                        {
                            bin.BinRanking = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "MaximumCubage":
                        {
                            bin.MaximumCubage = StringToDec(currentatribute.Value);
                            break;
                        }
                    case "MaximumWeight":
                        {
                            bin.MaximumWeight = StringToDec(currentatribute.Value);
                            break;
                        }
                    case "AdjustmentBin":
                        {
                            bin.AdjustmentBin = StringToBool(currentatribute.Value);
                            break;
                        }
                    case "SpecialEquipmentCode":
                        {
                            bin.SpecialEquipmentCode = currentatribute.Value;
                            break;
                        }
                    case "WarehouseClassCode":
                        {
                            bin.WarehouseClassCode = currentatribute.Value;
                            break;
                        }
                    default:
                        throw new InvalidOperationException("Impossible value GetBinFromXML > Name: " + currentatribute.Name.LocalName);
                }
            }
            return bin;
        }
        #endregion
        #region BinTemplate
        public static Task<int> CreateBinTemplate(BinTemplate bintemplate, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "CreateBinTemplate";
                        XElement body =
                            new XElement(myns + functionname,
                            new XElement(myns + "locationCode", bintemplate.LocationCode),
                            new XElement(myns + "zoneCode", bintemplate.ZoneCode),
                            new XElement(myns + "code", bintemplate.Code),
                            new XElement(myns + "description", bintemplate.Description),
                            new XElement(myns + "binTypeCode", bintemplate.BinTypeCode),
                            new XElement(myns + "warehouseClassCode", bintemplate.WarehouseClassCode),
                            new XElement(myns + "blockMovement", bintemplate.BlockMovement),
                            new XElement(myns + "specialEquipmentCode", bintemplate.SpecialEquipmentCode),
                            new XElement(myns + "binRanking", bintemplate.BinRanking),
                            new XElement(myns + "maximumCubage", bintemplate.MaximumCubage),
                            new XElement(myns + "maximumWeight", bintemplate.MaximumWeight),
                            new XElement(myns + "dedicated1", bintemplate.Dedicated));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        tcs.SetResult(0);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<int> ModifyBinTemplate(BinTemplate bintemplate, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "ModifyBinTemplate";

                        XElement body =
                            new XElement(myns + functionname,
                            new XElement(myns + "locationCode", bintemplate.LocationCode),
                            new XElement(myns + "zoneCode", bintemplate.ZoneCode),
                            new XElement(myns + "code", bintemplate.Code),
                            new XElement(myns + "description", bintemplate.Description),
                            new XElement(myns + "binTypeCode", bintemplate.BinTypeCode),
                            new XElement(myns + "warehouseClassCode", bintemplate.WarehouseClassCode),
                            new XElement(myns + "blockMovement", bintemplate.BlockMovement),
                            new XElement(myns + "specialEquipmentCode", bintemplate.SpecialEquipmentCode),
                            new XElement(myns + "binRanking", bintemplate.BinRanking),
                            new XElement(myns + "maximumCubage", bintemplate.MaximumCubage),
                            new XElement(myns + "maximumWeight", bintemplate.MaximumWeight),
                            new XElement(myns + "dedicated1", bintemplate.Dedicated));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        tcs.SetResult(0);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<int> DeleteBinTemplate(BinTemplate bintemplate, CancellationTokenSource cts)
        {
            //<element minOccurs="1" maxOccurs="1" name="code" type="string"/>
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "DeleteBinTemplate";
                        XElement body =
                            new XElement(myns + functionname,
                            new XElement(myns + "code", bintemplate.Code));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        tcs.SetResult(0);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<int> GetBinTemplateCount(CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "GetBinTemplateCount";
                        XElement body = new XElement(myns + functionname);
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        int rv = StringToInt(soapbodynode.Value);
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<List<BinTemplate>> GetBinTemplateList(int position, int count, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<BinTemplate>>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                    {
                        try
                        {
                            XNamespace myns = GetNameSpace();
                            string functionname = "GetBinTemplateList";
                            XElement body =
                                new XElement(myns + functionname,
                                new XElement(myns + "entriesPosition", position),
                                new XElement(myns + "entriesCount", count),
                                new XElement(myns + "responseDocument", ""));
                            XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                            string response = soapbodynode.Value;
                            XDocument document = GetDoc(response);
                            List<BinTemplate> rv = new List<BinTemplate>();
                            foreach (XElement currentnode in document.Root.Elements())
                            {
                                BinTemplate bintemplate = GetBinTemplateFromXML(currentnode);
                                rv.Add(bintemplate);
                            }
                            tcs.SetResult(rv);
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e.Message);
                            tcs.SetException(e);
                        }
                    });
            }
            else
            {
                tcs.SetResult(null);
            }
            return tcs.Task;
        }
        private static BinTemplate GetBinTemplateFromXML(XElement currentnode)
        {
            BinTemplate bintemplate = new BinTemplate();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "Code":
                        {
                            bintemplate.Code = currentatribute.Value;
                            break;
                        }
                    case "Description":
                        {
                            bintemplate.Description = currentatribute.Value;
                            break;
                        }
                    case "LocationCode":
                        {
                            bintemplate.LocationCode = currentatribute.Value;
                            break;
                        }
                    case "ZoneCode":
                        {
                            bintemplate.ZoneCode = currentatribute.Value;
                            break;
                        }
                    case "BinTypeCode":
                        {
                            bintemplate.BinTypeCode = currentatribute.Value;
                            break;
                        }
                    case "BlockMovement":
                        {
                            bintemplate.BlockMovement = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "BinDescription":
                        {
                            bintemplate.BinDescription = currentatribute.Value;
                            break;
                        }
                    case "MaximumCubage":
                        {
                            bintemplate.MaximumCubage = StringToDec(currentatribute.Value);
                            break;
                        }
                    case "MaximumWeight":
                        {
                            bintemplate.MaximumWeight = StringToDec(currentatribute.Value);
                            break;
                        }
                    case "SpecialEquipmentCode":
                        {
                            bintemplate.SpecialEquipmentCode = currentatribute.Value;
                            break;
                        }
                    case "WarehouseClassCode":
                        {
                            bintemplate.WarehouseClassCode = currentatribute.Value;
                            break;
                        }
                    case "BinRanking":
                        {
                            bintemplate.BinRanking = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "Dedicated":
                        {
                            bintemplate.Dedicated = StringToBool(currentatribute.Value);
                            break;
                        }
                }
            }
            return bintemplate;
        }
        #endregion
        #region BinType
        public static Task<int> GetBinTypeCount(CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "GetBinTypeCount";
                        XElement body = new XElement(myns + functionname);
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        int rv = StringToInt(soapbodynode.Value);
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<List<BinType>> GetBinTypeList(int position, int count, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<BinType>>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "GetBinTypeList";
                        XElement body =
                        new XElement(myns + functionname,
                        new XElement(myns + "entriesPosition", position),
                        new XElement(myns + "entriesCount", count),
                        new XElement(myns + "responseDocument", ""));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        string response = soapbodynode.Value;
                        XDocument document = GetDoc(response);
                        List<BinType> rv = new List<BinType>();
                        foreach (XElement currentnode in document.Root.Elements())
                        {
                            BinType bintype = GetBinTypeFromXML(currentnode);
                            rv.Add(bintype);
                        }
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(null);
            }
            return tcs.Task;
        }
        private static BinType GetBinTypeFromXML(XElement currentnode)
        {
            BinType bintype = new BinType();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "Code":
                        {
                            bintype.Code = currentatribute.Value;
                            break;
                        }
                    case "Description":
                        {
                            bintype.Description = currentatribute.Value;
                            break;
                        }
                    case "Pick":
                        {
                            bintype.Pick = StringToBool(currentatribute.Value);
                            break;
                        }
                    case "PutAway":
                        {
                            bintype.PutAway = StringToBool(currentatribute.Value);
                            break;
                        }
                    case "Receive":
                        {
                            bintype.Receive = StringToBool(currentatribute.Value);
                            break;
                        }
                    case "Ship":
                        {
                            bintype.Ship = StringToBool(currentatribute.Value);
                            break;
                        }
                }
            }
            return bintype;
        }
        #endregion
        #region BinContent
        public static Task<int> GetBinContentCount(string locationCodeFilter, string zoneCodeFilter, string binCodeFiler, string itemNoFilter, string variantCodeFilter, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "GetBinContentCount";
                        XElement body =
                        new XElement(myns + functionname,
                        new XElement(myns + "locationCodeFilter", locationCodeFilter),
                        new XElement(myns + "zoneCodeFilter", zoneCodeFilter),
                        new XElement(myns + "binCodeFilter", binCodeFiler),
                        new XElement(myns + "itemNoFilter", itemNoFilter),
                        new XElement(myns + "variantCodeFilter", variantCodeFilter));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        int rv = StringToInt(soapbodynode.Value);
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<List<BinContent>> GetBinContentList(string locationCodeFilter, string zoneCodeFilter, string binCodeFiler, string itemNoFilter, string variantCodeFilter, int position, int count, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<BinContent>>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "GetBinContentList";
                        XElement body =
                        new XElement(myns + functionname,
                        new XElement(myns + "locationCodeFilter", locationCodeFilter),
                        new XElement(myns + "zoneCodeFilter", zoneCodeFilter),
                        new XElement(myns + "binCodeFilter", binCodeFiler),
                        new XElement(myns + "itemNoFilter", itemNoFilter),
                        new XElement(myns + "variantCodeFilter", variantCodeFilter),
                        new XElement(myns + "entriesPosition", position),
                        new XElement(myns + "entriesCount", count),
                        new XElement(myns + "responseDocument", ""));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        string response = soapbodynode.Value;
                        XDocument document = GetDoc(response);
                        List<BinContent> rv = new List<BinContent>();
                        foreach (XElement currentnode in document.Root.Elements())
                        {
                            BinContent bincontent = GetBinContentFromXML(currentnode);
                            rv.Add(bincontent);
                        }
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(null);
            }
            return tcs.Task;
        }
        private static BinContent GetBinContentFromXML(XElement currentnode)
        {
            BinContent bincontent = new BinContent();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "LocationCode":
                        {
                            bincontent.LocationCode = currentatribute.Value;
                            break;
                        }
                    case "ZoneCode":
                        {
                            bincontent.ZoneCode = currentatribute.Value;
                            break;
                        }
                    case "BinCode":
                        {
                            bincontent.BinCode = currentatribute.Value;
                            break;
                        }
                    case "BinType":
                        {
                            bincontent.BinType = currentatribute.Value;
                            break;
                        }
                    case "BlockMovement":
                        {
                            bincontent.BlockMovement = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "ItemNo":
                        {
                            bincontent.ItemNo = currentatribute.Value;
                            break;
                        }
                    case "VariantCode":
                        {
                            bincontent.VariantCode = currentatribute.Value;
                            break;
                        }
                    case "Description":
                        {
                            bincontent.Description = currentatribute.Value;
                            break;
                        }
                    case "NegAdjmtQty":
                        {
                            bincontent.NegAdjmtQty = StringToDec(currentatribute.Value);
                            break;
                        }
                    case "PickQty":
                        {
                            bincontent.PickQty = StringToDec(currentatribute.Value);
                            break;
                        }
                    case "PosAdjmtQty":
                        {
                            bincontent.PosAdjmtQty = StringToDec(currentatribute.Value);
                            break;
                        }
                    case "PutawayQty":
                        {
                            bincontent.PutawayQty = StringToDec(currentatribute.Value);
                            break;
                        }
                    case "Quantity":
                        {
                            bincontent.Quantity = StringToDec(currentatribute.Value);
                            break;
                        }
                    case "QuantityBase":
                        {
                            bincontent.QuantityBase = StringToDec(currentatribute.Value);
                            break;
                        }
                    case "UnitofMeasureCode":
                        {
                            bincontent.UnitofMeasureCode = currentatribute.Value;
                            break;
                        }
                }
            }
            return bincontent;
        }
        #endregion

        #region ItemIdentifiers
        public static Task<int> GetItemIdentifierCount(string barCodeCodeFilter, string itemNoFilter, string variantCodeFilter, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "GetItemIdentifierCount";
                        XElement body =
                        new XElement(myns + functionname,
                        new XElement(myns + "barCodeFilter", barCodeCodeFilter),
                        new XElement(myns + "itemNoFilter", itemNoFilter),
                        new XElement(myns + "variantCodeFilter", variantCodeFilter));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        int rv = StringToInt(soapbodynode.Value);
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<List<ItemIdentifier>> GetItemIdentifierList(string barCodeCodeFilter, string itemNoFilter, string variantCodeFilter, int position, int count, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<ItemIdentifier>>();

            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
            {
                try
                {
                    XNamespace myns = GetNameSpace();
                    string functionname = "GetItemIdentifierList";
                    XElement body =
                        new XElement(myns + functionname,
                                    new XElement(myns + "barCodeFilter", barCodeCodeFilter),
                                    new XElement(myns + "itemNoFilter", itemNoFilter),
                                    new XElement(myns + "variantCodeFilter", variantCodeFilter),
                                    new XElement(myns + "entriesPosition", position),
                                    new XElement(myns + "entriesCount", count),
                                    new XElement(myns + "responseDocument", ""));
                    XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                    string response = soapbodynode.Value;
                    XDocument document = GetDoc(response);
                    List<ItemIdentifier> rv = new List<ItemIdentifier>();
                    foreach (XElement currentnode in document.Root.Elements())
                    {
                        ItemIdentifier itemidentifier = GetItemIdentifierFromXML(currentnode);
                        rv.Add(itemidentifier);
                    }
                    tcs.SetResult(rv);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    tcs.SetException(e);
                }
            });
            }
            else
            {
                tcs.SetResult(null);
            }
            return tcs.Task;
        }
        private static ItemIdentifier GetItemIdentifierFromXML(XElement currentnode)
        {
            ItemIdentifier itemidentifier = new ItemIdentifier();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "BarCode":
                        {
                            itemidentifier.BarCode = currentatribute.Value;
                            break;
                        }
                    case "ItemNo":
                        {
                            itemidentifier.ItemNo = currentatribute.Value;
                            break;
                        }
                    case "VariantCode":
                        {
                            itemidentifier.VariantCode = currentatribute.Value;
                            break;
                        }
                    case "UoM":
                        {
                            itemidentifier.UoM = currentatribute.Value;
                            break;
                        }
                }
            }
            return itemidentifier;
        }

        #endregion
        #region WarehouseClass
        public static Task<int> GetWarehouseClassCount(CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "GetWarehouseClassCount";
                        XElement body = new XElement(myns + functionname);
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        int rv = StringToInt(soapbodynode.Value);
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<List<WarehouseClass>> GetWarehouseClassList(int position, int count, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<WarehouseClass>>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "GetWarehouseClassList";
                        XElement body =
                        new XElement(myns + functionname,
                        new XElement(myns + "entriesPosition", position),
                        new XElement(myns + "entriesCount", count),
                        new XElement(myns + "responseDocument", ""));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        string response = soapbodynode.Value;
                        XDocument document = GetDoc(response);
                        List<WarehouseClass> rv = new List<WarehouseClass>();
                        foreach (XElement currentnode in document.Root.Elements())
                        {
                            WarehouseClass warehouseclass = GetWarehouseClassFromXML(currentnode);
                            rv.Add(warehouseclass);
                        }
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(null);
            }
            return tcs.Task;
        }
        private static WarehouseClass GetWarehouseClassFromXML(XElement currentnode)
        {
            WarehouseClass warehouseclass = new WarehouseClass();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "Code":
                        {
                            warehouseclass.Code = currentatribute.Value;
                            break;
                        }
                    case "Description":
                        {
                            warehouseclass.Description = currentatribute.Value;
                            break;
                        }
                }
            }
            return warehouseclass;
        }
        #endregion

        #region SpecialEquipment
        public static Task<int> GetSpecialEquipmentCount(CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "GetSpecialEquipmentCount";
                        XElement body = new XElement(myns + functionname);
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        int rv = StringToInt(soapbodynode.Value);
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<List<SpecialEquipment>> GetSpecialEquipmentList(int position, int count, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<SpecialEquipment>>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "GetSpecialEquipmentList";
                        XElement body =
                        new XElement(myns + functionname,
                        new XElement(myns + "entriesPosition", position),
                        new XElement(myns + "entriesCount", count),
                        new XElement(myns + "responseDocument", ""));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        string response = soapbodynode.Value;
                        XDocument document = GetDoc(response);
                        List<SpecialEquipment> rv = new List<SpecialEquipment>();
                        foreach (XElement currentnode in document.Root.Elements())
                        {
                            SpecialEquipment specialequipment = GetSpecialEquipmentFromXML(currentnode);
                            rv.Add(specialequipment);
                        }
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(null);
            }
            return tcs.Task;
        }
        private static SpecialEquipment GetSpecialEquipmentFromXML(XElement currentnode)
        {
            SpecialEquipment specialequipment = new SpecialEquipment();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "Code":
                        {
                            specialequipment.Code = currentatribute.Value;
                            break;
                        }
                    case "Description":
                        {
                            specialequipment.Description = currentatribute.Value;
                            break;
                        }
                }
            }
            return specialequipment;
        }
        #endregion

        #region WarehouseEntry
        public static Task<int> GetWarehouseEntryCount(string locationCodeFilter, string zoneCodeFilter, string binCodeFilter, string itemNoFilter, string variantCodeFilter, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "GetWarehouseEntryCount";
                        XElement body =
                        new XElement(myns + functionname,
                        new XElement(myns + "locationCodeFilter", locationCodeFilter),
                        new XElement(myns + "zoneCodeFilter", zoneCodeFilter),
                        new XElement(myns + "binCodeFilter", binCodeFilter),
                        new XElement(myns + "itemNoFilter", itemNoFilter),
                        new XElement(myns + "variantCodeFilter", variantCodeFilter));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        int rv = StringToInt(soapbodynode.Value);
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }
        public static Task<List<WarehouseEntry>> GetWarehouseEntryList(string locationCodeFilter, string zoneCodeFilter, string binCodeFilter, string itemNoFilter, string variantCodeFilter, int position, int count, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<WarehouseEntry>>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "GetWarehouseEntryList";

                        XElement body =
                        new XElement(myns + functionname,
                        new XElement(myns + "locationCodeFilter", locationCodeFilter),
                        new XElement(myns + "zoneCodeFilter", zoneCodeFilter),
                        new XElement(myns + "binCodeFilter", binCodeFilter),
                        new XElement(myns + "itemNoFilter", itemNoFilter),
                        new XElement(myns + "variantCodeFilter", variantCodeFilter),
                        new XElement(myns + "variantCodeFilter", position),
                        new XElement(myns + "variantCodeFilter", count),
                        new XElement(myns + "responseDocument", ""));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        string response = soapbodynode.Value;
                        XDocument document = GetDoc(response);
                        List<WarehouseEntry> rv = new List<WarehouseEntry>();
                        foreach (XElement currentnode in document.Root.Elements())
                        {
                            WarehouseEntry we = GetWarehouseEntryFromXML(currentnode);
                            rv.Add(we);
                        }
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(null);
            }
            return tcs.Task;
        }
        private static WarehouseEntry GetWarehouseEntryFromXML(XElement currentnode)
        {
            WarehouseEntry we = new WarehouseEntry();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "LocationCode":
                        {
                            we.LocationCode = currentatribute.Value;
                            break;
                        }
                    case "ZoneCode":
                        {
                            we.ZoneCode = currentatribute.Value;
                            break;
                        }
                    case "BinCode":
                        {
                            we.BinCode = currentatribute.Value;
                            break;
                        }
                    case "ItemNo":
                        {
                            we.ItemNo = currentatribute.Value;
                            break;
                        }
                    case "VariantCode":
                        {
                            we.VariantCode = currentatribute.Value;
                            break;
                        }
                    case "Description":
                        {
                            we.Description = currentatribute.Value;
                            break;
                        }
                    case "EntryType":
                        {
                            we.EntryType = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "RegisteringDate":
                        {
                            we.RegisteringDate = currentatribute.Value;
                            break;
                        }
                    case "SourceNo":
                        {
                            we.SourceNo = currentatribute.Value;
                            break;
                        }
                    case "Quantity":
                        {
                            we.Quantity = StringToDec(currentatribute.Value);
                            break;
                        }
                    case "QuantityBase":
                        {
                            we.QuantityBase = StringToDec(currentatribute.Value);
                            break;
                        }
                    case "UnitofMeasureCode":
                        {
                            we.UnitofMeasureCode = currentatribute.Value;
                            break;
                        }
                }
            }
            return we;
        }
        #endregion
        #region UserDefinedSelection
        public static Task<List<UserDefinedSelection>> LoadUDS(string locationCode, string zoneCode, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<UserDefinedSelection>>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "LoadUserDefinedSelectionList";
                        XElement body =
                        new XElement(myns + functionname,
                        new XElement(myns + "locationCode", locationCode),
                        new XElement(myns + "zoneCode", zoneCode),
                        new XElement(myns + "responseDocument", ""));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        string response = soapbodynode.Value;
                        XDocument document = GetDoc(response);
                        List<UserDefinedSelection> rv = new List<UserDefinedSelection>();
                        foreach (XElement currentnode in document.Root.Elements())
                        {
                            UserDefinedSelection uds = GetUserDefinedSelectionFromXML(currentnode);
                            rv.Add(uds);
                        }
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(null);
            }
            return tcs.Task;
        }
        private static UserDefinedSelection GetUserDefinedSelectionFromXML(XElement currentnode)
        {
            UserDefinedSelection uds = new UserDefinedSelection();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "ID":
                        {
                            uds.ID = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "Name":
                        {
                            uds.Name = currentatribute.Value;
                            break;
                        }
                    case "Detail":
                        {
                            uds.Detail = currentatribute.Value;
                            break;
                        }
                    case "HexColor":
                        {
                            uds.HexColor = currentatribute.Value;
                            break;
                        }
                    case "Value":
                        {
                            uds.Value = StringToInt(currentatribute.Value);
                            break;
                        }
                }
            }
            return uds;
        }
        public static Task<List<UserDefinedSelectionResult>> RunUDS(string locationCode, string zoneCode, int i, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<UserDefinedSelectionResult>>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "RunUDS";
                        XElement body =
                        new XElement(myns + functionname,
                        new XElement(myns + "locationCode", locationCode),
                        new XElement(myns + "zoneCode", zoneCode),
                        new XElement(myns + "functionIndex", i),
                        new XElement(myns + "responseDocument", ""));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        string response = soapbodynode.Value;
                        XDocument document = GetDoc(response);
                        List<UserDefinedSelectionResult> rv = new List<UserDefinedSelectionResult>();
                        foreach (XElement currentnode in document.Root.Elements())
                        {
                            UserDefinedSelectionResult uds = GetUserDefinedSelectionResultnFromXML(currentnode);
                            rv.Add(uds);
                        }
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(null);
            }
            return tcs.Task;
        }
        private static UserDefinedSelectionResult GetUserDefinedSelectionResultnFromXML(XElement currentnode)
        {
            UserDefinedSelectionResult uds = new UserDefinedSelectionResult();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "F":
                        {
                            uds.FunctionID = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "R":
                        {
                            uds.RackNo = currentatribute.Value;
                            break;
                        }
                    case "S":
                        {
                            uds.Section = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "L":
                        {
                            uds.Level = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "D":
                        {
                            uds.Depth = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "Q":
                        {
                            uds.Value = (int)Math.Round(StringToDec(currentatribute.Value));
                            break;
                        }
                    case "C":
                        {
                            uds.HexColor = currentatribute.Value;
                            break;
                        }
                }
            }
            return uds;
        }
        #endregion
        #region UserDefinedFunctions
        public static Task<List<UserDefinedFunction>> LoadUserDefinedFunctionList(string locationCode, string zoneCode, string rackno, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<UserDefinedFunction>>();

            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "LoadUserDefinedFunctionList";
                        XElement body =
                        new XElement(myns + functionname,
                        new XElement(myns + "locationCode", locationCode),
                        new XElement(myns + "zoneCode", zoneCode),
                        new XElement(myns + "rackNo", rackno),
                        new XElement(myns + "responseDocument", ""));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        string response = soapbodynode.Value;
                        XDocument document = GetDoc(response);
                        List<UserDefinedFunction> rv = new List<UserDefinedFunction>();
                        foreach (XElement currentnode in document.Root.Elements())
                        {
                            UserDefinedFunction udf = GetUserDefinedFunctionFromXML(currentnode);
                            rv.Add(udf);
                        }
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(null);
            }
            return tcs.Task;
        }
        private static UserDefinedFunction GetUserDefinedFunctionFromXML(XElement currentnode)
        {
            UserDefinedFunction udf = new UserDefinedFunction();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "ID":
                        {
                            udf.ID = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "Name":
                        {
                            udf.Name = currentatribute.Value;
                            break;
                        }
                    case "Detail":
                        {
                            udf.Detail = currentatribute.Value;
                            break;
                        }
                    case "Confirm":
                        {
                            udf.Confirm = currentatribute.Value;
                            break;
                        }
                }
            }
            return udf;
        }

        public static Task<string> RunFunction(int i, string locationCode, string zoneCode, string rackno, string bincode, string itemno, string variantcode, decimal quantity, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<string>();
            if (Global.CurrentConnection is Connection)
            {

                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "RunFunction";
                        XElement body =
                        new XElement(myns + functionname,
                        new XElement(myns + "functionindex", i),
                        new XElement(myns + "locationCode", locationCode),
                        new XElement(myns + "zoneCode", zoneCode),
                        new XElement(myns + "rackNo", rackno),
                        new XElement(myns + "binCode", bincode),
                        new XElement(myns + "itemNo", itemno),
                        new XElement(myns + "variantCode", variantcode),
                        new XElement(myns + "quantity", quantity));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        string rv = soapbodynode.Value;
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult("");
            }
            return tcs.Task;
        }
        #endregion

        #region Search
        public static Task<List<SearchResponse>> Search(string locationcode, string searchrequest, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<SearchResponse>>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = GetNameSpace();
                        string functionname = "Search";
                        XElement body =
                        new XElement(myns + functionname,
                        new XElement(myns + "locationCode", locationcode),
                        new XElement(myns + "request", searchrequest),
                        new XElement(myns + "responseDocument", ""));
                        XElement soapbodynode = await Process(functionname, body, myns, false, cts).ConfigureAwait(false);
                        string response = soapbodynode.Value;
                        XDocument document = GetDoc(response);
                        List<SearchResponse> rv = new List<SearchResponse>();
                        foreach (XElement currentnode in document.Root.Elements())
                        {
                            SearchResponse searchresponse = GetSearchResponseFromXML(currentnode);
                            rv.Add(searchresponse);
                        }
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            }
            else
            {
                tcs.SetResult(null);
            }
            return tcs.Task;
        }
        private static SearchResponse GetSearchResponseFromXML(XElement currentnode)
        {
            SearchResponse searchresponse = new SearchResponse();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "Z":
                        {
                            searchresponse.ZoneCode = currentatribute.Value;
                            break;
                        }
                    case "B":
                        {
                            searchresponse.BinCode = currentatribute.Value;
                            break;
                        }
                    case "R":
                        {
                            searchresponse.RackNo = currentatribute.Value;
                            break;
                        }
                    case "S":
                        {
                            searchresponse.Section = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "L":
                        {
                            searchresponse.Level = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "D":
                        {
                            searchresponse.Depth = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "Q":
                        {
                            searchresponse.QuantityBase = (int)Math.Round(StringToDec(currentatribute.Value));
                            break;
                        }
                }
            }
            return searchresponse;
        }
        #endregion
        public static Task<int> TestConnection(CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (Global.CurrentConnection is Connection)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        XNamespace myns = "urn:microsoft-dynamics-schemas/codeunit/" + Global.TestConnection.Codeunit;
                        string functionname = "APIVersion";
                        XElement body = new XElement(myns + functionname);
                        XElement soapbodynode = await Process(functionname, body, myns, true, cts).ConfigureAwait(false);
                        int rv = StringToInt(soapbodynode.Value);
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });

            }
            else
            {
                tcs.SetResult(0);
            }
            return tcs.Task;
        }

        public static async Task<XElement> Process(string functionname, XElement body, XNamespace myns, bool testconnection, CancellationTokenSource cts)
        {
            Connection connection;
            if (testconnection)
            {
                if (!(Global.TestConnection is Connection))
                {
                    return null;
                }

                connection = Global.TestConnection;
            }
            else
            {
                if (!(Global.CurrentConnection is Connection))
                {
                    return null;
                }
                connection = Global.CurrentConnection;
            }

            XElement rv = null;

            string requestbody = GetRequestText(CreateSOAPRequest(body, myns));

            var handler = new HttpClientHandler
            {
                UseDefaultCredentials = false,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            };

            switch (connection.ClientCredentialType)
            {
                case ClientCredentialTypeEnum.Windows:
                    handler.Credentials = new NetworkCredential(connection.Domen + "\\" + connection.User, connection.Password, "");
                    break;
                case ClientCredentialTypeEnum.Basic:
                    handler.Credentials = connection.GetCreditials();
                    break;
                default:
                    throw new InvalidOperationException("Impossible value");
            }

            using (var client = new HttpClient(handler))
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = connection.GetUri(),
                    Method = HttpMethod.Post
                };
                request.Content = new StringContent(requestbody, Encoding.UTF8, "text/xml");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                client.DefaultRequestHeaders.Add("SOAPAction", connection.GetSoapActionTxt() + "/" + functionname);

                using (var response = await client.SendAsync(request, cts.Token))
                {
                    Task<Stream> streamTask = response.Content.ReadAsStreamAsync();
                    Stream stream = streamTask.Result;
                    var sr = new StreamReader(stream);
                    XDocument xmldoc = XDocument.Load(sr);

                    if (response.IsSuccessStatusCode)
                    {
                        XElement bodysopeenvelopenode = xmldoc.Root.Element(ns + "Body");
                        if (bodysopeenvelopenode is XElement)
                        {
                            return bodysopeenvelopenode;
                        }
                    }
                    else
                    {
                        if (response.ReasonPhrase == "Internal Server Error")
                        {
                            throw CreateNAVException(xmldoc);
                        }
                        else
                        {
                            NAVUnknowException unknown = new NAVUnknowException(response.ReasonPhrase);
                            throw unknown;
                        }
                    }
                }
            }
            return rv;
        }

        /// <summary>
        /// SOAP ERROR (NAV ERROR)
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        private static NAVErrorException CreateNAVException(XDocument xmldoc)
        {
            NAVErrorException rv = null;
            XElement bodysopeenvelopenode = xmldoc.Root.Element(ns + "Body");
            if (bodysopeenvelopenode is XElement)
            {
                XElement faultnode = bodysopeenvelopenode.Element(ns + "Fault");
                if (faultnode is XElement)
                {
                    string faultcodetxt = "";
                    string faultstringtxt = "";
                    string detailstringtxt = "";
                    XElement faultcodenode = faultnode.Element("faultcode");
                    faultcodetxt = faultcodenode?.Value;
                    XElement faultstringnode = faultnode.Element("faultstring");
                    faultstringtxt = faultstringnode?.Value;
                    XElement detailnode = faultnode.Element("detail");
                    detailstringtxt = detailnode?.Value;
                    rv = new NAVErrorException(faultcodetxt, faultstringtxt, detailstringtxt);
                }
            }
            return rv;
        }

        private static XDocument CreateSOAPRequest(XElement body, XNamespace myns)
        {
            XDocument requestdoc = new XDocument(
                            new XElement(ns + "Envelope",
                                new XAttribute(XNamespace.Xmlns + "soapenv", ns),
                                new XAttribute(XNamespace.Xmlns + "ns1", myns),
                                new XElement(ns + "Body",
                                    body)));
            requestdoc.Declaration = new XDeclaration("1.0", "UTF-8", null);
            return requestdoc;
        }

        private static System.Xml.Linq.XDocument GetDoc(string input)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(input);
            MemoryStream stream = new MemoryStream(byteArray);
            System.Xml.Linq.XDocument doc = XDocument.Load(stream);
            return doc;
        }

        public static string GetRequestText(XDocument doc)
        {
            string rv = "";
            using (StringWriter writer = new Utf8StringWriter())
            {
                doc.Save(writer);
                rv = writer.ToString();
            }
            return rv;
        }

        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding { get { return Encoding.UTF8; } }
        }

        public static int StringToInt(string value)
        {
            int.TryParse(value, out int int1);
            return int1;
        }
        public static decimal StringToDec(string value)
        {
            decimal res = 0;
            if (decimal.TryParse(value, out res))
            {
                return res;
            }
            else
            {
                return 0;
            }
        }
        public static bool StringToBool(string value)
        {
            if (value == "1")
            {
                return true;
            }

            if (value == "0")
            {
                return false;
            }

            if (value.ToUpper() == "TRUE")
            {
                return true;
            }

            if (value.ToUpper() == "FALSE")
            {
                return false;
            }

            if (bool.TryParse(value, out bool rv))
            {
                return rv;
            }

            return false;
        }
    }
}
