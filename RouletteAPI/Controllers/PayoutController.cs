using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using RouletteAPI.Models;
using System.Reflection;

namespace RouletteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayoutController : ControllerBase
    {
        private BetController _betController;
        private SpinResultController _spinResultController;
        private readonly IConfiguration _configuration;
        private string _sqlDataSource;

        public PayoutController(IConfiguration configuration)
        {
            _configuration = configuration;
            _sqlDataSource = _configuration.GetConnectionString("RouletteApp");
            _betController = new BetController(_configuration);
            _spinResultController = new SpinResultController(_configuration);
        }

        private List<Bet> GetBetsForSpin(int spinIdNumber)
        {
            var betsToCheckJson = _betController.GetBetsForSpin(spinIdNumber);
            return MapDataTabletoBets((DataTable)betsToCheckJson.Result.Value);
        }


        private SpinResult GetSpinResult(int spinIdNumber)
        {
            var spinResultJson = _spinResultController.GetSpinResult(spinIdNumber);
            var dt = (DataTable)spinResultJson.Result.Value;
            var spinResult = dt.AsEnumerable().Select(row =>
                                                new SpinResult
                                                {
                                                    SpinIdNumber = row.Field<int>("SpinIdNumber"),
                                                    Result = row.Field<int>("Result"),
                                                }).ToList();
            return spinResult.FirstOrDefault();
        }

        public async Task<JsonResult> CalculatePayoutTotal(int spinIdNumber)
        {
            var betsToCheck = GetBetsForSpin(spinIdNumber);
            var spinResult = GetSpinResult(spinIdNumber);
            var winningBetTypes = GetWinningBetTypes(spinResult.Result);

            try
            {

                // Get all properties of the Bet class
                PropertyInfo[] properties = typeof(Bet).GetProperties();
                foreach (Bet bet in betsToCheck)
                {

                    decimal payoutValue = 0m;
                    // Loop through each betType
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
                        _ = AddPayout(payout);
                    }
                }
                return new JsonResult("All Payouts Calculated");
            }
            catch (Exception ex)
            {
                return new JsonResult("Could not calculate payout" + ex);
            }
        }

        private decimal GetOdds(BetType betType)
        {
            if (betType == BetType.BetOnColorRed || betType == BetType.BetOnColorBlack || betType == BetType.BetOnEven || betType == BetType.BetOnOdd || betType == BetType.BetOnHigh || betType == BetType.BetOnLow)
                return 1;

            if (betType == BetType.BetOnFirstColumn || betType == BetType.BetOnSecondColumn || betType == BetType.BetOnThirdColumn|| betType == BetType.BetOnFirstDozen|| betType == BetType.BetOnSecondDozen|| betType == BetType.BetOnThirdDozen)
                return 2;

            //default
            return 35;                
            
        }

        private List<BetType> GetWinningBetTypes(int winningNumber)
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

    
        

        [HttpPost]
        public async Task<JsonResult> AddPayout(Payout payout)
        {
            string query = "pr_AddPayout";
            using (SqlConnection conn = new SqlConnection(_sqlDataSource))
            {
                await conn.OpenAsync();
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Connection = conn;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SpinIdNumber", payout.SpinIdNumber);
                    command.Parameters.AddWithValue("@BetId", payout.BetId);
                    command.Parameters.AddWithValue("@PayoutAmount", payout.PayoutAmount);
                    await command.ExecuteNonQueryAsync();
                    conn.Close();
                }
            }
            return new JsonResult("Payout Placed");
        }


        [HttpGet]
        public async Task<JsonResult> GetAllPayouts()
        {
            string query = "pr_GetAllPayouts";
            DataTable table = new DataTable();
            using (SqlConnection conn = new SqlConnection(_sqlDataSource))
            {
                await conn.OpenAsync();
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Connection = conn;
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        table.Load(dataReader);
                    }
                    conn.Close();
                }
            }

            return new JsonResult(table);
        }

        [Route("GetPayout")]
        [HttpGet]
        public async Task<JsonResult> GetPayout(int payoutId)
        {
            string query = "pr_GetPayout";
            DataTable table = new DataTable();
            using (SqlConnection conn = new SqlConnection(_sqlDataSource))
            {
                await conn.OpenAsync();
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Connection = conn;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PayoutId", payoutId);
                    using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        table.Load(dataReader);
                    }
                    conn.Close();
                }
            }

            return new JsonResult(table);
        }



        private List<Bet> MapDataTabletoBets(DataTable dt)
        {
            return dt.AsEnumerable().Select(row =>
                                                new Bet
                                                {
                                                    BetId = row.Field<int>("BetId"),
                                                    UserID = row.Field<int>("UserID"),
                                                    BetOnColorRed = row.Field<decimal>("BetOnColorRed"),
                                                    BetOnColorBlack = row.Field<decimal>("BetOnColorBlack"),
                                                    BetOnEven = row.Field<decimal>("BetOnEven"),
                                                    BetOnOdd = row.Field<decimal>("BetOnOdd"),
                                                    BetOnLow = row.Field<decimal>("BetOnLow"),
                                                    BetOnHigh = row.Field<decimal>("BetOnHigh"),
                                                    BetOnFirstDozen = row.Field<decimal>("BetOnFirstDozen"),
                                                    BetOnSecondDozen = row.Field<decimal>("BetOnSecondDozen"),
                                                    BetOnThirdDozen = row.Field<decimal>("BetOnThirdDozen"),
                                                    BetOnFirstColumn = row.Field<decimal>("BetOnFirstColumn"),
                                                    BetOnSecondColumn = row.Field<decimal>("BetOnSecondColumn"),
                                                    BetOnThirdColumn = row.Field<decimal>("BetOnThirdColumn"),
                                                    BetOnNumber0 = row.Field<decimal>("BetOnNumber0"),
                                                    BetOnNumber1 = row.Field<decimal>("BetOnNumber1"),
                                                    BetOnNumber2 = row.Field<decimal>("BetOnNumber2"),
                                                    BetOnNumber3 = row.Field<decimal>("BetOnNumber3"),
                                                    BetOnNumber4 = row.Field<decimal>("BetOnNumber4"),
                                                    BetOnNumber5 = row.Field<decimal>("BetOnNumber5"),
                                                    BetOnNumber6 = row.Field<decimal>("BetOnNumber6"),
                                                    BetOnNumber7 = row.Field<decimal>("BetOnNumber7"),
                                                    BetOnNumber8 = row.Field<decimal>("BetOnNumber8"),
                                                    BetOnNumber9 = row.Field<decimal>("BetOnNumber9"),
                                                    BetOnNumber10 = row.Field<decimal>("BetOnNumber10"),
                                                    BetOnNumber11 = row.Field<decimal>("BetOnNumber11"),
                                                    BetOnNumber12 = row.Field<decimal>("BetOnNumber12"),
                                                    BetOnNumber13 = row.Field<decimal>("BetOnNumber13"),
                                                    BetOnNumber14 = row.Field<decimal>("BetOnNumber14"),
                                                    BetOnNumber15 = row.Field<decimal>("BetOnNumber15"),
                                                    BetOnNumber16 = row.Field<decimal>("BetOnNumber16"),
                                                    BetOnNumber17 = row.Field<decimal>("BetOnNumber17"),
                                                    BetOnNumber18 = row.Field<decimal>("BetOnNumber18"),
                                                    BetOnNumber19 = row.Field<decimal>("BetOnNumber19"),
                                                    BetOnNumber20 = row.Field<decimal>("BetOnNumber20"),
                                                    BetOnNumber21 = row.Field<decimal>("BetOnNumber21"),
                                                    BetOnNumber22 = row.Field<decimal>("BetOnNumber22"),
                                                    BetOnNumber23 = row.Field<decimal>("BetOnNumber23"),
                                                    BetOnNumber24 = row.Field<decimal>("BetOnNumber24"),
                                                    BetOnNumber25 = row.Field<decimal>("BetOnNumber25"),
                                                    BetOnNumber26 = row.Field<decimal>("BetOnNumber26"),
                                                    BetOnNumber27 = row.Field<decimal>("BetOnNumber27"),
                                                    BetOnNumber28 = row.Field<decimal>("BetOnNumber28"),
                                                    BetOnNumber29 = row.Field<decimal>("BetOnNumber29"),
                                                    BetOnNumber30 = row.Field<decimal>("BetOnNumber30"),
                                                    BetOnNumber31 = row.Field<decimal>("BetOnNumber31"),
                                                    BetOnNumber32 = row.Field<decimal>("BetOnNumber32"),
                                                    BetOnNumber33 = row.Field<decimal>("BetOnNumber33"),
                                                    BetOnNumber34 = row.Field<decimal>("BetOnNumber34"),
                                                    BetOnNumber35 = row.Field<decimal>("BetOnNumber35"),
                                                    BetOnNumber36 = row.Field<decimal>("BetOnNumber36"),

                                                }).ToList();



        }


    }
}



