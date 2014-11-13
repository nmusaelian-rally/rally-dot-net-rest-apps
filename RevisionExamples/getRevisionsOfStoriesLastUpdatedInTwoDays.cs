using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Rally.RestApi;
using Rally.RestApi.Response;

namespace getRevisionsOfStoriesLastUpdatedInTwoDays
{
    class Program
    {
        static void Main(string[] args)
        {
            RallyRestApi restApi;
            restApi = new RallyRestApi("user@co.com", "secret", "https://rally1.rallydev.com", "v2.0");

            String workspaceRef = "/workspace/12345";     //replace this OID with an OID of your workspace

            DateTime today = DateTime.Today;
            DateTime checkback = today.AddDays(-2); //last two days
            String checkbackString = checkback.ToString("yyyy-MM-dd");

            Request sRequest = new Request("HierarchicalRequirement");
            sRequest.Workspace = workspaceRef;
            sRequest.Fetch = new List<string>() { "Name", "FormattedID", "RevisionHistory", "Revisions", "LastUpdateDate" };
            sRequest.Query = new Query("LastUpdateDate", Query.Operator.GreaterThanOrEqualTo, checkbackString);
            QueryResult queryResults = restApi.Query(sRequest);

            foreach (var s in queryResults.Results)
            {
                Console.WriteLine("**********************************************************");
                Console.WriteLine("FormattedID: " + s["FormattedID"] + " Name: " + s["Name"]);
                Console.WriteLine("**********************************************************"); 
                String historyRef = s["RevisionHistory"]._ref;
                Request revisionsRequest = new Request("Revisions");
                revisionsRequest.Query = new Query("RevisionHistory", Query.Operator.Equals, historyRef);
                revisionsRequest.Fetch = new List<string>() { "User", "Description", "RevisionNumber", "CreationDate" };
                QueryResult revisionsResults = restApi.Query(revisionsRequest);
                foreach (var r in revisionsResults.Results)
                {
                    Console.WriteLine("-------------------------------------------------------");
                    Console.WriteLine("Description: " + r["Description"] + " Author: " + r["User"]._refObjectName + " CreationDate: " + r["CreationDate"]);
                    
                }
            }
        }
    }
}
