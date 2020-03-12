using System.Collections.Generic;
using System.Threading.Tasks;
using Notes.Logic.Models;
using Notes.Logic.Models.Database;

namespace Notes.Logic.Services.Users
{
    public interface IUsersService
    {
        public Task<LoginResult> LoginUser(string username, string password);
        public Task RegisterUser(string username, string password, string passwordRepeat);
        public Task<string> GetUsernameByUserId(int userId);
        public Task<int> GetUserIdByUsername(string username);
        public Task<ICollection<User>> ListUsers();
        public Task<ICollection<User>> ListUsers(string search, int sorting, int display, int page);
    }
}
