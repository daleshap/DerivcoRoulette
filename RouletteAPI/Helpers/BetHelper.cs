using Microsoft.AspNetCore.Mvc;
using RouletteAPI.Interfaces;
using RouletteAPI.Models;
using System.Data;
using System.Reflection;

namespace RouletteAPI.Helpers
{
    public class BetHelper: IBetHelper
    {

        public BetHelper()
        {
        }


        public List<Bet> MapDataTabletoBets(DataTable dt)
        {
            return dt.AsEnumerable().Select(row =>
                                                new Bet
                                                {
                                                    BetId = row.Field<int>("BetId"),
                                                    UserID = row.Field<int>("UserID"),
                                                    BetOnColorRed = row.Field<decimal>("BetOnColorRed"),
                                                    BetOnColorBlack = row.Field<decimal>("BetOnColorBlack"),
                                                    BetOnEven = row.Field<decimal>("BetOnEven"),
                                                    BetOnOdd = row.Field<decimal>("BetOnOdd"),
                                                    BetOnLow = row.Field<decimal>("BetOnLow"),
                                                    BetOnHigh = row.Field<decimal>("BetOnHigh"),
                                                    BetOnFirstDozen = row.Field<decimal>("BetOnFirstDozen"),
                                                    BetOnSecondDozen = row.Field<decimal>("BetOnSecondDozen"),
                                                    BetOnThirdDozen = row.Field<decimal>("BetOnThirdDozen"),
                                                    BetOnFirstColumn = row.Field<decimal>("BetOnFirstColumn"),
                                                    BetOnSecondColumn = row.Field<decimal>("BetOnSecondColumn"),
                                                    BetOnThirdColumn = row.Field<decimal>("BetOnThirdColumn"),
                                                    BetOnNumber0 = row.Field<decimal>("BetOnNumber0"),
                                                    BetOnNumber1 = row.Field<decimal>("BetOnNumber1"),
                                                    BetOnNumber2 = row.Field<decimal>("BetOnNumber2"),
                                                    BetOnNumber3 = row.Field<decimal>("BetOnNumber3"),
                                                    BetOnNumber4 = row.Field<decimal>("BetOnNumber4"),
                                                    BetOnNumber5 = row.Field<decimal>("BetOnNumber5"),
                                                    BetOnNumber6 = row.Field<decimal>("BetOnNumber6"),
                                                    BetOnNumber7 = row.Field<decimal>("BetOnNumber7"),
                                                    BetOnNumber8 = row.Field<decimal>("BetOnNumber8"),
                                                    BetOnNumber9 = row.Field<decimal>("BetOnNumber9"),
                                                    BetOnNumber10 = row.Field<decimal>("BetOnNumber10"),
                                                    BetOnNumber11 = row.Field<decimal>("BetOnNumber11"),
                                                    BetOnNumber12 = row.Field<decimal>("BetOnNumber12"),
                                                    BetOnNumber13 = row.Field<decimal>("BetOnNumber13"),
                                                    BetOnNumber14 = row.Field<decimal>("BetOnNumber14"),
                                                    BetOnNumber15 = row.Field<decimal>("BetOnNumber15"),
                                                    BetOnNumber16 = row.Field<decimal>("BetOnNumber16"),
                                                    BetOnNumber17 = row.Field<decimal>("BetOnNumber17"),
                                                    BetOnNumber18 = row.Field<decimal>("BetOnNumber18"),
                                                    BetOnNumber19 = row.Field<decimal>("BetOnNumber19"),
                                                    BetOnNumber20 = row.Field<decimal>("BetOnNumber20"),
                                                    BetOnNumber21 = row.Field<decimal>("BetOnNumber21"),
                                                    BetOnNumber22 = row.Field<decimal>("BetOnNumber22"),
                                                    BetOnNumber23 = row.Field<decimal>("BetOnNumber23"),
                                                    BetOnNumber24 = row.Field<decimal>("BetOnNumber24"),
                                                    BetOnNumber25 = row.Field<decimal>("BetOnNumber25"),
                                                    BetOnNumber26 = row.Field<decimal>("BetOnNumber26"),
                                                    BetOnNumber27 = row.Field<decimal>("BetOnNumber27"),
                                                    BetOnNumber28 = row.Field<decimal>("BetOnNumber28"),
                                                    BetOnNumber29 = row.Field<decimal>("BetOnNumber29"),
                                                    BetOnNumber30 = row.Field<decimal>("BetOnNumber30"),
                                                    BetOnNumber31 = row.Field<decimal>("BetOnNumber31"),
                                                    BetOnNumber32 = row.Field<decimal>("BetOnNumber32"),
                                                    BetOnNumber33 = row.Field<decimal>("BetOnNumber33"),
                                                    BetOnNumber34 = row.Field<decimal>("BetOnNumber34"),
                                                    BetOnNumber35 = row.Field<decimal>("BetOnNumber35"),
                                                    BetOnNumber36 = row.Field<decimal>("BetOnNumber36"),

                                                }).ToList();

        }
    }
}
