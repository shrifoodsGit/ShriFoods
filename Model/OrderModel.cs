using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShriFoods.Model
{
    public class OrderModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        public string OrderUniqueId { get; set; }
        public int CartId { get; set; }
        public string CartTotal { get; set; }
        public string CustomerName { get; set; }
        public string CustomerUniqueid { get; set; }
        public string CustomerContact { get; set; }
        public string CustomerEMail { get; set; }
        public string CustomerAddress { get; set; }

        public DateOnly OrderDate { get; set; }
        public string ProductName { get; set; }
        public string ProductQty { get; set; }
        public string ProductPrice { get; set; }
        public string GrandTotal { get; set; }

    }
}
