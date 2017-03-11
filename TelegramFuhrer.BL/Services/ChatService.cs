using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramFuhrer.BL.Models;
using TelegramFuhrer.BL.TL;
using TelegramFuhrer.Data.Entities;
using TelegramFuhrer.Data.Repositories;

namespace TelegramFuhrer.BL.Services
{
	public class ChatService : IChatService
    {
	    private readonly ChatRepository _chatRepository;

	    private readonly IChatTL _chatTL;

	    private readonly IUserService _userService;

		public ChatService(ChatRepository chatRepository, IChatTL chatTL, IUserService userService)
		{
			_chatRepository = chatRepository;
			_chatTL = chatTL;
			_userService = userService;
		}

		public async Task<ChatActionResult> AddUserAsync(string title, string username)
		{
			var user = await _userService.FindUserByUsernameAsync(username);
			var chats = await RegisterChat(title);
			
			if (chats.Count > 1)
				return new ChatActionResult
				{
					Success = false,
					Chats = chats,
					User = user
				};

			await _chatTL.AddUserAsync(chats[0], user);
			return new ChatActionResult {Success = true};
		}

		public async Task AddUserAsync(Chat chat, User user)
	    {
			await _chatTL.AddUserAsync(chat, user);
		}

		public async Task<ChatActionResult> RemoveUserAsync(string title, string username)
	    {
			var user = await _userService.FindUserByUsernameAsync(username);
			var chats = await _chatRepository.FindByTitleAsync(title);
			if (chats.Count == 0)
			{
				chats = await _chatTL.FindByTitleAsync(title);
				foreach (var chat in chats)
				{
					await _chatRepository.AddAsync(chat);
				}
			}

			if (chats.Count == 0) throw new ArgumentException($"Chat {title} doesnot exists");
			if (chats.Count > 1)
				return new ChatActionResult
				{
					Success = false,
					Chats = chats,
					User = user
				};

			await _chatTL.RemoveUserAsync(chats[0], user);
			return new ChatActionResult { Success = true };
		}

		public async Task RemoveUserAsync(Chat chat, User user)
	    {
			await _chatTL.RemoveUserAsync(chat, user);
		}

	    public async Task<string> GetChatList()
	    {
            var chats = await _chatRepository.GetAllAsync();
	        var header = " Autokick | AutoAdd | Id | Title \n";
            return header + string.Join("\n", chats.Select(c => $"{c.AutoKick} | {c.AutoAdd} | {c.Id} | {c.Title}"));
        }

	    public async Task<IList<Chat>> RegisterChat(string title)
	    {
            var chats = await _chatRepository.FindByTitleAsync(title);
            if (chats.Count == 0)
            {
                chats = await _chatTL.FindByTitleAsync(title);
                foreach (var chat in chats)
                {
                    await _chatRepository.AddAsync(chat);
                }
            }

            if (chats.Count == 0) throw new ArgumentException($"Chat {title} doesnot exists");
            return chats;
	    }
    }
}
