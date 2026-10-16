using System.ComponentModel.DataAnnotations;

namespace FastFood.Api.DTOs.Carts
{
    public class UpdateCartItemDto
    {
        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "تعداد محصول باید حداقل 1 باشد.")]
        public int Quantity { get; set; }
    }
}