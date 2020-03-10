using System.Threading.Tasks;
using Notes.Logic.Models.Database;

namespace Notes.Logic.Repositories.Users
{
    public interface IUsersRepository
    {
        public Task<User>  FindUserBy(string username, string password);
        public Task  AddUserWith(string username, string password);
        public Task<User> FindUserByUserId(int userId);
        public Task<User> FindUserByUsername(string username);
    }
}
