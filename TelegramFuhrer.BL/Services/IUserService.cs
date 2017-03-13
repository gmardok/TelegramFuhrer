using System.Threading.Tasks;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.BL.Services
{
    public interface IUserService
	{
		Task<User> FindUserByUsernameAsync(string username, bool? isAdmin = null);

        Task<string> GetListOfAdminsAsync();
	}
}
