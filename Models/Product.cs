using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductsValidation.Models
{
    public class Product : IValidatableObject
    {
        public enum Category { Toy, Technique, Clothes, Transport }

        public int Id { get; set; }

        [EnumDataType(typeof(Category))]
        public Category Type { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(typeof(decimal), "0", "100000")]
        public decimal Price { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Description))
            {
                yield break;
            }

            if (Description.Length <= 2)
            {
                yield return new ValidationResult(
                    "Description must contain more than 2 characters.",
                    new[] { nameof(Description) });
            }

            if (string.Equals(Description, Name, StringComparison.Ordinal))
            {
                yield return new ValidationResult(
                    "Description must not be equal to the product name.",
                    new[] { nameof(Description) });
            }

            if (!string.IsNullOrEmpty(Name) &&
                !Description.StartsWith(Name, StringComparison.Ordinal))
            {
                yield return new ValidationResult(
                    "Description must start with the product name.",
                    new[] { nameof(Description) });
            }
        }
    }
}
