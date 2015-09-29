using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rally.RestApi;
using Rally.RestApi.Response;
using Rally.RestApi.Json;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace CreateAttachment
{
    class Program
    {
        static void Main(string[] args)
        {
            RallyRestApi restApi = new RallyRestApi(webServiceVersion: "v2.0");
            String apiKey = "_abc123"; 
            restApi.Authenticate(apiKey, "https://rally1.rallydev.com", allowSSO: false);
            String workspaceRef = "/workspace/1011574887"; 
            String projectRef = "/project/1791269111"; 
            String userName = "user@co.com";

            try
            {

                Request storyRequest = new Request("hierarchicalrequirement");
                storyRequest.Workspace = workspaceRef;
                storyRequest.Project = projectRef;

                storyRequest.Fetch = new List<string>()
                {
                    "FormattedID"
                };

                
                storyRequest.Query = new Query("FormattedID", Query.Operator.Equals, "US2917");
                QueryResult queryResult = restApi.Query(storyRequest);
                var storyObject = queryResult.Results.First();
                String storyReference = storyObject["_ref"];

                Request userRequest = new Request("user");
                userRequest.Fetch = new List<string>()
                {
                    "UserName"
                };

                
                userRequest.Query = new Query("UserName", Query.Operator.Equals, userName);
                QueryResult queryUserResults = restApi.Query(userRequest);
                DynamicJsonObject user = new DynamicJsonObject();
                user = queryUserResults.Results.First();
                String userRef = user["_ref"];

                
                String imageFilePath = "C:\\images\\";
                String imageFileName = "rally.png";
                String fullImageFile = imageFilePath + imageFileName;
                Image myImage = Image.FromFile(fullImageFile);

                
                string imageBase64String = ImageToBase64(myImage, System.Drawing.Imaging.ImageFormat.Png);
                var imageNumberBytes = Convert.FromBase64String(imageBase64String).Length;
                Console.WriteLine("Image size: " + imageNumberBytes);

                DynamicJsonObject myAttachmentContent = new DynamicJsonObject();
                myAttachmentContent["Content"] = imageBase64String;
                CreateResult myAttachmentContentCreateResult = restApi.Create(workspaceRef,"AttachmentContent", myAttachmentContent);
                String myAttachmentContentRef = myAttachmentContentCreateResult.Reference;
                Console.WriteLine(myAttachmentContentRef);

                DynamicJsonObject myAttachment = new DynamicJsonObject();
                myAttachment["Artifact"] = storyReference;
                myAttachment["Content"] = myAttachmentContentRef;
                myAttachment["Name"] = "rally.png";
                myAttachment["Description"] = "Attachment Desc";
                myAttachment["ContentType"] = "image/png";
                myAttachment["Size"] = imageNumberBytes;
                myAttachment["User"] = userRef;

                CreateResult myAttachmentCreateResult = restApi.Create(workspaceRef, "Attachment", myAttachment);

                List<string> createErrors = myAttachmentContentCreateResult.Errors;
                for (int i = 0; i < createErrors.Count; i++)
                {
                    Console.WriteLine(createErrors[i]);
                }

                String myAttachmentRef = myAttachmentCreateResult.Reference;
                Console.WriteLine(myAttachmentRef);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
       
        public static string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);                
                byte[] imageBytes = ms.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
    }
}