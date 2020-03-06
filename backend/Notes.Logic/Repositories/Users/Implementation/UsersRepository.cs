using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

        public async Task AddUserWith(string username, string password)
        {
            await _context.Users.AddAsync(new User { Username = username, Password = password });
            await _context.SaveChangesAsync();
        }

        public async Task<User> FindUserByUserId(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> FindUserBy(string username, string password)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Username == username && u.Password == password);
        }
    }
}
