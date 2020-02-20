using NotesWebAPI.Data;
using NotesWebAPI.Models.Database;
using NotesWebAPI.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesWebAPI.Repository.Users.Implementation
{
    public class CUsersRepository : IUsersRepository
    {
        private NotesWebAPIContext _context;

        public CUsersRepository(NotesWebAPIContext context)
        {
            _context = context;
        }

        public void addUserWithUsernamePassword(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User findUserByUsernamePassword(AuthLoginModel model)
        {
            return _context.Users.SingleOrDefault(u => u.Username == model.Username && u.Password == model.Password);
        }
    }
}
