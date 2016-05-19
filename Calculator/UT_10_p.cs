using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public static class Converter10p
    {
        private static int _base;
        public static string DoTrasfer(double p1, int b)
        {
            if (b < 2 || b > 16) throw new Exception("Неверная система счисления");
            _base = b;
            var floor = Math.Floor(Math.Abs(p1));
            var fract = Math.Abs(p1) - floor;
            var result = Int10ToP(floor);
            if (fract != 0) result += Frac10ToP(fract);
            return p1 < 0 ? "-" + result : result;
        }

        private static char IntToPChar(long n)
        {
            char k;
            if (n >= 10)
            {
                var q = (char)'A';
                k = Convert.ToChar(65 + (n - 10));
            }
            else
            {
                k = Convert.ToChar(48 + n);
            }
            return k;
        }

        private static string Int10ToP(double floor)
        {
            var result = "";
            while (floor >= _base) //вычисление целой части в r системе
            {
                double c = floor % _base;
                floor /= _base;
                result = IntToPChar((long)c) + result;
            }
            result = IntToPChar((long)floor) + result;
            return result;
        }

        private static string Frac10ToP(double fract)
        {
            var result = ".";
            fract *= _base;
            int i = 0;
            while (Math.Abs(fract) > 1e-11 && i++ < 40)
            {
                double q = Math.Floor(fract);
                result += IntToPChar((long)q);
                fract -= q;
                fract *= _base;
            }
            return result;
        }

    }
}
