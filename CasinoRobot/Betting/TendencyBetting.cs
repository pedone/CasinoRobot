using CasinoRobot.Enums;
using CasinoRobot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasinoRobot.Betting
{
    public class TendencyBetting : BettingModeBase
    {
        private List<BetViewModel> _LastBets = new List<BetViewModel>();

        public override void PlaceBets()
        {
            RemoveBets();

            if (Settings.TendencyBettingSettings.ManualBetOnRed)
                _LastBets.Add(PlaceBet(BettingKind.Red));
            if (Settings.TendencyBettingSettings.ManualBetOnBlack)
                _LastBets.Add(PlaceBet(BettingKind.Black));

            if (Settings.TendencyBettingSettings.ManualBetOnEven)
                _LastBets.Add(PlaceBet(BettingKind.Even));
            if (Settings.TendencyBettingSettings.ManualBetOnOdd)
                _LastBets.Add(PlaceBet(BettingKind.Odd));

            if (Settings.TendencyBettingSettings.ManualBetOnTo18)
                _LastBets.Add(PlaceBet(BettingKind.To18));
            if (Settings.TendencyBettingSettings.ManualBetOnFrom19)
                _LastBets.Add(PlaceBet(BettingKind.From19));
        }

        public override void CalculateWinnings(ViewModels.CasinoNumberViewModel drawnNumber)
        {
            foreach (var bet in _LastBets)
                CalculateWinningsOnBet(bet, drawnNumber);

            _LastBets.Clear();
        }
    }
}
