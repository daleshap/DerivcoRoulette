using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using RouletteAPI.Models;
using RouletteAPI.Repos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace RouletteAPI.Test
{
    [TestFixture]
    public class BetTests
    {
        private BetRepository _betRepository;

        [SetUp]
        public void Setup()
        {
            try
            {
                var configValues = new Dictionary<string, string>
            {
                { "ConnectionStrings:RouletteApp", "Data Source=.;Initial Catalog=RouletteAppTest;Integrated security=true;Encrypt=false;" }
            };
                var configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(configValues)
                    .Build();

                _betRepository = new BetRepository(configuration);

            }
            catch (Exception ex)
            {
                var x = ex.InnerException;
            }
        }

        [Test]
        public async Task AddBetAsync_ValidBet_ReturnsBetId()
        {
            // Arrange
            var expectedResult = "Bet Placed Successfully"; 

            // Act
            var result = await _betRepository.AddBetAsync(new Bet() { UserID = 1 });

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public async Task GetAllBetsAsync_ReturnsDataTable()
        {
            // Act
            var result = await _betRepository.GetAllBetsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<DataTable>(result);
        }

        [Test]
        public async Task GetBetsForSpinAsync_ValidSpinId_ReturnsDataTable()
        {
            // Act
            var result = await _betRepository.GetBetsForSpinAsync(1); // Change this to a valid spinId

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<DataTable>(result);
        }

        [Test]
        public async Task GetBetsForUserAsync_ValidUserId_ReturnsDataTable()
        {
            // Act
            var result = await _betRepository.GetBetsForUserAsync(1); // Change this to a valid userId

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<DataTable>(result);
        }

        [Test]
        public async Task GetSingleBetAsync_ValidBetId_ReturnsDataTable()
        {
            // Act
            var result = await _betRepository.GetSingleBetAsync(1); // Change this to a valid betId

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<DataTable>(result);
        }

        [TearDown]
        public void TearDown()
        {
            _betRepository = null;
        }
    }

}
