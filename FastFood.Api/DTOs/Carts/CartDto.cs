using System.Collections.Generic;

namespace FastFood.Api.DTOs.Carts
{
    public class CartDto
    {
        public int Id { get; set; }

        public List<CartItemDto> Items { get; set; }
            = new List<CartItemDto>();
    }
}