using CasinoRobot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CasinoRobot.Extensions;

namespace CasinoRobot.Helpers
{
    public static class RouletteBoardHelper
    {
        public static void SetBet(CasinoBetButtonViewModel betButton)
        {
            Click(betButton.Position.Value);
        }

        public static void ClickButton(int number)
        {
            var casinoNumber = ApplicationViewModel.Instance.Settings.CasinoNumbers.FirstOrDefault(cur => cur.Number == number);
            if (casinoNumber == null)
                throw new ArgumentException(string.Format("NUmber {0} must be set!", casinoNumber.Number));

            Click(casinoNumber.Center);
        }

        public static void ClickButton(RouletteButtonKind buttonKind)
        {
            var targetButton = ApplicationViewModel.Instance.Settings.GetButton(buttonKind);
            if (targetButton == null)
                throw new ArgumentException(string.Format("Button {0} must be set!", buttonKind));

            if (targetButton.IsPositionAbsolute)
                ClickAbsolute(targetButton.Position.Value);
            else
                Click(targetButton.Position.Value);
        }

        private static void Click(Point position)
        {
            var targetPosition = ApplicationViewModel.Instance.Settings.FixedCasinoWindowPosition;
            targetPosition.Offset(position.X, position.Y);

            ClickAbsolute(targetPosition);
        }
        private static void ClickAbsolute(Point position)
        {
            ApplicationViewModel.InputSimulator.MoveMouseToPixel(position);
            ApplicationViewModel.InputSimulator.Mouse.LeftButtonClick();
        }
    }
}
