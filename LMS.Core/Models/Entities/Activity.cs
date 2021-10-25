using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Models.Entities
{
    public class Activity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int ActivityTypeId { get; set; }
        public int ModuleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        //Nav Prop
        public ActivityType ActivityType { get; set; }
        public Module Module { get; set; }
        public ICollection<Document> Documents { get; set; }
    }
}
