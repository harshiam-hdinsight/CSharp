using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Text;

namespace Microsoft.Bot.DiversityInclusionBot
{
    public class DiversityAnalysis
    {
        public static string GetSentimentPercentages(List<Face> profiles)
        {
            IDictionary<string, int> countPerRace = new Dictionary<string, int>();
            countPerRace.Add("Happy", 0);
            countPerRace.Add("Neutral", 0);
            countPerRace.Add("Unhappy", 0);

            foreach (var p in profiles)
            {
                if (p.attribute.smiling.value < 20)
                {
                    countPerRace["Unhappy"] = countPerRace["Unhappy"] + 1;
                }
                else if (p.attribute.smiling.value >= 20 && p.attribute.smiling.value < 50)
                {
                    countPerRace["Neutral"] = countPerRace["Neutral"] + 1;
                }
                else if (p.attribute.smiling.value < 60)
                {
                    countPerRace["Happy"] = countPerRace["Happy"] + 1;
                }
                else
                {
                    countPerRace["Neutral"] = countPerRace["Neutral"] + 1;
                }
            }

            var response = new StringBuilder("Sentiment Analysis: ");
            response.AppendLine();
            var diversityIndicator = "Very diverse";
            foreach (var raceDetected in countPerRace)
            {
                var percentage = 100*((double) raceDetected.Value/profiles.Count);

                if (percentage < 40 || percentage > 60)
                {
                    diversityIndicator = "Adequately diverse";

                    if (percentage < 30 || percentage > 70)
                    {
                        diversityIndicator = "Moderately diverse";
                        if (percentage < 10 || percentage > 90)
                        {
                            diversityIndicator = "Minimally diverse";
                        }
                    }
                }
                response.AppendLine(string.Format(" {0} : {1:00.00}%", raceDetected.Key, percentage));
            }

            response.AppendLine("Sentiment diversity indicator: " + diversityIndicator);
            response.AppendLine();
            return response.ToString();
        }

        public static string GetGenderPercentages(List<Face> profiles)
        {
             IDictionary<string, int> countPerRace = new Dictionary<string, int>();

            foreach (var p in profiles)
            {
                if (!countPerRace.ContainsKey(p.attribute.gender.value))
                {
                    countPerRace.Add(p.attribute.gender.value, 1);
                }
                else
                {
                    countPerRace[p.attribute.gender.value] = countPerRace[p.attribute.gender.value] + 1;
                }
            }

            var response = new StringBuilder("Gender diversity analysis: ");
            response.AppendLine();
            var differentRaces = countPerRace.Count;
            var diversityIndicator = "Very diverse";
            foreach (var raceDetected in countPerRace)
            {
                var percentage = 100*((double) raceDetected.Value/profiles.Count);

                if (percentage < 40 || percentage > 60)
                {
                    diversityIndicator = "Adequately diverse";

                    if (percentage < 30 || percentage > 70)
                    {
                        diversityIndicator = "Moderately diverse";
                        if (percentage < 10 || percentage > 90)
                        {
                            diversityIndicator = "Minimally diverse";
                        }
                    }
                }
                response.AppendLine(string.Format(" {0} : {1:00.00}%", raceDetected.Key, percentage));
            }

            response.AppendLine("Gender diversity indicator: " + diversityIndicator);
            response.AppendLine();
            return response.ToString();
        }

        public static string GetRacialPercentages(List<Face> profiles)
        {
            IDictionary<string, int> countPerRace = new Dictionary<string, int>();

            foreach (var p in profiles)
            {
                if (!countPerRace.ContainsKey(p.attribute.race.value))
                {
                    countPerRace.Add(p.attribute.race.value, 1);
                }
                else
                {
                    countPerRace[p.attribute.race.value] = countPerRace[p.attribute.race.value] + 1;
                }
            }

            var response = new StringBuilder("Racial diversity analysis:");
            var differentRaces = countPerRace.Count;
            var diversityIndicator = "Very diverse";
            foreach (var raceDetected in countPerRace)
            {
                var percentage = 100*((double) raceDetected.Value/profiles.Count);

                if (percentage < 40 || percentage > 60)
                {
                    diversityIndicator = "Adequately diverse";

                    if (percentage < 30 || percentage > 70)
                    {
                        diversityIndicator = "Moderately diverse";
                        if (percentage < 10 || percentage > 90)
                        {
                            diversityIndicator = "Minimally diverse";
                        }
                    }
                }
                response.AppendLine(string.Format(" {0} : {1:00.00}%", raceDetected.Key, percentage));
            }

            response.AppendLine("Racial diversity indicator: " + diversityIndicator);
            response.AppendLine();

            return response.ToString();
        }

        public static string GetAgePercentages(List<Face> profiles)
        {
            IDictionary<string, int> countPerRace = new Dictionary<string, int>();
            countPerRace.Add("0-18", 0);
            countPerRace.Add("18-40", 0);
            countPerRace.Add("40-60", 0);
            countPerRace.Add("60+", 0);

            foreach (var p in profiles)
            {
                if (p.attribute.age.value < 18)
                {
                    countPerRace["0-18"] = countPerRace["0-18"] + 1;
                }
                else if (p.attribute.age.value < 40)
                {
                    countPerRace["18-40"] = countPerRace["18-40"] + 1;
                }
                else if (p.attribute.age.value < 60)
                {
                    countPerRace["40-60"] = countPerRace["40-60"] + 1;
                }
                else
                {
                    countPerRace["60+"] = countPerRace["60+"] + 1;
                }
            }

            var response = new StringBuilder("Age group diversity analysis:");
            response.AppendLine();

            var diversityIndicator = "Very diverse";
            foreach (var raceDetected in countPerRace)
            {
                var percentage = 100*((double) raceDetected.Value/profiles.Count);

                if (percentage < 40 || percentage > 60)
                {
                    diversityIndicator = "Adequately diverse";

                    if (percentage < 30 || percentage > 70)
                    {
                        diversityIndicator = "Moderately diverse";
                        if (percentage < 10 || percentage > 90)
                        {
                            diversityIndicator = "Minimally diverse";
                        }
                    }
                }
                response.AppendLine(string.Format(" {0} : {1:00.00}%", raceDetected.Key, percentage));
            }

            response.AppendLine("Age diversity indicator: " + diversityIndicator);
            response.AppendLine();

            return response.ToString();
        }
    }
}