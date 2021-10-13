﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.ResourceParamaters
{
    public class AuthorsResourceParameters
    {
        //Todo Input Params for [FromBody] or [FromQuery] PageSize or Query Params.
        public bool IncludeWorks { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
