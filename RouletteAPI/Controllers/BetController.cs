using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using RouletteAPI.Models;
using System.Text.RegularExpressions;
using RouletteAPI.Interfaces;

namespace RouletteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetController : ControllerBase
    {
        private readonly IBetRepository _betRepository;
        private readonly IBetHelper _betHelper;
        public BetController(IBetRepository betRepository, IBetHelper betHelper)
        {
            _betRepository = betRepository;
            _betHelper = betHelper;
        }
        [Route("AddBet")]
        [HttpPost]
        public async Task<JsonResult> AddBet(Bet bet)
        {
            var result = await _betRepository.AddBetAsync(bet);
            return new JsonResult(result);
        }

        [Route("GetBetsForSpin")]
        [HttpGet]
        public async Task<JsonResult> GetBetsForSpin(int spinIdNumber)
        {
            DataTable betTable = await _betRepository.GetBetsForSpinAsync(spinIdNumber);
            return new JsonResult(betTable);

        }

        [Route("GetAllBets")]
        [HttpGet]
        public async Task<JsonResult> GetAllBets()
        {
            DataTable betTable = await _betRepository.GetAllBetsAsync();
            return new JsonResult(betTable);
        }

        [Route("GetUserBets")]
        [HttpGet]
        public async Task<JsonResult> GetBetForUser(int userId)
        {
            DataTable betTable = await _betRepository.GetBetsForUser(userId);
            return new JsonResult(betTable);
        }

        [Route("GetSingleBet")]
        [HttpGet]
        public async Task<JsonResult> GetSingleBet(int betId)
        {
            DataTable betTable = await _betRepository.GetSingleBetAsync(betId);
            return new JsonResult(betTable);
        }


    }
}
