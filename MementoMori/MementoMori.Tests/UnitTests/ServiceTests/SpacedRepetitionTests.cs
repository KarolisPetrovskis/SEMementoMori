using System;
using Xunit;
using MementoMori.Server.Models;
using MementoMori.Server.Service;

namespace MementoMori.Tests.UnitTests.ServiceTests
{
    public class SpacedRepetitionTests
    {
        private readonly SpacedRepetition _spacedRepetition;

        public SpacedRepetitionTests()
        {
            _spacedRepetition = new SpacedRepetition();
        }

        [Fact]
        public void UpdateCard_ShouldIncreaseRepetitions_WhenQualityIsHigh()
        {
            var card = new UserCardData
            {
                Repetitions = 1,
                Interval = 6,
                EaseFactor = 2.5,
                LastReviewed = DateTime.Now.AddDays(-7)
            };
            int quality = 6;

            var updatedCard = _spacedRepetition.UpdateCard(card, quality);

            Assert.Equal(2, updatedCard.Repetitions);
            Assert.Equal(6, updatedCard.Interval); 
            Assert.True(updatedCard.EaseFactor > 2.5); 
        }

        [Fact]
        public void UpdateCard_ShouldResetRepetitionsAndInterval_WhenQualityIsLow()
        {
            var card = new UserCardData
            {
                Repetitions = 3,
                Interval = 10,
                EaseFactor = 2.5,
                LastReviewed = DateTime.Now.AddDays(-11)
            };
            int quality = 2;

            var updatedCard = _spacedRepetition.UpdateCard(card, quality);

            Assert.Equal(0, updatedCard.Repetitions);
            Assert.Equal(1, updatedCard.Interval);
        }


        [Fact]
        public void IsDueForReview_ShouldReturnTrue_WhenCardIsDue()
        {
            var card = new UserCardData
            {
                LastReviewed = DateTime.Now.AddDays(-5),
                Interval = 3
            };

            var isDue = _spacedRepetition.IsDueForReview(card);

            Assert.True(isDue);
        }

        [Fact]
        public void IsDueForReview_ShouldReturnFalse_WhenCardIsNotDue()
        {
            var card = new UserCardData
            {
                LastReviewed = DateTime.Now.AddDays(-1),
                Interval = 3
            };

            var isDue = _spacedRepetition.IsDueForReview(card);

            Assert.False(isDue);
        }
    }
}
