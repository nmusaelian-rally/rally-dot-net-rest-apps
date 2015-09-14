using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rally.RestApi;
using Rally.RestApi.Response;

namespace ArtifactEndpoint
{
    class Program
    {
        static void Main(string[] args)
        {
            RallyRestApi restApi;
            //this app is using old 2.0.1 of .NET Toolkit
            restApi = new RallyRestApi("user@co.com", "secret", "https://rally1.rallydev.com", "v2.0");

            String workspaceRef = "/workspace/12345";     //replace this OID with an OID of your workspace

            Request request = new Request("Artifact");
            request.Workspace = workspaceRef;
            request.Fetch = new List<string>() { "FormattedID", "Name"};
            request.Query = new Query("FormattedID", Query.Operator.Equals, "DE4");
            QueryResult result = restApi.Query(request);
            //since artifact query disregards prefix in FormattedID, it will return DE4, TA4, US4 if they all exist, hence
            //there is no guarantee that the first result will be a defect id we use result.Results.First()._ref as below:
            //String defectRef = result.Results.First()._ref; 
            String defectRef = "";
            foreach (var artifact in result.Results)
            {
                Console.WriteLine(artifact["_type"] + ": " + artifact["_ref"]);
                if (artifact["_type"] == "Defect")
                {
                    defectRef = artifact["_ref"];
                }
            }
             
            Request conversationPostRequest = new Request("ConversationPost");
            conversationPostRequest.Workspace = workspaceRef;
            conversationPostRequest.Query = new Query("Artifact", Query.Operator.Equals, defectRef);
            conversationPostRequest.Fetch = new List<string>() { "Text", "User" };
            QueryResult conversationPostResult = restApi.Query(conversationPostRequest);
            Console.WriteLine("Discussions:-------------------");
            foreach (var post in conversationPostResult.Results)
            {
                Console.WriteLine(post["User"]["_refObjectName"] + ": " + post["Text"]);
            }
        }
    }
}