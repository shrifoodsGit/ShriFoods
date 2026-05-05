using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShriFoods.Model
{
    public class OrdersModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }


        public string CartId { get; set; }
        public string CartTotal { get; set; }
        public string CustomerName { get; set; }
        public string CustomerUniqueid { get; set; }
        public string CustomerContact { get; set; }
        public string CustomerEMail { get; set; }
        public string CustomerAddress { get; set; }

    }
}
