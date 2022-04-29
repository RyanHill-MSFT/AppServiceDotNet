using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebMeDown.Site.Models
{
    public class OfficerServiceContext : DbContext
    {
        public OfficerServiceContext() : base("Starfleet")
        {
        }

        public DbSet<Officer> Officers { get; set; }
    }
}