using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleSharp.TL;

namespace TelegramFuhrer.BL.TL
{
	public interface IUserTL
	{
		Task<TLUser> FindUserByUsernameAsync(string username);
	}
}
