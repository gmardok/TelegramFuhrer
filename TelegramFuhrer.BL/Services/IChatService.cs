using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramFuhrer.BL.Models;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.BL.Services
{
	public interface IChatService
	{
		Task<ChatActionResult> AddUserAsync(string title, string username, User actionUser);

		Task AddUserAsync(Chat chat, User user);

		Task<ChatActionResult> RemoveUserAsync(string title, string username, User actionUser);

		Task RemoveUserAsync(Chat chat, User user);

	    Task<string> GetChatList();

	    Task<IList<Chat>> RegisterChat(string title);

	    Task<ChatActionResult> AddChatAdminAsync(string title, string username);

	    Task AddChatAdminAsync(Chat chat, User user);

	    Task<ChatActionResult> RemoveChatAdminAsync(string title, string username);

	    Task RemoveChatAdminAsync(Chat chat, User user);
	}
}
