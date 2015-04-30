using System;
//using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rally.RestApi;
using Rally.RestApi.Response;

namespace ArtifactNotificationConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            Program program = new Program();
            String projectRef = "/project/12352608219"; 
            String aUTCdateString = "2015-04-08";

            DateTime start = DateTime.Now;
            var p = program.GetArtifactNotificationForDateRange(aUTCdateString, DateTime.Now.ToString("o"), projectRef);
            DateTime finish = DateTime.Now;
            Console.WriteLine(string.Format("{0}", finish.Subtract(start)));
        }
        public List<dynamic> GetArtifactNotificationForDateRange(string LastItemDateStamp, string currentRunTime, string projectRef)
        {
            List<dynamic> returnValue;
            RallyRestApi rallyApi = new RallyRestApi("user@co.com", "secret", "https://rally1.rallydev.com", "v2.0");
            Request request = new Request("artifactnotification");
            request.Project = projectRef;
            request.PageSize = 200;
            request.Limit = 1000;
            request.Query = new Query("CreationDate", Query.Operator.GreaterThan, LastItemDateStamp).And(new Query("CreationDate", Query.Operator.LessThanOrEqualTo, currentRunTime));
            try
            {

                QueryResult qr = rallyApi.Query(request);
                var results = qr.Results;
                returnValue = results.ToList();

                while (results.Count() == 200)
                {
                    request.Start = returnValue.Count + 1;
                    results = rallyApi.Query(request).Results;
                    returnValue.AddRange(results);
                }
            }
            catch (Exception)
            {
                throw;
            }
            
            return returnValue;
        }
    }
}
