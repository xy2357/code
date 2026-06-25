using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace International_Calling_Connoisseur
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public static class DialingCodes
    {
        public static Dictionary<int, string> GetEmptyDictionary()
        {
            return new Dictionary<int, string>();
        }

        public static Dictionary<int, string> GetExistingDictionary()
        {
            return new Dictionary<int, string>
            {
                {1, "United States of America"},
                {55, "Brazil"},
                {91, "India"}
            };
        }

        public static Dictionary<int, string> AddCountryToEmptyDictionary(int countryCode, string countryName)
        {
            Dictionary<int, string> dic= new Dictionary<int, string>();
            dic.Add(countryCode, countryName);
            return dic;
        }

        public static Dictionary<int, string> AddCountryToExistingDictionary(
            Dictionary<int, string> existingDictionary, int countryCode, string countryName)
        {
            existingDictionary.Add(countryCode, countryName);
            return existingDictionary;
        }

        public static string GetCountryNameFromDictionary(
            Dictionary<int, string> existingDictionary, int countryCode)
        {
            if (existingDictionary.ContainsKey(countryCode))
            {
                return existingDictionary[countryCode];
            }
            return "string.Empty";
        }

        public static bool CheckCodeExists(Dictionary<int, string> existingDictionary, int countryCode)
        {
            return existingDictionary.ContainsKey(countryCode);
        }

        public static Dictionary<int, string> UpdateDictionary(
            Dictionary<int, string> existingDictionary, int countryCode, string countryName)
        {
            if (existingDictionary.ContainsKey(countryCode))
            {
                existingDictionary[countryCode] = countryName;
                return existingDictionary;
            }
            return existingDictionary;
        }

        public static Dictionary<int, string> RemoveCountryFromDictionary(
            Dictionary<int, string> existingDictionary, int countryCode)
        {
            existingDictionary.Remove(countryCode);
            return existingDictionary;
        }

        public static string FindLongestCountryName(Dictionary<int, string> existingDictionary)
        {
            int longestCountry = 0;
            string longestCountryName = "";
            foreach (string countryName in existingDictionary.Values)
            {
                if (countryName.Length >= longestCountry)
                {
                    longestCountry = countryName.Length;
                    longestCountryName = countryName;
                }
            }
            return longestCountryName;
        }
    }
}
