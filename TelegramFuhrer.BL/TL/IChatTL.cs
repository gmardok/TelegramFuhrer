using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.BL.TL
{
	public interface IChatTL
	{
		Task<IList<Chat>> FindByTitleAsync(string keywords);

		Task AddUserAsync(Chat chat, User user);

		Task RemoveUserAsync(Chat chat, User user);
	}
}
