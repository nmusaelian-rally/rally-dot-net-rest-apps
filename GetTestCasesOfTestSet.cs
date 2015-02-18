using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rally.RestApi;
using Rally.RestApi.Response;

namespace GetTestCasesOfTestSet
{
    class Program
    {
        static void Main(string[] args)
        {
            RallyRestApi restApi;
            restApi = new RallyRestApi("user@co.com", "secret", "https://rally1.rallydev.com", "v2.0");
            String projectRef = "/project/12352608219";
            Request request = new Request("TestSet");
            request.Project = projectRef;
            request.Fetch = new List<string>()
            {
                "Name",
                "FormattedID",
                "TestCases"
            };
            request.Query = new Query("FormattedID", Query.Operator.Equals, "TS31");
            QueryResult result = restApi.Query(request);
            
            String testsetRef = result.Results.First()._ref;
            DynamicJsonObject testset = restApi.GetByReference(testsetRef, "TestCases");
            Request request2 = new Request(testset["TestCases"]);
            request2.Fetch = new List<string>()
            {
                "Name",
                "FormattedID"
            };
            request2.Limit = 1000;
            QueryResult result2 = restApi.Query(request2);
            int caseCount = 0;
            foreach (var testcase in result2.Results)
            {
                Console.WriteLine("TestCase: " + testcase["FormattedID"]);
                caseCount++;
            }
            Console.WriteLine(caseCount);
        }
    }
}
