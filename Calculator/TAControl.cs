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

        public string preH = "";
        public string editable => editor.Number;
        public string memN => GetMem();
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

        private bool isNewCalc = true;

        private string GetMem()
        {
            return memory.Num?.ToString() ?? "";
        }

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
                        break;
                    }
                case Mode.Complex:
                    {
                        editor = new ComplexEditor();
                        break;
                    }
                case Mode.Frac:
                    {
                        editor = new FracEditor();
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
                        ClearForNewCalc();
                        editor.Separate();
                        break;
                    case "+/-":
                        ClearForNewCalc();
                        editor.Sign();
                        break;
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
                        ClearForNewCalc();
                        editor.AddDigit(char.Parse(command));
                        break;

                    case "MR":
                        if (memory.Num != null)
                            editor.Number = memory.Num.ToString();
                        break;
                    case "MS":
                        if (editor.Number != "")
                            memory.Num = ToNumber(editor.Number);
                        editor.Clear();
                        break;
                    case "MC":
                        memory.Clear(); break;
                    case "M+":
                        if (editor.Number != "")
                            memory.Add(ToNumber(editor.Number));
                        break;
                    case "M-":
                        if (editor.Number != "")
                            memory.Sub(ToNumber(editor.Number));
                        break;

                    case "-":
                        SetNum(command);
                        proc.RunOperation();
                        proc.op = Operation.Sub;
                        SetResult(command);
                        break;
                    case "+":
                        SetNum(command);
                        proc.RunOperation();
                        proc.op = Operation.Add;
                        SetResult(command);
                        break;
                    case "*":
                        SetNum(command);
                        proc.RunOperation();
                        proc.op = Operation.Mult;
                        SetResult(command);
                        break;
                    case "/":
                        SetNum(command);
                        proc.RunOperation();
                        proc.op = Operation.Div;
                        SetResult(command);
                        break;
                    case "Sqr":
                        SetNum(command);
                        proc.func = Function.Sqr;
                        proc.RunFunction();
                        SetResult(command);
                        break;
                    case "Sqrt":
                        SetNum(command);
                        proc.func = Function.Sqrt;
                        proc.RunFunction();
                        SetResult(command);
                        break;
                    case "1/x":
                        SetNum(command);
                        proc.func = Function.Rev;
                        proc.RunFunction();
                        SetResult(command);
                        break;
                    case "=":
                        SetNum(command);
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

        private void ClearForNewCalc()
        {
            if (isNewCalc)
            {
                editor.Clear();
                isNewCalc = false;
            }
        }

        private void SetNum(string command)
        {
            if (editor.Number == "") return;

            if (proc.l == null)
                proc.l = ToNumber(editor.Number);
            else
            {
                if (!isNewCalc)
                    proc.r = ToNumber(editor.Number);
            }

            if (command == "Sqr" || command == "Sqrt" || command == "1/x")
            {
                preH = command == "1/x" ? "Rev" : command;
                preH += "(" + proc.l + ")";
            }
            else
                preH = preH = proc.l + " " + command;
        }
        private void SetResult(string command)
        {
            preH = preH = proc.l + " " + command;
            editor.Number = proc.l.ToString();
            isNewCalc = true;
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
