using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Notes.Logic.Models;
using Notes.Logic.Repositories.Users;

namespace Notes.Logic.Services.Users.Implementation
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IConfiguration _configuration;

        public UsersService(IUsersRepository usersRepository, IConfiguration configuration)
        {
            _usersRepository = usersRepository;
            _configuration = configuration;
        }

        public async Task<LoginResult> LoginUser(string username, string password)
        {
            var user = await _usersRepository.FindUserBy(username, password);

            if (user == null)
                throw new ArgumentException("Invalid login data!");

            if (user.IsSystem)
                throw new InvalidOperationException("Specified user is system reserved!");

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT").GetValue<string>("Key")));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: _configuration.GetSection("JWT").GetValue<string>("Issuer"),
                audience: _configuration.GetSection("JWT").GetValue<string>("Audience"),
                claims: new List<Claim>{
                        new Claim("user_id", user.Id.ToString())
                    },
                expires: DateTime.Now.AddMinutes(1488),
                signingCredentials: signinCredentials
            );

            return new LoginResult
            {
                Token = new JwtSecurityTokenHandler().WriteToken(tokeOptions),
                UserID = user.Id,
                Username = user.Username
            };
        }

        public async Task RegisterUser(string username, string password, string passwordRepeat)
        {
            if (password != passwordRepeat)
                throw new InvalidOperationException("Repeated password does not match!");

            await _usersRepository.AddUserWith(username, password);
        }

        public async Task<string> GetUsernameByUserId(int userId)
        {
            var user = await _usersRepository.FindUserByUserId(userId);

            if (user == null)
                throw new InvalidOperationException("Specified user is not found!");

            return user.Username;
        }
    }
}
