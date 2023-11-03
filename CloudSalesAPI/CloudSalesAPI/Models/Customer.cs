namespace CloudSalesAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<CustomerAccount> Accounts { get; set; } = new List<CustomerAccount>();
    }
}
