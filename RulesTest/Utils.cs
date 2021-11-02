using System;
using System.Linq;

namespace RulesTest
{
    public static class Utils
    {
        public static bool MinimumAgeIs(DateTime birthdate, int minimumAge)
        {
            var today = DateTime.Today;
            var age = today.Year - birthdate.Year;
            if (birthdate.Date > today.AddYears(-age)) age--;
            return age >= minimumAge;
        }

        public static bool CheckContains(string check, string valList)
        {
            if (String.IsNullOrEmpty(check) || String.IsNullOrEmpty(valList))
                return false;

            var list = valList.Split(',').ToList();
            return list.Contains(check);
        }
    }
}