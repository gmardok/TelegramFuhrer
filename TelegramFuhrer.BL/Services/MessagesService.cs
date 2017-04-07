using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TelegramFuhrer.BL.TL;
using TelegramFuhrer.Data.Entities;
using TelegramFuhrer.Data.Repositories;
using TeleSharp.TL;
using TeleSharp.TL.Messages;

namespace TelegramFuhrer.BL.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly IMessagesTL _messagesTL;

        private readonly IChatTL _chatTL;

        private readonly UserRepository _userRepository;

        private readonly ChatRepository _chatRepository;

        IUserService _userService;

        public MessagesService(IMessagesTL messagesTL, UserRepository userRepository, IChatTL chatTL, ChatRepository chatRepository, IUserService userService)
        {
            _messagesTL = messagesTL;
            _chatTL = chatTL;
            _userRepository = userRepository;
            _chatRepository = chatRepository;
            _userService = userService;
        }

        public async Task<List<User>> GetDialogsAsync(TLDialogs dialogs)
        {
            var result = new List<User>();
            if (dialogs == null)
                dialogs = await _messagesTL.GetDialogsAsync();
            if (dialogs?.dialogs == null || dialogs.dialogs.lists.Count == 0)
                return result;
            var newUserDialogs = dialogs.dialogs.lists.Where(d => d.unread_count > 0 && d.peer is TLPeerUser).ToList();
            foreach (var newUserDialog in newUserDialogs)
            {
                var tlUser = (newUserDialog.peer as TLPeerUser);
                if (tlUser == null) continue;
                var user = await _userRepository.GetUserByTLIdAsync(tlUser.user_id);
                if (user == null) continue;
                result.Add(user);
            }

            return result;
        }

        public async Task<string> GetUserMessagesAsync(User user)
        {
            if (!user.AccessHash.HasValue) return null;
            var tlUser = new TLInputPeerUser {user_id = user.Id, access_hash = user.AccessHash.Value};
            IList<TLMessage> messages;
            try
            {
                messages = await _messagesTL.GetMessagesAsync(tlUser);
            }
            catch (Exception ex)
            {
                if (!ex.Message.Equals("PEER_ID_INVALID"))
                    throw;
                await _userService.UpdateHashes();
                messages = await _messagesTL.GetMessagesAsync(tlUser);
            }

            if (messages == null || !messages.Any()) return null;
            await _messagesTL.MarkUserMessagesAsReadAsync(tlUser);
            return messages[0].message;
        }

        public async Task<string> WaitForMessageAsync(User user)
        {
            var gotMessage = false;
            while (!gotMessage)
            {
                var dialogs = await _messagesTL.GetDialogsAsync();
                if (dialogs?.dialogs != null && dialogs.dialogs.lists.Count > 0
                    &&
                    dialogs.dialogs.lists.Any(
                        d => d.unread_count > 0 && d.peer is TLPeerUser && ((TLPeerUser) d.peer).user_id == user.Id))
                    gotMessage = true;
                else
                    Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }

            return await GetUserMessagesAsync(user);
        }

        public async Task SendMessageAsync(User user, string message)
        {
            if (!user.AccessHash.HasValue) return;
            var tlUser = new TLInputPeerUser { user_id = user.Id, access_hash = user.AccessHash.Value };
            await _messagesTL.SendMessageAsync(tlUser, message);
        }

        public async Task<TLDialogs> AutoKickAsync()
        {
            var dialogs = await _messagesTL.GetDialogsAsync();
            if (dialogs?.dialogs == null || dialogs.dialogs.lists.Count == 0)
                return dialogs;
            var autoKickChats = await _chatRepository.GetAutoKickAsync();
            foreach (var dialog in dialogs.dialogs.lists.Where(d => d.unread_count > 0 && d.peer is TLPeerChat).ToList())
            {
                var chatId = ((TLPeerChat) dialog.peer).chat_id;
                if (autoKickChats.All(c => c.Id != chatId)) continue;
                var users =
                    await _messagesTL.GetAddUserMessagesAsync(chatId, dialog.unread_count);
                foreach (var tlUser in users)
                {
                    var user = await _userRepository.GetUserByTLIdAsync(tlUser.id) ?? new User {Id = tlUser.id};
                    UserService.CopyUserProps(user, tlUser, false);
                    if (user.UserId == 0)
                        await _userRepository.AddAsync(user);
                    else
                        await _userRepository.SaveChangesAsync();

                    try
                    {
                        await _chatTL.RemoveUserAsync(new Chat { Id = chatId }, user);
                    }
                    catch (InvalidOperationException ex)
                    {
                        if (!ex.Message.Equals("USER_NOT_PARTICIPANT", StringComparison.InvariantCultureIgnoreCase))
                            throw;
                    }
                }

                await _messagesTL.MarkUserMessagesAsReadAsync(new TLInputPeerChat {chat_id = chatId});
            }

            return dialogs;
        }

        public async Task MarkUserMessagesAsReadAsync(User user)
        {
            if (!user.AccessHash.HasValue) return;
            var tlUser = new TLInputPeerUser { user_id = user.Id, access_hash = user.AccessHash.Value };
            await _messagesTL.MarkUserMessagesAsReadAsync(tlUser);
        }
    }
}
