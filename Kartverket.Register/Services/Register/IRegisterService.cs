﻿using Kartverket.Register.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartverket.Register.Services.Versioning
{
    public interface IRegisterService
    {
        Kartverket.Register.Models.Register Filter(Kartverket.Register.Models.Register register, FilterParameters filter);
    }
}