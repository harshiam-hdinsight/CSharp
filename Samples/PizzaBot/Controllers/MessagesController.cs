using System.Web.Http;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Dialogs;
using System.Web.Http.Description;
using System.Net.Http;
using System.Diagnostics;

namespace Microsoft.Bot.DiversityInclusionBot.Old
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private static IForm<DiversityIndicatorAnalysisOrder> BuildForm()
        {
            var builder = new FormBuilder<DiversityIndicatorAnalysisOrder>();

            ActiveDelegate<DiversityIndicatorAnalysisOrder> isImage = (input) => input.InputType == InputFormat.Image;
            //ActiveDelegate<DiversityIndicatorAnalysisOrder> isImageFolder = (input) => input.InputType == InputFormat.Image;

            return builder
                .Message("Welcome to the diversity and inclusion bot!")
                .Message("Enter the type of diversity and inclusion metric you are interested 1. Racial, 2. Gender, 3.Age!")
                .Field(nameof(DiversityIndicatorAnalysisOrder.Metric))
                .Message("Enter the type of input 1. Image, 2. TBD!")
                .Field(nameof(DiversityIndicatorAnalysisOrder.InputType))
                .AddRemainingFields()
                .Confirm("Would you like to know {Metric}?", isImage)
                .Build()
                ;
        }

        internal static IDialog<DiversityIndicatorAnalysisOrder> MakeRoot()
        {
            return Chain.From(() => new DivesrityInclusionDialog(BuildForm));
        }

        /// <summary>
        /// POST: api/Messages
        /// receive a message from a user and send replies
        /// </summary>
        /// <param name="activity"></param>
        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            if (activity != null)
            {
                // one of these will have an interface and process it
                var type = activity.GetActivityType();
                switch (activity.GetActivityType())
                {
                    case ActivityTypes.Message:
                        await Conversation.SendAsync(activity, MakeRoot);
                        break;

                    case ActivityTypes.ConversationUpdate:
                    case ActivityTypes.ContactRelationUpdate:
                    case ActivityTypes.Typing:
                    case ActivityTypes.DeleteUserData:
                    default:
                        Trace.TraceError($"Unknown activity type ignored: {activity.GetActivityType()}");
                        break;
                }
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }
    }
}