namespace CloudSalesAPI.Models
{
    public class CustomerAccount
    {
        public int Id { get; set; }
        public int CustomerId { get; set; } // Foreign key to Customer class
        public string AccountName { get; set; } = string.Empty;
        public List<PurchasedProduct> PurchasedProduct { get; set; } = new List<PurchasedProduct>();
    }
}
