using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Interfaces
{
    public interface ILikeRepository
    {
        Task<UserLike> GetUserLikes(int sourceId,int targetId);

        Task<AppUser> GetUserWithLikes(int userId);

        Task<IEnumerable<LikeDto>> GetUserLikes(string predicate,int userId);
        
    }
}