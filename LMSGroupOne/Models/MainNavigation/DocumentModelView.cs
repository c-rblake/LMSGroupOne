using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSGroupOne.Models.MainNavigation
{
    public class DocumentModelView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DocumentUrl { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PersonId { get; set; }
        public string PersonName { get; set; }
        
    }
}
