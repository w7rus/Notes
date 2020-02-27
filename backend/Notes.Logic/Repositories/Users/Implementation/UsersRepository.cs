using System.Linq;
using Notes.Logic.Data;
using Notes.Logic.Models.Database;

namespace Notes.Logic.Repositories.Users.Implementation
{
    public class UsersRepository : IUsersRepository
    {
        private readonly NotesWebAPIContext _context;

        public UsersRepository(NotesWebAPIContext context)
        {
            _context = context;
        }

        public void AddUserWith(string username, string password)
        {
            _context.Users.Add(new User { Username = username, Password = password });
            _context.SaveChanges();
        }

        public User FindUserBy(string username, string password)
        {
            return _context.Users.SingleOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
