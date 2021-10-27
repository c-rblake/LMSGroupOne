using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Models.ViewModels
{
    public interface IModalViewModel
    {
        bool Success { get; set; }  // creation status
        string Message { get; set; }   // returnmessage
        int ReturnId { get; set; }    // return id when created

        string PersonReturnId { get; set; }   // a person id when needed
    }
}
