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
        private static Faker fake;
        public static async Task InitializeAsync(IServiceProvider services)
        {

            using var db = new LMSApiContext(services.GetRequiredService<DbContextOptions<LMSApiContext>>());
            fake = new Faker("sv");


            if (await db.Authors.AnyAsync()) return;
            

            List<string> exsampleTypes = new List<string>
            { "Book", "Article"};

            var types = GetTypes(exsampleTypes);
            var genres = GetGenres();
            db.Types.AddRange(types);
            db.Genres.AddRange(genres);
            //var authors = GetAuthors();
            //db.Authors.AddRange(authors);
            await db.SaveChangesAsync();

            var worksandAuthors = GetWorkAndAuthors(types, genres);
            db.AddRange(worksandAuthors);
            
            await db.SaveChangesAsync();
        }

        private static List<Work> GetWorkAndAuthors(List<Core.Entities.Type> types, List<Genre> genres)
        {
            var works = new List<Work>();
            for (int i = 0; i < 5; i++)
            {
                var work = new Work
                {
                    Description = fake.Lorem.Sentence(),
                    Title = fake.Random.Word(),
                    TypeId = fake.Random.Int(1, types.Count),
                    GenreId = fake.Random.Int(1, genres.Count),
                    Level = fake.Random.Word(),
                    PublicationDate = DateTime.Now.AddYears(fake.Random.Int(-15, 0)),
                    Authors = GetAuthors()
                };
            works.Add(work);
                        
            }
            return works;
        }

        private static Author GetNeroTulip()
        {
            var neroTulip = new Author
            {
                FirstName = "Nero",
                LastName = "Tulip",
                DateOfBirth = new DateTime(1960, 10, 18)
            };
            //ToDo add Nero's works.
            return neroTulip;
        }

        private static ICollection<Author> GetAuthors()
        {
            var authors = new List<Author>();
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    var neroTulip = GetNeroTulip();
                    authors.Add(neroTulip);
                }
                var author = new Author
                {
                    FirstName = fake.Name.FirstName(),
                    LastName = fake.Name.LastName(),                  
                    DateOfBirth = DateTime.Now.AddYears(fake.Random.Int(-65, -25)), 
                };
                if(fake.Random.Int(1, 100) > 75)
                {
                    author.DateOfDeath = DateTime.Now.AddYears(fake.Random.Int(-10, -2));
                }
                authors.Add(author);
            }
            return authors;
        }

        private static List<Genre> GetGenres()
        {
            var genres = new List<Genre>();

            for (int i = 0; i < 5; i++)
            {
                var genre = new Genre
                { 
                Description = fake.Random.Word() + fake.Company.CompanySuffix(),
                Name = fake.Random.Word()
                };
                genres.Add(genre);
            }
            return genres;
        }

        private static List<LMS.Api.Core.Entities.Type> GetTypes(List<string> exsampleTypes)
        {
            var types = new List<LMS.Api.Core.Entities.Type>();

            foreach (var exampleName in exsampleTypes)
            {
                var typ = new LMS.Api.Core.Entities.Type
                {
                    Name = exampleName,
                    Description = fake.Lorem.Sentence()
                };
                types.Add(typ);
            }

            return types;
        }
    }
}
