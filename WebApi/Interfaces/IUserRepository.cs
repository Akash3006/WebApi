using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.DTOs;
using WebApi.Helpers;

namespace WebApi.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserById(int id);
        Task<AppUser> GetUserByNameAsync(string name);

        Task<PagedList<AppUserDto>> GetMappedUsersAsync(UserParams userParams);
        Task<AppUserDto> GetMappedUserAsync(string name);
    }
}