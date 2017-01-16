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

		public async Task AddAsync(T entity)
		{
			Context.Set<T>().Add(entity);
			await Context.SaveChangesAsync();
		}

		public async Task SaveChanges()
		{
			await Context.SaveChangesAsync();
		}
	}
}
