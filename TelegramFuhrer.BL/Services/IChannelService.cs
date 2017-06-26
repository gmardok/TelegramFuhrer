using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramFuhrer.BL.Models;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.BL.Services
{
	public interface IChannelService
	{
		Task<ChatActionResult> AddUserAsync(string channelName, string username, User actionUser);

		Task<ChatActionResult> RemoveUserAsync(string channelName, string username, User actionUser);
	}
}
