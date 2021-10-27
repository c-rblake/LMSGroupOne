using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSGroupOne.Models.Delete
{
    public class DeletePersonModelView
    {     
        public string Id { get; set; } 
        public string Name { get; set; }

        public string ReturnId { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
