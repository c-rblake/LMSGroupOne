using LMS.Core.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMSGroupOne.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        DbSet<Activity> Activities { get; set; }
        DbSet<ActivityType> ActivityTypes { get; set; }
        DbSet<Course> Courses { get; set; }
        DbSet<Document> Documents { get; set; }
        DbSet<Module> Modules { get; set; }
        DbSet<Person> Persons { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
