using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NotesWebAPI.Data;
using NotesWebAPI.Models.View;
using NotesWebAPI.Repository.Users;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NotesWebAPI.Services.Users.Implementation
{
    public class CUsersService : ControllerBase, IUsersService
    {
        private IUsersRepository _usersRepository;

        public CUsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public IActionResult loginUser(AuthLoginModel model)
        {
            var user = _usersRepository.findUserByUsernamePassword(model);

            if (user == null)
                return Unauthorized();

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: "http://localhost:5000",
                audience: "http://localhost:5000",
                claims: new List<Claim>{
                        new Claim("user_id", user.Id.ToString())
                    },
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: signinCredentials
            );

            return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(tokeOptions), UserID = user.Id, Username = user.Username });
        }

        public IActionResult registerUser(AuthRegisterModel model)
        {
            if (model.Password != model.PasswordRepeat)
                return BadRequest("Password do not match!");

            var user = new Models.Database.User { Username = model.Username, Password = model.Password };

            _usersRepository.addUserWithUsernamePassword(user);

            return Ok(user);
        }
    }
}
