﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BelibaHoma.BLL.Enums
{
    public enum Area
    {
        [Description("צפון")]
        North = 0,
        GushDan = 1,
        Jerusalem = 2,
        South = 3
    }    
    public enum InstitutionType
    {
        [Description("מכינה")]
        Mechina = 0,
        University = 1,
        Collage = 2
    }

    public enum UserRole
    {
        Admin = 0,
        Rackaz = 1,
        Tutor = 2,
        Trainee = 3 
    }
}