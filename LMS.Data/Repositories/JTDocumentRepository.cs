using LMS.Core.Dto;
using LMS.Core.Repositories;
using LMS.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LMS.Data.Repositories
{
    public class JTDocumentRepository : IJTDocumentRepository
    {


        private readonly ApplicationDbContext db;

        public JTDocumentRepository(ApplicationDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<DocumentDto> GetDocument(int id, CancellationToken cancellationToken = default)
        {            

            var model = db.Documents.Where(d=>d.Id==id).Select(s => new DocumentDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                DocumentUrl = s.Description,
                TimeStamp = s.TimeStamp,
                PersonId = s.PersonId,
                PersonFirstName = s.Person.FirstName,
                PersonLastName = s.Person.LastName

            }).FirstOrDefaultAsync();


            return await model;
        }
    }
        
}

