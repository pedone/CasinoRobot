using CasinoRobot.Enums;
using CasinoRobot.Helpers;
using CasinoRobot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoRobot.Betting
{
    public class MartingaleBetting : BettingModeBase
    {

        private int _CurrentBettingStreakCount;

        public override bool IsInBettingStreak
        {
            get
            {
                return _CurrentBettingStreakCount > 0;
            }
        }

        private BetViewModel _LastBet;
        private BetViewModel LastBet
        {
            get
            {
                return _LastBet;
            }
            set
            {
                _LastBet = value;
                if (_LastBet == null)
                    _CurrentBettingStreakCount = 0;
            }
        }

        public override void ResetBettingStreak()
        {
            LastBet = null;
        }

        public override void PlaceBets()
        {
            RemoveBets();

            if (PlaceFollowUpBet())
                return;
            else if (!Session.IsPauseScheduled && !Session.IsStopScheduled)
                PlaceNewBet();
        }

        public override void CalculateWinnings(CasinoNumberViewModel drawnNumber)
        {
            if (CalculateWinningsOnBet(LastBet, drawnNumber) != BetResultKind.Loss)
                LastBet = null;
        }

        private bool PlaceFollowUpBet()
        {
            if (LastBet != null && LastBet.Result == BetResultKind.Win)
                LastBet = null;
            if (LastBet == null)
                return false;

            if (_CurrentBettingStreakCount < Settings.MaxBettingStreakLength)
            {
                //double up on last loss
                double newBetAmount = LastBet.Amount;
                if (Settings.DoubleUpOnLoss)
                    newBetAmount *= 2;

                int curStreakCount;
                int maxStreakCount;
                GetStreakCount(LastBet.BetKind, out curStreakCount, out maxStreakCount);

                LastBet = PlaceBet(LastBet.BetKind, curStreakCount, newBetAmount);
                LastBet.GroupId = LastBettingGroupId;
                _CurrentBettingStreakCount++;

                return true;
            }
            else
            {
                LastBet.Result = BetResultKind.Loss;
                LastBet = null;

                return false;
            }
        }

        private void PlaceNewBet()
        {
            //TODO calculate best bet count based on max streaks counts and stuff

            var bettingKind = BettingKind.None;
            if (IsValidBet(BettingKind.Red))
            {
                bettingKind = BettingKind.Red;
            }
            else if (IsValidBet(BettingKind.Black))
            {
                bettingKind = BettingKind.Black;
            }
            else if (IsValidBet(BettingKind.Odd))
            {
                bettingKind = BettingKind.Odd;
            }
            else if (IsValidBet(BettingKind.Even))
            {
                bettingKind = BettingKind.Even;
            }
            else if (IsValidBet(BettingKind.To18))
            {
                bettingKind = BettingKind.To18;
            }
            else if (IsValidBet(BettingKind.From19))
            {
                bettingKind = BettingKind.From19;
            }
            else if (IsValidBet(BettingKind.EvenAltOdd))
            {
                bettingKind = BettingKind.EvenAltOdd;
            }
            else if (IsValidBet(BettingKind.RedAltBlack))
            {
                bettingKind = BettingKind.RedAltBlack;
            }
            else if (IsValidBet(BettingKind.To18AltFrom19))
            {
                bettingKind = BettingKind.To18AltFrom19;
            }

            if (bettingKind != BettingKind.None)
            {
                int curStreakCount;
                int maxStreakCount;
                GetStreakCount(bettingKind, out curStreakCount, out maxStreakCount);

                LastBet = PlaceBet(bettingKind, curStreakCount);
                if (LastBet != null)
                    LastBet.GroupId = GetNewBettingGroupId();

                _CurrentBettingStreakCount = 1;
            }
        }

        private void GetStreakCount(BettingKind BettingKind, out int curStreakCount, out int maxStreakCount)
        {
            curStreakCount = 0;
            maxStreakCount = 0;
            switch (BettingKind)
            {
                case BettingKind.Black:
                    curStreakCount = Statistics.RedStreakCount;
                    maxStreakCount = Statistics.MaxRedStreakCount;
                    break;
                case BettingKind.Red:
                    curStreakCount = Statistics.BlackStreakCount;
                    maxStreakCount = Statistics.MaxBlackStreakCount;
                    break;
                case BettingKind.Odd:
                    curStreakCount = Statistics.EvenStreakCount;
                    maxStreakCount = Statistics.MaxEvenStreakCount;
                    break;
                case BettingKind.Even:
                    curStreakCount = Statistics.OddStreakCount;
                    maxStreakCount = Statistics.MaxOddStreakCount;
                    break;
                case BettingKind.To18:
                    curStreakCount = Statistics.From19StreakCount;
                    maxStreakCount = Statistics.MaxFrom19StreakCount;
                    break;
                case BettingKind.From19:
                    curStreakCount = Statistics.To18StreakCount;
                    maxStreakCount = Statistics.MaxTo18StreakCount;
                    break;
                case BettingKind.RedAltBlack:
                    curStreakCount = Statistics.AlternatingRedBlackStreakCount;
                    maxStreakCount = Statistics.MaxAlternatingRedBlackStreakCount;
                    break;
                case BettingKind.EvenAltOdd:
                    curStreakCount = Statistics.AlternatingOddEvenStreakCount;
                    maxStreakCount = Statistics.MaxAlternatingOddEvenStreakCount;
                    break;
                case BettingKind.To18AltFrom19:
                    curStreakCount = Statistics.Alternating1818StreakCount;
                    maxStreakCount = Statistics.MaxAlternating1818StreakCount;
                    break;
                default:
                    return;
            }
        }

        private bool IsValidBet(BettingKind BettingKind)
        {
            int curStreakCount;
            int maxStreakCount;
            GetStreakCount(BettingKind, out curStreakCount, out maxStreakCount);

            int maxBettingStreakCount = Settings.MaxBettingStreakCount;
            if (Settings.AutoAdjustBettingStreakCount && maxStreakCount > maxBettingStreakCount)
                maxBettingStreakCount = maxStreakCount;

            int minBettingOffset = Settings.MaxBettingStreakCount - Settings.MinBettingStreakCount;
            int minBettingStreakCount = maxBettingStreakCount - minBettingOffset;

            return curStreakCount >= minBettingStreakCount && curStreakCount <= maxBettingStreakCount;
        }

    }
}
