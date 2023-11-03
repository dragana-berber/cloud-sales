namespace CloudSalesAPI.Models
{
    public class PurchasedProduct { 
        public int Id { get; set; }
        public int ExternalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public State State { get; set; }
        public DateTime ValidToDate { get; set; }
        public int CustomerAccountId { get; set; }
}

    public enum State
    {
        Active,
        Expired,
        Suspended
    }
}
