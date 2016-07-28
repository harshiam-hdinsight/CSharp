using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microsoft.Bot.DiversityInclusionBot
{
    public class EchoChainDialog
    {
        protected static List<string> FileUrls = new List<string>();
        protected static string FolderPath;

        private static readonly List<string> _analyzeChoice = new List<string>()
        {
            "1. Racial diversity",
            "2. Gender diversity",
            "3. Age diversity",
            "4. Sentiment diversity",
            "5. All Categories"
        };

        private static readonly List<string> _analyzeChoice2 = new List<string>()
        {
            "Racial diversity",
            "Gender diversity",
            "Age diversity",
            "Sentiment diversity",
            "All Categories"
        };

        public static readonly IDialog<string> dialog = Chain.PostToChain()
            .Select(msg => msg.Text)
            .Switch(
                new Case<string, IDialog<string>>(text =>
                {
                    var regex = new Regex("^reset");
                    return regex.Match(text).Success;
                }, (context, txt) =>
                {
                    return Chain.From(() => new PromptDialog.PromptString("Are you sure you want to reset? Yes/No",
                        "Didn't get that!", 3)).ContinueWith<string, string>(async (ctx, res) =>
                        {
                            string reply;
                            
                            if (res.GetAwaiter().IsCompleted)
                            {
                                var choice = res.GetAwaiter().GetResult();

                                if (choice == "yes" || choice == "y" || choice == "Yes")
                                {
                                    FileUrls = new List<string>();
                                    FolderPath = string.Empty;
                                    reply = "Reset succeeded.";
                                }
                                else
                                {
                                    reply = "Did not reset image Url input list.";
                                }
                            }
                            else
                            {
                                reply = "Did not reset image Url input list.";
                            }
                            return Chain.Return(reply);
                        });
                }),
                new RegexCase<IDialog<string>>(new Regex("^help", RegexOptions.IgnoreCase), (context, txt) =>
                {
                    return
                        Chain.Return(
                            "I am the diversity and inclusion bot! Provide me an image URL analyze by typing \"image\"!");
                }),
                new RegexCase<IDialog<string>>(new Regex("^hi", RegexOptions.IgnoreCase), (context, txt) =>
                {
                    return
                        Chain.Return(
                            "Hi! I am the diversity and inclusion bot! Provide me an image URL analyze by typing \"image\"!");
                }),
                new RegexCase<IDialog<string>>(new Regex("^image", RegexOptions.IgnoreCase), (context, txt) =>
                {
                    return Chain.From(() => new PromptDialog.PromptString("Enter URL of the image you want to analyze.",
                        "Didn't get that!", 3)).ContinueWith<string, string>(async (ctx, res) =>
                        {
                            string reply;
                            var r = res.GetAwaiter().IsCompleted;
                            if (res.GetAwaiter().IsCompleted)
                            {
                                //ctx.UserData.SetValue("imageUrlOrPath", res);
                                FileUrls.Add(res.GetAwaiter().GetResult());
                                reply = "Added image URL to the list. Type \"image\" to add more images or \"analyze\" to see the results.";
                            }
                            else
                            {
                                reply = "Did not add the image URL to list.";
                            }
                            return Chain.Return(reply);
                        });
                }),
                new RegexCase<IDialog<string>>(new Regex("^folderpath", RegexOptions.IgnoreCase), (context, txt) =>
                {
                    return Chain.From(
                        () => new PromptDialog.PromptString("Enter the image folder path you want to analyze.",
                            "Didn't get that!", 3)).ContinueWith<string, string>(async (ctx, res) =>
                            {
                                string reply;
                                var r = res.GetAwaiter().IsCompleted;
                                if (res.GetAwaiter().IsCompleted)
                                {
                                    //ctx.UserData.SetValue("imageUrlOrPath", res);
                                    FolderPath = res.GetAwaiter().GetResult();
                                    reply = "Added image folder path.";
                                }
                                else
                                {
                                    reply = "Did not add the image folder path.";
                                }
                                return Chain.Return(reply);
                            });
                }),
                new RegexCase<IDialog<string>>(new Regex("^analyze", RegexOptions.IgnoreCase), (context, txt) =>
                {
                    return Chain.From(() => new PromptDialog.PromptString(string.Format("Enter the category you want to analyze. Options: {0}", string.Join(", ", _analyzeChoice)),
                        "Didn't get that!", 3)).ContinueWith<string, string>(async (ctx, res) =>
                        {
                            string reply;
                            var r = res.GetAwaiter().IsCompleted;
                            if (res.GetAwaiter().IsCompleted)
                            {
                                var input = res.GetAwaiter().GetResult();
                                string choice;
                                switch (input)
                                {
                                    case "1":
                                        choice = "Racial diversity";
                                        break;
                                    case "2":
                                        choice = "Gender diversity";
                                        break;
                                    case "3":
                                        choice = "Age diversity";
                                        break;
                                    case "4":
                                        choice = "Sentiment diversity";
                                        break;
                                    
                                    default:
                                        choice = "All Categories";
                                        break;
                                }
                                reply = Analyze(choice);
                            }
                            else
                            {
                                reply = "Could not analyze images.";
                            }
                            return Chain.Return(reply);
                        });
                }),
                new RegexCase<IDialog<string>>(new Regex("^analyze", RegexOptions.IgnoreCase), (context, txt) =>
                {
                    return Chain.From(() => new PromptDialog.PromptChoice<string>(_analyzeChoice2, "Enter the category you want to analyze",
                        "Didn't get that!", 3)).ContinueWith<string, string>(async (ctx, res) =>
                        {
                            string reply;
                            var r = res.GetAwaiter().IsCompleted;
                            if (res.GetAwaiter().IsCompleted)
                            {
                                var choice = res.GetAwaiter().GetResult();
                                reply = Analyze(choice);
                            }
                            else
                            {
                                reply = "Could not analyze images.";
                            }
                            return Chain.Return(reply);
                        });
                }),
                new DefaultCase<string, IDialog<string>>((context, txt) =>
                {
                    string reply = string.Format("I don't recognize '{0}'. Please type \"help\" to see options.", txt);
                    return Chain.Return(reply);
                }))
            .Unwrap()
            .PostToUser();

        private static string Analyze(string choice)
        {
            StringBuilder response = new StringBuilder(" Diversity and Inclusion Analysis Report: ");
            response.AppendLine();

            var responseList = new List<string>();

            try
            {

            Parallel.ForEach(FileUrls,
                async url =>
                    responseList.Add(await FaceDetectionApiCall(url, choice)));
            }
            catch (Exception)
            {
                return "Service currently unavailable. Please try again later.";
            }

            List<FaceApiWebResponsePayload> facesDetected = new List<FaceApiWebResponsePayload>();
            foreach (var jsonObj in responseList)
            {
                facesDetected.Add(JsonConvert.DeserializeObject<FaceApiWebResponsePayload>(jsonObj));
            }

            var aggregatefaces = new List<Face>();

            foreach (var faces in facesDetected)
            {
                aggregatefaces.AddRange(faces.face);
            }

            switch (choice)
            {
                case "Racial diversity":
                    response.AppendLine(DiversityAnalysis.GetRacialPercentages(aggregatefaces));
                    break;
                case "Gender diversity":
                    response.AppendLine(DiversityAnalysis.GetGenderPercentages(aggregatefaces));
                    break;
                case "Age diversity":
                    response.AppendLine(DiversityAnalysis.GetAgePercentages(aggregatefaces));
                    break;
                case "Sentiment diversity":
                    response.AppendLine(DiversityAnalysis.GetSentimentPercentages(aggregatefaces));
                    break;
                case "All Categories":
                    response.AppendLine(DiversityAnalysis.GetRacialPercentages(aggregatefaces));
                    response.AppendLine(DiversityAnalysis.GetGenderPercentages(aggregatefaces));
                    response.AppendLine(DiversityAnalysis.GetAgePercentages(aggregatefaces));
                    response.AppendLine(DiversityAnalysis.GetSentimentPercentages(aggregatefaces));

                    break;
            }

            return response.ToString();
        }

        private static async Task<string> FaceDetectionApiCall(string imageUrl, string category)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return string.Empty;
            }

            if (!Uri.IsWellFormedUriString(imageUrl, UriKind.RelativeOrAbsolute))
            {
                return string.Format("Please check url '{0}'. Error: URL Malformed", imageUrl);
            }

            string attributes;
            switch (category)
            {
                case "Racial diversity":
                    attributes = "race";
                    break;
                case "Gender diversity":
                    attributes = "gender";
                    break;
                case "Age diversity":
                    attributes = "age";
                    break;
                case "Sentiment diversity":
                    attributes = "smiling";
                    break;
                default:
                    attributes = "gender,age,race,smiling";
                    break;
            }

            string webRequest =
                string.Format(
                    "https://apius.faceplusplus.com/v2/detection/detect?url={0}&api_secret=cPntRrQ_jSENKTiIiiOOT6JXrQLe_ItE&api_key=140f6b5f844ea335043eda89ea5adaa0&attribute={1}",
                    imageUrl, attributes);

            WebRequest request =
                WebRequest.Create(webRequest);

            request.Method = "POST";

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/json";
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse) response).StatusDescription);
            // Get the stream containing content returned by the server.
            var dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            if (dataStream != null)
            {
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                Console.WriteLine(responseFromServer);
                // Clean up the streams.
                reader.Close();
                dataStream?.Close();
                response.Close();

                return responseFromServer;
            }

            return string.Empty;
        }
    }
}