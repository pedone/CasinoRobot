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
    public abstract class BettingModeBase
    {

        protected StatisticsViewModel Statistics
        {
            get
            {
                return ApplicationViewModel.Instance.Statistics;
            }
        }

        protected SettingsViewModel Settings
        {
            get
            {
                return ApplicationViewModel.Instance.Settings;
            }
        }

        protected SessionViewModel Session
        {
            get
            {
                return ApplicationViewModel.Instance.Session;
            }
        }

        protected int LastBettingGroupId { get; private set; }

        public abstract void PlaceBets();

        public abstract void CalculateWinnings(CasinoNumberViewModel drawnNumber);

        public virtual void ResetBettingStreak()
        { }

        public virtual bool IsInBettingStreak
        {
            get { return false; }
        }

        protected int GetNewBettingGroupId()
        {
            LastBettingGroupId++;
            return LastBettingGroupId;
        }

        protected void RemoveBets()
        {
            RouletteBoardHelper.ClickButton(RouletteButtonKind.RemoveBets);
        }

        protected NumberBetViewModel PlaceBetOnNumber(int number, double amount = 0)
        {
            if (!Settings.IsBettingEnabled)
                return null;

            if (amount == 0)
                amount = Settings.DefaultBet.Amount.Value;

            if (amount < Settings.MinBetAmount)
                amount = Settings.MinBetAmount;
            else if (amount > Settings.MaxBetAmount)
                amount = Settings.MaxBetAmount;

            if (!Settings.IsSimulationMode)
            {
                RouletteBoardHelper.SetBet(Settings.DefaultBet);

                int betClicksToAmount = 1;
                if (amount > 0)
                    betClicksToAmount = (int)(amount / Settings.DefaultBet.Amount.Value);

                for (int i = 0; i < betClicksToAmount; i++)
                {
                    RouletteBoardHelper.ClickButton(number);
                }
            }

            //int curStreakCount = GetOppositeStreakCount(betKind);
            double betAmount = amount == 0 ? Settings.DefaultBet.Amount.Value : amount;

            var newBet = new NumberBetViewModel(number, betAmount, ApplicationViewModel.Instance.Session.ElapsedTime);
            Statistics.AddBetToHistory(newBet);

            return newBet;
        }

        /// <summary>
        /// Place a single bet.
        /// </summary>
        /// <param name="amount">Amount to place. 0 for DefaultBet.</param>
        protected BetViewModel PlaceBet(BettingKind betKind, int streakCount = 0, double amount = 0)
        {
            RouletteButtonKind? betButton = null;
            switch (betKind)
            {
                case BettingKind.To18:
                    betButton = RouletteButtonKind.To18;
                    break;
                case BettingKind.From19:
                    betButton = RouletteButtonKind.From19;
                    break;
                case BettingKind.Even:
                    betButton = RouletteButtonKind.Even;
                    break;
                case BettingKind.Odd:
                    betButton = RouletteButtonKind.Odd;
                    break;
                case BettingKind.Red:
                    betButton = RouletteButtonKind.Red;
                    break;
                case BettingKind.Black:
                    betButton = RouletteButtonKind.Black;
                    break;
                case BettingKind.To18AltFrom19:
                    {
                        bool lastWasTo18 = RouletteHelper.IsNumberOfType(Statistics.LastNumber.Value, NumberKind.To18);
                        if (lastWasTo18)
                            betButton = RouletteButtonKind.To18;
                        else
                            betButton = RouletteButtonKind.From19;
                    }
                    break;
                case BettingKind.EvenAltOdd:
                    {
                        bool lastWasEven = RouletteHelper.IsNumberOfType(Statistics.LastNumber.Value, NumberKind.Even);
                        if (lastWasEven)
                            betButton = RouletteButtonKind.Even;
                        else
                            betButton = RouletteButtonKind.Odd;
                    }
                    break;
                case BettingKind.RedAltBlack:
                    {
                        bool lastWasRed = RouletteHelper.IsNumberOfType(Statistics.LastNumber.Value, NumberKind.Red);
                        if (lastWasRed)
                            betButton = RouletteButtonKind.Red;
                        else
                            betButton = RouletteButtonKind.Black;
                    }
                    break;
                default:
                    return null;
            }

            return PlaceBet(betKind, betButton.Value, streakCount, amount);
        }

        private BetViewModel PlaceBet(BettingKind betKind, RouletteButtonKind betButtonKind, int streakCount, double amount = 0)
        {
            //TODO select correct bet amount button (when auto adjust is enabled)
            if (!Settings.IsBettingEnabled)
                return null;

            if (amount == 0)
                amount = Settings.DefaultBet.Amount.Value;

            if (amount < Settings.MinBetAmount)
                amount = Settings.MinBetAmount;
            else if (amount > Settings.MaxBetAmount)
                amount = Settings.MaxBetAmount;

            if (!Settings.IsSimulationMode)
            {
                RouletteBoardHelper.SetBet(Settings.DefaultBet);

                int betClicksToAmount = 1;
                if (amount > 0)
                    betClicksToAmount = (int)(amount / Settings.DefaultBet.Amount.Value);

                for (int i = 0; i < betClicksToAmount; i++)
                {
                    RouletteBoardHelper.ClickButton(betButtonKind);
                }
            }

            //int curStreakCount = GetOppositeStreakCount(betKind);
            double betAmount = amount == 0 ? Settings.DefaultBet.Amount.Value : amount;

            BetViewModel newBet = new BetViewModel(betKind, streakCount, Statistics.LastNumber.Value, betAmount, ApplicationViewModel.Instance.Session.ElapsedTime);
            Statistics.AddBetToHistory(newBet);

            return newBet;
        }

        protected BetResultKind CalculateWinningsOnBet(BetViewModel bet, CasinoNumberViewModel curNumber)
        {
            if (bet == null)
                return BetResultKind.None;

            if (curNumber == null)
            {
                bet.Result = BetResultKind.Unclear;
            }
            else
            {

                bool isZero = RouletteHelper.IsNumberOfType(curNumber.Number, NumberKind.Zero);
                bool isRed = RouletteHelper.IsNumberOfType(curNumber.Number, NumberKind.Red);
                bool isBlack = RouletteHelper.IsNumberOfType(curNumber.Number, NumberKind.Black);
                bool isOdd = RouletteHelper.IsNumberOfType(curNumber.Number, NumberKind.Odd);
                bool isEven = RouletteHelper.IsNumberOfType(curNumber.Number, NumberKind.Even);
                bool isTo18 = RouletteHelper.IsNumberOfType(curNumber.Number, NumberKind.To18);
                bool isFrom19 = RouletteHelper.IsNumberOfType(curNumber.Number, NumberKind.From19);

                bool isWin = false;
                switch (bet.BetKind)
                {
                    case BettingKind.To18:
                        isWin = isTo18;
                        break;
                    case BettingKind.From19:
                        isWin = isFrom19;
                        break;
                    case BettingKind.Even:
                        isWin = isEven;
                        break;
                    case BettingKind.Odd:
                        isWin = isOdd;
                        break;
                    case BettingKind.Red:
                        isWin = isRed;
                        break;
                    case BettingKind.Black:
                        isWin = isBlack;
                        break;
                    case BettingKind.To18AltFrom19:
                        bool isLastNumberTo18 = RouletteHelper.IsNumberOfType(bet.LastNumber, NumberKind.To18);
                        isWin = (isTo18 == isLastNumberTo18);
                        break;
                    case BettingKind.EvenAltOdd:
                        bool isLastNumberEven = RouletteHelper.IsNumberOfType(bet.LastNumber, NumberKind.Even);
                        isWin = (isEven == isLastNumberEven);
                        break;
                    case BettingKind.RedAltBlack:
                        bool isLastNumberRed = RouletteHelper.IsNumberOfType(bet.LastNumber, NumberKind.Red);
                        isWin = (isRed == isLastNumberRed);
                        break;
                    default:
                        isWin = false;
                        break;
                }

                if (isWin)
                {
                    Statistics.WinningsAmount += bet.Amount;
                    Statistics.WinCount++;

                    bet.Result = BetResultKind.Win;
                }
                else
                {
                    Statistics.LossesAmount += bet.Amount;
                    Statistics.LossCount++;

                    bet.Result = BetResultKind.Loss;
                }
            }

            return bet.Result;
        }


        protected BetResultKind CalculateWinningsOnBet(NumberBetViewModel bet, CasinoNumberViewModel curNumber)
        {
            if (bet == null)
                return BetResultKind.None;

            if (curNumber == null)
            {
                bet.Result = BetResultKind.Unclear;
            }
            else
            {
                bool isWin = curNumber.Number == bet.Number;
                if (isWin)
                {
                    Statistics.WinningsAmount += bet.Amount * 35;
                    Statistics.WinCount++;

                    bet.Result = BetResultKind.Win;
                }
                else
                {
                    Statistics.LossesAmount += bet.Amount;
                    Statistics.LossCount++;

                    bet.Result = BetResultKind.Loss;
                }
            }

            return bet.Result;
        }

        public virtual bool IsNumberBetPlaced
        {
            get
            {
                return false;
            }
        }
    }

}
