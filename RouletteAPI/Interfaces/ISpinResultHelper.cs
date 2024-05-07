using Microsoft.AspNetCore.Mvc;
using RouletteAPI.Models;
using System.Data;

namespace RouletteAPI.Interfaces
{
    public interface ISpinResultHelper
    {
        SpinResult MapDataTabletoSpinResult(DataTable table);
        int SpinTheWheel();
    }

}
