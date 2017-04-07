using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.Data.Repositories
{
    public class UserChatRepository : BaseRepository<UserChat>
    {
        public async Task<IList<UserChat>> GetUserChats(int userId)
        {
            return await Context.UserChats.Include(uc => uc.Chat).Include(uc => uc.User).Where(uc => uc.UserId == userId).ToListAsync();
        }

        public async Task AddChatAdminAsync(Chat chat, User user)
        {
            var existingChatAdmin = await GetChatAdminAsync(user.UserId, chat.Id);
            if (existingChatAdmin != null) return;
            Context.UserChats.Add(new UserChat
            {
                UserId = user.UserId,
                ChatId = chat.Id
            });

            await Context.SaveChangesAsync();
        }

        public async Task RemoveChatAdminAsync(Chat chat, User user)
        {
            var existingChatAdmin = await GetChatAdminAsync(user.UserId, chat.Id);
            if (existingChatAdmin == null) return;
            Context.UserChats.Remove(existingChatAdmin);
            await Context.SaveChangesAsync();
        }

        public async Task<UserChat> GetChatAdminAsync(int userId, int chatId)
        {
            return await Context.UserChats.FirstOrDefaultAsync(uc => uc.UserId == userId && uc.ChatId == chatId);
        }
    }
}
