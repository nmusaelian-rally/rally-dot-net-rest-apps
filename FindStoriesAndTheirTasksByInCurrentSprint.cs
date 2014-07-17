using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Rally.RestApi;
using Rally.RestApi.Response;

namespace FindStoriesAndTheirTasksByInCurrentSprint
{
    class Program
    {
        static void Main(string[] args)
        {
            int storyCount = 0;
            int taskCount = 0;
            RallyRestApi restApi;
            restApi = new RallyRestApi("user@co.com", "secret", "https://rally1.rallydev.com", "v2.0");

            String workspaceRef = "/workspace/12352608129";     //replace this OID with an OID of your workspace

            Request sRequest = new Request("HierarchicalRequirement");
            sRequest.Workspace = workspaceRef;
            sRequest.Fetch = new List<string>() { "FormattedID", "Name", "Tasks", "Iteration"};
            sRequest.Query = new Query("(Iteration.StartDate <= Today)").And(new Query("(Iteration.EndDate >= Today)"));
            QueryResult queryResults = restApi.Query(sRequest);

            foreach (var s in queryResults.Results)
            {
                Console.WriteLine("FormattedID: " + s["FormattedID"] + " Name: " + s["Name"]);
                storyCount++;
                Request tasksRequest = new Request(s["Tasks"]);
                QueryResult queryTaskResult = restApi.Query(tasksRequest);
                foreach (var t in queryTaskResult.Results)
                {
                    Console.WriteLine("Task: " + t["FormattedID"] + " State: " + t["State"]);
                    taskCount++;
                }
            }
            Console.WriteLine(storyCount + " stories, " + taskCount + " tasks ");
        }
    }
}