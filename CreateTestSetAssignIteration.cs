using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rally.RestApi;
using Rally.RestApi.Response;
using Rally.RestApi.Json;

namespace CreateTestSet
{
    class Program
    {
        static void Main(string[] args)
        {
            RallyRestApi restApi = new RallyRestApi(webServiceVersion: "v2.0");
            String apiKey = "_abc123"; 
            restApi.Authenticate(apiKey, "https://rally1.rallydev.com", allowSSO: false);
            String workspaceRef = "/workspace/1011574887"; //non-default workspace of the user
            String projectRef = "/project/1791269111"; //a non-default project of the user (inside the workspace above)
            try
            {
                //create testset
                DynamicJsonObject myTestSet = new DynamicJsonObject();
                myTestSet["Name"] = "important set " + DateTime.Now;
                myTestSet["Project"] = projectRef;

                CreateResult createTestSet = restApi.Create(workspaceRef, "TestSet", myTestSet);
                myTestSet = restApi.GetByReference(createTestSet.Reference, "FormattedID", "Project");
                Console.WriteLine(myTestSet["FormattedID"] + " " + myTestSet["Project"]._refObjectName);

                //find current iteration

                Request iterationRequest = new Request("Iteration");
                iterationRequest.Project = projectRef;
                iterationRequest.ProjectScopeDown = false;
                iterationRequest.ProjectScopeUp = false;
                iterationRequest.Fetch = new List<string>() { "ObjectID", "Name" };
                iterationRequest.Query = new Query("(StartDate <= Today)").And(new Query("(EndDate >= Today)"));
                QueryResult queryResults = restApi.Query(iterationRequest);
                if (queryResults.TotalResultCount > 0)
                {
                    Console.WriteLine(queryResults.Results.First()["Name"] + " " + queryResults.Results.First()["ObjectID"]);
                    myTestSet["Iteration"] = queryResults.Results.First()._ref;
                    OperationResult updateResult = restApi.Update(myTestSet["_ref"], myTestSet);
                }
                else
                {
                    Console.WriteLine("No current iterations");
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}