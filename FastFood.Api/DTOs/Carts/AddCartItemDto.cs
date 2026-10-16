using System.ComponentModel.DataAnnotations;

namespace FastFood.Api.DTOs.Carts
{
    public class AddCartItemDto
    {
        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "تعداد محصول باید حداقل 1 باشد.")]
        public int Quantity { get; set; }

        public int ProductId { get; set; }
    }
}