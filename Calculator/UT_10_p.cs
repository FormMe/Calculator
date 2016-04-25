using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convertor_p1_p2
{
    class UT_10_p
    {
        private int _base;
        public int Base
        {
            get { return _base; }
            set
            {
                if (value >= 2 && value <= 16)
                {
                    _base = value;
                }
            }
        }

        public UT_10_p()
        {
            _base = 2;
        }
        public UT_10_p(int b)
        {
            Base = b;
        }

        public string DoTrasfer(double p1)
        {
            var floor = Math.Floor(Math.Abs(p1));
            var fract = Math.Abs(p1) - floor;
            var result = Int10ToP(floor);
            if (fract != 0) result += Frac10ToP(fract);
            return p1 < 0 ? "-" + result : result;
        }

        private char IntToPChar(long n)
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

        private string Int10ToP(double floor)
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

        private string Frac10ToP(double fract)
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
