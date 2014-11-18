using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Rally.RestApi;
using Rally.RestApi.Response;

namespace addTaskOnDefect
{
    class Program
    {
        static void Main(string[] args)
        {
            RallyRestApi restApi;
            restApi = new RallyRestApi("user@co.com", "secret", "https://rally1.rallydev.com", "v2.0");

            String projectRef = "/project/12352608219";
            Request defectRequest = new Request("Defect");
            defectRequest.Project = projectRef;
            defectRequest.Fetch = new List<string>()
                {
                    "Name",
		    "FormattedID",
                    "Tasks"
                };

            defectRequest.Query = new Query("FormattedID", Query.Operator.Equals, "DE8");
            QueryResult defectResults = restApi.Query(defectRequest);
            String defRef = defectResults.Results.First()._ref;
            String defName = defectResults.Results.First().Name;
            Console.WriteLine(defName + " " + defRef);
            DynamicJsonObject defect = restApi.GetByReference(defRef, "Name", "FormattedID", "Tasks");
            String taskCollectionRef = defect["Tasks"]._ref;
            Console.WriteLine(taskCollectionRef);

            ArrayList taskList = new ArrayList();

            foreach (var d in defectResults.Results)
            {
                Request tasksRequest = new Request(d["Tasks"]);
                QueryResult tasksResult = restApi.Query(tasksRequest);
                foreach (var t in tasksResult.Results)
                {
                    var tName = t["Name"];
                    var tFormattedID = t["FormattedID"];
                    Console.WriteLine("Task: " + tName + " " + tFormattedID);
                    DynamicJsonObject task = new DynamicJsonObject();
                    task["_ref"] = t["_ref"];
                    taskList.Add(task); 
                }
            }

            Console.WriteLine("Count of elements in the collection before adding a new task: " + taskList.Count);

            DynamicJsonObject newTask = new DynamicJsonObject();
            newTask["Name"] = "another last task";
            newTask["WorkProduct"] = defRef;
            CreateResult createResult = restApi.Create(projectRef, "Task", newTask);
            newTask = restApi.GetByReference(createResult.Reference, "FormattedID", "Name", "WorkProduct");
            Console.WriteLine(newTask["FormattedID"] + " " + newTask["Name"] + " WorkProduct:" + newTask["WorkProduct"]["FormattedID"]);
            taskList.Add(newTask);

            Console.WriteLine("Count of elements in the array after adding a new task: " + taskList.Count);
            defect["Tasks"] = taskList;
            OperationResult updateResult = restApi.Update(defRef, defect);

        }
    }
}

