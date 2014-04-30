using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Rally.RestApi;
using Rally.RestApi.Response;

namespace aRESTgetUser
{
    class Program
    {
        static void Main(string[] args)
        {
            //Initialize the REST API
            RallyRestApi restApi;
           restApi = new RallyRestApi("admin@co.com", "topsecret", "https://rally1.rallydev.com", "v2.0");

            //Set our Workspace and Project scopings
            String workspaceRef = "/workspace/1111"; //use valid OID of your workspace
            String projectRef = "/project/2222";     //use valid OID of your project
            bool projectScopingUp = false;
            bool projectScopingDown = true;
            Request userRequest = new Request("User");

            userRequest.Workspace = workspaceRef;
            userRequest.Project = projectRef;
            userRequest.ProjectScopeUp = projectScopingUp;
            userRequest.ProjectScopeDown = projectScopingDown;
		
            /*
                userRequest.Fetch = new List<string>()
                {
                   "Role",
		"CostCenter",
                   "LastLoginDate",
                   "OfficeLocation",
                   "CreationDate"
                };*/

            userRequest.Query = new Query("UserName", Query.Operator.Equals, "nick@wsapi.com");
            
            QueryResult userResults = restApi.Query(userRequest);
            String userRef = userResults.Results.First()._ref;
            DynamicJsonObject user = restApi.GetByReference(userRef, "Name", "Role", "CostCenter", "LastLoginDate", "OfficeLocation", "CreationDate");
            String role = user["Role"];
            String costCenter = user["CostCenter"];
            String lastLoginDate = user["LastLoginDate"];
            String officeLocation = user["OfficeLocation"];
            String creationDate = user["CreationDate"];

            Console.WriteLine("Role: " + role + " CostCenter: " + costCenter + " LastLoginDate: " + lastLoginDate + " OfficeLocation: " + officeLocation + " CreationDate: " + creationDate);

        }
    }
}