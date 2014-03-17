using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Rally.RestApi;
using Rally.RestApi.Response;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Initialize the REST API
            RallyRestApi restApi;
            restApi = new RallyRestApi("user@co.com", "secret", "https://rally1.rallydev.com", "v2.0");

            //Set Workspace 
            String workspaceRef = "/workspace/12352608129"; //please replace this OID with an OID of your workspace

            Request revRequest = new Request("Revision");
            revRequest.Workspace = workspaceRef;

            
            revRequest.Fetch = new List<string>() { "Description"};

            DateTime now = DateTime.Today;
            DateTime oneMonthBack = now.AddMonths(-1);
            String thenString = oneMonthBack.ToString("yyyy-MM-dd");

            revRequest.Query = new Query("CreationDate", Query.Operator.GreaterThanOrEqualTo, thenString);
            
            QueryResult queryRevResults = restApi.Query(revRequest);
            Console.WriteLine(queryRevResults.TotalResultCount);
        }
    }
}