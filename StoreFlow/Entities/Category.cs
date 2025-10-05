using System.ComponentModel.DataAnnotations;

namespace StoreFlow.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }

        
        public bool CategoryStatus { get; set; }
    
        public  string? CategoryName { get; set; }
      
        public List<Product>? Products { get; set; }
    }
}
