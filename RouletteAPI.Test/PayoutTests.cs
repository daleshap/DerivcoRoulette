using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using RouletteAPI.Repos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace RouletteAPI.Test
{
    [TestFixture]
    public class PayoutRepositoryTests
    {
        private PayoutRepository _payoutRepository;

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

                _payoutRepository = new PayoutRepository(configuration);

            }
            catch (Exception ex)
            {
                var x = ex.InnerException;
            }

        }

        [Test]
        public async Task GetAllPayoutsAsync_ValidResult()
        {
            // Arrange

            // Act
            var result = await _payoutRepository.GetAllPayoutsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<DataTable>(result);
        }

        // Add similar tests for other methods (GetAllPayoutsAsync, GetPayoutAsync)...

        [TearDown]
        public void TearDown()
        {
            _payoutRepository = null;
        }
    }
}
