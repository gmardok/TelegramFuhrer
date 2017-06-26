using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramFuhrer.Data.Entities;
using TeleSharp.TL;

namespace TelegramFuhrer.BL.TL
{
	public interface IChannelTL
	{
		Task<IList<TLChannel>> FindByTitleAsync(string keywords);

		Task RemoveUserAsync(TLChannel channel, User user);

		Task AddUserAsync(TLChannel channel, User user);
	}
}
