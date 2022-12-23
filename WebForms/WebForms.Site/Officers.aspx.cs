using System;
using System.Linq;
using WebForms.Site.Models;

namespace WebForms.Site
{
    public partial class Officers : System.Web.UI.Page
    {
        private readonly OfficerServiceContext _context;

        public Officers()
        {
            _context = new OfficerServiceContext();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable<Officer> OfficersGrid_GetData()
        {
            using (var db = new OfficerServiceContext())
            {
                return db.Officers;
            }
        }
    }
}