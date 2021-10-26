using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSGroupOne.Models.MainNavigation
{
    public enum NodeType
    {
        root=0,
        folder=1,
        student=2,
        teacher=3,
        course=4,
        module=5,
        activity=6,
        file=7,
        search=8,
        none=9,
        trash=10,

    }
}
