using System;
using System.Collections.Generic;
using System.Security.Claims;
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
using WebApi.Extensions;

namespace WebApi.Controllers
{
    [Authorize]//To Autthenticate the Controller only registered users can use this 
    public class UsersController:BaseApiController
    {

        private IMapper _mapper;
        private IUserRepository _userRepository;

        private IPhotoService _photoService;        
        public UsersController(IUserRepository userRepository,IMapper mapper,IPhotoService photoService){

            _mapper = mapper;
            _userRepository = userRepository;
            _photoService = photoService;
        }

        //EndPoint
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUserDto>>> GetUsers(){

            // var users = await _userRepository.GetUsersAsync();
            
            // // Mapp all users object to userdto objects
            // var usersReponse = _mapper.Map<IEnumerable<AppUserDto>>(users);

            // //Get all users 
            // return Ok(usersReponse);

            var users = await _userRepository.GetMappedUsersAsync();
            return Ok(users);
        }

        //EndPoint        
        [HttpGet("{username}")]
        public async Task<ActionResult<AppUserDto>> GetUser(string username){

            // var user = await _userRepository.GetUserByNameAsync(username);

            // var response = _mapper.Map<AppUserDto>(user);

            // return Ok(user);  

            //after optimization, making it queryable in databse 
            return await _userRepository.GetMappedUserAsync(username);         
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){

            var username = User.GetUserName();//Extension Method

            var user =  await _userRepository.GetUserByNameAsync(username);

            if(user == null) return NotFound();

            // Console.Write(user);
            
            _mapper.Map(memberUpdateDto,user);
            
            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest();
            
        }
    
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> UploadPhoto(IFormFile file)
        {

            var user = await _userRepository.GetUserByNameAsync(User.GetUserName());

            if(user == null) return NotFound();

            var result = await _photoService.AddPhotoAsycn(file);

            if(result.Error!=null) return BadRequest(result.Error.Message);

            var photo =  new Photo{
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if(user.Photos.Count == 0) photo.IsMain =true;

            user.Photos.Add(photo);

            if(await _userRepository.SaveAllAsync()){

                //Get the end point location of the resource created
                return CreatedAtAction(nameof(GetUser),new{username = user.UserName},_mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem Adding Photo");

        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId){

            //Get user
            var user = await _userRepository.GetUserByNameAsync(User.GetUserName());

            if(user == null) return NotFound();

            //Get photo
            var photo = user.Photos.FirstOrDefault(p=>p.Id == photoId);

            if(photo == null) return NotFound();

            //Check photo is defualt/main
            if(photo.IsMain) return BadRequest("Photo is already set as default");

            //Find out which photo is main
            var currentPhoto = user.Photos.FirstOrDefault(p=>p.IsMain);
            
            //Mark it as not main or default photo
            if(currentPhoto!=null)
                currentPhoto.IsMain = false;
            
            //Make the other photo as default
            photo.IsMain = true;

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Problem in setting the default photo");


        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByNameAsync(User.GetUserName());

            var photo = user.Photos.FirstOrDefault(p=> p.Id == photoId);

            if(photo == null) return NotFound();

            if(photo.IsMain) return BadRequest("Main photo can't be deleted");

            if(photo.PublicId!=null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);

                if(result.Error!= null) return BadRequest(result.Error.Message);    
            }

            user.Photos.Remove(photo);

            if(await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Problem in deleting photo");
        }
    
    }
}