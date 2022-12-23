using WebForms.Site.Models;

namespace WebForms.Site.Helpers
{
    public class Crew
    {
        public static Officer[] TheNextGeneration => new[]
        {
                new Officer
                {
                    Department = DepartmentArea.Command,
                    Rank = StarfleetRank.Captain,
                    FirstName = "Jean-Luc",
                    LastName = "Picard",
                    SerialNo = "SP-937-215",
                    Assignment = "Commanding Officer"
                },
                new Officer
                {
                    Department = DepartmentArea.Command,
                    Rank = StarfleetRank.Commander,
                    FirstName = "William",
                    MiddleName = "Thomas",
                    LastName = "Riker",
                    SerialNo = "SC-231-427",
                    Assignment = "Executive Officer"
                },
                new Officer
                {
                    Department = DepartmentArea.Operations,
                    Rank = StarfleetRank.Lieutenant | StarfleetRank.Commander,
                    FirstName = "Data",
                    Assignment = "Second Officer",
                    SerialNo = Officer.GenerateSerialNumber()
                },
                new Officer
                {
                    Department = DepartmentArea.Operations,
                    Rank = StarfleetRank.Lieutenant | StarfleetRank.Commander,
                    FirstName = "Geordi",
                    LastName = "LaForge",
                    Assignment = "Chief Engineer",
                    SerialNo = Officer.GenerateSerialNumber()
                },
                new Officer
                {
                    Department = DepartmentArea.Science,
                    Rank = StarfleetRank.Lieutenant | StarfleetRank.Commander,
                    FirstName = "Deanna",
                    LastName = "Troi",
                    Assignment = "Counselor",
                    SerialNo = Officer.GenerateSerialNumber()
                },
                new Officer
                {
                    Department = DepartmentArea.Medical,
                    Rank = StarfleetRank.Lieutenant | StarfleetRank.Commander,
                    FirstName = "Beverly",
                    LastName = "Crusher",
                    Assignment = "Chief Medical Officer",
                    SerialNo = Officer.GenerateSerialNumber()
               }
        };
    }
}