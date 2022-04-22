using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebMeDown.Site.Models
{
    public class OfficerContext : DbContext
    {
        public OfficerContext() : base("Starfleet")
        {
        }

        public DbSet<Officer> Officers { get; set; }
    }
}