using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSGroupOne.Models.MainNavigation
{
    public class ActivityModelView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string TypeDescription { get; set; }

    }
}
