using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Extensions;
using WebApi.Interfaces;

namespace WebApi.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            //To set photo url in AppUserDto 
            CreateMap<AppUser,AppUserDto>().
                ForMember(dest=>dest.PhotoUrl,opt=>opt.MapFrom(src=>src.Photos.FirstOrDefault(x=>x.IsMain).Url)).
                ForMember(dest=> dest.Age,opt=>opt.MapFrom(src=> src.DateOfBirth.CalculateAge()));
                //To set age
            CreateMap<Photo,PhotoDto>();
            CreateMap<RegisterDto,AppUser>();
            CreateMap<MemberUpdateDto,AppUser>();
            CreateMap<Message,MessageDto>()
            .ForMember(d=>d.SenderPhotoUrl,s=>s.MapFrom(s=>s.Sender.Photos.FirstOrDefault(x=>x.IsMain).Url))
            .ForMember(d=>d.RecipientPhotoUrl,s=>s.MapFrom(s=>s.Recipient.Photos.FirstOrDefault(x=>x.IsMain).Url));
        }
    }
}