using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rally.RestApi;
using Rally.RestApi.Response;

namespace GetRecycleBinEntry
{
    class Program
    {
        static void Main(string[] args)
        {
            String apiKey = "_abc123";
            RallyRestApi restApi = new RallyRestApi(apiKey, "https://rally1.rallydev.com");
            String projectRef = "/project/12352608219"; 
            Request request = new Request("RecycleBinEntry");
            request.Project = projectRef;
            request.Fetch = new List<string>() { "Name"};
            request.Query = new Query("ID", Query.Operator.Equals, "DE6");
            QueryResult result = restApi.Query(request);
            Console.WriteLine("Name: " + result.Results.First()["Name"]);
        }
    }
}