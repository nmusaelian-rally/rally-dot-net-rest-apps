using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Rally.RestApi;
using Rally.RestApi.Response;

namespace BulkDelete
{
    class Program
    {
        static void Main(string[] args)
        {
            RallyRestApi restApi;

            restApi = new RallyRestApi("user@co.com", "secret", "https://rally1.rallydev.com", "v2.0");
            String workspaceRef = "/workspace/12352608129"; //use your workspace object id

            Request defectRequest = new Request("defect");
            defectRequest.Workspace = workspaceRef;
            defectRequest.Query = new Query("LastUpdateDate", Query.Operator.GreaterThanOrEqualTo, "2014-07-20");
            defectRequest.Fetch = new List<string>()
                {
                    "Name",
                    "FormattedID"
                };

            int stopAfter = 2;  //KEEP IT LOW UNTIL YOU MAKE SURE IT IS DELETING THE INTENDED SUBSET OF YOUR DATA
            int count = 0;
            QueryResult queryDefectResults = restApi.Query(defectRequest);
            foreach (var d in queryDefectResults.Results)
            {
                if (count == stopAfter)
                {
                    break;
                }
                Console.WriteLine("Deleting: " + d["FormattedID"] + " " + d["Name"]);
                OperationResult deleteResult = restApi.Delete(workspaceRef, d["_ref"]); //DELTED DEFECTS WILL BE FOUND IN THE RECYCLE BIN
                count++; 
            }
        }
    }
}