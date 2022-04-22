using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebMeDown.Site.Models
{
    public class OfficerDatabaseInitializer : DropCreateDatabaseIfModelChanges<OfficerContext>
    {
    }
}