using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Rally.RestApi;
using Rally.RestApi.Response;

namespace getChangesetsOfStories
{
    class Program
    {
        static void Main(string[] args)
        {
            RallyRestApi restApi;
            restApi = new RallyRestApi("user@co.com", "secret", "https://rally1.rallydev.com", "v2.0");

            String workspaceRef = "/workspace/12352608129";     //replace this OID with an OID of your workspace

            DateTime today = DateTime.Today;
            DateTime checkback = today.AddDays(-5);
            String checkbackString = checkback.ToString("yyyy-MM-dd");

            Request sRequest = new Request("HierarchicalRequirement");
            sRequest.Workspace = workspaceRef;
            sRequest.Fetch = new List<string>() { "Name", "FormattedID", "Changesets", "LastUpdateDate" };
            sRequest.Query = new Query("LastUpdateDate", Query.Operator.GreaterThanOrEqualTo, checkbackString);
            QueryResult queryResults = restApi.Query(sRequest);

            foreach (var s in queryResults.Results)
            {
                Console.WriteLine("**********************************************************");
                Console.WriteLine("FormattedID: " + s["FormattedID"] + " Name: " + s["Name"]);
                Console.WriteLine("**********************************************************");
                Request changesetRequest = new Request("Changesets");
                QueryResult changesetResult = restApi.Query(changesetRequest);
                String author = "None";
                if (changesetResult.TotalResultCount > 0)
                {
                    foreach (var c in changesetResult.Results)
                    {
                        if (c["Author"] != null)
                        {
                            author = c["Author"]._refObjectName;
                        }
                        Console.WriteLine("changeset: Author:" + author + " SCMRepository: " + c["SCMRepository"]._refObjectName);
                        Request changesRequest = new Request(c["Changes"]);
                        QueryResult changesResult = restApi.Query(changesRequest);
                        foreach (var ch in changesResult.Results)
                        {
                            Console.WriteLine("Change: " + ch["Action"] + " PathAndFilename: " + ch["PathAndFilename"]);
                        }
                    }
                }
                
            }
        }
    }
}