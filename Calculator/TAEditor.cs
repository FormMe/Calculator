using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convertor_p1_p2
{
    public abstract class Editor
    {
        protected int _base;
        public string Number { get; set; }

        public int Base
        {
            set
            {
                if (value >= 2 && value <= 16)
                    _base = value;
            }
            get { return _base; }
        }

        public virtual void Sign()
        {
            if (Number[0] == '-')
                Number = Number.Substring(1, Number.Length - 1);
            else
                Number = "-" + Number;
        }
        public void Clear()
        {
            Number = "";
        }
        public abstract void Separate();
        public abstract void BackSpace();
        public abstract void AddDigit(char n);

        protected int PCharToInt(char a)
        {
            return (a >= 'A') ? 10 + ((int)a - (int)'A') : int.Parse(a.ToString());
        }
    }

    public class RealEditor : Editor
    {
        public RealEditor()
        {
            Number = "0";
        }
        public override void Separate()
        {
            if (Number.Contains('.')) return;
            if (Number.Length == 0) Number += "0.";
            else Number += ".";
        }

        public override void BackSpace()
        {
            if (Number.Length == 1) Number = "0";
            else
                if (Number[Number.Length - 1] == '0' && Number[Number.Length - 2] == '.')
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
            if (Number.Length == 0) Number += "0.";
            else Number += "/";
        }
        public override void BackSpace()
        {
            if (Number.Length == 1) Number = "0";
            else
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
