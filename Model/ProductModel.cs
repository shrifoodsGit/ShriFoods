
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShriFoods.Model
{
    public class ProductModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        public string ProductUniqueId { get; set; }
        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }
        public byte[] ProductImage { get; set; }
        public string ProductWeight { get; set; }
        public string ProductDescription { get; set; }
        public string ProductQty { get; set; }
        public string ProductRating { get; set; }
        public string ProductCategory { get; set; }

        // Insert all available products from Database as a list here

    }
}
