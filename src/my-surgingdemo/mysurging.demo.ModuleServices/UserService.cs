using mysurging.demo.IModuleServices;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using System;
using System.Threading.Tasks;

namespace mysurging.demo.ModuleServices
{
    [ModuleName("User")]  //标识实例化名称
    public class UserService : ProxyServiceBase, IUserService
    {
        public Task<string> GetUserName(int id)
        {
            return Task.FromResult<string>("user number : " + id);
        }
    }
}
