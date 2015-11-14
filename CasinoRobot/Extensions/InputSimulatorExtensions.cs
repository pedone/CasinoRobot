using CasinoRobot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WindowsInput;

namespace CasinoRobot.Extensions
{
    public static class InputSimulatorExtensions
    {

        public static void MoveMouseToPixel(this InputSimulator inputSimulator, Point pixelPosition)
        {
            var mickeyX = (int)((65535.0f * (pixelPosition.X / SystemParameters.PrimaryScreenWidth)) + 0.5f);
            var mickeyY = (int)((65535.0f * (pixelPosition.Y / SystemParameters.PrimaryScreenHeight)) + 0.5f);

            ApplicationViewModel.InputSimulator.Mouse.MoveMouseTo(mickeyX, mickeyY);
        }

    }
}
