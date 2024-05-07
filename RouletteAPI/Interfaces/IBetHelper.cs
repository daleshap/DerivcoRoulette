using Microsoft.AspNetCore.Mvc;
using RouletteAPI.Models;
using System.Data;

namespace RouletteAPI.Interfaces
{
    public interface IBetHelper
    {
        List<Bet> MapDataTabletoBets(DataTable table);
    }

}
