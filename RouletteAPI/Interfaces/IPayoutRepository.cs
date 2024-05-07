using Microsoft.AspNetCore.Mvc;
using RouletteAPI.Models;
using System.Data;

namespace RouletteAPI.Interfaces
{
    public interface IPayoutRepository
    {
        Task AddPayoutAsync(Payout payout);
        Task<DataTable> GetAllPayoutsAsync();
        Task<DataTable> GetPayoutAsync(int payoutId);
    }

}
