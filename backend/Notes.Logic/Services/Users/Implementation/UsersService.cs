﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Notes.Logic.Models;
using Notes.Logic.Repositories.Users;

namespace Notes.Logic.Services.Users.Implementation
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public LoginResult LoginUser(string username, string password)
        {
            var user = _usersRepository.FindUserBy(username, password);

            if (user == null)
                throw new ArgumentException("Invalid login data!");

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

            return new LoginResult
            {
                Token = new JwtSecurityTokenHandler().WriteToken(tokeOptions),
                UserID = user.Id,
                Username = user.Username
            };
        }

        public void RegisterUser(string username, string password, string passwordRepeat)
        {
            if (password != passwordRepeat)
                throw new InvalidOperationException("Repeated password does not match!");

            _usersRepository.AddUserWith(username, password);
        }
    }
}
