namespace MyDpprProject.Models
{
    public class Sale
    {
        public int SaleId { get; set; }

        public int PropertyId { get; set; }

        public string Title { get; set; }

        public string Address { get; set; }

        public int UserId { get; set; }

        public string FullName { get; set; }

        public DateTime SaleDate { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
