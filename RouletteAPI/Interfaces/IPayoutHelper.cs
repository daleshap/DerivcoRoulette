using Microsoft.AspNetCore.Mvc;
using RouletteAPI.Models;
using System.Data;

namespace RouletteAPI.Interfaces
{
    public interface IPayoutHelper
    {
        decimal GetOdds(BetType betType);
        List<BetType> GetWinningBetTypes(int winningNumber);
        Task<JsonResult> CalculatePayoutTotalAsync(int spinIdNumber);
    }

}
