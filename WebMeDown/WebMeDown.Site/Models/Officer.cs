using System;
using System.ComponentModel;

namespace WebMeDown.Site.Models
{
    public class Officer
    {
        public string SerialNo { get; set; } = GenerateSerialNumber();

        public StarfleetRank Rank { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Middle")]
        public string MiddleName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public string Assignment { get; set; }

        public string Station { get; set; }

        public DepartmentArea Department { get; set; }

        public void Promote()
        {
            if (Rank < StarfleetRank.Admiral)
            {
                Rank += 1;
            }
        }

        public void Demote()
        {
            if (Rank > StarfleetRank.Crewman)
            {
                Rank -= 1;
            }
        }

        public static string GenerateSerialNumber()
        {
            string[] prefixes = new[] { "SP", "SQ", "SR", "JL", "AR", "KI", "SK", "GH", "CU", "HG", "BL", "OL", "NN", "CX", "WI" };
            var rnd = new Random();
            return $"{prefixes[rnd.Next(prefixes.Length)]}-{rnd.Next(999):000}-{rnd.Next(999):000}";
        }
    }

    public enum StarfleetRank
    {
        Crewman,
        [Description("Ens")]
        Ensign,
        [Description("Lt")]
        Lieutenant,
        [Description("Cmdr")]
        Commander,
        [Description("Capt")]
        Captain,
        [Description("Admr")]
        Admiral
    }

    public enum DepartmentArea
    {
        Science,
        Operations,
        Command,
        Sciences,
        Medical
    }
}