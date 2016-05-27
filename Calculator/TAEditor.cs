using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public abstract class Editor
    {
        protected int _base = 10;
        public string Number { get; set; }

        public int Base
        {
            set
            {
                if (value < 2 || value > 16) throw new Exception("Неверная система счисления");
                var prevBase = _base;
                _base = value;
                Number = Cntrl.Convert(Number, prevBase, _base);
            }
            get { return _base; }
        }

        public virtual void Sign()
        {
            if (!string.IsNullOrEmpty(Number) && Number[0] == '-')
                Number = Number.Substring(1, Number.Length - 1);
            else
            {
                if (string.IsNullOrEmpty(Number))
                    Number = "0";
                Number = "-" + Number;
            }
        }
        public void Clear()
        {
            Number = "";
        }
        public abstract void Separate();
        public abstract void BackSpace();
        public abstract void AddDigit(char n);

        public static int PCharToInt(char a)
        {
            return (a >= 'A') ? 10 + ((int)a - (int)'A') : int.Parse(a.ToString());
        }
    }

    public class RealEditor : Editor
    {
        public RealEditor()
        {
            Number = "";
        }
        public override void Separate()
        {
            if (Number.Contains(Cntrl.dot)) return;
            if (Number.Length == 0) Number += "0" + Cntrl.dot;
            else Number += Cntrl.dot.ToString();
        }

        public override void BackSpace()
        {
            if (string.IsNullOrEmpty(Number)) return;

            if (Number[Number.Length - 1] == '0' && Number[Number.Length - 2] == Cntrl.dot ||
                Number.Length == 2 && Number.First() == '-')
                Number = Number.Substring(0, Number.Length - 2);
            else
                Number = Number.Substring(0, Number.Length - 1);
        }

        public override void AddDigit(char n)
        {
            if (PCharToInt(n) > _base - 1) return;
            switch (Number)
            {
                case "0":
                    {
                        Number = n.ToString();
                        break;
                    }
                case "-0":
                    {
                        Number = "-" + n;
                        break;
                    }
                default:
                    {
                        Number += n;
                        break;
                    }
            }
        }
    }

    public class FracEditor : RealEditor
    {
        public FracEditor()
        {
            Number = "";
        }
        public override void Separate()
        {
            if (Number.Contains('/')) return;
            if (Number.Length == 0) Number += "0/";
            else Number += "/";
        }
        public override void BackSpace()
        {
            if (string.IsNullOrEmpty(Number)) return;
            if (Number[Number.Length - 1] == '0' && Number[Number.Length - 2] == '/')
                Number = Number.Substring(0, Number.Length - 2);
            else
                Number = Number.Substring(0, Number.Length - 1);
        }
    }

    public class ComplexEditor : Editor
    {
        public override void Separate()
        {
            throw new NotImplementedException();
        }

        public override void BackSpace()
        {
            throw new NotImplementedException();
        }

        public override void AddDigit(char n)
        {
            throw new NotImplementedException();
        }
    }
}
