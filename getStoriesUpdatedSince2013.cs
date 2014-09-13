using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rally.RestApi;
using Rally.RestApi.Response;

namespace aRESTgetStoriesLastUpdated
{
    class Program
    {
        static void Main(string[] args)
        {
            RallyRestApi restApi;
            restApi = new RallyRestApi("_abc123", "https://rally1.rallydev.com");

            String projectRef = "/project/12352608219";
            bool projectScopingUp = false;
            bool projectScopingDown = false;

            Request storyRequest = new Request("hierarchicalrequirement");
            storyRequest.Project = projectRef;
            storyRequest.ProjectScopeUp = projectScopingUp;
            storyRequest.ProjectScopeDown = projectScopingDown;
            storyRequest.Limit = 10000;
            storyRequest.Query = new Query("LastUpdateDate", Query.Operator.GreaterThanOrEqualTo, "2013-01-01");

            storyRequest.Fetch = new List<string>()
                {
                    "Name",
                    "FormattedID"
                };

            QueryResult queryResults = restApi.Query(storyRequest);
            int count = 0;
            foreach (var d in queryResults.Results)
            {
                count++;
                Console.WriteLine(count + ". FormattedID: " + d["FormattedID"] + " Name: " + d["Name"]);
            }
            Console.WriteLine("Found " + queryResults.TotalResultCount);
        }
    }
}

