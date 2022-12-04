using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.Data
{
    public class ApplicationDataContext:DbContext
    {
        public ApplicationDataContext(DbContextOptions options):base(options){

        }

        public DbSet<AppUser> Users{get;set;}
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserLike> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder){

            base.OnModelCreating(builder);
            builder.Entity<UserLike>().HasKey(x=> new{x.SourceUserId,x.TargetUserId});

            builder.Entity<UserLike>()
            .HasOne(x=>x.SourceUser)
            .WithMany(l=>l.Liked)
            .HasForeignKey(f=>f.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserLike>()
            .HasOne(x=>x.TargetUser)
            .WithMany(l=>l.LikedBy)
            .HasForeignKey(f=>f.TargetUserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
            .HasOne(x=>x.Recipient)
            .WithMany(m=>m.MessagesRecieved)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
            .HasOne(x=>x.Sender)
            .WithMany(m=>m.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);

        }

    }
}