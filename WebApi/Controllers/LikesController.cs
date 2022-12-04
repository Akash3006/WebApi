using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Extensions;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    public class LikesController:BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikeRepository _likeRepository;

        public LikesController(IUserRepository userRepository,ILikeRepository likeRepository)
        {
            _userRepository = userRepository;
            _likeRepository = likeRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username){

            var user = await _userRepository.GetMappedUserAsync(User.GetUserName());
            var likedUser = await _userRepository.GetMappedUserAsync(username);
            var sourceUser = await _likeRepository.GetUserWithLikes(user.Id);

            if(likedUser == null) return NotFound();

            if(sourceUser.UserName == username) return BadRequest("You can't like your self");

            var userlike = await _likeRepository.GetUserLikes(sourceUser.Id,likedUser.Id);

            if(userlike!=null) return BadRequest("You already liked this user");

            userlike = new UserLike{
                SourceUserId = sourceUser.Id,
                TargetUserId = likedUser.Id
            };

            sourceUser.Liked.Add(userlike);

            if(await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Problem while liking user");

        }

        [HttpGet()]
       public async Task<IEnumerable<LikeDto>> GetLikes(string predicate){

            var user = await _userRepository.GetMappedUserAsync(User.GetUserName());          
            var sourceUser = await _userRepository.GetUserById(user.Id);
            return await _likeRepository.GetUserLikes(predicate,sourceUser.Id);
       }
    }
}