using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rally.RestApi;
using Rally.RestApi.Response;
using Rally.RestApi.Json;
//using System.Diagnostics;

namespace TestVersion3
{
    class Program
    {
        static void Main(string[] args)
        {
                RallyRestApi restApi = new RallyRestApi(webServiceVersion: "v2.0");
                String apiKey = "_abc123";
                restApi.Authenticate(apiKey, "https://rally1.rallydev.com", allowSSO: false); 
                String projectRef = "/project/32904827032";
                try
                {
                    //create story
                    DynamicJsonObject myStory = new DynamicJsonObject();
                    myStory["Name"] = "another story " + DateTime.Now;
                    myStory["Project"] = projectRef;

                    CreateResult createStory = restApi.Create(workspaceRef, "HierarchicalRequirement", myStory);
                    myStory = restApi.GetByReference(createStory.Reference, "FormattedID", "Project");


                    //update story
                    myStory["Description"] = "updated " + DateTime.Now;
                    myStory["c_CustomString"] = "abc123";
                    Console.WriteLine("--------------------");
                    Console.WriteLine(myStory["FormattedID"]);
                    OperationResult updateResult = restApi.Update(myStory["_ref"], myStory);

                    //create tasks
                    for (int i = 1; i <= 3; i++)
                    {
                        DynamicJsonObject myTask = new DynamicJsonObject();
                        myTask["Name"] = "task " + i + DateTime.Now;
                        myTask["State"] = "In-Progress";
                        myTask["WorkProduct"] = myStory["_ref"];
                        CreateResult createTask = restApi.Create(workspaceRef, "Task", myTask);
                        myTask = restApi.GetByReference(createTask.Reference, "FormattedID", "Owner", "State");
                        Console.WriteLine(myTask["FormattedID"]);

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

        }
    }
}