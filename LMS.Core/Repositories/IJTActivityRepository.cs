using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using LMS.Core.Dto;

namespace LMS.Core.Repositories
{
    public interface IJTActivityRepository
    {
        Task<ActivityDto> GetActivity(int id, CancellationToken cancellationToken = default);
    }
}
