using System.ComponentModel.DataAnnotations;

namespace FastFood.Api.DTOs.Categories
{
    public class UpdateCategoryDto
    {
        [Required(ErrorMessage = "نام دسته‌بندی الزامی است.")]
        [StringLength(
            50,
            MinimumLength = 2,
            ErrorMessage = "نام دسته‌بندی باید بین 2 تا 50 کاراکتر باشد.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(
            300,
            ErrorMessage = "توضیحات دسته‌بندی نمی‌تواند بیشتر از 300 کاراکتر باشد.")]
        public string? Description { get; set; }
    }
}