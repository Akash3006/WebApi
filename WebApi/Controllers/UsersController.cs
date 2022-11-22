using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    [Authorize]//To Autthenticate the Controller only registered users can use this 
    public class UsersController:BaseApiController
    {

        private IMapper _mapper;
        private IUserRepository _userRepository;        
        public UsersController(IUserRepository userRepository,IMapper mapper){

            _mapper = mapper;
            _userRepository = userRepository;
        }

        //EndPoint
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUserDto>>> GetUsers(){

            var users = await _userRepository.GetUsersAsync();
            
            // Mapp all users object to userdto objects
            var usersReponse = _mapper.Map<IEnumerable<AppUserDto>>(users);

            //Get all users 
            return Ok(usersReponse);
        }

        //EndPoint        
        [HttpGet("{username}")]
        public async Task<ActionResult<AppUser>> GetUsers(string username){

            var user = await _userRepository.GetUserByNameAsync(username);

            var response = _mapper.Map<AppUserDto>(user);

            return Ok(user);           
        }
    }
}