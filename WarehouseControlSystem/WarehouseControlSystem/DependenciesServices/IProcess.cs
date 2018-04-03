using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WarehouseControlSystem.DependenciesServices
{
    public interface IProcess
    {
        Task<XElement> Process(string functionname, string requestbody, CancellationTokenSource cts);
    }
}
