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

        RallyRestApi restApi = new RallyRestApi("user@co.com", "secret", "https://rally1.rallydev.com", "v2.0");
        DynamicJsonObject user = restApi.GetCurrentUser();
        String userRef = user["_ref"];
        String workspaceRef = "/workspace/11111"; //use valid workspace OID in your Rally
	String projectRef = "/project/12345";         //use valid project OID in your Rally

        DynamicJsonObject myStory = new DynamicJsonObject();
        myStory["Name"] = "my story";
        myStory["Project"] = projectRef;
        myStory["Owner"] = userRef;
        CreateResult createResult = restApi.Create(workspaceRef, "HierarchicalRequirement", myStory);
        myStory = restApi.GetByReference(createResult.Reference, "FormattedID", "Owner", "Project");
        Console.WriteLine(myStory["FormattedID"] + " " + myStory["Owner"]._refObjectName + " " + myStory["Project"]._refObjectName);
    }
}