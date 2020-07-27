using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           // modelBuilder.Seed();
        }

       // public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    }

    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string CustomTag { get; set; }
        [PersonalData]
        public string DOB { get; set; }

    }
}
