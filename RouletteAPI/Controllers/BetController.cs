using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using RouletteAPI.Models;
using System.Text.RegularExpressions;

namespace RouletteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private string _sqlDataSource;
        public BetController(IConfiguration configuration)
        {
            _configuration = configuration;
            _sqlDataSource = _configuration.GetConnectionString("RouletteApp");
        }
        [HttpPost]
        public async Task<JsonResult> CreateBet(Bet bet)
        {
            string query = "pr_CreateBet";
            using (SqlConnection conn = new SqlConnection(_sqlDataSource))
            {
                await conn.OpenAsync();
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Connection = conn;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", bet.UserID);
                    command.Parameters.AddWithValue("@BetOnNumber0", bet.BetOnNumber0);
                    command.Parameters.AddWithValue("@BetOnNumber1", bet.BetOnNumber1);
                    command.Parameters.AddWithValue("@BetOnNumber2", bet.BetOnNumber2);
                    command.Parameters.AddWithValue("@BetOnNumber3", bet.BetOnNumber3);
                    command.Parameters.AddWithValue("@BetOnNumber4", bet.BetOnNumber4);
                    command.Parameters.AddWithValue("@BetOnNumber5", bet.BetOnNumber5);
                    command.Parameters.AddWithValue("@BetOnNumber6", bet.BetOnNumber6);
                    command.Parameters.AddWithValue("@BetOnNumber7", bet.BetOnNumber7);
                    command.Parameters.AddWithValue("@BetOnNumber8", bet.BetOnNumber8);
                    command.Parameters.AddWithValue("@BetOnNumber9", bet.BetOnNumber9);
                    command.Parameters.AddWithValue("@BetOnNumber10", bet.BetOnNumber10);
                    command.Parameters.AddWithValue("@BetOnNumber11", bet.BetOnNumber11);
                    command.Parameters.AddWithValue("@BetOnNumber12", bet.BetOnNumber12);
                    command.Parameters.AddWithValue("@BetOnNumber13", bet.BetOnNumber13);
                    command.Parameters.AddWithValue("@BetOnNumber14", bet.BetOnNumber14);
                    command.Parameters.AddWithValue("@BetOnNumber15", bet.BetOnNumber15);
                    command.Parameters.AddWithValue("@BetOnNumber16", bet.BetOnNumber16);
                    command.Parameters.AddWithValue("@BetOnNumber17", bet.BetOnNumber17);
                    command.Parameters.AddWithValue("@BetOnNumber18", bet.BetOnNumber18);
                    command.Parameters.AddWithValue("@BetOnNumber19", bet.BetOnNumber19);
                    command.Parameters.AddWithValue("@BetOnNumber20", bet.BetOnNumber20);
                    command.Parameters.AddWithValue("@BetOnNumber21", bet.BetOnNumber21);
                    command.Parameters.AddWithValue("@BetOnNumber22", bet.BetOnNumber22);
                    command.Parameters.AddWithValue("@BetOnNumber23", bet.BetOnNumber23);
                    command.Parameters.AddWithValue("@BetOnNumber24", bet.BetOnNumber24);
                    command.Parameters.AddWithValue("@BetOnNumber25", bet.BetOnNumber25);
                    command.Parameters.AddWithValue("@BetOnNumber26", bet.BetOnNumber26);
                    command.Parameters.AddWithValue("@BetOnNumber27", bet.BetOnNumber27);
                    command.Parameters.AddWithValue("@BetOnNumber28", bet.BetOnNumber28);
                    command.Parameters.AddWithValue("@BetOnNumber29", bet.BetOnNumber29);
                    command.Parameters.AddWithValue("@BetOnNumber30", bet.BetOnNumber30);
                    command.Parameters.AddWithValue("@BetOnNumber31", bet.BetOnNumber31);
                    command.Parameters.AddWithValue("@BetOnNumber32", bet.BetOnNumber32);
                    command.Parameters.AddWithValue("@BetOnNumber33", bet.BetOnNumber33);
                    command.Parameters.AddWithValue("@BetOnNumber34", bet.BetOnNumber34);
                    command.Parameters.AddWithValue("@BetOnNumber35", bet.BetOnNumber35);
                    command.Parameters.AddWithValue("@BetOnNumber36", bet.BetOnNumber36);
                    command.Parameters.AddWithValue("@BetOnColorRed", bet.BetOnColorRed);
                    command.Parameters.AddWithValue("@BetOnColorBlack", bet.BetOnColorBlack);
                    command.Parameters.AddWithValue("@BetOnEven", bet.BetOnEven);
                    command.Parameters.AddWithValue("@BetOnOdd", bet.BetOnOdd);
                    command.Parameters.AddWithValue("@BetOnLow", bet.BetOnLow);
                    command.Parameters.AddWithValue("@BetOnHigh", bet.BetOnHigh);
                    command.Parameters.AddWithValue("@BetOnFirstDozen", bet.BetOnFirstDozen);
                    command.Parameters.AddWithValue("@BetOnSecondDozen", bet.BetOnSecondDozen);
                    command.Parameters.AddWithValue("@BetOnThirdDozen", bet.BetOnThirdDozen);
                    command.Parameters.AddWithValue("@BetOnFirstColumn", bet.BetOnFirstColumn);
                    command.Parameters.AddWithValue("@BetOnSecondColumn", bet.BetOnSecondColumn);
                    command.Parameters.AddWithValue("@BetOnThirdColumn", bet.BetOnThirdColumn);
                    await command.ExecuteNonQueryAsync();
                    conn.Close();
                }
            }
            return new JsonResult("Bet Placed");
        }

        [Route("GetBetsForSpin")]
        [HttpGet]
        public async Task<JsonResult> GetBetsForSpin(int spinIdNumber)
        {
            string query = "pr_GetBetsForSpin";
            DataTable table = new DataTable();
            using (SqlConnection conn = new SqlConnection(_sqlDataSource))
            {
                await conn.OpenAsync();
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Connection = conn;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SpinIdNumber", spinIdNumber);
                    using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        table.Load(dataReader);
                    }
                    conn.Close();
                }
            }

            return new JsonResult(table);

        }

        [HttpGet]
        public async Task<JsonResult> GetAllBets()
        {
            string query = "pr_GetAllBets";
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

        [Route("GetUserBets")]
        [HttpGet]
        public async Task<JsonResult> GetBetForUser(int userId)
        {
            string query = "pr_GetBetsForUser";
            DataTable table = new DataTable();
            using (SqlConnection conn = new SqlConnection(_sqlDataSource))
            {
                await conn.OpenAsync();
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Connection = conn;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userId);
                    using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        table.Load(dataReader);
                    }
                    conn.Close();
                }
            }

            return new JsonResult(table);
        }

        [Route("GetSingleBet")]
        [HttpGet]
        public async Task<JsonResult> GetSingleBet(int betId)
        {
            string query = "pr_GetSingleBet";
            DataTable table = new DataTable();
            using (SqlConnection conn = new SqlConnection(_sqlDataSource))
            {
                await conn.OpenAsync();
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Connection = conn;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BetId", betId);
                    using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        table.Load(dataReader);
                    }
                    conn.Close();
                }
            }

            return new JsonResult(table);
        }






    }
}
