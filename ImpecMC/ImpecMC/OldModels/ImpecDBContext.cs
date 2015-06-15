using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ImpecMC.Models;

namespace ImpecMC.Models
{
    public class ImpecDBContext : DbContext
    {
        public ImpecDBContext()
            : base("name=DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
    }
}