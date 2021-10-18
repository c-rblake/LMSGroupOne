using LMS.Api.Core.Entities;
using LMS.Api.Helpers;
using LMS.Api.ResourceParamaters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.Core.Repositories
{
    public interface IWorksRepository
    {
        //Async => Task en typ av async
        //Go do something else. Egentligen. Add, Uppdate och Remove är nog oftast INTE async//Task
        //Task<IEnumerable<Work>> GetAllWorksAsync(WorksResourceParameters workResourceParameters);

        //Task<PagedList<Work>> GetAllWorksAsync(WorksResourceParameters worksResourceParameters);
        PagedList<Work> GetAllWorks(WorksResourceParameters worksResourceParameters);
        //PagedList<Work> GetAllWorks(WorksResourceParameters worksResourceParameters);
        Task<Work> GetWorkAsync(int id);
        Task<Work> FindAsync(int? id);
        Task<bool> AnyAsync(int? id);
        Task AddAsync(Work work); 
        Task Update(Work work);
        Task Remove(Work work);
    }
}
