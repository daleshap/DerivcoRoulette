using Microsoft.AspNetCore.Mvc;
using RouletteAPI.Interfaces;
using RouletteAPI.Models;
using System.Data;
using System.Reflection;

namespace RouletteAPI.Helpers
{
    public class PayoutHelper : IPayoutHelper
    {
        private readonly IBetHelper _betHelper;
        private readonly ISpinResultHelper _spinResultHelper;
        private readonly IPayoutRepository _payoutRepository;
        private readonly ISpinResultRepository _spinResultRepository;
        private readonly IBetRepository _betRepository;

        public PayoutHelper(IBetHelper betHelper, ISpinResultHelper spinResultHelper, IPayoutRepository payoutRepository, ISpinResultRepository spinResultRepository, IBetRepository betRepository)
        {
            _betHelper = betHelper;
            _spinResultHelper = spinResultHelper;
            _payoutRepository = payoutRepository;
            _spinResultRepository = spinResultRepository;
            _betRepository = betRepository;
        }

        public async Task<JsonResult> CalculatePayoutTotalAsync(int spinIdNumber)
        {
            var betsToCheck = _betHelper.MapDataTabletoBets(_betRepository.GetBetsForSpinAsync(spinIdNumber).Result);
            var spinResult = _spinResultHelper.MapDataTabletoSpinResult(_spinResultRepository.GetSpinResultAsync(spinIdNumber).Result);
            var winningBetTypes = GetWinningBetTypes(spinResult.Result);

            try
            {
                PropertyInfo[] properties = typeof(Bet).GetProperties();
                foreach (Bet bet in betsToCheck)
                {
                    decimal payoutValue = 0m;
                    foreach (var property in properties)
                    {
                        BetType betType;
                        if (Enum.TryParse(property.Name, out betType))
                        {
                            if (winningBetTypes.Contains(betType))
                            {
                                object propertyValue = property.GetValue(bet);
                                decimal betAmount = Convert.ToDecimal(propertyValue);
                                if (betAmount > 0)
                                {
                                    payoutValue += betAmount + (betAmount * GetOdds(betType));
                                }
                            }
                        }
                    }

                    if (payoutValue > 0)
                    {
                        Payout payout = new Payout() { BetId = bet.BetId, SpinIdNumber = spinIdNumber, PayoutAmount = payoutValue };
                        await _payoutRepository.AddPayoutAsync(payout);
                    }
                }
                return new JsonResult("All Payouts Calculated");
            }
            catch (Exception ex)
            {
                return new JsonResult("Could not calculate payout" + ex);
            }
        }


        public decimal GetOdds(BetType betType)
        {
            if (betType == BetType.BetOnColorRed || betType == BetType.BetOnColorBlack || betType == BetType.BetOnEven || betType == BetType.BetOnOdd || betType == BetType.BetOnHigh || betType == BetType.BetOnLow)
                return 1;

            if (betType == BetType.BetOnFirstColumn || betType == BetType.BetOnSecondColumn || betType == BetType.BetOnThirdColumn || betType == BetType.BetOnFirstDozen || betType == BetType.BetOnSecondDozen || betType == BetType.BetOnThirdDozen)
                return 2;

            //default
            return 35;

        }

        public List<BetType> GetWinningBetTypes(int winningNumber)
        {
            var winningBetTypes = new List<BetType>();
            PropertyInfo[] properties = typeof(Bet).GetProperties();
            BetType winningNumberBetType;

            if (Enum.TryParse(string.Concat("BetOnNumber", winningNumber.ToString()), false, out winningNumberBetType))
            {
                winningBetTypes.Add(winningNumberBetType);
            }
            else
            {
                return winningBetTypes;
                //log here could not set winning number
            }

            //Side bets
            if (winningNumber % 2 != 0)
                winningBetTypes.Add(BetType.BetOnColorBlack);

            if (winningNumber % 2 == 0 && winningNumber != 0)
                winningBetTypes.Add(BetType.BetOnColorRed);

            if (winningNumber % 2 == 0 && winningNumber != 0)
                winningBetTypes.Add(BetType.BetOnEven);

            if (winningNumber % 2 != 0)
                winningBetTypes.Add(BetType.BetOnOdd);

            if (winningNumber >= 1 && winningNumber <= 18)
                winningBetTypes.Add(BetType.BetOnLow);

            if (winningNumber >= 19 && winningNumber <= 36)
                winningBetTypes.Add(BetType.BetOnHigh);

            if (winningNumber >= 1 && winningNumber <= 12)
                winningBetTypes.Add(BetType.BetOnFirstDozen);

            if (winningNumber >= 13 && winningNumber <= 24)
                winningBetTypes.Add(BetType.BetOnSecondDozen);

            if (winningNumber >= 25 && winningNumber <= 36)
                winningBetTypes.Add(BetType.BetOnThirdDozen);

            if (winningNumber % 3 == 1 && winningNumber != 0)
                winningBetTypes.Add(BetType.BetOnFirstColumn);

            if (winningNumber % 3 == 2 && winningNumber != 0)
                winningBetTypes.Add(BetType.BetOnSecondColumn);

            if (winningNumber % 3 == 0 && winningNumber != 0)
                winningBetTypes.Add(BetType.BetOnThirdColumn);


            return winningBetTypes;
        }

    }
}
