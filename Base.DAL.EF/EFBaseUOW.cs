using System;

namespace Base.DAL.EF
{
    using Base.Contracts.DAL;
    using Microsoft.EntityFrameworkCore;


    public class EFBaseUOW<TDbContext> : IBaseUOW
        where TDbContext : DbContext
    {
        protected readonly TDbContext UowDbContext;

        public EFBaseUOW(TDbContext dataContext)
        {
            UowDbContext = dataContext;
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await UowDbContext.SaveChangesAsync();
        }
    }
}
