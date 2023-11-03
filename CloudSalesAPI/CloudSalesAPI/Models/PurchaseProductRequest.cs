namespace CloudSalesAPI.Models
{
    public class PurchaseProductRequest
    {
        public int ExternalProductId { get; set; }
        public int AccountId { get; set; }
        public int Quantity { get; set; }
    }
}
