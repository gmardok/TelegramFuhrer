using System.Data.Entity;
using System.Threading.Tasks;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.Data.Repositories
{
	public class UserRepository : BaseRepository<User>
	{
		public async Task<User> GetUserByTLIdAsync(int tlId)
		{
			return await Context.Users.FirstOrDefaultAsync(u => u.Id == tlId);
		}

		public async Task<User> GetUserByUsernameAsync(string username)
		{
			return await Context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
		}
	}
}
