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
    public class WorksRepository : IWorksRepository
    {
        private readonly LMSApiContext db;
        private readonly IPropertyMappingService _propertyMappingService;

        public WorksRepository(LMSApiContext db, IPropertyMappingService _propertyMappingService)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this._propertyMappingService = _propertyMappingService ?? throw new ArgumentNullException(nameof(_propertyMappingService));
        }

        public async Task AddAsync(Work work)
        {
            await db.Works.AddAsync(work);
        }

        public Task<bool> AnyAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<Work> FindAsync(int? id)
        {
            return await db.Works.FirstOrDefaultAsync(w => w.Id == id);
        }

        //public async Task<IEnumerable<Work>> GetAllWorksAsync(WorksResourceParameters workResourceParameters)
        //public async Task<PagedList<Work>> GetAllWorksAsync(WorksResourceParameters workResourceParameters)
        public PagedList<Work> GetAllWorks(WorksResourceParameters workResourceParameters)
        {
            var query = db.Works
                .Include(w => w.Authors)
                .Include(w => w.Genre)
                .Include(w => w.Type)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(workResourceParameters.Title))
            {
                query = query.Where(q => q.Title.Contains(workResourceParameters.Title)); //== workResourceParameters.Title);
            }
            if (!string.IsNullOrWhiteSpace(workResourceParameters.AuthorName))
            {
                var authors = db.Authors.Where(a => a.FirstName.Contains(workResourceParameters.AuthorName) || a.LastName.Contains(workResourceParameters.AuthorName));

                //query = query.Include(q => q.Authors.Where(a => author.Contains(a))); //Empty Author Lists
                query = query.Where(q => q.Authors.Any(a => authors.Contains(a)));
                //var result = lista.Where(a => listb.Any(b => b.Contains(a)));

            }
            if (!string.IsNullOrWhiteSpace(workResourceParameters.GenreName))
            {
                query = query.Where(q => q.Genre.Name == workResourceParameters.GenreName);

            }
            //if (!string.IsNullOrWhiteSpace(workResourceParameters.OrderBy)) // On title
            //{
            //    if (workResourceParameters.OrderBy.ToLowerInvariant() == "title")
            //    {
            //        query = query.OrderBy(q => q.Title);
            //    }
            //}

            var workPropertyMappingDictionary = _propertyMappingService.GetPropertyMapping<WorkDto, Work>();

            query.ApplySort(workResourceParameters.OrderBy, workPropertyMappingDictionary);

            //Paging Last.

            //query = query
            //    .Skip(workResourceParameters.PageSize * (workResourceParameters.PageNumber - 1))
            //    .Take(workResourceParameters.PageSize);

            var collection = PagedList<Work>.Create(query,
                workResourceParameters.PageNumber,
                workResourceParameters.PageSize);

            return collection;

        }

        public async Task<Work> GetWorkAsync(int id)
        {
            var work = await db.Works
                .Include(w => w.Genre)
                .Include(w => w.Authors)
                .Include(w => w.Type)
                .FirstOrDefaultAsync(w => w.Id == id);

            return work;
        }

        public Task Remove(Work work)
        {
            throw new NotImplementedException();
        }

        public Task Update(Work work)
        {
            throw new NotImplementedException();
        }
    }
}
