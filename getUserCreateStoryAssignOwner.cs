using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Rally.RestApi;
using Rally.RestApi.Response;


namespace getUserCreateStoryAssignOwner
{
    class Program
    {
        static void Main(string[] args)
        {
            RallyRestApi restApi;
            restApi = new RallyRestApi("apiuser@co.com", "secret", "https://rally1.rallydev.com", "v2.0");

            String projectRef = "/project/123456";     //use valid OID of your project 


            Request userRequest = new Request("User");

            userRequest.Query = new Query("UserName", Query.Operator.Equals, "user@co.com");

            QueryResult userResults = restApi.Query(userRequest);

            String userRef = userResults.Results.First()._ref;
            Console.WriteLine(userRef);

            DynamicJsonObject myStory = new DynamicJsonObject();
            myStory["Name"] = "a new story";
            myStory["Project"] = projectRef;
            myStory["Owner"] = userRef;
            CreateResult createResult = restApi.Create("HierarchicalRequirement", myStory);
            myStory = restApi.GetByReference(createResult.Reference, "FormattedID", "Owner", "Project");
            Console.WriteLine(myStory["FormattedID"] + " " + myStory["Owner"]._refObjectName + " " + myStory["Project"]._refObjectName);

        }
    }
}