using Microsoft.AspNetCore.Mvc;
using RouletteAPI.Interfaces;
using RouletteAPI.Models;
using System.Data;
using System.Reflection;

namespace RouletteAPI.Helpers
{
    public class SpinResultHelper : ISpinResultHelper
    {

        public SpinResultHelper()
        {
        }

        public int SpinTheWheel()
        {
            return new Random().Next(0, 37);
        }

        public SpinResult MapDataTabletoSpinResult(DataTable dt)
        {
            return dt.AsEnumerable().Select(row =>
                                                new SpinResult
                                                {
                                                    SpinIdNumber = row.Field<int>("SpinIdNumber"),
                                                    Result = row.Field<int>("result")}).FirstOrDefault() ?? new SpinResult();

        }
    }
}
