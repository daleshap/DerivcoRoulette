using Microsoft.AspNetCore.Mvc;
using RouletteAPI.Models;
using System.Data;

namespace RouletteAPI.Interfaces
{
    public interface ISpinResultRepository
    {
        Task<int> AddSpinResultAsync(int result);
        Task<DataTable> GetAllSpinResultsAsync();
        Task<DataTable> GetSpinResultAsync(int spinResultId);
    }

}
