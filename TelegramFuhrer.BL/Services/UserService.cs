using System;
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

		public async Task<User> FindUserByUsernameAsync(string username)
		{
			var user = await _userRepository.GetUserByUsernameAsync(username.TrimStart('@'));
			if (user == null)
			{
				var tlUser = await _userTL.FindUserByUsernameAsync(username);
				if (tlUser == null)
					throw new ArgumentException($"User {username} does not exists");

				var existingUser = await _userRepository.GetUserByTLIdAsync(tlUser.id);
				if (existingUser != null)
				{
					CopyUserProps(existingUser, tlUser);
					await _userRepository.SaveChanges();
					user = existingUser;
				}
				else
				{
					user = new User {Id = tlUser.id};
					CopyUserProps(user, tlUser);
					await _userRepository.AddAsync(user);
				}
			}

			return user;
		}

		private void CopyUserProps(User user, TLUser tlUser)
		{
			user.Username = tlUser.username;
			user.AccessHash = tlUser.access_hash;
			user.FirstName = tlUser.first_name;
			user.LastName = tlUser.last_name;
			user.Phone = tlUser.phone;
		}
	}
}
