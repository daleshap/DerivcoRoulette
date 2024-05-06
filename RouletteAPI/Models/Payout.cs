namespace RouletteAPI.Models
{
    public class Payout
    {
        public int PayoutId { get; set; }
        public int BetId { get; set; }
        public int SpinIdNumber { get; set; } 
        public decimal PayoutAmount { get; set; }
    }
}
