namespace ShriFoods.Model
{
    public class ProductModel
    {

        public int ProductId { get; set; }

        public string ProductUniqueId { get; set; }
        public string ProductName { get; set; }

        public string ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public string ProductWeight { get; set; }
        public string ProductDescription { get; set; }
        public string ProductQTY { get; set; }
        public string ProductRating { get; set; }
        public string ProductCategory { get; set; }

        // Insert all available products from Database as a list here

    }
}
