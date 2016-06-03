using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Media;
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
        public virtual void Clear()
        {
            Number = "";
        }
        public abstract void Separate();
        public abstract void BackSpace();
        public abstract void AddDigit(char n);
        public virtual void ComplexSeparate() { }
        protected static int PCharToInt(char a)
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

    public class FracEditor : Editor
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
                        if (string.IsNullOrEmpty(Number) || Number.Last() != '/' || n != '0')
                            Number += n;
                        else
                            SystemSounds.Beep.Play();
                        break;
                    }
            }
        }
    }

    public class ComplexEditor : Editor
    {
        public ComplexEditor()
        {
            Number = "";
            Re = new RealEditor();
            Im = new RealEditor();
        }

        RealEditor Re;
        RealEditor Im;
        bool _isIm;

        public override void Separate()
        {
            if (_isIm) Im.Separate();
            else Re.Separate();
            SetNumber();
        }
        public override void ComplexSeparate()
        {
            if(_isIm) return;
            _isIm = true;
            if (string.IsNullOrEmpty(Re.Number)) Re.Number = "0";
            SetNumber();
        }
        public override void BackSpace()
        {
            if (_isIm) Im.BackSpace();
            else Re.BackSpace();
            _isIm = !string.IsNullOrEmpty(Im.Number);
            SetNumber();
        }
        public override void AddDigit(char n)
        {
            if (_isIm) Im.AddDigit(n);
            else Re.AddDigit(n);
            SetNumber();
        }
        public override void Sign()
        {
            if (_isIm) Im.Sign();
            else Re.Sign();
            SetNumber();
        }

        private void SetNumber()
        {
            Number = Re.Number;
            if (_isIm) Number += " + ";
            if (!string.IsNullOrEmpty(Im.Number))
                Number += Im.Number + "i";
        }

        public override void Clear()
        {
            Re.Clear();
            Im.Clear();
            Number = "";
            _isIm = false;
        }

    }
}
