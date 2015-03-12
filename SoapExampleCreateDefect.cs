//WARNING: THIS CODE IS NOT SUPPORTED BY RALLY
//SOAP IS NOT SUPPORTED BY RALLY
//THIS CODE IS USING DEPRECATED VERSION OF WS API

using ConsoleApp_createDefect42.com.rallydev.rally1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp_createDefect_42
{
    class Program
    {
        static void Main(string[] args)
        {


            // create a service object
            RallyServiceService service = new RallyServiceService();

            service.Url = "https://rally1.rallydev.com/slm/webservice/1.42/RallyService";


            // login to service using HTTP Basic auth
            System.Net.NetworkCredential crediential =
               new System.Net.NetworkCredential("user@co.com", "secret");


            Uri uri = new Uri(service.Url);
            System.Net.ICredentials credentials = crediential.GetCredential(uri, "Basic");
            service.Credentials = credentials;
            service.PreAuthenticate = true;

            // Configure the service to maintain an HTTP session cookie
            service.CookieContainer = new System.Net.CookieContainer();

            Workspace workspace = null;


            // Make the web service call


            //---------------------------
            Defect defect = new Defect();
            service.create(defect);

            // Name is required
            defect.Name = "bad defect";
            defect.Description = "trouble";



            //Create defect on the server
            CreateResult createResult = service.create(defect);
            if (hasErrors(createResult))
            {
                // something went wrong
                Console.WriteLine("Could not create defect result:");
                printWarningsErrors(createResult);
            }
            else
            {
                // look at the object returned from create()
                defect = (Defect)createResult.Object;
                Console.WriteLine("Created defect, ref = " + defect.@ref);
            }


        }

        //--------------------------
        // determine if the result had errors
        static bool hasErrors(OperationResult result)
        {
            return (result.Errors.Length > 0);
        }

        // print warnings and errors to the console
        static void printWarningsErrors(OperationResult result)
        {
            if (result.Warnings.Length > 0)
            {
                Console.WriteLine("Result has warnings:");
                for (int i = 0; i < result.Warnings.Length; i++)
                {
                    Console.WriteLine("  warnings[" + i + "] = " + result.Warnings[i]);
                }
            }
            if (result.Errors.Length > 0)
            {
                Console.WriteLine("Result has errors:");
                for (int i = 0; i < result.Errors.Length; i++)
                {
                    Console.WriteLine("  errors[" + i + "] = " + result.Errors[i]);

                }
            }
        }
    }
}