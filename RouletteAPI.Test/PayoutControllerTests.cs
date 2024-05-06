using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using RouletteAPI.Controllers;
using RouletteAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace RouletteAPI.Tests.Controllers
{
    [TestFixture]
    public class PayoutControllerTests
    {
        private PayoutController _controller;
        private Mock<IConfiguration> _configurationMock;
        private string _testWord;
        private int _testWordId = 0;

        [SetUp]
        public void Setup()
        {
            try
            {
                var configValues = new Dictionary<string, string>
                {
                    { "ConnectionStrings:RouletteApp", "Data Source=.;Initial Catalog=RouletteApp;Integrated security=true;Encrypt=false;" }
                };
                var configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(configValues)
                    .Build();

                _controller = new PayoutController(configuration);


                _testWord = Guid.NewGuid().ToString();
            }
            catch (Exception ex)
            {
                var x = ex.InnerException;
            }
        }
        
        [Test]
        public async Task TestCalculatePayout()
        {
            // Arrange
            

            // Act
            var result = await _controller.CalculatePayoutTotal(2);

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
            var jsonResult = (JsonResult)result;
            Assert.AreEqual($"{_testWord} added to list of banned words", jsonResult.Value);
        }

      

        /*
        [Test]
        foreach(


        */
    }
}