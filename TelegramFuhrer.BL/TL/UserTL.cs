using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleSharp.TL;
using TLSharp.Core;

namespace TelegramFuhrer.BL.TL
{
	public class UserTL : IUserTL
	{
	    private readonly TelegramClient _telegramClient;

        public UserTL(TelegramClient telegramClient)
        {
            _telegramClient = telegramClient;
        }

	    public async Task<TLUser> FindUserByUsernameAsync(string username)
	    {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException($"Please provide username");

            var contacts = await _telegramClient.GetContactsAsync();

            var user = contacts.users.lists
                .Where(x => x.GetType() == typeof(TLUser))
                .Cast<TLUser>()
                .FirstOrDefault(x => x.username == username.TrimStart('@'));

            if (user == null)
            {
                var result = await _telegramClient.SearchUserAsync(username);

                user = result.users.lists
                    .Where(x => x.GetType() == typeof(TLUser))
                    .Cast<TLUser>()
                    .FirstOrDefault(x => x.username.Equals(username.TrimStart('@'), StringComparison.OrdinalIgnoreCase));
            }

            return user;
        }
    }
}
