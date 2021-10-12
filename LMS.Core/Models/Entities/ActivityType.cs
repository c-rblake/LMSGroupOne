using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Models.Entities
{
    public class ActivityType
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get ; set; }

    }
}
