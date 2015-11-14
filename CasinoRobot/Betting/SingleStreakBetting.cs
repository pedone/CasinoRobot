using CasinoRobot.Enums;
using CasinoRobot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoRobot.Betting
{
    public class SingleStreakBetting : BettingModeBase
    {

        private List<BetViewModel> _LastBets = new List<BetViewModel>();

        public override void PlaceBets()
        {
            RemoveBets();

            if (Settings.SingleStreakBettingSettings.BetOnRedAndBlack)
            {
                if (IsValidBet(BettingKind.Red))
                    _LastBets.Add(PlaceBet(BettingKind.Red));
                if (IsValidBet(BettingKind.Black))
                    _LastBets.Add(PlaceBet(BettingKind.Black));
            }

            if (Settings.SingleStreakBettingSettings.BetOnEvenAndOdd)
            {
                if (IsValidBet(BettingKind.Odd))
                    _LastBets.Add(PlaceBet(BettingKind.Odd));
                if (IsValidBet(BettingKind.Even))
                    _LastBets.Add(PlaceBet(BettingKind.Even));
            }


            if (Settings.SingleStreakBettingSettings.BetOnTo18AndFrom19)
            {
                if (IsValidBet(BettingKind.To18))
                    _LastBets.Add(PlaceBet(BettingKind.To18));
                if (IsValidBet(BettingKind.From19))
                    _LastBets.Add(PlaceBet(BettingKind.From19));
            }

            var groupId = GetNewBettingGroupId();
            foreach (var bet in _LastBets)
                bet.GroupId = groupId;
        }
        private bool IsValidBet(BettingKind betKind)
        {
            int curStreakCount = 0;
            switch (betKind)
            {
                case BettingKind.Black:
                    curStreakCount = Statistics.RedStreakCount;
                    break;
                case BettingKind.Red:
                    curStreakCount = Statistics.BlackStreakCount;
                    break;
                case BettingKind.Odd:
                    curStreakCount = Statistics.EvenStreakCount;
                    break;
                case BettingKind.Even:
                    curStreakCount = Statistics.OddStreakCount;
                    break;
                case BettingKind.To18:
                    curStreakCount = Statistics.From19StreakCount;
                    break;
                case BettingKind.From19:
                    curStreakCount = Statistics.To18StreakCount;
                    break;
                default:
                    return false;
            }

            return Settings.SingleStreakBettingSettings.StreakCounts.Any(cur => cur == curStreakCount);
        }

        public override void CalculateWinnings(ViewModels.CasinoNumberViewModel drawnNumber)
        {
            foreach (var bet in _LastBets)
                CalculateWinningsOnBet(bet, drawnNumber);

            _LastBets.Clear();
        }
    }
}
