using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quests.Server.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Amount { get; set; }
        public InvoiceStatus InvoiceStatus { get; set; }

    }

    public enum InvoiceStatus
    {
        NotPayment = 0,
        Payment = 1
    }
}
