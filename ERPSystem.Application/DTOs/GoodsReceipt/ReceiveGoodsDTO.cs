using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.DTOs.GoodsReceipt
{
    public class ReceiveGoodsDTO
    {
        public string? Notes { get; set; }
        public int ReceivedById { get; set; }

        public List<ReceiveGoodsLinesDTO> Lines { get; set; } = new List<ReceiveGoodsLinesDTO>();
    }

    public class ReceiveGoodsLinesDTO
    {
        public int ProductId { get; set; }
        [Range(0, int.MaxValue)]
        public int ReceivedQuantity { get; set; }
        [Range(0, int.MaxValue)]
        public int DamagedQuantity { get; set; }
    }
}
