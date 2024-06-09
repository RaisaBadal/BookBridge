using BookBridge.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace BookBridge.Infrastructure.Repositories
{
    public abstract class AbstractClass<T> where T : class
    {
        protected readonly BookBridgeDb Context;
        protected readonly DbSet<T> DbSet;

        protected AbstractClass(BookBridgeDb context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

    }
}
