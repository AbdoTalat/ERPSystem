using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Enums
{
    public enum PaymentMethod
    {
        Cash = 0,
        BankTransfer = 1,
        CreditCard = 2,
        PayPal = 3,
        Another = 4
    }
}
