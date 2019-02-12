using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace mysurging.demo.IModuleServices
{
    [ServiceBundle("api/{Service}")]  //服务标记
    public interface IUserService : IServiceKey
    {
        Task<string> GetUserName(int id);
    }
}
