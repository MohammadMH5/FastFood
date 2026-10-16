using FastFood.Api.DTOs.Carts;
using FastFood.Api.Exceptions;
using FastFood.Api.Models;
using FastFood.Api.Services.Interfaces;
using FastFood.Api.UnitOfWork;

namespace FastFood.Api.Services.Implementations
{
    public class CartService : ICartService
    {
        private const int MaxQuantityPerProduct = 20;

        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }                                         

        public async Task<CartDto?> GetCartAsync(int userId)
        {
            var cart = await _unitOfWork.Carts
                .GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                return null;
            }

            return new CartDto
            {
                Id = cart.Id,
                Items = cart.CartItems
                    .Select(ci => new CartItemDto
                    {
                        ProductId = ci.ProductId,
                        ProductName = ci.Product.Name,
                        Price = ci.Product.Price,
                        ImageUrl = ci.Product.ImageUrl,
                        Quantity = ci.Quantity
                    })
                    .ToList()
            };
        }

        public async Task<Cart> CreateCartAsync(int userId)
        {
            var existingCart = await _unitOfWork.Carts
                .GetCartByUserIdAsync(userId);

            if (existingCart != null)
            {
                return existingCart;
            }

            var cart = new Cart
            {
                UserId = userId
            };

            await _unitOfWork.Carts.AddAsync(cart);
            await _unitOfWork.SaveChangesAsync();

            return cart;
        }

        public async Task<bool> AddItemAsync(
            int userId,
            int productId,
            int quantity)
        {
            ValidateQuantity(quantity);

            var product = await _unitOfWork.Products
                .GetByIdAsync(productId);

            if (product == null)
            {
                throw new BusinessException(
                    "محصول مورد نظر وجود ندارد.");
            }

            if (!product.IsAvailable)
            {
                throw new BusinessException(
                    "این محصول در حال حاضر موجود نیست.");
            }

            var cart = await _unitOfWork.Carts
                .GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = await CreateCartAsync(userId);
            }

            var existingItem = cart.CartItems
                .FirstOrDefault(ci => ci.ProductId == productId);

            if (existingItem != null)
            {
                if (existingItem.Quantity > MaxQuantityPerProduct - quantity)
                {
                    throw new BusinessException(
                        "حداکثر تعداد هر محصول در سبد خرید ۲۰ عدد است.");
                }

                existingItem.Quantity += quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity
                });
            }

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateItemAsync(
            int userId,
            int productId,
            int quantity)
        {
            ValidateQuantity(quantity);

            var cart = await _unitOfWork.Carts
                .GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                return false;
            }

            var cartItem = cart.CartItems
                .FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem == null)
            {
                return false;
            }

            if (cartItem.Product == null)
            {
                throw new BusinessException(
                    "محصول مورد نظر دیگر وجود ندارد.");
            }

            if (!cartItem.Product.IsAvailable)
            {
                throw new BusinessException(
                    "این محصول در حال حاضر موجود نیست.");
            }

            cartItem.Quantity = quantity;

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveItemAsync(
            int userId,
            int productId)
        {
            var cart = await _unitOfWork.Carts
                .GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                return false;
            }

            var cartItem = cart.CartItems
                .FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem == null)
            {
                return false;
            }

            await _unitOfWork.Carts.RemoveItemAsync(cartItem);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ClearCartAsync(int userId)
        {
            var cart = await _unitOfWork.Carts
                .GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                return false;
            }

            await _unitOfWork.Carts.ClearItemsAsync(cart);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private static void ValidateQuantity(int quantity)
        {
            if (quantity < 1 || quantity > MaxQuantityPerProduct)
            {
                throw new BusinessException(
                    "تعداد محصول باید بین ۱ تا ۲۰ باشد.");
            }
        }
    }
}
