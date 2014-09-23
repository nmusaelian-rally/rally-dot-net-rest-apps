using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Rally.RestApi;
using Rally.RestApi.Response;

namespace getProjectsWithUpdatedArtifacts
{
    class Program
    {
        static void Main(string[] args)
        {
            //Initialize the REST API using API KEYd 
            RallyRestApi restApi;
            restApi = new RallyRestApi("_abc123", "https://rally1.rallydev.com"); 

            DateTime now = DateTime.Today;
            DateTime checkback = now.AddDays(-2); //last two days
            String checkbackString = checkback.ToString("yyyy-MM-dd");

            //get the subscription
            DynamicJsonObject sub = restApi.GetSubscription("Workspaces");

            Request wRequest = new Request(sub["Workspaces"]);
            wRequest.Limit = 1000;
            QueryResult queryResult = restApi.Query(wRequest);
            foreach (var result in queryResult.Results)
            {
                var workspaceReference = result["_ref"];
                var workspaceName = result["Name"];
                Request projectsRequest = new Request(result["Projects"]);
                projectsRequest.Fetch = new List<string>()
                {
                    "Name"
                };
                projectsRequest.Limit = 1000; 
                QueryResult queryProjectResult = restApi.Query(projectsRequest);
                foreach (var p in queryProjectResult.Results)
                {
                    Request artifactRequest = new Request("artifact");
                    artifactRequest.Project = p["_ref"];
                    artifactRequest.Query = new Query("LastUpdateDate", Query.Operator.GreaterThanOrEqualTo, checkbackString);
                    QueryResult queryResults = restApi.Query(artifactRequest);
                    if (queryResults.TotalResultCount > 0)
                    {
                        Console.WriteLine("Project: " + p["Name"] + " (Workspace " + workspaceName + ")");
                    }
                    
                }
            }

        }
    }
}