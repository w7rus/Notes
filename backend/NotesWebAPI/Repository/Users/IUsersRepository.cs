using NotesWebAPI.Models.Database;
using NotesWebAPI.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesWebAPI.Repository.Users
{
    public interface IUsersRepository
    {
        User  findUserByUsernamePassword(AuthLoginModel model);
        void  addUserWithUsernamePassword(User user);
    }
}
