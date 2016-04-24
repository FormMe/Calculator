using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    abstract class Editor
    {
        private int _base;
        public int Base
        {
            get { return _base; }
            set
            {
                if (value >= 2 && value <= 16)
                    _base = value;
            }
        }
        

        public string number { get; protected set; }

        public abstract bool EqZero();
        public abstract void Sign();
        public abstract void BackSpace();
        public abstract void Clear();
        public abstract void Separate();
        public abstract void AddDigit(char n);

        protected abstract void AddDigitLS(char n);
        protected abstract void AddDigitRS(char n);
    }

    class FracEditor : Editor
    {
        public override bool EqZero()
        {
            throw new NotImplementedException();
        }

        public override void Sign()
        {
            throw new NotImplementedException();
        }

        public override void BackSpace()
        {
            throw new NotImplementedException();
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        public override void Separate()
        {
            throw new NotImplementedException();
        }

        public override void AddDigit(char n)
        {
            throw new NotImplementedException();
        }

        protected override void AddDigitLS(char n)
        {
            throw new NotImplementedException();
        }

        protected override void AddDigitRS(char n)
        {
            throw new NotImplementedException();
        }
    }

    class RealEditor : Editor
    {
        public RealEditor()
        {
            number = "0";
        }

        public override bool EqZero()
        {
            return number == "0";
        }

        public override void Sign()
        {
            if (number.First() == '-')
                number = number.Substring(1, number.Length - 1);
            else
                number = "-" + number;
        }

        public override void BackSpace()
        {
            if (number.Length == 1) number = "0";
            else
                if (number.Last() == '0' && number[number.Length - 2] == '.')
                number = number.Substring(0, number.Length - 2);
            else
                number = number.Substring(0, number.Length - 1);
        }

        public override void Clear()
        {
            number = "0";
        }

        public override void Separate()
        {
            if (!number.Contains('.'))
                number += ".";
        }

        public override void AddDigit(char n)
        {
            if (PCharToInt(n) > Base - 1 || number.Length >= 34) return;
            switch (number)
            {
                case "0":
                    {
                        number = n.ToString();
                        break;
                    }
                case "-0":
                    {
                        number = "-" + n;
                        break;
                    }
                default:
                    {
                        number += n;
                        break;
                    }
            }
        }

        protected override void AddDigitLS(char n)
        {
            throw new NotImplementedException();
        }

        protected override void AddDigitRS(char n)
        {
            throw new NotImplementedException();
        }

        private int PCharToInt(char a)
        {
            return (a >= 'A') ? 10 + ((int)a - (int)'A') : int.Parse(a.ToString());
        }
    }

    class ComplexEditor : Editor
    {
        RealEditor Re;
        RealEditor Im;
        public override bool EqZero()
        {
            return Re.EqZero() && Im.EqZero();
        }

        public override void Sign()
        {
            throw new NotImplementedException();
        }

        public override void BackSpace()
        {
            throw new NotImplementedException();
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        public override void Separate()
        {
            throw new NotImplementedException();
        }

        public override void AddDigit(char n)
        {
            throw new NotImplementedException();
        }

        protected override void AddDigitLS(char n)
        {
            throw new NotImplementedException();
        }

        protected override void AddDigitRS(char n)
        {
            throw new NotImplementedException();
        }
    }
}
