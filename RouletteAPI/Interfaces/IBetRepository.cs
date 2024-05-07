using Microsoft.AspNetCore.Mvc;
using RouletteAPI.Models;
using System.Data;

namespace RouletteAPI.Interfaces
{
    public interface IBetRepository
    {
        Task<DataTable> GetBetsForSpinAsync(int spinId);
        Task<string> AddBetAsync(Bet bet);
        Task<DataTable> GetAllBetsAsync();
        Task<DataTable> GetBetsForUserAsync(int userId);
        Task<DataTable> GetSingleBetAsync(int betId);

    }

}
