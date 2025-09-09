using ERPSystem.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public int? CreatedById { get; set; }
        public AppUser? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int? LastUpdatedById { get; set; }
        public AppUser? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}
