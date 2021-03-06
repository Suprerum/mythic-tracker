﻿using MythicTracker.Application.GameDatabase;
using System.Threading.Tasks;
using Xunit;

namespace MythicTracker.Application.Tests
{
    public class GameCardsInfoDatabaseProviderTests
    {
        private readonly FileCardsProvider _cardsProvider = new FileCardsProvider("./GameDatabase/database.json");

        [Fact]
        public async Task ShouldGetOneCard()
        {
            var cardTest = _cardsProvider.GetCard(6873);

            Assert.Equal(6873, cardTest.Id);
            Assert.Equal("Crash of Rhinos", cardTest.Name);
        }

        [Fact]
        public async Task ShouldGetNullWithNullId()
        {
            var cardTest = _cardsProvider.GetCard(0);

            Assert.Null(cardTest);
        }

        [Fact]
        public async Task ShouldGetAllCards()
        {
            Card[] allCards = _cardsProvider.GetAllCards();

            Assert.Equal(2882, allCards.Length);
        }
    }
}
