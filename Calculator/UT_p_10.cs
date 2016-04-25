using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Convertor_p1_p2
{
    public class UT_p_10
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

        public UT_p_10()
        {
            _base = 2;
        }
        public UT_p_10(int b)
        {
            Base = b;
        }

        public double DoTrasfer(string p1)
        {
            int sign = Convert.ToInt32(p1[0] == '-');
            var dot_ind = p1.IndexOf('.');
            double result = 0;
            if (dot_ind == -1) result = PIntTo10(p1);
            else
            {
                var floor = p1.Substring(sign, dot_ind);
                var fract = p1.Substring(dot_ind + 1, p1.Length - dot_ind - 1);
                result = PIntTo10(floor) + PFracTo10(fract);
            }
            return sign == 1 ? -1 * result : result;
        }

        private long PCharToInt(char a)
        {
            return (a >= 'A') ? 10 + ((int)a - (int)'A') : int.Parse(a.ToString());
        }

        private double PIntTo10(string floor) 
        {
            double dec = 0;
            int j, k;
            for (j = floor.Length - 1, k = 0; k < floor.Length; j--, k++)
                dec += (double)PCharToInt(floor[k]) * Math.Pow(_base, j);
            return dec;
        }

        private double PFracTo10(string fract)
        {
            double DEC = 0;
            int j, k;
            for (j = -1, k = 0; k < fract.Length; j--, k++)
                DEC += PCharToInt(fract[k]) * Math.Pow(_base, j);
            return DEC;
        }


    }
}
