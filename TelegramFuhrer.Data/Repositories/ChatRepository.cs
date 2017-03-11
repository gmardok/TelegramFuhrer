using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.Data.Repositories
{
	public class ChatRepository : BaseRepository<Chat>
	{
		public async Task<IList<Chat>> FindByTitleAsync(string keywords)
		{
			return await Context.Chats.Where(c => c.Title.Contains(keywords)).ToListAsync();
		}

        public async Task<IList<Chat>> GetAutoKickAsync()
        {
            return await Context.Chats.Where(c => c.AutoKick).ToListAsync();
        }

        public async Task<IList<Chat>> GetAutoAddAsync()
        {
            return await Context.Chats.Where(c => c.AutoAdd).ToListAsync();
        }
    }
}
