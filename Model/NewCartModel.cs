using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShriFoods.Model
{
    public class NewCartModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }

        public string UserId { get; set; }

        public int ProductId { get; set; }

        // 500g / 1kg
        public string ProductWeight { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public DateTime AddedDate { get; set; }

        public string ProductName { get; set; }


        [ForeignKey("ProductId")]
        public ProductModel? Product { get; set; }

    }
}
