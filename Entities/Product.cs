using System.ComponentModel.DataAnnotations.Schema;

namespace adventureWorks.Entities
{
  [Table("Product", Schema = "SalesLT")]  
  public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string ProductNumber { get; set; }
        public string Color { get; set; }
        public decimal StandardCost { get; set; }
        public decimal ListPrice { get; set; }
        public DateTime SellStartDate { get; set; }
        public DateTime SellEndDate { get; set; }
        public DateTime DiscontinuedDate { get; set; }

        public override string ToString(){
          return $"Product Name: {Name} - Product Id: {ProductID} - List Price: {ListPrice} ";
        }
    }

}