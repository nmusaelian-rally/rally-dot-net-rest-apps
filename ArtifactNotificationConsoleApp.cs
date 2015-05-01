using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rally.RestApi;
using Rally.RestApi.Response;
using System.Threading;

namespace ArtifactNotificationConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            RallyRestApi rallyApi = new RallyRestApi("user@co.com", "secret", "https://sandbox.rallydev.com", "v2.0");
            Program program = new Program();
            String projectRef = "/project/12352608219"; //Company X
            String aUTCdateString = "2015-04-08";
            while (true)
            {
                
                Console.WriteLine("===============");
                DateTime start = DateTime.Now;
                var p = program.GetArtifactNotificationForDateRange(rallyApi, aUTCdateString, DateTime.Now.ToString("o"), projectRef);
                DateTime finish = DateTime.Now;
                Console.WriteLine(string.Format("{0}", finish.Subtract(start)));
                Thread.Sleep(5000);
            }
        }
        public List<dynamic> GetArtifactNotificationForDateRange(RallyRestApi rallyApi, string LastItemDateStamp, string currentRunTime, string projectRef)
        {
            List<dynamic> returnValue;
            Request request = new Request("artifactnotification");
            request.Project = projectRef;
            //request.PageSize = 200;
            request.Limit = 5000;
            request.Fetch = new List<string>() { "IDPrefix", "ID"};
            request.Query = new Query("CreationDate", Query.Operator.GreaterThan, LastItemDateStamp).And(new Query("CreationDate", Query.Operator.LessThanOrEqualTo, currentRunTime));
            try
            {
                QueryResult qr = rallyApi.Query(request);
                var results = qr.Results;
                foreach (var r in results)
                {
                    Console.WriteLine(r["IDPrefix"] + r["ID"]);
                }
                returnValue = results.ToList();
                return returnValue;
            }
            catch (Exception)
            {
                throw;
            } 
        }
    }
}
