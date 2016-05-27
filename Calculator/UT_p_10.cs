using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Calculator
{
    public static class ConverterP10
    {
        private static int _base;

        public static double DoTrasfer(string p1, int b)
        {
            if (b < 2 || b > 16) throw new Exception("Неверная система счисления");
            _base = b;
            var sign = Convert.ToInt32(p1[0] == '-');
            var dotInd = p1.IndexOf(Cntrl.dot);
            double result = 0;
            if (dotInd == -1) result = PIntTo10(p1);
            else
            {
                var floor = p1.Substring(sign, dotInd - 1);
                var fract = p1.Substring(dotInd + 1, p1.Length - dotInd - 1);
                result = PIntTo10(floor) + PFracTo10(fract);
            }
            return sign == 1 ? -1 * result : result;
        }

        private static long PCharToInt(char a)
        {
            return (a >= 'A') ? 10 + ((int)a - (int)'A') : int.Parse(a.ToString());
        }

        private static double PIntTo10(string floor)
        {
            double dec = 0;
            int j, k;
            for (j = floor.Length - 1, k = 0; k < floor.Length; j--, k++)
                dec += (double)PCharToInt(floor[k]) * Math.Pow(_base, j);
            return dec;
        }

        private static double PFracTo10(string fract)
        {
            double DEC = 0;
            int j, k;
            for (j = -1, k = 0; k < fract.Length; j--, k++)
                DEC += PCharToInt(fract[k]) * Math.Pow(_base, j);
            return DEC;
        }


    }
}
