using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using RouletteAPI.Controllers;
using RouletteAPI.Models;
using Microsoft.Extensions.Configuration;
using RouletteAPI.Interfaces;
using RouletteAPI.Helpers;

namespace RouletteAPI.Tests.Controllers
{
    [TestFixture]
    public class PayoutControllerTests
    {
        private PayoutController _controller;
        private Mock<IPayoutRepository> _payoutRepositoryMock;
        private Mock<IPayoutHelper> _payoutHelperMock;

        [SetUp]
        public void Setup()
        {
            _payoutRepositoryMock = new Mock<IPayoutRepository>();
            _payoutHelperMock = new Mock<IPayoutHelper>();
            _controller = new PayoutController(_payoutRepositoryMock.Object, _payoutHelperMock.Object);
        }

        [Test]
        public async Task TestCalculatePayout()
        {
            // Arrange
            int spinIdNumber = 2;
            var x = _payoutHelperMock.Setup(x => x.CalculatePayoutTotalAsync(spinIdNumber));

            // Act
            var result = await _controller.CalculatePayoutTotalAsync(spinIdNumber);

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
            var jsonResult = (JsonResult)result;
            Assert.AreEqual("All Payouts Calculated", jsonResult.Value);
        }
    }
}
