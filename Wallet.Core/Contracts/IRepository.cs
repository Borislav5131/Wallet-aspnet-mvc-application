namespace Wallet.Core.Constants
{
    public interface IRepository
    {
        void Add<T>(T entity)
            where T : class;

        void Remove<T>(T entity)
            where T : class;

        IQueryable<T> All<T>()
            where T : class;

        int SaveChanges();
    }
}
