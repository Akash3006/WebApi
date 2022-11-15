using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Entities;

namespace WebApi.Controllers
{
    
    public class UsersController:BaseApiController
    {

        private ApplicationDataContext _dataContext;
        public UsersController(ApplicationDataContext dataContext){

            _dataContext = dataContext;
        }

        //EndPoint
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){
            var users  = await _dataContext.Users.ToListAsync();
            return users;
        }

        //EndPoint
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUsers(int id){
            var user = await _dataContext.Users.FindAsync(id);             
             return user;            
        }
    }
}