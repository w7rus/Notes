using Notes.Logic.Models;

namespace Notes.Logic.Services.Users
{
    public interface IUsersService
    {
        LoginResult LoginUser(string username, string password);
        void RegisterUser(string username, string password, string passwordRepeat);
    }
}
