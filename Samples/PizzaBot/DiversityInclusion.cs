using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Text;
#pragma warning disable 649

namespace Microsoft.Bot.DiversityInclusionBot.Old
{
    public enum MetricOptions
    {
        // 0 value in enums is reserved for unknown values.  Either you can supply an explicit one or start enumeration at 1.
        Unknown,
        [Terms(new string[] { "race", "racial" })]
        RacialDiversity,

        [Terms(new string[] { "sex", "men", "women", "male", "female" })]
        GenderDiversity,

        [Terms(new string[] { "age", "young", "old", "middleage" })]
        AgeDiversity,

        [Terms(new string[] { "sentiment", "senti", })]
        SentimentDiversity,

        [Terms(new string[] { "all", "total", })]
        All
    };

    public enum InputFormat
    {
        Unkown,
        Image,
        Im,
        Email,
    };

    [Serializable]
    class DiversityIndicatorAnalysisOrder
    {
        [Prompt("What type of divesrity and inclusive metric do you wish to measure??{||}")]
        [Template(TemplateUsage.NotUnderstood, "What does \"{0}\" mean???")]
        [Describe("Divesity indicators")]
        public MetricOptions Metric;

        [Prompt("What type of input do you wish to provide??{||}")]
        [Template(TemplateUsage.NotUnderstood, "What does \"{0}\" mean???")]
        [Describe("Input types")]
        public InputFormat InputType;

        [Prompt("Please provide the URL of the input.{||}")]
        [Template(TemplateUsage.NotUnderstood, "What does \"{0}\" mean???")]
        [Describe("Input URL")]
        public string InputUrlAddress;
        
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendFormat("Divesrity indicators({0}, ", Metric);
            switch (InputType)
            {
                case InputFormat.Image:
                    builder.AppendFormat("{0}, {1}", InputType, InputUrlAddress);
                    break;
                default:
                    builder.AppendFormat("not supported for input type {0}", InputType);
                    break;
            }
            builder.AppendFormat(")");
            return builder.ToString();
        }
    };
}