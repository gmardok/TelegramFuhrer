using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramFuhrer.Data.Entities;
using TeleSharp.TL.Messages;

namespace TelegramFuhrer.BL.Services
{
    public interface IMessagesService
    {
        Task<List<User>> GetDialogsAsync(TLDialogs dialogs);

        Task<string> GetUserMessagesAsync(User user);

        Task<string> WaitForMessageAsync(User user);

        Task SendMessageAsync(User user, string message);

        Task<TLDialogs> AutoKickAsync();

        Task MarkUserMessagesAsReadAsync(User user);

        Task SendChatMessageAsync(int chatId, string message);
    }
}
