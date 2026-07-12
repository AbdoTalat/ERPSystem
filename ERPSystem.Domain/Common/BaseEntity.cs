using ERPSystem.Domain.Entities;
using ERPSystem.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Common
{
    public abstract class BaseEntity : AuditableEntity
    {
        public int Id { get; set; }
    }
}
