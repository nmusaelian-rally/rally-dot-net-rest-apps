using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rally.RestApi;
using Rally.RestApi.Response;
using Rally.RestApi.Json;

namespace CreateDefect
{
    class Program
    {
        static void Main(string[] args)
        {
            RallyRestApi restApi = new RallyRestApi(webServiceVersion: "v2.0");
            String apiKey = "_abc777";
            restApi.Authenticate(apiKey, "https://rally1.rallydev.com", allowSSO: false);
            String workspaceRef = "/workspace/123"; 
            String projectRef = "/project/134";

            DynamicJsonObject badDefect = new DynamicJsonObject();
            badDefect["Name"] = "bad defect 2" + DateTime.Now;
            badDefect["Project"] = projectRef;

            CreateResult createRequest = restApi.Create(workspaceRef, "Defect", badDefect);
            badDefect = restApi.GetByReference(createRequest.Reference, "FormattedID", "Project", "State");
            Console.WriteLine(badDefect["FormattedID"] + " " + badDefect["Project"]._refObjectName + " " + badDefect["State"]);

            badDefect["State"] = "Open";
            OperationResult updateRequest = restApi.Update(badDefect["_ref"], badDefect);
            Console.WriteLine("Success? " + updateRequest.Success);
            Console.WriteLine("updated State: " + badDefect["State"]);
        }
    }
}
