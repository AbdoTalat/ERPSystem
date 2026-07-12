using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Common
{
    public interface IHasTenant
    {
        public Guid TenantId { get; set; }
    }
}
