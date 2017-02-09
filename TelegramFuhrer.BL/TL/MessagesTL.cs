using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeleSharp.TL;
using TeleSharp.TL.Messages;
using TLSharp.Core;

namespace TelegramFuhrer.BL.TL
{
    public class MessagesTL : IMessagesTL
    {
        private readonly TelegramClientEx _telegramClient;

        public MessagesTL(TelegramClientEx telegramClient)
        {
            _telegramClient = telegramClient;
        }

        public async Task<TLDialogs> GetDialogsAsync()
        {
            var dialogs = await _telegramClient.GetUserDialogsAsync();
            var result = dialogs as TLDialogs;
            if (result == null)
            {
                var slice = (TLDialogsSlice)dialogs;
                result = new TLDialogs
                {
                    chats = slice.chats,
                    dialogs = slice.dialogs,
                    messages = slice.messages,
                    users = slice.users
                };
            }

            return result;
        }

        public async Task<IList<TLMessage>> GetMessagesAsync(TLAbsInputPeer peer, int limit = 1)
        {
            var r = new TLRequestGetHistory
            {
                peer = peer,//new TLInputPeerUser { user_id = 52086439, access_hash = -5263285720256180879 },
                max_id = 0,
                limit = limit,
                offset_id = 0,//maxid
                add_offset = 0
            };

            var result = await _telegramClient.SendRequestAsync<object>(r);
            if (result is TLMessagesSlice)
                return ((TLMessagesSlice)result).messages.lists.OfType<TLMessage>().ToList();
            return ((TLMessages)result).messages.lists.OfType<TLMessage>().ToList();
        }

        public async Task<IList<TLUser>> GetAddUserMessagesAsync(int chatId, int limit = 1)
        {
            var r = new TLRequestGetHistory
            {
                peer = new TLInputPeerChat { chat_id = chatId },
                max_id = 0,
                limit = limit,
                offset_id = 0,//maxid
                add_offset = 0
            };

            var history = await _telegramClient.SendRequestAsync<TLMessagesSlice>(r);
            var result = new List<TLUser>();
            foreach (var message in history.messages.lists.Where(
                m => m is TLMessageService && ((TLMessageService)m).action is TLMessageActionChatAddUser)
                .OfType<TLMessageService>()
                .ToList())
            {
                foreach (var id in ((TLMessageActionChatAddUser)message.action).users.lists)
                {
                    if (result.Any(u => u.id == id)) continue;
                    var user = history.users.lists.FirstOrDefault(u => u is TLUser && ((TLUser) u).id == id);
                    if (user != null) result.Add((TLUser)user);
                }
            }
            
            return result;
        }

        public async Task MarkUserMessagesAsReadAsync(TLAbsInputPeer peer)
        {
            var r = new TLRequestReadHistory
            {
                peer = peer,
                max_id = 0
            };

            await _telegramClient.SendRequestAsync<object>(r);
        }

        public async Task MarkChatMessagesAsReadAsync(TLAbsInputChannel channel)
        {
            var r = new TeleSharp.TL.Channels.TLRequestReadHistory
            {
                channel = channel,
                max_id = 0
            };

            await _telegramClient.SendRequestAsync<object>(r);
        }

        public async Task SendMessageAsync(TLAbsInputPeer peer, string message)
        {
            await _telegramClient.SendMessageAsync(peer, message);
        }
    }
}
