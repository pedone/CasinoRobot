using CasinoRobot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoRobot.Betting
{
    public class JustLastNumberBetting : BettingModeBase
    {

        private NumberBetViewModel _LastBet;

        public override void PlaceBets()
        {
            RemoveBets();

            if (Statistics.LastNumber.HasValue)
                _LastBet = PlaceBetOnNumber(Statistics.LastNumber.Value);
        }

        public override void CalculateWinnings(ViewModels.CasinoNumberViewModel drawnNumber)
        {
            if (_LastBet != null)
                CalculateWinningsOnBet(_LastBet, drawnNumber);
        }
    }
}
