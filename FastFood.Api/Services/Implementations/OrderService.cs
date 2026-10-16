using FastFood.Api.DTOs.Orders;
using FastFood.Api.Exceptions;
using FastFood.Api.Models;
using FastFood.Api.Services.Interfaces;
using FastFood.Api.UnitOfWork;

namespace FastFood.Api.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private const int MaxQuantityPerProduct = 20;

        private static readonly string[] AllowedStatuses =
        {
            "Pending",
            "Preparing",
            "Ready",
            "Completed",
            "Cancelled"
        };

        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderDto?> CreateOrderFromCartAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new BusinessException(
                    "شناسه کاربر نامعتبر است.");
            }

            var cart = await _unitOfWork.Carts
                .GetCartByUserIdAsync(userId);

            if (cart == null || !cart.CartItems.Any())
            {
                return null;
            }

            // Validate all cart items before creating the order.
            foreach (var cartItem in cart.CartItems)
            {
                if (cartItem.Quantity <= 0 ||
                    cartItem.Quantity > MaxQuantityPerProduct)
                {
                    throw new BusinessException(
                        "تعداد محصول در سبد خرید نامعتبر است.");
                }

                if (cartItem.Product == null)
                {
                    throw new BusinessException(
                        "یکی از محصولات سبد خرید دیگر وجود ندارد.");
                }

                if (!cartItem.Product.IsAvailable)
                {
                    throw new BusinessException(
                        $"محصول «{cartItem.Product.Name}» در حال حاضر موجود نیست.");
                }

                if (cartItem.Product.Price < 0)
                {
                    throw new BusinessException(
                        "قیمت یکی از محصولات نامعتبر است.");
                }
            }

            var order = new Order
            {
                UserId = userId,
                Status = "Pending"
            };

            foreach (var cartItem in cart.CartItems)
            {
                var product = cartItem.Product
                    ?? throw new BusinessException(
                        "یکی از محصولات سبد خرید دیگر وجود ندارد.");

                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Quantity = cartItem.Quantity
                };

                order.OrderItems.Add(orderItem);
            }

            order.TotalPrice = order.OrderItems
                .Sum(item => item.UnitPrice * item.Quantity);

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.Carts.ClearItemsAsync(cart);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(order);
        }

        public async Task<IEnumerable<OrderDto>> GetMyOrdersAsync(int userId)
        {
            var orders = await _unitOfWork.Orders
                .GetByUserIdAsync(userId);

            return orders.Select(MapToDto).ToList();
        }

        public async Task<OrderDto?> GetOrderByIdAsync(
            int orderId,
            int userId)
        {
            var order = await _unitOfWork.Orders
                .GetByIdWithItemsAsync(orderId);

            if (order == null || order.UserId != userId)
            {
                return null;
            }

            return MapToDto(order);
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Orders
                .GetAllWithItemsAsync();

            return orders.Select(MapToDto).ToList();
        }

        public async Task<OrderDto?> GetOrderByIdForAdminAsync(int orderId)
        {
            var order = await _unitOfWork.Orders
                .GetByIdWithItemsAsync(orderId);

            if (order == null)
            {
                return null;
            }

            return MapToDto(order);
        }

        public async Task<bool> UpdateOrderStatusAsync(
            int orderId,
            string status)
        {
            if (orderId <= 0)
            {
                throw new BusinessException(
                    "شناسه سفارش نامعتبر است.");
            }

            var order = await _unitOfWork.Orders
                .GetByIdAsync(orderId);

            if (order == null)
            {
                return false;
            }

            var normalizedStatus = status?.Trim();

            var validStatus = AllowedStatuses.FirstOrDefault(
                allowed => string.Equals(
                    allowed,
                    normalizedStatus,
                    StringComparison.OrdinalIgnoreCase));

            if (validStatus == null)
            {
                throw new BusinessException(
                    "وضعیت سفارش نامعتبر است.");
            }

            order.Status = validStatus;

            _unitOfWork.Orders.Update(order);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private static OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                Items = order.OrderItems
                    .Select(item => new OrderItemDto
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity
                    })
                    .ToList()
            };
        }
    }
}
