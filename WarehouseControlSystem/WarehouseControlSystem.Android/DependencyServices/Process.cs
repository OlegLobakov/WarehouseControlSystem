using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using System.Threading;
using System.Xml.Linq;
using WarehouseControlSystem.Helpers.NAV;
using WarehouseControlSystem.DependenciesServices;
using ModernHttpClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(WarehouseControlSystem.Droid.DependencyServices.ProcessDependency))]
namespace WarehouseControlSystem.Droid.DependencyServices
{
    public class ProcessDependency : IProcess
    {
        static XNamespace myns = "urn:microsoft-dynamics-schemas/codeunit/WarehouseControlManagement";
        static XNamespace ns = "http://schemas.xmlsoap.org/soap/envelope/";


        public async Task<XElement> Process(string functionname, string requestbody, CancellationTokenSource cts)
        {
            if (!(Global.CurrentConnection is Connection))
                return null;

            Connection connection = Global.CurrentConnection;
            XElement rv = null;

            Uri u1 = connection.GetUri();

            //var handler = new HttpClientHandler()
            //{
            //    AllowAutoRedirect = false,
            //    UseProxy = true,
            //    UseDefaultCredentials = true,
            //    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            //};

            //Use ModernHttpClient
            var handler = new NativeMessageHandler()
            {
                AllowAutoRedirect = false,
                UseProxy = true,
                UseDefaultCredentials = true,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            };


            switch (connection.ClientCredentialType)
            {
                case ClientCredentialTypeEnum.Windows:
                    {
                        handler.Credentials = new NetworkCredential(connection.Domen + "\\" + connection.User, connection.Password, "");
                        break;
                    }
                case ClientCredentialTypeEnum.Basic:
                    {
                        handler.Credentials = connection.GetCreditials();
                        break;
                    }
            }

            using (var client = new HttpClient(handler))
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = connection.GetUri(),
                    Method = HttpMethod.Post
                };
                request.Content = new StringContent(requestbody, Encoding.UTF8, "text/xml");

                //request.Headers.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                //request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
                client.DefaultRequestHeaders.Add("SOAPAction", connection.GetUri().ToString() + "/" + functionname);

                //client.DefaultRequestHeaders.Add("Postman-token", "bd819d31-dfed-4a95-84e0-10730c7f36b5");
                //client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.1.1");             
                //var content = new StringContent(requestbody, Encoding.UTF8, "text/xml");
                //content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");

                using (var response = await client.SendAsync(request, cts.Token))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Task<Stream> streamTask = response.Content.ReadAsStreamAsync();
                        Stream stream = streamTask.Result;
                        var sr = new StreamReader(stream);
                        XDocument xmldoc = XDocument.Load(sr);
                        XElement bodysopeenvelopenode = xmldoc.Root.Element(ns + "Body");
                        if (bodysopeenvelopenode is XElement)
                        {
                            return bodysopeenvelopenode;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        //SOAP ERROR (NAV ERROR)
                        if (response.ReasonPhrase == "Internal Server Error")
                        {
                            Task<Stream> streamTask = response.Content.ReadAsStreamAsync();
                            Stream stream = streamTask.Result;
                            var sr = new StreamReader(stream);
                            XDocument xmldoc = XDocument.Load(sr);
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
                                    NAVErrorException ne = new NAVErrorException(faultcodetxt, faultstringtxt, detailstringtxt);
                                    throw ne;
                                }
                            }
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

    }
}