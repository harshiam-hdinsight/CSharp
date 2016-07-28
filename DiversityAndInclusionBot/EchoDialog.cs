using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Microsoft.Bot.DiversityInclusionBot
{
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        protected List<string> fileUrls= new List<string>();

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            if (message.Text == "reset")
            {
                PromptDialog.Confirm(
                    context,
                    AfterResetAsync,
                    "Enter the file URL you want to analyze. It will be added to the existing list.",
                    "Didn't get that!",
                    promptStyle: PromptStyle.None);
            }
            else
            {
                this.fileUrls.Add(message.Text);
                await context.PostAsync(string.Format("Total images: {0}.", this.fileUrls.Count)); 
                context.Wait(MessageReceivedAsync);
            }
        }

        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                this.fileUrls = new List<string>();
                await context.PostAsync("Reset successful.");
            }
            else
            {
                await context.PostAsync("Did not reset.");
            }
            context.Wait(MessageReceivedAsync);
        }
    }
}