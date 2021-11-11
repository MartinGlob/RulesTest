using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RulesEngine;
using RulesEngine.Models;
using Newtonsoft;
using Newtonsoft.Json;
using static RulesEngine.Extensions.ListofRuleResultTreeExtension;
//using RulesTest.HelperFunctions;

namespace RulesTest
{
    class Program
    {
        static void Main(string[] args)
        {

            var jsonRules = File.ReadAllText("./workflows/RulesPerson.json");
            var workflow = JsonConvert.DeserializeObject<List<Workflow>>(jsonRules);

            var reSettingsWithCustomTypes = new ReSettings { CustomTypes = new Type[] { typeof(Utils) } };

            var rulesEngine = new RulesEngine.RulesEngine(workflow.ToArray(), null, reSettingsWithCustomTypes);

            var parameters = new
            {
                CountryList = "dk,no,se,fi",
                MinimumAge = 18
            };

            var person = new Person
            {
                FirstName = "Bob",
                LastName = "Hope",
                DateOfBirth = new DateTime(1970, 1, 1),
                Country = "dk"
            };


            var resultList = rulesEngine.ExecuteAllRulesAsync(
                "PersonValidation",
                new RuleParameter[]
                {
                    new RuleParameter("input1",person),
                    new RuleParameter("input2",parameters)
                }
            ).Result;



            PrintResults(resultList);


            Console.ReadLine();
        }

        private static void PrintResults(List<RuleResultTree> resultList)
        {

            foreach (var r in resultList)
            {
                Console.WriteLine($"{r.Rule.RuleName} : {r.IsSuccess} : {r.ExceptionMessage}");
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; }
    }
}
