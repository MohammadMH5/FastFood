using FastFood.Api.Data;
using FastFood.Api.Models;
using FastFood.Api.Repositories;

namespace FastFood.Api.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private IProductRepository? _products;
        private IGenericRepository<Category>? _categories;
        private ICartRepository? _carts;
        private IOrderRepository? _orders;
        private IPaymentRepository? _payments;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IProductRepository Products
        {
            get
            {
                return _products ??=
                    new ProductRepository(_context);
            }
        }

        public IGenericRepository<Category> Categories
        {
            get
            {
                return _categories ??=
                    new GenericRepository<Category>(_context);
            }
        }

        public ICartRepository Carts
        {
            get
            {
                return _carts ??=
                    new CartRepository(_context);
            }
        }

        public IOrderRepository Orders
        {
            get
            {
                return _orders ??=
                    new OrderRepository(_context);
            }
        }

        public IPaymentRepository Payments
        {
            get
            {
                return _payments ??=
                    new PaymentRepository(_context);
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            // ApplicationDbContext is managed by dependency injection.
            // Do not dispose it manually here.
        }
    }
}