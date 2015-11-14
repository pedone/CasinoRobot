using CasinoRobot.Enums;
using CasinoRobot.Helpers;
using CasinoRobot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CasinoRobot.Betting
{
    public class LastNumberBetting : BettingModeBase
    {

        private List<BetViewModel> _LastBets = new List<BetViewModel>();

        private int _curLossStreakCountRedBlack;
        private int _curLossStreakCountEvenOdd;
        private int _curLossStreakCountTo18From19;

        private bool _alternateRedBlack;
        private bool _alternateEvenOdd;
        private bool _alternateTo18From19;

        private TendencyMeasurement _RedBlackTendencyMeasurement = new TendencyMeasurement();
        private TendencyMeasurement _EvenOddTendencyMeasurement = new TendencyMeasurement();
        private TendencyMeasurement _To18From19TendencyMeasurement = new TendencyMeasurement();

        public override bool IsNumberBetPlaced
        {
            get
            {
                return _LastBets.Count > 0;
            }
        }

        public override void PlaceBets()
        {
            if (Statistics.LastNumber == null)
                return;

            RemoveBets();

            bool isRed = RouletteHelper.IsNumberOfType(Statistics.LastNumber.Value, NumberKind.Red);
            bool isBlack = RouletteHelper.IsNumberOfType(Statistics.LastNumber.Value, NumberKind.Black);
            bool isOdd = RouletteHelper.IsNumberOfType(Statistics.LastNumber.Value, NumberKind.Odd);
            bool isEven = RouletteHelper.IsNumberOfType(Statistics.LastNumber.Value, NumberKind.Even);
            bool isTo18 = RouletteHelper.IsNumberOfType(Statistics.LastNumber.Value, NumberKind.To18);
            bool isFrom19 = RouletteHelper.IsNumberOfType(Statistics.LastNumber.Value, NumberKind.From19);

            bool betOnOpposite = Keyboard.IsKeyDown(Key.Space);

            if (Settings.LastNumberBettingSettings.BetOnRedAndBlack)
            {
                if (betOnOpposite)
                {
                    if (isRed)
                        _LastBets.Add(!_alternateRedBlack ? PlaceBet(BettingKind.Black) : PlaceBet(BettingKind.Red));
                    else if (isBlack)
                        _LastBets.Add(!_alternateRedBlack ? PlaceBet(BettingKind.Red) : PlaceBet(BettingKind.Black));
                }
                else
                {
                    if (isRed)
                        _LastBets.Add(_alternateRedBlack ? PlaceBet(BettingKind.Black) : PlaceBet(BettingKind.Red));
                    else if (isBlack)
                        _LastBets.Add(_alternateRedBlack ? PlaceBet(BettingKind.Red) : PlaceBet(BettingKind.Black));
                }
            }

            if (Settings.LastNumberBettingSettings.BetOnEvenAndOdd)
            {
                if (betOnOpposite)
                {
                    if (isOdd)
                        _LastBets.Add(!_alternateRedBlack ? PlaceBet(BettingKind.Even) : PlaceBet(BettingKind.Odd));
                    else if (isEven)
                        _LastBets.Add(!_alternateRedBlack ? PlaceBet(BettingKind.Odd) : PlaceBet(BettingKind.Even));
                }
                else
                {
                    if (isOdd)
                        _LastBets.Add(_alternateRedBlack ? PlaceBet(BettingKind.Even) : PlaceBet(BettingKind.Odd));
                    else if (isEven)
                        _LastBets.Add(_alternateRedBlack ? PlaceBet(BettingKind.Odd) : PlaceBet(BettingKind.Even));
                }
            }

            if (Settings.LastNumberBettingSettings.BetOnTo18AndFrom19)
            {
                if (betOnOpposite)
                {
                    if (isTo18)
                        _LastBets.Add(!_alternateRedBlack ? PlaceBet(BettingKind.From19) : PlaceBet(BettingKind.To18));
                    else if (isFrom19)
                        _LastBets.Add(!_alternateRedBlack ? PlaceBet(BettingKind.To18) : PlaceBet(BettingKind.From19));
                }
                else
                {
                    if (isTo18)
                        _LastBets.Add(_alternateRedBlack ? PlaceBet(BettingKind.From19) : PlaceBet(BettingKind.To18));
                    else if (isFrom19)
                        _LastBets.Add(_alternateRedBlack ? PlaceBet(BettingKind.To18) : PlaceBet(BettingKind.From19));

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

            if (Settings.LastNumberBettingSettings.IsTendencyAdjustmentEnabled)
            {
                //measure tendencies
                //red black
                var redBlackBet = _LastBets.FirstOrDefault(cur => cur.BetKind == BettingKind.Red || cur.BetKind == BettingKind.Black);
                if (redBlackBet != null)
                {
                    _RedBlackTendencyMeasurement.SpinCount++;
                    if (redBlackBet.Result != BetResultKind.Win)
                        _RedBlackTendencyMeasurement.LossCount++;
                }
                //even odd
                var evenOddBet = _LastBets.FirstOrDefault(cur => cur.BetKind == BettingKind.Even || cur.BetKind == BettingKind.Odd);
                if (evenOddBet != null)
                {
                    _EvenOddTendencyMeasurement.SpinCount++;
                    if (evenOddBet.Result != BetResultKind.Win)
                        _EvenOddTendencyMeasurement.LossCount++;
                }
                //red black
                var to18From19Bet = _LastBets.FirstOrDefault(cur => cur.BetKind == BettingKind.To18 || cur.BetKind == BettingKind.From19);
                if (to18From19Bet != null)
                {
                    _To18From19TendencyMeasurement.SpinCount++;
                    if (to18From19Bet.Result != BetResultKind.Win)
                        _To18From19TendencyMeasurement.LossCount++;
                }

                //adjust to tendencies
                if (_RedBlackTendencyMeasurement.SpinCount >= Settings.LastNumberBettingSettings.TendencyAdjustmentMeasurementCount)
                {
                    if (_RedBlackTendencyMeasurement.LossCount >= Settings.LastNumberBettingSettings.TendencyAdjustmentLossThreshold)
                        _alternateRedBlack = !_alternateRedBlack;

                    _RedBlackTendencyMeasurement.Reset();
                }
                if (_EvenOddTendencyMeasurement.SpinCount >= Settings.LastNumberBettingSettings.TendencyAdjustmentMeasurementCount)
                {
                    if (_EvenOddTendencyMeasurement.LossCount >= Settings.LastNumberBettingSettings.TendencyAdjustmentLossThreshold)
                        _alternateEvenOdd = !_alternateEvenOdd;

                    _EvenOddTendencyMeasurement.Reset();
                }
                if (_To18From19TendencyMeasurement.SpinCount >= Settings.LastNumberBettingSettings.TendencyAdjustmentMeasurementCount)
                {
                    if (_To18From19TendencyMeasurement.LossCount >= Settings.LastNumberBettingSettings.TendencyAdjustmentLossThreshold)
                        _alternateTo18From19 = !_alternateTo18From19;

                    _To18From19TendencyMeasurement.Reset();
                }
            }
            else if (Settings.LastNumberBettingSettings.AlternateAfterLossCount > 0)
            {
                //count loss streaks
                _curLossStreakCountRedBlack += _LastBets.Where(cur => (cur.BetKind == BettingKind.Red || cur.BetKind == BettingKind.Black) && cur.Result == BetResultKind.Loss).Count();
                _curLossStreakCountEvenOdd += _LastBets.Where(cur => (cur.BetKind == BettingKind.Even || cur.BetKind == BettingKind.Odd) && cur.Result == BetResultKind.Loss).Count();
                _curLossStreakCountTo18From19 += _LastBets.Where(cur => (cur.BetKind == BettingKind.To18 || cur.BetKind == BettingKind.From19) && cur.Result == BetResultKind.Loss).Count();

                //alternate
                if (_curLossStreakCountRedBlack >= Settings.LastNumberBettingSettings.AlternateAfterLossCount)
                {
                    _alternateRedBlack = !_alternateRedBlack;
                    _curLossStreakCountRedBlack = 0;
                }
                if (_curLossStreakCountEvenOdd >= Settings.LastNumberBettingSettings.AlternateAfterLossCount)
                {
                    _alternateEvenOdd = !_alternateEvenOdd;
                    _curLossStreakCountEvenOdd = 0;
                }
                if (_curLossStreakCountTo18From19 >= Settings.LastNumberBettingSettings.AlternateAfterLossCount)
                {
                    _alternateTo18From19 = !_alternateTo18From19;
                    _curLossStreakCountTo18From19 = 0;
                }

                if (_LastBets.Any(cur => (cur.BetKind == BettingKind.Red || cur.BetKind == BettingKind.Black) && cur.Result == BetResultKind.Win))
                    _curLossStreakCountRedBlack = 0;
                if (_LastBets.Any(cur => (cur.BetKind == BettingKind.Even || cur.BetKind == BettingKind.Odd) && cur.Result == BetResultKind.Win))
                    _curLossStreakCountEvenOdd = 0;
                if (_LastBets.Any(cur => (cur.BetKind == BettingKind.To18 || cur.BetKind == BettingKind.From19) && cur.Result == BetResultKind.Win))
                    _curLossStreakCountTo18From19 = 0;
            }

            _LastBets.Clear();
        }
    }
}
