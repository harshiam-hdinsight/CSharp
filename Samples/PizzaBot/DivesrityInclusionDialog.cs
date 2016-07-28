using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Luis.Models;

namespace Microsoft.Bot.DiversityInclusionBot.Old
{
    [LuisModel("4311ccf1-5ed1-44fe-9f10-a6adbad05c14", "6d0966209c6e4f6b835ce34492f3e6d9")]
    [Serializable]
    internal class DivesrityInclusionDialog : LuisDialog<DiversityIndicatorAnalysisOrder>
    {
        private readonly BuildFormDelegate<DiversityIndicatorAnalysisOrder> AnalyzeDni;

        internal DivesrityInclusionDialog(BuildFormDelegate<DiversityIndicatorAnalysisOrder> analyzeDni)
        {
            this.AnalyzeDni = analyzeDni;
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry. I didn't understand you.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("DiversityAnalysis")]
        [LuisIntent("InclusionAnalyis")]

        public async Task ProcessDniForm(IDialogContext context, LuisResult result)
        {
            var entities = new List<EntityRecommendation>(result.Entities);
            if (!entities.Any((entity) => entity.Type == "InputType"))
            {
                // Infer kind
                foreach (var entity in result.Entities)
                {
                    string kind = null;
                    switch (entity.Type)
                    {
                        case "Image":
                            kind = "Image";
                            break;
                        case "Email":
                            kind = "Email";
                            break;
                        case "IM":
                            kind = "IM";
                            break;
                        default:
                            if (entity.Type.StartsWith("BYO")) kind = "byo";
                            break;
                    }
                    if (kind != null)
                    {
                        entities.Add(new EntityRecommendation(type: "InputType") {Entity = kind});
                        break;
                    }
                }
            }

            var form = new FormDialog<DiversityIndicatorAnalysisOrder>(new DiversityIndicatorAnalysisOrder(),
                this.AnalyzeDni, FormOptions.PromptInStart, entities);
            context.Call<DiversityIndicatorAnalysisOrder>(form, AnalysisFormComplete);
        }

        private async Task AnalysisFormComplete(IDialogContext context,
            IAwaitable<DiversityIndicatorAnalysisOrder> result)
        {
            DiversityIndicatorAnalysisOrder order = null;
            try
            {
                order = await result;
            }
            catch (OperationCanceledException)
            {
                await context.PostAsync("You canceled the form!");
                return;
            }

            if (order != null)
            {
                await context.PostAsync("Your divesrity and inclusion inputs" + order.ToString());
            }
            else
            {
                await context.PostAsync("Form returned empty response!");
            }

            context.Wait(MessageReceived);
        }
    }
}