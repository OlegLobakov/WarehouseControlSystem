using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseControlSystem.UWP.NAV;
using WarehouseControlSystem.Helpers.NAV;
using Xamarin.Forms;

//[assembly: Dependency(typeof(WarehouseControlSystem.UWP.DependenciesServices.WSNAV))]
//namespace WarehouseControlSystem.UWP.DependenciesServices
//{
//    public class WSNAV : INAV
//    {
//        public Task<int> GetPlanWidth()
//        {


//            ////NAV.WarehouseControlManagement_PortClient pc = new NAV.WarehouseControlManagement_PortClient();

//            ////NAV.APIVersion request = new NAV.APIVersion();
           
//            ////pc.APIVersionAsync(request);

//            ////var tcs = new TaskCompletionSource<int>();
//            ////string username = "";

//            ////Task.Run(async () =>
//            ////{
//            ////    try
//            ////    {
//            ////        NAV.DBAuthorization_Result result = await GetNAV().DBAuthorizationAsync(Global.DeviceID, code, username);
//            ////        bool rv = result.Body.return_value;
//            ////        Global.UserName = result.Body.v_UserName;
//            ////        tcs.SetResult(rv);
//            ////    }
//            ////    catch (Exception ex)
//            ////    {
//            ////        tcs.SetException(ex);
//            ////    }
//            ////});
//            ////return tcs.Task;

//        }

//        private void Fc_Faulted(object sender, EventArgs e)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
