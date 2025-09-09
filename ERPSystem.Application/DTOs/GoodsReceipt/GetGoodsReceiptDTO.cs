using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.DTOs.GoodsReceipt
{
    public class GetGoodsReceiptDTO
    {
        public DateTime ReceivedDate { get; set; }
        public string? Notes { get; set; }
        public int ReceivedByName { get; set; }
    }
}
