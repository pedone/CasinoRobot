using CasinoRobot.Enums;
using CasinoRobot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasinoRobot.Betting
{
    public class NumberNegligenceBetting : BettingModeBase
    {
        private List<NumberBetViewModel> _LastBets = new List<NumberBetViewModel>();
        private int _waitingOnRepititionCount;
        private int WaitingOnRepetitionCount
        {
            get { return _waitingOnRepititionCount; }
            set
            {
                _waitingOnRepititionCount = value;
                if (_waitingOnRepititionCount == 0)
                {
                    _repetingNumbers.Clear();
                    _repetitionLossCount = 0;
                }
            }
        }
        private List<int> _repetingNumbers = new List<int>();
        private int _repetitionLossCount;

        public override bool IsInBettingStreak
        {
            get
            {
                return WaitingOnRepetitionCount > 0;
            }
        }

        public override void ResetBettingStreak()
        {
            WaitingOnRepetitionCount = 0;
        }

        public override bool IsNumberBetPlaced
        {
            get
            {
                return _LastBets.Count > 0;
            }
        }

        public override void PlaceBets()
        {
            RemoveBets();

            var neglectedNumbers = Statistics.NumberNegligenceStreaks
                                    .Where(cur => cur.NegligenceCount >= Settings.NumberNegligenceBettingSettings.NegligenceStreakThreshold);

            if (Settings.NumberNegligenceBettingSettings.BetOnSameEndings)
            {
                if (IsInBettingStreak)
                {
                    foreach (var num in _repetingNumbers)
                        _LastBets.Add(PlaceBetOnNumber(num));
                }
                else
                {
                    var firstCompleteSameEndingGroup = neglectedNumbers.GroupBy(cur => cur.Number % 10).Where(group => group.Count() == 4).FirstOrDefault();
                    if (firstCompleteSameEndingGroup == null)
                        return;

                    foreach (var num in firstCompleteSameEndingGroup)
                        _LastBets.Add(PlaceBetOnNumber(num.Number));
                }
            }
            else
            {
                List<NumberNegligenceViewModel> orderedNeglectedNumbers = null;
                if (Settings.NumberNegligenceBettingSettings.FrequentNumbersFirst)
                {
                    orderedNeglectedNumbers = neglectedNumbers.OrderByDescending(cur =>
                        {
                            var numberFrequency = Statistics.NumberFrequency.FirstOrDefault(f => f.Number == cur.Number);
                            if (numberFrequency != null)
                                return numberFrequency.MaxCount;

                            return cur.NegligenceCount;
                        }).ToList();
                }
                else if (Settings.NumberNegligenceBettingSettings.MaxStreakCountNumberLast)
                {
                    orderedNeglectedNumbers = neglectedNumbers.OrderBy(cur => cur.MaxNegligenceCount).ToList();
                }
                else
                {
                    orderedNeglectedNumbers = neglectedNumbers.OrderByDescending(cur => cur.NegligenceCount).ToList();
                }

                if (orderedNeglectedNumbers.Count >= Settings.NumberNegligenceBettingSettings.MinNumberBettingCount)
                {
                    int count = 0;
                    foreach (var num in orderedNeglectedNumbers)
                    {
                        _LastBets.Add(PlaceBetOnNumber(num.Number));
                        count++;

                        if (count >= Settings.NumberNegligenceBettingSettings.MaxNumberBettingCount)
                            break;
                    }
                }
            }

            var groupId = GetNewBettingGroupId();
            foreach (var bet in _LastBets)
                bet.GroupId = groupId;
        }

        public override void CalculateWinnings(CasinoNumberViewModel drawnNumber)
        {
            foreach (var bet in _LastBets)
                CalculateWinningsOnBet(bet, drawnNumber);

            if (Settings.NumberNegligenceBettingSettings.BetOnSameEndings && Settings.NumberNegligenceBettingSettings.WaitForRepetitionCount > 0)
            {
                if (_LastBets.All(cur => cur.Result == BetResultKind.Loss))
                {
                    _repetitionLossCount++;
                    if (_repetitionLossCount >= Settings.NumberNegligenceBettingSettings.AbortWaitOnLossCount)
                        WaitingOnRepetitionCount = 0;
                }
                else if (_LastBets.Any(cur => cur.Result == BetResultKind.Win))
                {
                    if (IsInBettingStreak)
                    {
                        WaitingOnRepetitionCount--;
                    }
                    else if (Settings.NumberNegligenceBettingSettings.WaitForRepetitionCount > 0)
                    {
                        _repetingNumbers.Clear();
                        WaitingOnRepetitionCount = Settings.NumberNegligenceBettingSettings.WaitForRepetitionCount;
                        foreach (var bet in _LastBets)
                            _repetingNumbers.Add(bet.Number);
                    }
                }
            }

            _LastBets.Clear();
        }

    }
}
