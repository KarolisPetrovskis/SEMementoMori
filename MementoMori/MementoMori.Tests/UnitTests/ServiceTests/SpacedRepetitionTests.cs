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
        public void UpdateCard_ShouldIncreaseIntervalAndRepetitions_WhenQualityIsHigh()
        {
            // Arrange
            var card = new UserCardData
            {
                Repetitions = 1,
                Interval = 6,
                EaseFactor = 2.5,
                LastReviewed = DateTime.Now.AddDays(-7)
            };
            int quality = 4;

            // Act
            var updatedCard = _spacedRepetition.UpdateCard(card, quality);

            // Assert
            Assert.Equal(2, updatedCard.Repetitions);
            Assert.True(updatedCard.Interval > 6); // Interval should increase
            Assert.True(updatedCard.EaseFactor > 2.5); // Ease factor should increase
        }

        [Fact]
        public void UpdateCard_ShouldResetRepetitionsAndInterval_WhenQualityIsLow()
        {
            // Arrange
            var card = new UserCardData
            {
                Repetitions = 3,
                Interval = 10,
                EaseFactor = 2.5,
                LastReviewed = DateTime.Now.AddDays(-11)
            };
            int quality = 2;

            // Act
            var updatedCard = _spacedRepetition.UpdateCard(card, quality);

            // Assert
            Assert.Equal(0, updatedCard.Repetitions);
            Assert.Equal(1, updatedCard.Interval);
        }

        [Fact]
        public void UpdateCard_ShouldNotReduceEaseFactorBelowMinimum()
        {
            // Arrange
            var card = new UserCardData
            {
                Repetitions = 5,
                Interval = 15,
                EaseFactor = 1.4,
                LastReviewed = DateTime.Now.AddDays(-16)
            };
            int quality = 1;

            // Act
            var updatedCard = _spacedRepetition.UpdateCard(card, quality);

            // Assert
            Assert.Equal(1.3, updatedCard.EaseFactor); // Minimum ease factor
        }

        [Fact]
        public void IsDueForReview_ShouldReturnTrue_WhenCardIsDue()
        {
            // Arrange
            var card = new UserCardData
            {
                LastReviewed = DateTime.Now.AddDays(-5),
                Interval = 3
            };

            // Act
            var isDue = _spacedRepetition.IsDueForReview(card);

            // Assert
            Assert.True(isDue);
        }

        [Fact]
        public void IsDueForReview_ShouldReturnFalse_WhenCardIsNotDue()
        {
            // Arrange
            var card = new UserCardData
            {
                LastReviewed = DateTime.Now.AddDays(-1),
                Interval = 3
            };

            // Act
            var isDue = _spacedRepetition.IsDueForReview(card);

            // Assert
            Assert.False(isDue);
        }
    }
}
