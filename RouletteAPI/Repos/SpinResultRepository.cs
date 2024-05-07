using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RouletteAPI.Interfaces;
using RouletteAPI.Models;
using System.Data;

namespace RouletteAPI.Repos
{
    public class SpinResultRepository : ISpinResultRepository
    {

        private readonly IConfiguration _configuration;
        private readonly string _sqlDataSource;

        public SpinResultRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _sqlDataSource = _configuration.GetConnectionString("RouletteApp");
        }

        public async Task<int> AddSpinResultAsync(int result)
        {
            string query = "pr_AddSpinResult";
            int spinIdNumber = 0;
            using (SqlConnection conn = new SqlConnection(_sqlDataSource))
            {
                await conn.OpenAsync();
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Connection = conn;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Result", result);
                    spinIdNumber = int.Parse(command.ExecuteScalar().ToString());
                    
                    conn.Close();
                }
            }
            return spinIdNumber;
        }


        public async Task<DataTable> GetAllSpinResultsAsync()
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
            return table;
        }

        public async Task<DataTable> GetSpinResultAsync(int spinResultId)
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
            return table;
        }

    }
}
