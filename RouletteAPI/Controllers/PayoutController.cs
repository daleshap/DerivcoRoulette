using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using RouletteAPI.Models;
using System.Reflection;
using RouletteAPI.Interfaces;

namespace RouletteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayoutController : ControllerBase
    {
        private readonly IPayoutRepository _payoutRepository;
        private readonly IPayoutHelper _payoutHelper;
        private readonly IConfiguration _configuration;
        public PayoutController(IPayoutRepository payoutRepository, IPayoutHelper payoutHelper)
        {
            _payoutRepository = payoutRepository;
            _payoutHelper = payoutHelper;
        }


        [Route("CalculateAndAddAllPayouts")]
        [HttpPost]
        public async Task<JsonResult> CalculatePayoutTotalAsync(int spinIdNumber)
        {
            await _payoutHelper.CalculatePayoutTotalAsync(spinIdNumber);
            return new JsonResult("All Payouts Calculated");
        }


        [Route("AddSinglePayout")]
        [HttpPost]
        public async Task<JsonResult> AddPayout(Payout payout)
        {
            await _payoutRepository.AddPayoutAsync(payout);
            return new JsonResult("Payout Placed");
        }


        [Route("GetAllPayouts")]
        [HttpGet]
        public async Task<JsonResult> GetAllPayouts()
        {
            DataTable payouts = await _payoutRepository.GetAllPayoutsAsync();
            return new JsonResult(payouts);
        }

        [Route("GetPayout")]
        [HttpGet]
        public async Task<JsonResult> GetPayout(int payoutId)
        {
            DataTable payout = await _payoutRepository.GetPayoutAsync(payoutId);
            return new JsonResult(payout);
        }


    }
}



//CalculateWinningsPerBet()
//CreditUserAccount()