//CalculateWinningsPerBet()
//CreditUserAccount()

/*
 * 
[Route("SanitizeText")]
[HttpPost]
public JsonResult SanitizeText([FromBody] string textToSanitize)
{
    try
    {
        DataTable dt = (DataTable)GetAllBets().Result.Value;


        List<Bet> bets = dt.AsEnumerable().Select(row =>
                                                                    new Bet
                                                                    {
                                                                        IdKey = row.Field<int>("idKey"),
                                                                        Word = row.Field<string>("word"),
                                                                        CaseSensitive = row.Field<bool>("caseSensitive"),
                                                                        WholeWordOnly = row.Field<bool>("wholeWordOnly"),
                                                                        TrimWord = row.Field<bool>("trimWord")

                                                                    }).ToList();

        string result = textToSanitize;
        Regex regex = new Regex(@"^[a-zA-Z0-9_[\])({}-]+$");
        foreach (Bet bet in bets.OrderByDescending(w => w.Word.Length))
        {
            string replacementString = bet.Word;
            if (bet.TrimWord)
            {
                replacementString = bet.Word.Trim();
            }
            if (bet.WholeWordOnly)
            {
                replacementString = @"\b" + replacementString + @"\b";
            }
            //Handle Special Characters here (this case is only *)
            if (replacementString.Contains("*"))
            {
                replacementString = replacementString.Replace("*", "\\*");
            }
            if (bet.CaseSensitive)
            {
                result = Regex.Replace(result, replacementString, new string('*', bet.Word.Length));
            }
            else
            {
                result = Regex.Replace(result, replacementString, new string('*', bet.Word.Length), RegexOptions.IgnoreCase);
            }

        }

        return new JsonResult(result);
    }
    catch (Exception ex)
    {
        //TODO: log exception ex 
        return new JsonResult("Could not sanitize string");

    }
}*/
