using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RouletteAPI.Interfaces;
using RouletteAPI.Models;
using System.Data;

namespace RouletteAPI.Repos
{
    public class PayoutRepository : IPayoutRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _sqlDataSource;

        public PayoutRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _sqlDataSource = _configuration.GetConnectionString("RouletteApp");
        }

        public async Task AddPayoutAsync(Payout payout)
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
        }

        public async Task<DataTable> GetAllPayoutsAsync()
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
            return table;
        }

        public async Task<DataTable> GetPayoutAsync(int payoutId)
        {
            string query = "pr_GetSinglePayout";
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
            return table;
        }

      
    }
}
