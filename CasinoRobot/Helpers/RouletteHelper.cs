using CasinoRobot.Enums;
using CasinoRobot.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoRobot.Helpers
{
    public static class RouletteHelper
    {
        static List<int> _RedNumbers = new List<int>() { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };
        static List<int> _BlackNumbers = new List<int>() { 2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35 };

        public static bool IsNumberOfType(int number, NumberKind kind)
        {
            if (number == 0 && kind != NumberKind.Zero)
                return false;

            switch (kind)
            {
                case NumberKind.Zero:
                    return number == 0;
                case NumberKind.Red:
                    return _RedNumbers.Contains(number);
                case NumberKind.Black:
                    return _BlackNumbers.Contains(number);
                case NumberKind.Odd:
                    return number % 2 == 1;
                case NumberKind.Even:
                    return number % 2 == 0;
                case NumberKind.To18:
                    return number <= 18;
                case NumberKind.From19:
                    return number >= 19;
            }

            return false;
        }


        internal static DozenKind GetDozenKind(int number)
        {
            if (number >= 1 && number <= 12)
                return DozenKind.First;
            if (number >= 13 && number <= 24)
                return DozenKind.Second;
            if (number >= 25 && number <= 36)
                return DozenKind.Third;

            return DozenKind.None;
        }

        internal static ThirdKind GetThirdKind(int number)
        {
            if (number <= 1 || number >= 36)
                return ThirdKind.None;

            int devisionRest = number % 3;
            if (devisionRest == 1)
                return ThirdKind.First;
            if (devisionRest == 2)
                return ThirdKind.Second;
            if (devisionRest == 0)
                return ThirdKind.Third;

            return ThirdKind.None;
        }
    }
}
