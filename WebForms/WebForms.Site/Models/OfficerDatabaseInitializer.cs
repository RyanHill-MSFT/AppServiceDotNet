using System.Data.Entity;
using WebForms.Site.Helpers;

namespace WebForms.Site.Models
{
    public class OfficerDatabaseInitializer : DropCreateDatabaseIfModelChanges<OfficerServiceContext>
    {
        protected override void Seed(OfficerServiceContext context)
        {
            context.Officers.AddRange(Crew.TheNextGeneration);
            context.SaveChanges();
        }
    }
}