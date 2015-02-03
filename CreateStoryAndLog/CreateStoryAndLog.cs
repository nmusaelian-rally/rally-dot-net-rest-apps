using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rally.RestApi;
using Rally.RestApi.Response;

class Program
{
    static void Main(string[] args)
    {

        RallyRestApi restApi = new RallyRestApi("user@sandbox.com", "secret", "https://sandbox.rallydev.com", "v2.0");
        DynamicJsonObject user = restApi.GetCurrentUser();
        String userRef = user["_ref"];
        String workspaceRef = "/workspace/12352608129"; //use valid workspace OID in your Rally
        String projectRef = "/project/14018981229";         //use valid project OID in your Rally

        System.Diagnostics.TextWriterTraceListener myListener = new System.Diagnostics.TextWriterTraceListener("log.log", "myListener");

        try
        {
            //create story
            DynamicJsonObject myStory = new DynamicJsonObject();
            myStory["Name"] = "my story " + DateTime.Now;
            myStory["Project"] = projectRef;
            myStory["Owner"] = userRef;
            CreateResult createStory = restApi.Create(workspaceRef, "HierarchicalRequirement", myStory);
            myStory = restApi.GetByReference(createStory.Reference, "FormattedID", "Owner", "Project");
            myListener.WriteLine(DateTime.Now + "___________\r\n" +  myStory["FormattedID"] + " Owner: " + myStory["Owner"]._refObjectName + " Project: " + myStory["Project"]._refObjectName);

            //update story
            myStory["Description"] = "updated " + DateTime.Now;

            //create tasks
            for (int i = 1; i <= 3; i++)
            {
                DynamicJsonObject myTask = new DynamicJsonObject();
                myTask["Name"] = "task " + i + DateTime.Now;
                myTask["Owner"] = userRef;
                myTask["State"] = "In-Progress";
                myTask["WorkProduct"] = myStory["_ref"];
                CreateResult createTask = restApi.Create(workspaceRef, "Task", myTask);
                myTask = restApi.GetByReference(createTask.Reference, "FormattedID", "Owner", "State");
                myListener.WriteLine(myTask["FormattedID"] + " State: " + myTask["StateX"]);
            }
        }
        catch(Exception e)
        {
            myListener.WriteLine(e);
        }
        
        myListener.Flush();
    }
}