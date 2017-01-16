using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.BL.Services
{
	public interface IUserService
	{
		Task<User> FindUserByUsernameAsync(string username);
	}
}
