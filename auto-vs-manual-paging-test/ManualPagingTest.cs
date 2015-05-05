using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Rally.RestApi;
using Rally.RestApi.Response;

namespace GetStoriesWithManualPaging
{
    class Program
    {
        static void Main(string[] args)
        {

            Program program = new Program();
            String projectRef = "/project/1234"; 
            DateTime start = DateTime.Now;
            var p = program.GetStories(DateTime.Now.ToString("o"), projectRef);
            DateTime finish = DateTime.Now;
            Console.WriteLine(string.Format("{0}", finish.Subtract(start)));
        }
        public List<dynamic> GetStories(string currentRunTime, string projectRef)
        {
            List<dynamic> returnValue;
            RallyRestApi rallyApi = new RallyRestApi("user@co.com", "secret", "https://rally1.rallydev.com", "v2.0");
            Request request = new Request("HierarchicalRequirement");
            request.Project = projectRef;
            request.ProjectScopeUp = false;
            request.ProjectScopeDown = false;
            request.PageSize = 200;
            try
            {

                QueryResult qr = rallyApi.Query(request);
                var results = qr.Results;
                returnValue = results.ToList();
                while (results.Count() == 200)
                {
                    Console.WriteLine("inside while loop...  " + results.Count());
                    request.Start = returnValue.Count + 1;
                    results = rallyApi.Query(request).Results;
                    returnValue.AddRange(results);
                }
                Console.WriteLine("total results returned...  " + returnValue.Count);
            }
            catch (Exception)
            {
                throw;
            }

            return returnValue;
        }
    }
}