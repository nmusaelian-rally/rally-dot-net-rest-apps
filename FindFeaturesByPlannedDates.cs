using System.Text;
using Rally.RestApi;
using Rally.RestApi.Response;

namespace FindFeaturesByPlannedDates
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime today = DateTime.Today;
            DateTime forward = today.AddDays(90);
            String forwardString = forward.ToString("yyyy-MM-dd");
            DateTime back = today.AddDays(-90);
            String backString = back.ToString("yyyy-MM-dd");

            RallyRestApi restApi;
            restApi = new RallyRestApi("_abc123", "https://rally1.rallydev.com"); //either username/passowrd or API Key authetnication is valid
            String workspaceRef = "/workspace/12352608129";     //replace this OID with an OID of your workspace

            Request request = new Request("PortfolioItem/Feature");
            request.Workspace = workspaceRef;
            request.Fetch = new List<string>() {"Name","FormattedID","PlannedStartDate","PlannedEndDate"};
            request.Query = new Query("PlannedStartDate", Query.Operator.GreaterThan, backString).And(new Query("PlannedEndDate", Query.Operator.LessThanOrEqualTo, forwardString));
            QueryResult results = restApi.Query(request);

            foreach (var f in results.Results)
            {
                Console.WriteLine("FormattedID: " + f["FormattedID"] + " Name: " + f["Name"] + " PlannedStartDate: " + f["PlannedStartDate"] + " PlannedEndDate: " + f["PlannedEndDate"]);
                Console.WriteLine("**********************************************************");
            }
        }
    }
}