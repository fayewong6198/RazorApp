using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using aspnetapp.Models;

namespace aspnetapp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

         public DbSet<Feed> Feed { get; set; }
         public DbSet<FeedItem> FeedItem { get; set; }
         public DbSet<ApplicationUser> ApplicationUser { get; set; }
         public DbSet<ApplicationUserToken> ApplicationUserToken { get; set; }



    }
}
