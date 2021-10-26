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

            var neroTulip = GetNeroTulip();
            var nerosWorks = GetNerosWorks();
            neroTulip.Works.AddRange(nerosWorks);
            db.Add(neroTulip);
            await db.SaveChangesAsync();


        }

        private static List<Work> GetWorkAndAuthors(List<Core.Entities.Type> types, List<Genre> genres)
        {
            var works = new List<Work>();
            for (int i = 0; i < 20; i++)
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
                DateOfBirth = new DateTime(1960, 10, 18),
                //Works = GetNerosWorks()
            };
            //ToDo add Nero's works.
            return neroTulip;
        }
        private static List<Work> GetNerosWorks()
        {

            var myNewGenre = new Genre //Todo <-- Get this ID is not possible. But It can be a Variable? Reference type works or not... lets find out. WORKS
            {
                Name = "Non-Fiction",
                Description = "Based not on Fiction but real events."
            };

            var nerosWorks = new List<Work>();
            nerosWorks.Add(new Work
            {
                Title = "White Swans",
                PublicationDate = new DateTime(2000, 10, 18),
                Description = "On the Dangers of not Preventing and building the complext",
                Level = "intermediate",
                Genre = myNewGenre,
                Type = new Core.Entities.Type
                {
                    Name = "Paperback",
                    Description = "Not Digital"
                }
                
            });
            nerosWorks.Add(new Work
            { 
            Title = "Very Robust",
            PublicationDate = new DateTime(2002, 10, 18),
            Description = "Coffee cup made from light is superior",
            Level = "intermediate",
            Genre = myNewGenre,
            Type = new Core.Entities.Type // This is undesireable but good to know about. Should be a variable like Genre
            {
                Name = "Paperback",
                Description = "Not Digital"
            }

            });
            return nerosWorks;

        }

        private static ICollection<Author> GetAuthors()
        {
            var authors = new List<Author>();
            for (int i = 0; i < 2; i++)
            {
                //if (i == 0) Several calls are made to here..
                //{
                //    var neroTulip = GetNeroTulip();
                //    authors.Add(neroTulip);
                //}
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
