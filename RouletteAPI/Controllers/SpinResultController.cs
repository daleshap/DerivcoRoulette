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
    public class SpinResultController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private string _sqlDataSource;
        public SpinResultController(IConfiguration configuration)
        {
            _configuration = configuration;
            _sqlDataSource = _configuration.GetConnectionString("RouletteApp");
        }
        public async Task<JsonResult> SpinTheWheel()
        {
            int result = new Random().Next(0,37);
            return await AddSpinResult(result);
        }

        [HttpPost]
        public async Task<JsonResult> AddSpinResult(int result)
        {
            string query = "pr_AddSpinResult";
            using (SqlConnection conn = new SqlConnection(_sqlDataSource))
            {
                await conn.OpenAsync();
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Connection = conn;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Result", result);
                    await command.ExecuteNonQueryAsync();
                    conn.Close();
                }
            }
            return new JsonResult("SpinResult Placed");
        }


        [HttpGet]
        public async Task<JsonResult> GetAllSpinResults()
        {
            string query = "pr_GetAllSpinResults";
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

        [Route("GetSpinResult")]
        [HttpGet]
        public async Task<JsonResult> GetSpinResult(int spinResultId)
        {
            string query = "pr_GetSpinResult";
            DataTable table = new DataTable();
            using (SqlConnection conn = new SqlConnection(_sqlDataSource))
            {
                await conn.OpenAsync();
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Connection = conn;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SpinIdNumber", spinResultId);
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
