﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Users.Domain;
using CORE.APP.Features;

namespace APP.Users.Features
{
    public abstract class UsersDbHandler : Handler
    {
        protected readonly UsersDb _db;

        protected UsersDbHandler(UsersDb db) : base(new CultureInfo("en-US"))
        {
            _db = db;
        }
    }
}
