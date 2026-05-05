using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ShriFoods.Model
{
    public class CartItemModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }

        public string ProductId { get; set; }

        public string ProductUniqueId { get; set; }

        public string ProductName { get; set; }

        public string ProductPrice { get; set; }
        
        public string ProductQty { get; set; }
        public string CartQty { get; set; }
        public string CartTotal { get; set; }
        public string UserUniqueId { get; set; }
        public string UserFirstName { get; set; }

    }
}
