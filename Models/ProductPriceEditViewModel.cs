using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductsValidation.Models
{
    public class ProductPriceEditViewModel
    {
        [EnumDataType(typeof(Product.Category))]
        public Product.Category Category { get; set; }

        [Required]
        public List<ProductPriceEditItemViewModel> Products { get; set; } =
            new List<ProductPriceEditItemViewModel>();
    }

    public class ProductPriceEditItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Range(typeof(decimal), "0", "100000")]
        public decimal Price { get; set; }
    }
}
