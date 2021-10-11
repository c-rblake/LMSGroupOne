using Bogus;
using LMS.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.Data
{
    public class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {

            using var db = new LMSApiContext(services.GetRequiredService<DbContextOptions<LMSApiContext>>());


            if (await db.Authors.AnyAsync()) return;
            var faker = new Faker("sv");
            var works = new List<Work>();

            for (int i = 0; i < 5; i++)
            {

            }





        }
    }
}
