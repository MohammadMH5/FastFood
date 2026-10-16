using System.Text.Json;
using FastFood.Web.Models;

namespace FastFood.Web.Services
{
    public class CartService
    {
        private const string CartSessionKey = "CartItems";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<CartItem> GetItems()
        {
            var session = _httpContextAccessor.HttpContext?.Session;

            if (session == null)
                return new List<CartItem>();

            var json = session.GetString(CartSessionKey);

            if (string.IsNullOrEmpty(json))
                return new List<CartItem>();

            return JsonSerializer.Deserialize<List<CartItem>>(json)
                   ?? new List<CartItem>();
        }

        public void AddItem(CartItem item)
        {
            var items = GetItems();

            var existingItem = items.FirstOrDefault(
                x => x.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += 1;
            }
            else
            {
                item.Quantity = 1;
                items.Add(item);
            }

            SaveItems(items);
        }
        public void UpdateQuantity(int productId, int quantity)
        {
            var items = GetItems();

            var item = items.FirstOrDefault(x => x.ProductId == productId);

            if (item == null)
                return;

            if (quantity <= 0)
            {
                items.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }

            SaveItems(items);
        }

        public void RemoveItem(int productId)
        {
            var items = GetItems();

            items.RemoveAll(x => x.ProductId == productId);

            SaveItems(items);
        }

        private void SaveItems(List<CartItem> items)
        {
            var session = _httpContextAccessor.HttpContext?.Session;

            if (session == null)
                throw new InvalidOperationException(
                    "نشست کاربر در دسترس نیست.");

            session.SetString(
                CartSessionKey,
                JsonSerializer.Serialize(items));
        }
    }
}
