using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LMS.Api.Core.Entities;

namespace LMS.Api.Data
{
    public class LMSApiContext : DbContext
    {
        public LMSApiContext (DbContextOptions<LMSApiContext> options)
            : base(options)
        {
        }

        public DbSet<Work> Works { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres{ get; set; }
        public DbSet<LMS.Api.Core.Entities.Type> Types { get; set; }

        public DbSet<AuthorWork> AuthorWorks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorWork>()
                .HasKey(aw => new { aw.AuthorId, aw.WorkId });

            //modelBuilder.Entity<Author>()
            //    .HasMany(v => v.Works)
            //    .WithMany(ps => ps.Authors)
            //    .UsingEntity<AuthorWork>
            //    (vps => vps.HasOne(p => p.Works).WithMany(p => p.Authors),
            //    vps => vps.HasOne(p => p.Author).WithMany(p => p.Works));
        }




    }
}
