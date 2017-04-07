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

	    private readonly UserChatRepository _userChatRepository;

        private readonly IChatTL _chatTL;

	    private readonly IUserService _userService;

		public ChatService(ChatRepository chatRepository, IChatTL chatTL, IUserService userService, UserChatRepository userChatRepository)
		{
			_chatRepository = chatRepository;
			_chatTL = chatTL;
			_userService = userService;
		    _userChatRepository = userChatRepository;
		}

		public async Task<ChatActionResult> AddUserAsync(string title, string username, User actionUser)
		{
			var user = await _userService.FindUserByUsernameAsync(username);
		    var chats = actionUser.IsGlobalAdmin
		        ? await RegisterChat(title)
		        : (await _chatRepository.GetUserChats(actionUser.UserId)).Where(c => c.Title.Contains(title)).ToList();
			
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

		public async Task<ChatActionResult> RemoveUserAsync(string title, string username, User actionUser)
	    {
			var user = await _userService.FindUserByUsernameAsync(username);
		    var chats = actionUser.IsGlobalAdmin
		        ? await _chatRepository.FindByTitleAsync(title)
		        : (await _chatRepository.GetUserChats(actionUser.UserId)).Where(c => c.Title.Contains(title)).ToList();

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
            var existingChats = await _chatRepository.FindByTitleAsync(title);
            var chats = await _chatTL.FindByTitleAsync(title);
            foreach (var chat in chats)
            {
                if (existingChats.Any(ec => ec.Id == chat.Id)) continue;
                await _chatRepository.AddAsync(chat);
            }

            if (chats.Count == 0) throw new ArgumentException($"Chat {title} doesnot exists");
            return chats;
	    }

        public async Task<ChatActionResult> AddChatAdminAsync(string title, string username)
        {
            var user = await _userService.FindUserByUsernameAsync(username);
            var chats = await _chatRepository.FindByTitleAsync(title);

            if (chats.Count > 1)
                return new ChatActionResult
                {
                    Success = false,
                    Chats = chats,
                    User = user
                };

            await _userChatRepository.AddChatAdminAsync(chats[0], user);
            return new ChatActionResult { Success = true };
        }

        public async Task AddChatAdminAsync(Chat chat, User user)
        {
            await _userChatRepository.AddChatAdminAsync(chat, user);
        }

        public async Task<ChatActionResult> RemoveChatAdminAsync(string title, string username)
        {
            var user = await _userService.FindUserByUsernameAsync(username);
            var chats = await _chatRepository.FindByTitleAsync(title);

            if (chats.Count == 0) throw new ArgumentException($"Chat {title} doesnot exists");
            if (chats.Count > 1)
                return new ChatActionResult
                {
                    Success = false,
                    Chats = chats,
                    User = user
                };

            await _userChatRepository.RemoveChatAdminAsync(chats[0], user);
            return new ChatActionResult { Success = true };
        }

        public async Task RemoveChatAdminAsync(Chat chat, User user)
        {
            await _userChatRepository.RemoveChatAdminAsync(chat, user);
        }
    }
}
