using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebMeDown.Site.Helpers;

namespace WebMeDown.Site.Models
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