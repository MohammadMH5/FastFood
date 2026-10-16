using System.ComponentModel.DataAnnotations;

namespace FastFood.Api.DTOs.Products
{
    public class UpdateProductDto
    {
        [Required(ErrorMessage = "نام محصول الزامی است.")]
        [StringLength(
            100,
            MinimumLength = 2,
            ErrorMessage = "نام محصول باید بین 2 تا 100 کاراکتر باشد.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(
            500,
            ErrorMessage = "توضیحات محصول نمی‌تواند بیشتر از 500 کاراکتر باشد.")]
        public string? Description { get; set; }

        [Range(
            0.01,
            double.MaxValue,
            ErrorMessage = "قیمت محصول باید بیشتر از صفر باشد.")]
        public decimal Price { get; set; }

        

        public bool IsAvailable { get; set; }

        
        public int CategoryId { get; set; }
    }
}