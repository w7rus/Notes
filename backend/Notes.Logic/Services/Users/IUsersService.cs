using System.Threading.Tasks;
using Notes.Logic.Models;

namespace Notes.Logic.Services.Users
{
    public interface IUsersService
    {
        public Task<LoginResult> LoginUser(string username, string password);
        public Task RegisterUser(string username, string password, string passwordRepeat);
        public Task<string> GetUsernameByUserId(int userId);
    }
}
