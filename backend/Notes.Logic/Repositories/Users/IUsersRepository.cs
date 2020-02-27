using Notes.Logic.Models.Database;

namespace Notes.Logic.Repositories.Users
{
    public interface IUsersRepository
    {
        User  FindUserBy(string username, string password);
        void  AddUserWith(string username, string password);
    }
}
