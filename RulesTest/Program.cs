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

            //var inputs = new object[]
            //{
            //    input1
            //};

            List<RuleResultTree> resultList = null;

            // will fail as too young
            var input1 = new Person
            {
                FirstName = "Bob",
                LastName = "Hope",
                DateOfBirth = new DateTime(2010, 1, 1),
                Country = "dk"
            };

            resultList = rulesEngine.ExecuteAllRulesAsync("PersonValidation", new object[]
            {
                input1
            }).Result;

            PrintResults(resultList);

            input1 = new Person
            {
                FirstName = "Dean",
                LastName = "Martin",
                DateOfBirth = new DateTime(1980, 1, 1),
                Country = "de"
            };

            resultList = rulesEngine.ExecuteAllRulesAsync("PersonValidation", new object[]
            {
                input1
            }).Result;

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
