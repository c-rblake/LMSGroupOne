using LMS.Api.Core.Dtos;
using LMS.Api.Core.Entities;
using LMS.Api.Data;
using LMS.Api.Helpers;
using LMS.Api.ResourceParamaters;
using LMS.Api.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.Core.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LMSApiContext db;
        private readonly IPropertyMappingService _propertyMappingService;

        public AuthorRepository(LMSApiContext db, IPropertyMappingService propertyMappingService)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this._propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }
        public async Task AddAsync(Author author)
        {
            await db.Authors.AddAsync(author);
        }

        public Task<bool> AnyAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<Author> FindAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync(AuthorsResourceParameters authorResourceParameters)
        {
            var query = db.Authors.AsQueryable();

            if (authorResourceParameters.IncludeWorks)
            {
                query = query.Include(a => a.Works)
                    .ThenInclude(w => w.Genre)
                    .Include(a => a.Works)
                    .ThenInclude(w => w.Type);
            };
            if (!string.IsNullOrWhiteSpace(authorResourceParameters.Name))
            {


                //Expanded search. necessary unfortunatley. Usecase "Nero Tulip"
                var names = authorResourceParameters.Name.Trim().Split(" ");
                if (names.Length == 1)
                {
                    var name = authorResourceParameters.Name.Trim();
                    //query = query.Where(a => a.FirstName == name || a.LastName == name); Cannot search for "Nero Tulip" which would be the use case.
                    query = query.Where(a => a.FirstName.Contains(name) || a.LastName.Contains(name));
                }
                else
                {
                    //query = query.Where(q => q.FirstName.Any(fn => listofstirngs.Any(s=> s == fn))); // DOES NOT WORK WELL HERE.
                    query = query.Where(a => names.Contains(a.FirstName) || names.Contains(a.LastName)); // think for each row in Q as author.

                    //query = query.Where(a => a.FirstName.Contains(name) || a.LastName.Contains(name));
                }
            }
            
            //if (!string.IsNullOrWhiteSpace(authorResourceParameters.LastName)) Replaced by Name on Dto
            //{
            //    var lastName = authorResourceParameters.LastName.Trim();
            //    query = query.Where(a => a.LastName == lastName);
            //}
            //if(authorResourceParameters.SortOnLastName)  Replaced by Apply Sort Extension.
            //{
            //    query = query.OrderBy(q => q.LastName);
            //}

            var authorPropertyMappingDictionary =
                _propertyMappingService.GetPropertyMapping<AuthorDto, Author>();


            query = query.ApplySort(authorResourceParameters.OrderBy,
                authorPropertyMappingDictionary);


            return await query.ToListAsync();
        }

        public async Task<Author> GetAuthorAsync(int? id, bool includeworks=true)
        {
            var query = db.Authors.AsQueryable();

            if (includeworks)
            {
                query = query.Include(a => a.Works)
                    .ThenInclude(w => w.Genre)
                    .Include(a => a.Works)
                    .ThenInclude(w => w.Type);
            };

            return await query.FirstOrDefaultAsync(a=>a.Id == id);
        }

        public Task<Author> GetAuthorNameAsync(string firstName, string lastName)
        {
            throw new NotImplementedException();
        }

        public Task Remove(Author author)
        {
            throw new NotImplementedException();
        }

        public Task Update(Author author)
        {
            throw new NotImplementedException();
        }
    }
}
