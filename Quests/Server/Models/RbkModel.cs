using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quests.Server.Models
{
    public class RbkModel
    {
        public string shopID { get; set; }
        public DateTime dueDate { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public string product { get; set; }
        public string description { get; set; }
        public Cart[] cart { get; set; }
        public Metadata metadata { get; set; }
    }
    public class Metadata
    {
        public string order_id { get; set; }
    }

    public class Cart
    {
        public int price { get; set; }
        public string product { get; set; }
        public int quantity { get; set; }
        public Taxmode taxMode { get; set; }
    }

    public class Taxmode
    {
        public string rate { get; set; }
        public string type { get; set; }
    }

}
