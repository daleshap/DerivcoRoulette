using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using RouletteAPI.Helpers;
using RouletteAPI.Repos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RouletteAPI.Test
{
    [TestFixture]
    public class SpinResultRepositoryTests
    {
        private SpinResultRepository _spinResultRepository;
        private SpinResultHelper _spinResultHelper;

        [SetUp]
        public void Setup()
        {
            try
            {
                _spinResultHelper = new SpinResultHelper();
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
            var latestSpin = _spinResultHelper.MapDataTabletoSpinResult(_spinResultRepository.GetLatestSpinResultAsync().Result);
            int expectedResult = latestSpin.SpinIdNumber +1; // Change this to the dynamic spinIdNumber

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
