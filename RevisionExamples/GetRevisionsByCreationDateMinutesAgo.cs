using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rally.RestApi;
using Rally.RestApi.Response;

namespace GetRevisionsByCreationDateMinutesAgo
{
    class Program
    {
        static void Main(string[] args)
        {
            RallyRestApi restApi;
            restApi = new RallyRestApi("user@co.com", "secret", "https://rally1.rallydev.com", "v2.0");

            String workspaceRef = "/workspace/12345"; //replace this OID with ObjectID of your workspace
            Request revRequest = new Request("Revision");
            revRequest.Workspace = workspaceRef;
            revRequest.Fetch = new List<string>() { "Description", "User", "CreationDate" };

            DateTime now = DateTime.Now;
            Console.WriteLine("now: " + now);

            //look 4 hours back
            //DateTime fourHoursBack = now.AddHours(-4);
            //String fourHoursBackString = fourHoursBack.ToString("yyyy-MM-dd" + "T" + "HH:mm");
            //Console.WriteLine(fourHoursBack);
            //Console.WriteLine(fourHoursBackString);
            //revRequest.Query = new Query("CreationDate", Query.Operator.GreaterThanOrEqualTo, fourHoursBackString);

            //look 10 min back
            DateTime someMinutesBack = now.AddMinutes(-10);
            String someMinutesBackString = someMinutesBack.ToString("yyyy-MM-dd" + "T" + "HH:mm");
            Console.WriteLine("someMinutesBack: " + someMinutesBack);
            Console.WriteLine("someMinutesBackString: " + someMinutesBackString);
            revRequest.Query = new Query("CreationDate", Query.Operator.GreaterThanOrEqualTo, someMinutesBackString);

            QueryResult queryRevResults = restApi.Query(revRequest);

            foreach (var rev in queryRevResults.Results)
            {
                Console.WriteLine("----------");
                Console.WriteLine("Description: " + rev["Description"] + " Author: " + rev["User"]._refObjectName + " CreationDate: " + rev["CreationDate"]);
            }

        }
    }
}
