using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Rally.RestApi;
using Rally.RestApi.Response;

namespace Rest_v2._0_test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Initialize the REST API
            RallyRestApi restApi;
//with this user I get hundreds of workspaces - this is a subadmin
            restApi = new RallyRestApi("admin@company.com", "supersecret", "https://rally1.rallydev.com", "v2.0");
//with this user I get only two workspaces
            //restApi = new RallyRestApi("user@company.com", "scret", "https://rally1.rallydev.com", "v2.0"); 


/***************THIS CODE PRINTS OUT ALL WORKSPACES AND PROJECTS IN THE SUB ******************************/
            
          
            //get the current subscription
            DynamicJsonObject sub = restApi.GetSubscription("Workspaces");

            Request wRequest = new Request(sub["Workspaces"]);
            wRequest.Limit = 1000;
            QueryResult queryResult = restApi.Query(wRequest);
            int allProjects = 0;
            foreach (var result in queryResult.Results)
            {
                var workspaceReference = result["_ref"];
                var workspaceName = result["Name"];
                Console.WriteLine("Workspace: " + workspaceName);
                Request projectsRequest = new Request(result["Projects"]);
                projectsRequest.Fetch = new List<string>()
                {
                    "Name",
		    "State",
                };
                projectsRequest.Limit = 10000; //project requests are made per workspace
                QueryResult queryProjectResult = restApi.Query(projectsRequest);
                int projectsPerWorkspace = 0;
                foreach (var p in queryProjectResult.Results)
                {
                    allProjects++;
                    projectsPerWorkspace++;
                    Console.WriteLine(projectsPerWorkspace + " Project: " + p["Name"] + " State: " + p["State"]);
                } 
		Console.WriteLine("----------------------------");
            }
            Console.WriteLine("Returned " + allProjects + " projects in the subscription");
           
        }
    }
}
