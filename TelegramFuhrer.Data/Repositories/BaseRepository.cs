using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Threading.Tasks;

namespace TelegramFuhrer.Data.Repositories
{
	public class BaseRepository<T> where T:class
	{
		protected readonly FuhrerContext Context;

		public BaseRepository()
		{
            Context = new FuhrerContext();
		}

		public virtual async Task AddAsync(T entity)
		{
			Context.Set<T>().Add(entity);
			await Context.SaveChangesAsync();
		}

	    public async Task<IList<T>> GetAllAsync()
	    {
	        return await Context.Set<T>().ToListAsync();
	    }

        public async Task<T> GetAsync(int id)
        {
            return await Context.Set<T>().FindAsync(id);
        }

        public virtual async Task SaveChangesAsync()
		{
			await Context.SaveChangesAsync();
		}
	}
}
