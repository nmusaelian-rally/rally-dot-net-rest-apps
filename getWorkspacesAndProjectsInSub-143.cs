using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Rally.RestApi;
using Rally.RestApi.Response;

namespace RESTexample_storiesFromIteration
{
    class Program
    {
        static void Main(string[] args)
        {

            //Initialize the REST API
            RallyRestApi restApi;
            restApi = new RallyRestApi("user@co.com", "secret", "https://rally1.rallydev.com", "1.43"); //NOTE: 1.43 will be unsupported after June 2014
            Request subRequest = new Request("Subscription");
            subRequest.Fetch = new List<string>()
                {
                    "Name",
                    "ObjectID",
                    "Workspaces",
                    "Projects"
                };

            String subQueryString = "(Name = \"Rally Support\")";
            subRequest.Query = new Query(subQueryString);

            QueryResult subQueryResults = restApi.Query(subRequest);
            foreach (var s in subQueryResults.Results)
            {
                Console.WriteLine("Name: " + s["Name"]);
                Console.WriteLine("ObjectID: " + s["ObjectID"]);
                var workspaces = s["Workspaces"];
                int projectCounter = 0;
                foreach (var w in workspaces)
                {
                    Console.WriteLine("Workspace: " + w["ObjectID"] + " " + w["Name"]);
                    var projects = w["Projects"];
                    foreach (var p in projects)
                    {
                        Console.WriteLine("Project: " + p["ObjectID"] + " " + p["Name"]);
                        projectCounter++;
                    }
                    Console.WriteLine("projects total in the sub: " + projectCounter);
               }
            }

        }

    }
}