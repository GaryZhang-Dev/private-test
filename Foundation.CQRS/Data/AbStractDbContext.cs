﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Foudation.CQRS.Data
{
    public class AbStractDbContext : DbContext
    {
        protected AbStractDbContext(DbContextOptions options) : base(options)
        {

        }
        protected AbStractDbContext() : base() { }
    }
}
