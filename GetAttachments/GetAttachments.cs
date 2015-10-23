using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rally.RestApi;
using Rally.RestApi.Response;

namespace GetAttachments
{
    class Program
    {
        static void Main(string[] args)
        {
            RallyRestApi restApi = new RallyRestApi(webServiceVersion: "v2.0");
            String apiKey = "_abc123";
            restApi.Authenticate(apiKey, "https://rally1.rallydev.com", allowSSO: false);
            String workspaceRef = "/workspace/12352608129";
            //String projectRef = "/project/39468725060";
            Request request = new Request("Attachment");
            request.Workspace = workspaceRef;
            //request.Project = projectRef;
            request.Fetch = new List<string>() { "Artifact", "TestCaseResult", "Size", "CreationDate" };
            request.Limit = 400;
            request.Order = "CreationDate Desc";
            QueryResult results = restApi.Query(request);

            foreach (var a in results.Results)
            {
                if (a["Artifact"] != null)
                {
                    Console.WriteLine("Artifact: " + a["Artifact"]["_ref"]);
                }
                else if (a["TestCaseResult"] != null)
                {
                    Console.WriteLine("TestCaseResult: " + a["TestCaseResult"]["_ref"]);
                }
                Console.WriteLine("Size: " + a["Size"]); 
            }
        }
    }
}