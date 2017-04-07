using System;
using System.Linq;
using System.Threading.Tasks;
using TelegramFuhrer.BL.TL;
using TelegramFuhrer.Data.Entities;
using TelegramFuhrer.Data.Repositories;
using TeleSharp.TL;

namespace TelegramFuhrer.BL.Services
{
	public class UserService : IUserService
	{
		private readonly UserRepository _userRepository;

		private readonly IUserTL _userTL;

		public UserService(UserRepository userRepository, IUserTL userTL)
		{
			_userRepository = userRepository;
			_userTL = userTL;
		}

		public async Task<User> FindUserByUsernameAsync(string username, bool? isAdmin = null)
		{
			var user = await _userRepository.GetUserByUsernameAsync(username.TrimStart('@'));
			if (user == null || (isAdmin.HasValue && user.IsGlobalAdmin != isAdmin))
			{
				var tlUser = await _userTL.FindUserByUsernameAsync(username);
				if (tlUser == null)
					throw new ArgumentException($"User {username} does not exists");

				var existingUser = user ?? await _userRepository.GetUserByTLIdAsync(tlUser.id);
				if (existingUser != null)
				{
					CopyUserProps(existingUser, tlUser, isAdmin);
					await _userRepository.SaveChangesAsync();
					user = existingUser;
				}
				else
				{
					user = new User {Id = tlUser.id};
					CopyUserProps(user, tlUser, isAdmin);
					await _userRepository.AddAsync(user);
				}
			}

			return user;
		}

	    public async Task<string> GetListOfAdminsAsync()
	    {
            var users = await _userRepository.GetAdminsAsync();
	        return string.Join("\n", users.Select(u => "@" + u.Username));
	    }

	    public async Task UpdateHashes()
	    {
	        var users = await _userRepository.GetAllAsync();
	        foreach (var user in users)
	        {
                var tlUser = await _userTL.FindUserByUsernameAsync(user.Username);
                if (tlUser == null)
                    throw new ArgumentException($"User {user.Username} does not exists");
                CopyUserProps(user, tlUser, null);
            }
            await _userRepository.SaveChangesAsync();
        }

        public static void CopyUserProps(User user, TLUser tlUser, bool? isAdmin)
		{
			user.Username = tlUser.username;
			user.AccessHash = tlUser.access_hash;
			user.FirstName = tlUser.first_name;
			user.LastName = tlUser.last_name;
			user.Phone = tlUser.phone;
            if (isAdmin.HasValue)
		        user.IsGlobalAdmin = isAdmin.Value;
		}
	}
}
