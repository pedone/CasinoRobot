using CasinoRobot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoRobot.Betting
{
    public class RythmBetting : BettingModeBase
    {

        private BetViewModel _LastBet;

        private int _defaultBetAmount = 1;
        private int _betIndex = 0;

        public override void PlaceBets()
        {
            if (!Statistics.LastNumber.HasValue)
                return;

            RemoveBets();

            //Rythm
            //1xR - 1xB - 2xR - 2xB ...

            double betAmount = _defaultBetAmount;
            if (_LastBet != null && _LastBet.Result == Enums.BetResultKind.Loss)
                betAmount = _LastBet.Amount * 2;

            if (_betIndex == 0)
                _LastBet = PlaceBet(Enums.BettingKind.Red, amount: betAmount);
            else if (_betIndex == 1)
                _LastBet = PlaceBet(Enums.BettingKind.Black, amount: betAmount);
            else if (_betIndex == 2)
                _LastBet = PlaceBet(Enums.BettingKind.Red, amount: betAmount);
            else if (_betIndex == 3)
                _LastBet = PlaceBet(Enums.BettingKind.Red, amount: betAmount);
            else if (_betIndex == 4)
                _LastBet = PlaceBet(Enums.BettingKind.Black, amount: betAmount);
            else if (_betIndex == 5)
            {
                _LastBet = PlaceBet(Enums.BettingKind.Black, amount: betAmount);
                _betIndex = -1;
            }

            _betIndex++;
        }

        public override void CalculateWinnings(ViewModels.CasinoNumberViewModel drawnNumber)
        {
            if (_LastBet != null)
                CalculateWinningsOnBet(_LastBet, drawnNumber);
        }
    }
}
