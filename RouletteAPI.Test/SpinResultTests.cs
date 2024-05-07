using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using RouletteAPI.Interfaces;
using RouletteAPI.Models;
using RouletteAPI.Repos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;

namespace RouletteAPI.Tests.Repos
{
    [TestFixture]
    public class SpinResultRepositoryTests
    {
        private SpinResultRepository _spinResultRepository;

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

                _spinResultRepository = new SpinResultRepository(configuration);

            }
            catch (Exception ex)
            {
                var x = ex.InnerException;
            }

        }

        [Test]
        public async Task AddSpinResultAsync_ValidResult_ReturnsSpinIdNumber()
        {
            // Arrange
            int expectedResult = 9; // Change this to the expected spinIdNumber

            // Act
            var result = await _spinResultRepository.AddSpinResultAsync(31);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        // Add similar tests for other methods (GetAllSpinResultsAsync, GetSpinResultAsync)...

        [TearDown]
        public void TearDown()
        {
            _spinResultRepository = null;
        }
    }
}
