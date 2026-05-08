using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShriFoods.Model
{
    public class NewOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        public string UserId { get; set; }

        public string OrderNumber { get; set; }

        public decimal TotalAmount { get; set; }

        public string OrderStatus { get; set; }

        public string PaymentStatus { get; set; }

        public string PaymentMethod { get; set; }

        public string ShippingAddress { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime OrderedDate { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
