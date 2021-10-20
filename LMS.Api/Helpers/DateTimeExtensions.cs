using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.Helpers
{
    public static class DateTimeExtensions
    {
        public static int GetCurrentAge(this DateTimeOffset dateTime, DateTimeOffset? dateOfDeath)
        {
            var dateToCalculateTo = DateTime.UtcNow;

            if (dateOfDeath != null)
            {
                dateToCalculateTo = dateOfDeath.Value.UtcDateTime;
            }

            var age = dateToCalculateTo.Year - dateTime.Year;

            if(dateToCalculateTo < dateTime.AddYears(age))
            {
                age--;
            }

            return age;
        }
    }
}
