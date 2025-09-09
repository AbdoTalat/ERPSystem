using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.IRepository
{
    public interface IProductRepository
    {
        Task<List<int>> CheckProductsExistById(List<int> ProductIDs);
    }
}
