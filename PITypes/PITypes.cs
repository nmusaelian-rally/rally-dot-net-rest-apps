using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rally.RestApi;
using Rally.RestApi.Response;
using Rally.RestApi.Json;


namespace PITypes
{
    class Program
    {
        static void Main(string[] args)
        {
            RallyRestApi restApi = new RallyRestApi(webServiceVersion: "v2.0");
            String apiKey = "_abc123";
            restApi.Authenticate(apiKey, "https://rally1.rallydev.com", allowSSO: false);

            String[] workspaces = new String[] { "/workspace/12352608129", "/workspace/34900020610" };

            foreach (var s in workspaces)
            {
                Console.WriteLine(" ______________ " + s + " _________________");
                Request typedefRequest = new Request("TypeDefinition");
                typedefRequest.Workspace = s;
                typedefRequest.Fetch = new List<string>() { "ElementName", "Ordinal" };
                typedefRequest.Query = new Query("Parent.Name", Query.Operator.Equals, "Portfolio Item");
                QueryResult typedefResponse = restApi.Query(typedefRequest);

                foreach (var t in typedefResponse.Results)
                {

                    if (t["Ordinal"] > -1)
                    {
                        Console.WriteLine("ElementName: " + t["ElementName"] + " Ordinal: " + t["Ordinal"]);
                    }
                }
            }
        }
    }
}