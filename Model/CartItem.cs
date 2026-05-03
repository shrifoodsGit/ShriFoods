using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ShriFoods.Model
{
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }

        public string ProductUniqueId { get; set; }

        public string ProductName { get; set; }

        public string ProductPrice { get; set; }
        

        public string ProductQty { get; set; }
    }
}
