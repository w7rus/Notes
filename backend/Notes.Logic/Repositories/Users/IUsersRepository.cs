using Notes.Logic.Models.Database;

namespace Notes.Logic.Repositories.Users
{
    public interface IUsersRepository
    {
        public User  FindUserBy(string username, string password);
        public void  AddUserWith(string username, string password);
        public User FindUserByUserId(int userId);
    }
}
