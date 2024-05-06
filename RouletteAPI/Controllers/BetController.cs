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
                    command.Parameters.AddWithValue("@BetOn0", bet.BetOn0);
                    command.Parameters.AddWithValue("@BetOn1", bet.BetOn1);
                    command.Parameters.AddWithValue("@BetOn2", bet.BetOn2);
                    command.Parameters.AddWithValue("@BetOn3", bet.BetOn3);
                    command.Parameters.AddWithValue("@BetOn4", bet.BetOn4);
                    command.Parameters.AddWithValue("@BetOn5", bet.BetOn5);
                    command.Parameters.AddWithValue("@BetOn6", bet.BetOn6);
                    command.Parameters.AddWithValue("@BetOn7", bet.BetOn7);
                    command.Parameters.AddWithValue("@BetOn8", bet.BetOn8);
                    command.Parameters.AddWithValue("@BetOn9", bet.BetOn9);
                    command.Parameters.AddWithValue("@BetOn10", bet.BetOn10);
                    command.Parameters.AddWithValue("@BetOn11", bet.BetOn11);
                    command.Parameters.AddWithValue("@BetOn12", bet.BetOn12);
                    command.Parameters.AddWithValue("@BetOn13", bet.BetOn13);
                    command.Parameters.AddWithValue("@BetOn14", bet.BetOn14);
                    command.Parameters.AddWithValue("@BetOn15", bet.BetOn15);
                    command.Parameters.AddWithValue("@BetOn16", bet.BetOn16);
                    command.Parameters.AddWithValue("@BetOn17", bet.BetOn17);
                    command.Parameters.AddWithValue("@BetOn18", bet.BetOn18);
                    command.Parameters.AddWithValue("@BetOn19", bet.BetOn19);
                    command.Parameters.AddWithValue("@BetOn20", bet.BetOn20);
                    command.Parameters.AddWithValue("@BetOn21", bet.BetOn21);
                    command.Parameters.AddWithValue("@BetOn22", bet.BetOn22);
                    command.Parameters.AddWithValue("@BetOn23", bet.BetOn23);
                    command.Parameters.AddWithValue("@BetOn24", bet.BetOn24);
                    command.Parameters.AddWithValue("@BetOn25", bet.BetOn25);
                    command.Parameters.AddWithValue("@BetOn26", bet.BetOn26);
                    command.Parameters.AddWithValue("@BetOn27", bet.BetOn27);
                    command.Parameters.AddWithValue("@BetOn28", bet.BetOn28);
                    command.Parameters.AddWithValue("@BetOn29", bet.BetOn29);
                    command.Parameters.AddWithValue("@BetOn30", bet.BetOn30);
                    command.Parameters.AddWithValue("@BetOn31", bet.BetOn31);
                    command.Parameters.AddWithValue("@BetOn32", bet.BetOn32);
                    command.Parameters.AddWithValue("@BetOn33", bet.BetOn33);
                    command.Parameters.AddWithValue("@BetOn34", bet.BetOn34);
                    command.Parameters.AddWithValue("@BetOn35", bet.BetOn35);
                    command.Parameters.AddWithValue("@BetOn36", bet.BetOn36);
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
