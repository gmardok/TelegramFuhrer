using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.BL.Services
{
    public interface IMessagesService
    {
        Task<List<User>> GetDialogsAsync();

        Task<string> GetUserMessagesAsync(User user);

        Task<string> WaitForMessageAsync(User user);

        Task SendMessageAsync(User user, string message);

        Task AutoKickAsync();
    }
}
