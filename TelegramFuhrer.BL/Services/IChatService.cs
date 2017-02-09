using System.Threading.Tasks;
using TelegramFuhrer.BL.Models;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.BL.Services
{
	public interface IChatService
	{
		Task<ChatActionResult> AddUserAsync(string title, string username);

		Task AddUserAsync(Chat chat, User user);

		Task<ChatActionResult> RemoveUserAsync(string title, string username);

		Task RemoveUserAsync(Chat chat, User user);
	}
}
