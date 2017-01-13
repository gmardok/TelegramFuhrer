using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TeleSharp.TL;

namespace TelegramFuhrer.Data.Repositories
{
	public class ChatRepository : BaseRepository<TLChat>
	{
		public async Task<IList<TLChat>> FindByTitleAsync(string keywords)
		{
			return await Context.Chats.Where(c => c.title.Contains(keywords)).ToListAsync();
		}
	}
}
