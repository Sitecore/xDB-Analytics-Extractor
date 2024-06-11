using xDBAnalyticsExtractor.XConnectConfiguration;
using Sitecore.XConnect;
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.UnitTests.Calculator
{
    [TestFixture]
    public class InteractionCalculatorTests
    {
        private Interaction _InteractionNoEvents;
        private Interaction _InteractionOnePageEvent_EngagementValue_5;
        private Interaction _InteractionMultipleEvents_Valid;
        private Interaction _InteractionMultipleEvents_NoPageEvents;

        private Interaction _Invalid_Interaction_TimeOnSiteExceedsIntMax;
        private Interaction _Invalid_Interaction_EngagementValueExceedsMaxInt;
        private Interaction _Invalid_Interaction_OutcomeMonetaryValue_ExceedsDecimalMax;
        private Interaction _Invalid_Interaction_OutcomeMonetaryValue_ExceedsDecimalMin;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var newContact = new Sitecore.XConnect.Contact();
            Guid channelId = Guid.Parse("86c7467a-d019-460d-9fa9-85d6d5d77fc4");
            string userAgent = "Sample User Agent";

            // Valid Interactions

            _InteractionNoEvents = new Interaction(newContact, InteractionInitiator.Brand, channelId, userAgent);

            _InteractionOnePageEvent_EngagementValue_5 = new Interaction(newContact, InteractionInitiator.Brand, channelId, userAgent);
            _InteractionOnePageEvent_EngagementValue_5.Events.Add(PageEvent(5));

            // Complex Interaction Analysis
            // Engagement Value Summary          : 58
            // Page View Count                   :  2
            // Goals Count                       :  3
            // Outcomes Count                    :  3
            // Outcomes Monetary Value Summary   : 85.4
            // Time On Site Summary (in seconds) : 200

            _InteractionMultipleEvents_Valid = new Interaction(newContact, InteractionInitiator.Brand, channelId, userAgent);
            _InteractionMultipleEvents_Valid.Events.Add(PageEvent(5, new TimeSpan(0, 0, 100)));
            _InteractionMultipleEvents_Valid.Events.Add(PageEvent(7, new TimeSpan(0, 0, 100)));
            _InteractionMultipleEvents_Valid.Events.Add(Goal(21, new TimeSpan(0, 0, 100)));
            _InteractionMultipleEvents_Valid.Events.Add(Goal(5, new TimeSpan(0, 0, 100)));
            _InteractionMultipleEvents_Valid.Events.Add(Goal(5, new TimeSpan(0, 0, 100)));
            _InteractionMultipleEvents_Valid.Events.Add(Outcome(50m, 5,new TimeSpan(0, 0, 100)));
            _InteractionMultipleEvents_Valid.Events.Add(Outcome(25.1m, 5,new TimeSpan(0, 0, 100)));
            _InteractionMultipleEvents_Valid.Events.Add(Outcome(10.3m, 5,new TimeSpan(0, 0, 100)));

            _InteractionMultipleEvents_NoPageEvents = new Interaction(newContact, InteractionInitiator.Brand, channelId, userAgent);
            _InteractionMultipleEvents_NoPageEvents.Events.Add(Goal(21));
            _InteractionMultipleEvents_NoPageEvents.Events.Add(Goal(5, new TimeSpan(0, 0, 100)));
            _InteractionMultipleEvents_NoPageEvents.Events.Add(Goal(5, new TimeSpan(0, 0, 100)));
            _InteractionMultipleEvents_NoPageEvents.Events.Add(Outcome(50m));
            _InteractionMultipleEvents_NoPageEvents.Events.Add(Outcome(25.1m));
            _InteractionMultipleEvents_NoPageEvents.Events.Add(Outcome(10.3m));

            // Invalid Interactions

            _Invalid_Interaction_TimeOnSiteExceedsIntMax = new Interaction(newContact, InteractionInitiator.Brand, channelId, userAgent);
            _Invalid_Interaction_TimeOnSiteExceedsIntMax.Events.Add(PageEvent(1, new TimeSpan(50000000, 0, 0)));
            _Invalid_Interaction_TimeOnSiteExceedsIntMax.Events.Add(PageEvent(2, new TimeSpan(50000000, 0, 0)));
            _Invalid_Interaction_TimeOnSiteExceedsIntMax.Events.Add(PageEvent(4, new TimeSpan(50000000, 0, 0)));

            _Invalid_Interaction_EngagementValueExceedsMaxInt = new Interaction(newContact, InteractionInitiator.Brand, channelId, userAgent);
            _Invalid_Interaction_EngagementValueExceedsMaxInt.Events.Add(PageEvent(Int32.MaxValue));
            _Invalid_Interaction_EngagementValueExceedsMaxInt.Events.Add(PageEvent(7));

            _Invalid_Interaction_OutcomeMonetaryValue_ExceedsDecimalMax = new Interaction(newContact, InteractionInitiator.Brand, channelId, userAgent);
            _Invalid_Interaction_OutcomeMonetaryValue_ExceedsDecimalMax.Events.Add(Outcome(decimal.MaxValue));
            _Invalid_Interaction_OutcomeMonetaryValue_ExceedsDecimalMax.Events.Add(Outcome(25.1m));
            _Invalid_Interaction_OutcomeMonetaryValue_ExceedsDecimalMax.Events.Add(Outcome(10.3m));

            _Invalid_Interaction_OutcomeMonetaryValue_ExceedsDecimalMin = new Interaction(newContact, InteractionInitiator.Brand, channelId, userAgent);
            _Invalid_Interaction_OutcomeMonetaryValue_ExceedsDecimalMin.Events.Add(Outcome(decimal.MinValue));
            _Invalid_Interaction_OutcomeMonetaryValue_ExceedsDecimalMin.Events.Add(Outcome(-25.1m));
            _Invalid_Interaction_OutcomeMonetaryValue_ExceedsDecimalMin.Events.Add(Outcome(10.3m));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _InteractionNoEvents.Events.Clear();
            _InteractionOnePageEvent_EngagementValue_5.Events.Clear();
            _InteractionMultipleEvents_Valid.Events.Clear();
            _InteractionMultipleEvents_NoPageEvents.Events.Clear();

            _Invalid_Interaction_TimeOnSiteExceedsIntMax.Events.Clear();
            _Invalid_Interaction_EngagementValueExceedsMaxInt.Events.Clear();
            _Invalid_Interaction_OutcomeMonetaryValue_ExceedsDecimalMax.Events.Clear();
            _Invalid_Interaction_OutcomeMonetaryValue_ExceedsDecimalMin.Events.Clear();

            _InteractionNoEvents = null;
            _InteractionOnePageEvent_EngagementValue_5 = null;
            _InteractionMultipleEvents_Valid = null;
            _InteractionMultipleEvents_NoPageEvents = null;

            _Invalid_Interaction_TimeOnSiteExceedsIntMax = null;
            _Invalid_Interaction_EngagementValueExceedsMaxInt = null;
            _Invalid_Interaction_OutcomeMonetaryValue_ExceedsDecimalMax = null;
            _Invalid_Interaction_OutcomeMonetaryValue_ExceedsDecimalMin = null;
        }

        //EngagementValue
        [Test]
        public void Calculate_GivenNonNullInteraction_ReturnsMetrics()
        {
            // Arrange: -

            // Act: 
            var metrics = InteractionCalculator.Calculate(_InteractionNoEvents);

            // Assert:
            Assert.That(metrics, !Is.Null);
        }
        [Test]
        public void Calculate_GivenNoEvents_ReturnsZeroEngagementValue()
        {
            // Arrange:

            // Act:
            var metrics = InteractionCalculator.Calculate(_InteractionNoEvents);
            // Assert:
            Assert.That(metrics.EngagementValue, Is.EqualTo(0));
        }
        [Test]
        public void Calculate_GivenOnePageEvent_ReturnsCorrectEngagementValue()
        {
            // Arrange:

            // Act:
            var metrics = InteractionCalculator.Calculate(_InteractionOnePageEvent_EngagementValue_5);
            // Assert:
            Assert.That(metrics.EngagementValue, Is.EqualTo(5));
        }
        [Test]
        public void Calculate_GivenMultipleEvents_ReturnsCorrectEngagementValue()
        {
            // Arrange:

            // Act:
            var metrics = InteractionCalculator.Calculate(_InteractionMultipleEvents_Valid);
            // Assert:
            Assert.That(metrics.EngagementValue, Is.EqualTo(58));
        }
        [Test]
        public void Calculate_GivenOverflowingEngagementValue_ThrowsOverflowException()
        {
            // Arrange:

            // Act:

            // Assert:
            Assert.Throws<System.OverflowException>(() => InteractionCalculator.Calculate(_Invalid_Interaction_EngagementValueExceedsMaxInt));
        }

        //Bounces
        [Test]
        public void Calculate_GivenNoPageEvents_ReturnsZeroBounces()
        {
            // Arrange:

            // Act:
            var metrics = InteractionCalculator.Calculate(_InteractionNoEvents);
            // Assert:
            Assert.That(metrics.Bounces, Is.EqualTo(0));
        }
        [Test]
        public void Calculate_GivenOnePageEvent_ReturnsOneBounce()
        {
            // Arrange:

            // Act:
            var metrics = InteractionCalculator.Calculate(_InteractionOnePageEvent_EngagementValue_5);
            // Assert:
            Assert.That(metrics.Bounces, Is.EqualTo(1));
        }
        [Test]
        public void Calculate_GivenMultiplePageEvents_ReturnsZeroBounces()
        {
            // Arrange:

            // Act:
            var metrics = InteractionCalculator.Calculate(_InteractionMultipleEvents_Valid);
            // Assert:
            Assert.That(metrics.Bounces, Is.EqualTo(0));
        }

        //Conversions
        [Test]
        public void Calculate_GivenNoGoals_ReturnsZeroConversions()
        {
            // Arrange:

            // Act:
            var metrics = InteractionCalculator.Calculate(_InteractionOnePageEvent_EngagementValue_5);
            // Assert:
            Assert.That(metrics.Conversions, Is.EqualTo(0));
        }
        [Test]
        public void Calculate_GivenMultipleGoals_ReturnsCorrectConversionCount()
        {
            // Arrange:

            // Act:
            var metrics = InteractionCalculator.Calculate(_InteractionMultipleEvents_Valid);
            // Assert:
            Assert.That(metrics.Conversions, Is.EqualTo(3));
        }
        
        //Converted
        [Test]
        public void Calculate_GivenNoEvents_ReturnsZeroConverted()
        {
            // Arrange:

            // Act:
            var metrics = InteractionCalculator.Calculate(_InteractionNoEvents);
            // Assert:
            Assert.That(metrics.Converted, Is.EqualTo(0));
        }
        [Test]
        public void Calculate_GivenNoGoals_ReturnsZeroConverted()
        {
            // Arrange:

            // Act:
            var metrics = InteractionCalculator.Calculate(_InteractionOnePageEvent_EngagementValue_5);
            // Assert:
            Assert.That(metrics.Converted, Is.EqualTo(0));
        }
        [Test]
        public void Calculate_GivenOneGoal_ReturnsOneConverted()
        {
            // Arrange:

            // Act:
            var metrics = InteractionCalculator.Calculate(_InteractionMultipleEvents_Valid);
            // Assert:
            Assert.That(metrics.Converted, Is.EqualTo(1));
        }
        [Test]
        public void Calculate_GivenMultipleGoals_ReturnsOneConverted()
        {
            // Arrange:

            // Act:
            var metrics = InteractionCalculator.Calculate(_InteractionMultipleEvents_Valid);
            // Assert:
            Assert.That(metrics.Converted, Is.EqualTo(1));
        }

        // TimeOnSite
        [Test]
        public void Calculate_GivenNoPageEvents_ReturnsZeroTimeOnSite()
        {
            // Arrange:

            // Act:
            var metrics = InteractionCalculator.Calculate(_InteractionMultipleEvents_NoPageEvents);
            // Assert:
            Assert.That(metrics.TimeOnSite, Is.EqualTo(0));
        }
        [Test]
        public void Calculate_GivenMultiplePageEvents_ReturnsCorrectTimeOnSite()
        {
            // Arrange:

            // Act:
            var metrics = InteractionCalculator.Calculate(_InteractionMultipleEvents_Valid);
            // Assert:
            Assert.That(metrics.TimeOnSite, Is.EqualTo(200));
        }

        [Test]
        public void Calculate_GivenMultiplePageEventsWithDurationSummaryOverflow_ReturnsIntMinValue()
        {
            // TimeOnSite is an integer that is supposed to hold duration in seconds
            // If overall duration (kept as timespan) in seconds of an interaction exceeds int.MaxValue then an int.MinValue will occur.
            // Arrange:

            // Act:
            var metrics = InteractionCalculator.Calculate(_Invalid_Interaction_TimeOnSiteExceedsIntMax);
            // Assert:
            Assert.That(metrics.TimeOnSite, Is.EqualTo(int.MinValue));
        }

        //PageViews
        [Test]
        public void Calculate_GivenNoPageViews_ReturnsZeroPageViews()
        {
            // Arrange:

            // Act:
            var metrics = InteractionCalculator.Calculate(_InteractionMultipleEvents_NoPageEvents);
            // Assert:
            Assert.That(metrics.PageViews, Is.EqualTo(0));
        }
        [Test]
        public void Calculate_GivenMultiplePageViews_ReturnsCorrectPageViewCount()
        {
            // Arrange:

            // Act:
            var metrics = InteractionCalculator.Calculate(_InteractionMultipleEvents_Valid);
            // Assert:
            Assert.That(metrics.PageViews, Is.EqualTo(2));
        }

        //OutcomeOccurrences
        [Test]
        public void Calculate_GivenNoOutcomeOccurrences_ReturnsZeroOutcomeOccurrences()
        {
            // Arrange:

            // Act:
            var metrics = InteractionCalculator.Calculate(_InteractionOnePageEvent_EngagementValue_5);
            // Assert:
            Assert.That(metrics.OutcomeOccurrences, Is.EqualTo(0));
        }
        [Test]
        public void Calculate_GivenMultipleOutcomeOccurrences_ReturnsCorrectOutcomeOccurrenceCount()
        {
            // Arrange:

            // Act:
            var metrics = InteractionCalculator.Calculate(_InteractionMultipleEvents_Valid);
            // Assert:
            Assert.That(metrics.OutcomeOccurrences, Is.EqualTo(3));
        }

        //MonetaryValue
        [Test]
        public void Calculate_GivenNoOutcomeOccurrences_ReturnsZeroMonetaryValue()
        {
            // Arrange:

            // Act:
            var metrics = InteractionCalculator.Calculate(_InteractionOnePageEvent_EngagementValue_5);
            // Assert:
            Assert.That(metrics.MonetaryValue, Is.EqualTo(0));
        }
        [Test]
        public void Calculate_GivenMultipleOutcomeOccurrences_ReturnsCorrectMonetaryValue()
        {
            // Arrange:

            // Act:
            var metrics = InteractionCalculator.Calculate(_InteractionMultipleEvents_Valid);
            // Assert:
            Assert.That(metrics.MonetaryValue, Is.EqualTo(85.4m));
        }
        [Test]
        public void Calculate_GivenOverflowingPositiveMonetaryValue_ThrowsOverflowException()
        {
            // Arrange:

            // Act:

            // Assert:
            Assert.Throws<System.OverflowException>(() => InteractionCalculator.Calculate(_Invalid_Interaction_OutcomeMonetaryValue_ExceedsDecimalMax));
        }
        [Test]
        public void Calculate_GivenOverflowingNegativeMonetaryValue_ThrowsOverflowException()
        {
            // Arrange:

            // Act:

            // Assert:
            Assert.Throws<System.OverflowException>(() => InteractionCalculator.Calculate(_Invalid_Interaction_OutcomeMonetaryValue_ExceedsDecimalMin));
        }


        //Helper Methods
        private PageViewEvent PageEvent(int engagementValue = 5, TimeSpan? duration = null)
        {
            var itemId = Guid.NewGuid();
            var itemVersion = 2;
            var timeOfEvent = new DateTime(2016, 10, 10, 13, 22, 22).ToUniversalTime();
            var itemLanguage = "en";
            PageViewEvent pageView = new PageViewEvent(timeOfEvent, itemId, itemVersion, itemLanguage);
            pageView.ItemLanguage = "en";
            pageView.Duration = duration ?? new TimeSpan(0, 0, 30);
            pageView.EngagementValue = engagementValue;
            return pageView;
        }
        private Goal Goal(int engagementValue = 5, TimeSpan? duration = null)
        {
            var goalId = Guid.NewGuid();
            var timeOfEvent = new DateTime(2016, 10, 10, 13, 22, 22).ToUniversalTime();
            var goal = new Goal(goalId, timeOfEvent);
            goal.EngagementValue = engagementValue;
            goal.Duration = duration ?? new TimeSpan(0, 0, 30);
            return goal;
        }
        private Outcome Outcome(decimal monetaryValue = 0, int engagementValue = 5, TimeSpan? duration = null)
        {
            var outcomeId = Guid.NewGuid();
            var timeOfEvent = new DateTime(2016, 10, 10, 13, 22, 22).ToUniversalTime();
            var outcome = new Outcome(outcomeId, timeOfEvent, "USD", monetaryValue);
            outcome.EngagementValue = engagementValue;
            outcome.Duration = duration ?? new TimeSpan(0, 0, 30);
            return outcome;
        }
    }
}
