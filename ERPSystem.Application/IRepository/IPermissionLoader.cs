using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.IRepository
{
    public interface IPermissionLoader
    {
        List<string> LoadAllPermissions();
        Dictionary<string, List<string>> LoadGroupedPermissions();
    }
}
