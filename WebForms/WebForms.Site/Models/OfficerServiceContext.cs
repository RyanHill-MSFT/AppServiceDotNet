using System.Data.Entity;

namespace WebForms.Site.Models
{
    public class OfficerServiceContext : DbContext
    {
        public OfficerServiceContext() : base("Starfleet")
        {
        }

        public DbSet<Officer> Officers { get; set; }
    }
}