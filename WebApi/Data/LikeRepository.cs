using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Extensions;
using WebApi.Interfaces;

namespace WebApi.Data
{
    public class LikeRepository : ILikeRepository
    {
        private readonly ApplicationDataContext _context;

        public LikeRepository(ApplicationDataContext context)
        {
            _context = context;
        }
        public async Task<UserLike> GetUserLikes(int sourceId, int targetId)
        {
            return await _context.Likes.FindAsync(sourceId,targetId);           
        }

        public async Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)
        {
            var user = _context.Users.AsQueryable();
            var likes = _context.Likes.AsQueryable();

            if(predicate == "liked"){
                likes = likes.Where(x=> x.SourceUserId == userId);
                user = likes.Select(x=>x.TargetUser);
            }
            if(predicate == "likedBy"){
                likes = likes.Where(x=> x.TargetUserId == userId);
                user = likes.Select(x=>x.SourceUser);
            }

            return await user.Select(x=> new LikeDto{
                Id = x.Id,
                UserName = x.UserName,
                Age = x.DateOfBirth.CalculateAge(),
                KnownAs = x.KnownAs,
                PhotoUrl = x.Photos.FirstOrDefault(p=>p.IsMain).Url
            }).ToListAsync();
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users
                        .Include(l=>l.Liked).
                        FirstOrDefaultAsync(x=>x.Id == userId);
        }
    }
}