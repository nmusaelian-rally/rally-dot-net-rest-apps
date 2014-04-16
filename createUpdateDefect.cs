using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rally.RestApi;
using Rally.RestApi.Response;

namespace createUpdateDefect
{
    class Program
    {
        static void Main(string[] args)
        {
            RallyRestApi restApi = new RallyRestApi("user@co.com", "secret", "https://rally1.rallydev.com", "v2.0");
            String workspaceRef = "/workspace/11111"; //use valid workspace OID in your Rally
            String projectRef = "/project/12345";         //use valid project OID in your Rally
            DynamicJsonObject d = new DynamicJsonObject();
            d["Name"] = "some bug";
            d["Project"] = projectRef;

            CreateResult createResult = restApi.Create(workspaceRef, "Defect", d);
            DynamicJsonObject defect = restApi.GetByReference(createResult.Reference, "FormattedID");
            Console.WriteLine(defect["FormattedID"]);

            //update defect
            defect["Description"] = "bad bug";
            OperationResult updateResult = restApi.Update(defect["_ref"], defect);
        }
    }
}