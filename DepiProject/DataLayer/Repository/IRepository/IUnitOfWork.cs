namespace DataLayer.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IShoppingCartRepository ShoppingCart { get; }
        IUserRepository User { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailsRepository OrderDetails { get; }
        IProductRepository Product { get; }
        ICategoryRepository Category { get; }

        void Save();
    }
}
