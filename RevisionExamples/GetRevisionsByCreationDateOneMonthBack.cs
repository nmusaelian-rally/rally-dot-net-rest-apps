using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rally.RestApi;
using Rally.RestApi.Response;

namespace GetRevisionsByCreationDateOneMonthsBack
{
    class Program
    {
        static void Main(string[] args)
        {
            RallyRestApi restApi;
            restApi = new RallyRestApi("user@co.com", "secret", "https://rally1.rallydev.com", "v2.0");

            String workspaceRef = "/workspace/12345"; //replace this OID with an ObjectID of your workspace
            Request revRequest = new Request("Revision");
            revRequest.Workspace = workspaceRef;
            revRequest.Fetch = new List<string>() { "Description", "User", "CreationDate" };

            DateTime today = DateTime.Today;
            DateTime oneMonthBack = today.AddMonths(-1);
            String todayString = today.ToString("yyyy-MM-dd");
            String thenString = oneMonthBack.ToString("yyyy-MM-dd");
            revRequest.Query = new Query("CreationDate", Query.Operator.GreaterThanOrEqualTo, thenString);

            QueryResult queryRevResults = restApi.Query(revRequest);

            foreach (var rev in queryRevResults.Results)
            {
                 Console.WriteLine("----------");
                 Console.WriteLine("Description: " + rev["Description"] + " Author: " + rev["User"]._refObjectName + " CreationDate: " + rev["CreationDate"]);
            }

        }
    }
}
