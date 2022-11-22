using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Entities;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    [Authorize]//To Autthenticate the Controller only registered users can use this 
    public class UsersController:BaseApiController
    {

        private IUserRepository _userRepository;        
        public UsersController(IUserRepository userRepository){

            _userRepository = userRepository;
        }

        //EndPoint
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){
            //Get all users 
            return Ok(await _userRepository.GetUsersAsync());
        }

        //EndPoint        
        [HttpGet("{username}")]
        public async Task<ActionResult<AppUser>> GetUsers(string username){

            return await _userRepository.GetUserByNameAsync(username);           
        }
    }
}