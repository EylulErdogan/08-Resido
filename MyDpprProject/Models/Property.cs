using System.ComponentModel.DataAnnotations;
namespace MyDpprProject.Models
{
    public class Property
    {
        public int PropertyId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string Address { get; set; }

        public int BedCount { get; set; }

        public int BathCount { get; set; }

        public int SquareMeter { get; set; }

        public string ImageUrl { get; set; }

        public int PropertyTypeId { get; set; }

        public string TypeName { get; set; }
    }
}
