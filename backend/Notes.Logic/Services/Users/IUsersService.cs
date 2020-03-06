using Notes.Logic.Models;

namespace Notes.Logic.Services.Users
{
    public interface IUsersService
    {
        public LoginResult LoginUser(string username, string password);
        public void RegisterUser(string username, string password, string passwordRepeat);
        public string GetUsernameByUserId(int userId);
    }
}
