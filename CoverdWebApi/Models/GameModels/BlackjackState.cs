using Coverd.Common.Enums;

namespace CoverdWebApi.Models.GameModels
{
    public class BlackjackState
    {
        public List<string> PlayerHand { get; set; } = new List<string>();
        public List<string> DealerHand { get; set; } = new List<string>();
        public int PlayerValue { get; set; }
        public int DealerValue { get; set; }
        public decimal Balance { get; set; }
        public bool IsGameOver { get; set; }
        public string GameResult { get; set; }
    }
}
