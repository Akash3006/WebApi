using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    public class AccountController:BaseApiController
    {
        private ApplicationDataContext _dbContext;
        private IMapper _mapper;
        private ITokenService _tokenServices;
        public AccountController(ApplicationDataContext dbContext,ITokenService tokenService,IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _tokenServices = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto){

            if(await userExists(registerDto.Username)) return BadRequest("User already exists");
            
            var user = _mapper.Map<AppUser>(registerDto);

            using var hmac = new HMACSHA512();

            user.UserName = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;

            _dbContext.Users.Add(user);

            await _dbContext.SaveChangesAsync();
            return new UserDto{
                Username = user.UserName,
                Token = _tokenServices.CreateToken(user),
                KnownAs = user.KnownAs
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login){

            var user = await _dbContext.Users.
            Include(p=>p.Photos).
            SingleOrDefaultAsync(obj => obj.UserName == login.Username);

            if(user == null)
                return Unauthorized("Invalid User");
            
            using var hmac = new HMACSHA512(user.PasswordSalt);
            
            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));

            for(int i =0;i < computeHash.Length;i++)
                if(computeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid User");

            return new UserDto{
                Username = user.UserName,
                PhotoUrl = user.Photos.FirstOrDefault(x=>x.IsMain).Url,
                Token = _tokenServices.CreateToken(user),
                KnownAs = user.KnownAs
            };
        }
        private async Task<bool> userExists(string user){

            return await _dbContext.Users.AnyAsync(obj => obj.UserName == user.ToLower());
        }
    }
}