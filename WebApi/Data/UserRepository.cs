using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Interfaces;

namespace WebApi.Data
{
    public class UserRepository : IUserRepository
    {
        private IMapper _mapper;

        private ApplicationDataContext _context;
        public UserRepository(ApplicationDataContext context,IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppUserDto> GetMappedUserAsync(String name)
        {
            //after optimization, making it queryable in databse 
            return await _context.Users.
                        Where(x=>x.UserName == name).
                        Include(p=>p.Photos).
                        ProjectTo<AppUserDto>(_mapper.ConfigurationProvider).
                        SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<AppUserDto>> GetMappedUsersAsync()
        {
            return await _context.Users.
                Include(p=>p.Photos).
                ProjectTo<AppUserDto>(_mapper.ConfigurationProvider).
                ToListAsync();
        }

        public async Task<AppUser> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByNameAsync(string name)
        {
            return await _context.Users.
                        Include(p=>p.Photos).
                        SingleOrDefaultAsync(x=> x.UserName == name);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            //Eager loading, fetching the child table  data 
            return await _context.Users.Include(p=>p.Photos).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
            //SaveChangesAsync return interger value

        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
            //A flag to track the entity status as modified,the value is not being updated to database but being tracked as changed.
        }
    }
}