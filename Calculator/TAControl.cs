using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Convertor_p1_p2;

namespace Calculator
{
    enum Mode
    {
        Real,
        Complex,
        Frac
    };

    enum Stat
    {
        IsLeft,
        IsOp,
        IsRight,
        OpBegin,
        OpDone
    }
    class Cntrl
    {
        public History history;
        private Editor editor;
        private Memory memory;
        private Proc proc;
        private Number num;

        public string preH;
        public string editable => editor.Number;
        public string operation;
        public int Base
        {
            set
            {
                editor.Base = value;
            }
            get { return editor.Base; }
        }

        public Mode mode;
        public Stat status;

        private UT_10_p converter10P;
        private UT_p_10 converterP10;

        public Cntrl(Mode m)
        {
            mode = m;
            history = new History();
            memory = new Memory();
            proc = new Proc();
            switch (mode)
            {
                case Mode.Real:
                    {
                        editor = new RealEditor();
                        num = new Real(0);
                        break;
                    }
                case Mode.Complex:
                    {
                        editor = new ComplexEditor();
                        num = new Complex(0, 0);
                        break;
                    }
                case Mode.Frac:
                    {
                        editor = new FracEditor();
                        num = new Frac(0, 1);
                        break;
                    }

            }
        }

        public void DoCommand(string command)
        {
            try
            {
                switch (command)
                {
                    case ".":
                    case "Decimal":
                        editor.Separate(); break;
                    case "+/-":
                        editor.Sign(); break;
                    case "CE":
                    case "Delete":
                        editor.Clear(); break;
                    case "BackSpace":
                    case "Back":
                        editor.BackSpace(); break;
                    case "C":
                        proc.Reset();
                        editor.Clear();
                        preH = "";
                        break;
                    default:
                        editor.AddDigit(char.Parse(command)); break;

                    case "MR":
                        proc.r = memory.num; break;
                    case "MS":
                        memory.num = proc.r; break;
                    case "MC":
                        memory.Clear(); break;
                    case "Mplus":
                        memory.Add(proc.r); break;

                    case "-":
                        SetNum();
                        proc.RunOperation();
                        proc.op = Operation.Sub;
                        SetResult(command);
                        break;
                    case "+":
                        SetNum();
                        proc.RunOperation();
                        proc.op = Operation.Add;
                        SetResult(command);
                        break;
                    case "*":
                        SetNum();
                        proc.RunOperation();
                        proc.op = Operation.Mult;
                        SetResult(command);
                        break;
                    case "/":
                        SetNum();
                        proc.RunOperation();
                        proc.op = Operation.Div;
                        SetResult(command);
                        break;
                    case "Sqr":
                        SetNum();
                        preH = command + "(" + proc.l + ")";
                        proc.func = Function.Sqr;
                        proc.RunFunction();
                        SetResult(command);
                        break;
                    case "Sqrt":
                        SetNum();
                        preH = command + "(" + proc.l + ")";
                        proc.func = Function.Sqrt;
                        proc.RunFunction();
                        SetResult(command);
                        break;
                    case "1/x":
                        SetNum();
                        preH = "Rev(" + proc.l + ")";
                        proc.func = Function.Rev;
                        proc.RunFunction();
                        SetResult(command);
                        break;
                    case "=":
                        SetNum();
                        proc.RunFunction();
                        proc.RunOperation();
                        editor.Number = proc.l.ToString();
                        preH = "";
                        proc.Reset();
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void SetNum( )
        {
            if (editor.Number == "") return;

            if (proc.l == null)
                proc.l = ToNumber(editor.Number);
            else
                proc.r = ToNumber(editor.Number);
        }
        private void SetResult(string command)
        {
            if (command == "Sqr" || command == "Sqrt" || command == "1/x")
            {
                editor.Number = proc.l.ToString();
            }
            else
            {
                preH = proc.l + " " + command;
                editor.Clear();
            }
        }

        private Number ToNumber(string n)
        {
            switch (mode)
            {
                case Mode.Real:
                    return new Real(n);
                case Mode.Complex:
                    return new Complex(n);
                case Mode.Frac:
                    return new Frac(n);
                default:
                    throw new Exception("Ошибка приведения типа");
            }
        }

        private string Convert(string num, int b)
        {
            if (Base == b) return num;
            if (Base == 10)
                return converter10P.DoTrasfer(double.Parse(editor.Number.Replace('.', ',')));

            return b == 10 ? converterP10.DoTrasfer(editor.Number).ToString() : converter10P.DoTrasfer(converterP10.DoTrasfer(editor.Number));

        }
    }
}
