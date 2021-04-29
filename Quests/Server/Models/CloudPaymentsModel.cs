using System;

namespace Quests.Server.Models
{
    public class CloudPaymentsModel
    {
        public int TransactionId { get; set; } = 0;
        public string Amount { get; set; } = "";
        public string Currency { get; set; } = "";
        public string DateTime { get; set; } = "";
        public string CardFirstSix { get; set; } = "";
        public string CardLastFour { get; set; } = "";
        public string CardType { get; set; } = "";
        public string CardExpDate { get; set; } = "";
        public string TestMode { get; set; } = "";
        public string Status { get; set; } = "";
        public string OperationType { get; set; } = "";
        public string GatewayName { get; set; } = "";
        public string InvoiceId { get; set; } = "";
        public string AccountId { get; set; } = "";
        public string SubscriptionId { get; set; } = "";
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string IpAddress { get; set; } = "";
        public string IpCountry { get; set; } = "";
        public string IpCity { get; set; } = "";
        public string IpRegion { get; set; } = "";
        public string IpDistrict { get; set; } = "";
        public string Issuer { get; set; } = "";
        public string IssuerBankCountry { get; set; } = "";
        public string Description { get; set; } = "";
        public string Data { get; set; } = "";
        public string Token { get; set; } = "";
        public string TotalFee { get; set; } = "";
        public string CardProduct { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public string FallBackScenarioDeclinedTransactionId { get; set; } = "";

    }
}
