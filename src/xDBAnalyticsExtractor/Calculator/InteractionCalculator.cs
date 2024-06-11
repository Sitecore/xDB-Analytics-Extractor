using xDBAnalyticsExtractor.Models;
using Sitecore.XConnect;
using Sitecore.XConnect.Collection.Model;
using Interaction = Sitecore.XConnect.Interaction;

namespace xDBAnalyticsExtractor.XConnectConfiguration
{
    public static class InteractionCalculator
    {
        public static InteractionMetrics Calculate(Interaction interaction)
        {
            var pageViewEvents = interaction.Events.OfType<PageViewEvent>().ToList();
            var goalEvents = interaction.Events.OfType<Goal>().ToList();
            var outcomeEvents = interaction.Events.OfType<Outcome>().ToList();
            return new InteractionMetrics()
            {
                EngagementValue = GetEngagementValue(interaction),
                Bounces = GetBounces(pageViewEvents),
                Conversions = GetConversions(goalEvents),
                Converted = GetConverted(goalEvents),
                TimeOnSite = GetTimeOnSite(pageViewEvents),
                PageViews = GetPageViews(pageViewEvents),
                OutcomeOccurrences = GetOutcomeOccurrences(outcomeEvents),
                MonetaryValue = GetMonetaryValue(outcomeEvents)
            };
        }

        private static int GetEngagementValue(Interaction interaction)
        {
            return interaction.EngagementValue;
        }

        private static int GetBounces(IEnumerable<PageViewEvent> events)
        {
            return events.Count() == 1 ? 1 : 0;
        }

        private static int GetConversions(IEnumerable<Goal> events)
        {
            return events.Count();
        }

        private static int GetConverted(IEnumerable<Goal> events)
        {
            return events.Any() ? 1 : 0;
        }

        private static int GetTimeOnSite(IEnumerable<PageViewEvent> events)
        {
            return ConvertDurationToSeconds(events.Sum(evt => evt.Duration.TotalMilliseconds));
        }

        private static int GetPageViews(IEnumerable<PageViewEvent> events)
        {
            return events.Count();
        }

        private static int GetOutcomeOccurrences(IEnumerable<Outcome> events)
        {
            return events.Count();
        }

        private static decimal GetMonetaryValue(IEnumerable<Outcome> events)
        {
            return events.Sum(outcome => outcome.MonetaryValue);
        }

        private static int ConvertDurationToSeconds(double duration)
        {
            return duration > 0 ? (int)Math.Round(duration / 1000) : 0;
        }
    }
}