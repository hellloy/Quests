using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quests.Shared.VM
{
    public class RbkMoneyInvoiceResponse
    {
        public Invoice invoice { get; set; }
        public Invoiceaccesstoken invoiceAccessToken { get; set; }

        public class Invoice
        {
            public string id { get; set; }
            public string shopID { get; set; }
            public string externalID { get; set; }
            public DateTime createdAt { get; set; }
            public DateTime dueDate { get; set; }
            public int amount { get; set; }
            public string currency { get; set; }
            public string product { get; set; }
            public string description { get; set; }
            public string invoiceTemplateID { get; set; }
            public Cart[] cart { get; set; }
            public Metadata metadata { get; set; }
            public string status { get; set; }
            public string reason { get; set; }
        }

        public class Metadata
        {
        }

        public class Cart
        {
            public string product { get; set; }
            public int quantity { get; set; }
            public int price { get; set; }
            public int cost { get; set; }
            public Taxmode taxMode { get; set; }
        }

        public class Taxmode
        {
            public string type { get; set; }
        }

        public class Invoiceaccesstoken
        {
            public string payload { get; set; }
        }

    }
}
