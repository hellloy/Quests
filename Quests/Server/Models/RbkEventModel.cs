using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quests.Server.Models.Rbk
{
    public class RbkEventModel
    {
        public string eventType { get; set; }
        public int eventID { get; set; }
        public DateTime occuredAt { get; set; }
        public string topic { get; set; }
        public Invoice invoice { get; set; }
        public Payment payment { get; set; }
    }

    public class Invoice
    {
        public string id { get; set; }
        public string shopID { get; set; }
        public DateTime createdAt { get; set; }
        public string status { get; set; }
        public DateTime dueDate { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public Metadata metadata { get; set; }
        public string product { get; set; }
        public string description { get; set; }
        public Cart[] cart { get; set; }
    }

    public class Metadata
    {
        public string order_id { get; set; }
    }

    public class Cart
    {
        public int quantity { get; set; }
        public int price { get; set; }
        public int cost { get; set; }
        public Taxmode taxMode { get; set; }
    }

    public class Taxmode
    {
        public string rate { get; set; }
    }

    public class Payment
    {
        public string id { get; set; }
        public DateTime createdAt { get; set; }
        public string status { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public string paymentToolToken { get; set; }
        public string paymentSession { get; set; }
        public Contactinfo contactInfo { get; set; }
        public string ip { get; set; }
        public string fingerprint { get; set; }
        public Payer payer { get; set; }
        public int fee { get; set; }
    }

    public class Contactinfo
    {
        public string email { get; set; }
    }

    public class Payer
    {
        public string payerType { get; set; }
        public string paymentToolToken { get; set; }
        public string paymentSession { get; set; }
        public Paymenttooldetails paymentToolDetails { get; set; }
        public Clientinfo clientInfo { get; set; }
        public Contactinfo1 contactInfo { get; set; }
    }

    public class Paymenttooldetails
    {
        public string detailsType { get; set; }
        public string bin { get; set; }
        public string lastDigits { get; set; }
        public string cardNumberMask { get; set; }
        public string paymentSystem { get; set; }
        public string issuerCountry { get; set; }
        public string bankName { get; set; }
    }

    public class Clientinfo
    {
        public string ip { get; set; }
        public string fingerprint { get; set; }
    }

    public class Contactinfo1
    {
        public string email { get; set; }
    }
}
