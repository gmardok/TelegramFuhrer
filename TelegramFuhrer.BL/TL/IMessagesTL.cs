using System.Collections.Generic;
using System.Threading.Tasks;
using TeleSharp.TL;
using TeleSharp.TL.Messages;

namespace TelegramFuhrer.BL.TL
{
    public interface IMessagesTL
    {
        Task<TLDialogs> GetDialogsAsync();

        Task<IList<TLMessage>> GetMessagesAsync(TLAbsInputPeer peer, int limit = 1);

        Task<IList<TLUser>> GetAddUserMessagesAsync(int chatId, int limit = 1);

        Task MarkUserMessagesAsReadAsync(TLAbsInputPeer peer);

        Task MarkChatMessagesAsReadAsync(TLAbsInputChannel channel);

        Task SendMessageAsync(TLAbsInputPeer peer, string message);
    }
}
