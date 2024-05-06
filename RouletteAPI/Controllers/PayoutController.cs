using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using RouletteAPI.Models;
using System.Text.RegularExpressions;
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
                        if (property.PropertyType == typeof(decimal) && (decimal)property.GetValue(this) != 0)
                        {
                            if (Enum.TryParse(property.Name, out betType))
                            {
                                payoutValue += (decimal)property.GetValue(this) + (decimal)property.GetValue(this) * (int)betType;
                            }
                        }
                        else
                        {
                            return new JsonResult("Could not calculate payout");
                        }
                    }
                    Payout payout = new Payout() { BetId = bet.BetId, SpinIdNumber = spinIdNumber, PayoutAmount = payoutValue};
                    _ = AddPayout(payout);
                }
            }
            catch (Exception ex)
            {
                return new JsonResult("Could not calculate payout" + ex);
            }
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
                                                    BetOn0 = row.Field<decimal>("BetOn0"),
                                                    BetOn1 = row.Field<decimal>("BetOn1"),
                                                    BetOn2 = row.Field<decimal>("BetOn2"),
                                                    BetOn3 = row.Field<decimal>("BetOn3"),
                                                    BetOn4 = row.Field<decimal>("BetOn4"),
                                                    BetOn5 = row.Field<decimal>("BetOn5"),
                                                    BetOn6 = row.Field<decimal>("BetOn6"),
                                                    BetOn7 = row.Field<decimal>("BetOn7"),
                                                    BetOn8 = row.Field<decimal>("BetOn8"),
                                                    BetOn9 = row.Field<decimal>("BetOn9"),
                                                    BetOn10 = row.Field<decimal>("BetOn10"),
                                                    BetOn11 = row.Field<decimal>("BetOn11"),
                                                    BetOn12 = row.Field<decimal>("BetOn12"),
                                                    BetOn13 = row.Field<decimal>("BetOn13"),
                                                    BetOn14 = row.Field<decimal>("BetOn14"),
                                                    BetOn15 = row.Field<decimal>("BetOn15"),
                                                    BetOn16 = row.Field<decimal>("BetOn16"),
                                                    BetOn17 = row.Field<decimal>("BetOn17"),
                                                    BetOn18 = row.Field<decimal>("BetOn18"),
                                                    BetOn19 = row.Field<decimal>("BetOn19"),
                                                    BetOn20 = row.Field<decimal>("BetOn20"),
                                                    BetOn21 = row.Field<decimal>("BetOn21"),
                                                    BetOn22 = row.Field<decimal>("BetOn22"),
                                                    BetOn23 = row.Field<decimal>("BetOn23"),
                                                    BetOn24 = row.Field<decimal>("BetOn24"),
                                                    BetOn25 = row.Field<decimal>("BetOn25"),
                                                    BetOn26 = row.Field<decimal>("BetOn26"),
                                                    BetOn27 = row.Field<decimal>("BetOn27"),
                                                    BetOn28 = row.Field<decimal>("BetOn28"),
                                                    BetOn29 = row.Field<decimal>("BetOn29"),
                                                    BetOn30 = row.Field<decimal>("BetOn30"),
                                                    BetOn31 = row.Field<decimal>("BetOn31"),
                                                    BetOn32 = row.Field<decimal>("BetOn32"),
                                                    BetOn33 = row.Field<decimal>("BetOn33"),
                                                    BetOn34 = row.Field<decimal>("BetOn34"),
                                                    BetOn35 = row.Field<decimal>("BetOn35"),
                                                    BetOn36 = row.Field<decimal>("BetOn36"),

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
