using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Controllers
{
    public class AccountController:BaseApiController
    {
        private ApplicationDataContext _dbContext;
        public AccountController(ApplicationDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto){

            if(await userExists(registerDto.Username)) return BadRequest("User already exists");
            
            using var hmac = new HMACSHA512();

            var user = new AppUser(){
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
                
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        private async Task<bool> userExists(string user){

            return await _dbContext.Users.AnyAsync(obj => obj.UserName == user.ToLower());
        }
    }
}