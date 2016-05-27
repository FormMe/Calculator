using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO.Packaging;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Calculator
{
    enum Mode
    {
        Real,
        Complex,
        Frac
    };

    class Cntrl
    {
        public History history;
        private Editor editor;
        private Memory memory;
        private Proc proc;

        public string preH => MakePreH();
        public string editable => GetNum();
        public string memN => GetMem();
        public int Base
        {
            set
            {
                editor.Base = value;
                if (proc.r != null) proc.r.Base = value;
                if (proc.l != null) proc.l.Base = value;
                if (memory.Num != null) memory.Num.Base = value;
                if (prevNum != null) prevNum.Base = value;
            }
            private get { return editor.Base; }
        }

        private Number prevNum;
        private Operation prevOp;
        public Mode mode;

        string[] operations =
        {
            "Sqr",
            "1/x",
            "Rev",
            "Sqrt"
        };

        public static char dot = ',';

        private bool isNewCalc = true;
        private bool isTimeToEqual = false;
        private bool isOpAndFunc = false;

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
                        prevNum = new Real(0, Base);
                        break;
                    }
                case Mode.Complex:
                    {
                        editor = new ComplexEditor();
                        prevNum = new Complex(0, 0, Base);
                        break;
                    }
                case Mode.Frac:
                    {
                        editor = new FracEditor();
                        //кривой конструктор
                        prevNum = new Frac(0, Base);
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
                    case ",":
                    case "Decimal":
                    case "/":
                        ClearForNewCalc();
                        editor.Separate();
                        break;
                    case "±":
                        editor.Sign();
                        break;
                    case "CE":
                    case "Delete":
                        ClearForNewCalc();
                        editor.Clear(); break;
                    case "BackSpace":
                    case "Back":
                        editor.BackSpace(); break;
                    case "Clear":
                        ClearForNewCalc();
                        proc.Reset();
                        editor.Clear();
                        break;
                    default:
                        if (isTimeToEqual)
                        {
                            proc.r = proc.l = null;
                            isTimeToEqual = false;
                        }
                        ClearForNewCalc();
                        editor.AddDigit(char.Parse(command));
                        break;

                    case "MR":
                        if (memory.Num != null)
                        {
                            editor.Number = memory.Num.ToString();
                            isNewCalc = false;
                        }
                        break;
                    case "MS":
                        if (!string.IsNullOrEmpty(editor.Number))
                        {
                            memory.Num = ToNumber(editor.Number);
                            isNewCalc = true;
                        }
                        break;
                    case "MC":
                        memory.Clear();
                        isNewCalc = true;
                        break;
                    case "M+":
                        if (!string.IsNullOrEmpty(editor.Number))
                        {
                            memory.Add(ToNumber(editor.Number));
                            isNewCalc = true;
                        }
                        break;
                    case "M-":
                        if (!string.IsNullOrEmpty(editor.Number))
                        {
                            memory.Sub(ToNumber(editor.Number));
                            isNewCalc = true;
                        }
                        break;

                    case "-":
                        SetNum(command);
                        proc.RunOperation();
                        proc.op = prevOp = Operation.Sub;
                        SetResult();
                        break;
                    case "+":
                        SetNum(command);
                        proc.RunOperation();
                        proc.op = prevOp = Operation.Add;
                        SetResult();
                        break;
                    case "*":
                        SetNum(command);
                        proc.RunOperation();
                        proc.op = prevOp = Operation.Mult;
                        SetResult();
                        break;
                    case "÷":
                        SetNum(command);
                        proc.RunOperation();
                        proc.op = prevOp = Operation.Div;
                        SetResult();
                        break;
                    case "Sqr":
                        SetNum(command);
                        proc.func = Function.Sqr;
                        proc.RunFunction();
                        SetResult();
                        break;
                    case "Sqrt":
                        SetNum(command);
                        proc.func = Function.Sqrt;
                        proc.RunFunction();
                        SetResult();
                        break;
                    case "1/x":
                        SetNum(command);
                        proc.func = Function.Rev;
                        proc.RunFunction();
                        SetResult();
                        break;
                    case "=":
                        SetNum(command);
                        if (!isTimeToEqual && proc.l != null) break;
                        proc.RunOperation();
                        SetResult();
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
            if (!isNewCalc) return;
            editor.Clear();
            isNewCalc = false;
        }

        private void SetNum(string command)
        {
            proc.func = Function.None;
            if (editor.Number == "") return;

            if (editor.Number == "NaN") return;
            if (editor.Number == "∞")
            {
                prevNum = proc.l;
                proc.Reset();
                return;
            }
            var tmpNum = ToNumber(editor.Number);
            if (proc.l == null)
            {
                if (command == "=") return;
                prevNum = proc.l = tmpNum;
            }
            else
            {
                if (command == "=")
                {
                    if (!isTimeToEqual || prevNum == null) prevNum = tmpNum;
                    proc.op = prevOp;
                    proc.r = prevNum;
                    isTimeToEqual = true;
                    return;
                }
                if (operations.Contains(command) && proc.op == Operation.None && proc.r == null)
                {
                    prevNum = proc.l;
                    proc.l = tmpNum;
                    isTimeToEqual = false;
                    return;
                }
                isTimeToEqual = false;
                prevNum = null;
                if (isNewCalc && !operations.Contains(command))
                {
                    if (!isNewCalc) proc.r = tmpNum;
                    return;
                }
                prevNum = proc.r ?? tmpNum;
                proc.r = tmpNum;
            }
        }
        private void SetResult()
        {
            var num = proc.r ?? proc.l;
            editor.Number = num?.ToString();
    
            isNewCalc = true;
        }

        private string MakePreH()
        {
            if (proc?.l == null || isTimeToEqual) return "";
            var command = proc.func.ToString();

            var isContains = operations.Contains(command);
            var func = command + "(" + prevNum + ")";
            if (isContains && proc.r == null && proc.l != null)
                return func;

            var upString = proc.l + " " + OperationToString(proc.op) + " ";
            if (!isContains) return upString;

            return upString + func;
        }

        private string GetNum()
        {
            //рев для дробей не робит. деление на ноль. сделать нан
            if (isNewCalc && proc.r != null && proc.r.IsNaN())
            {
                proc.r = null;
                return "Недопустимый ввод";
            }
            if (isNewCalc && proc.l != null && proc.l.IsNaN())
            {
                proc.l = null;
                return "Недопустимый ввод";
            }
            return editor.Number;
        }

        private Number ToNumber(string n)
        {
            switch (mode)
            {
                case Mode.Real:
                    return new Real(n, Base);
                case Mode.Complex:
                    return new Complex(n, Base);
                case Mode.Frac:
                    return new Frac(n, Base);
                default:
                    throw new Exception("Ошибка приведения типа");
            }
        }

        public static string Convert(string number, int p1, int p2)
        {
            if (string.IsNullOrEmpty(number) || number == "0") return "0";
            if (p1 == p2) return number;
            return p1 == 10
                ? Converter10p.DoTrasfer(double.Parse(number.Replace('.', ',')), p2)
                : (p2 == 10
                    ? ConverterP10.DoTrasfer(number, p1).ToString(CultureInfo.InvariantCulture)
                    : Converter10p.DoTrasfer(ConverterP10.DoTrasfer(number, p1), p2));
        }

        public static string OperationToString(Operation op)
        {
            switch (op)
            {
                case Operation.Add: return "+";
                case Operation.Sub: return "-";
                case Operation.Div: return "÷";
                case Operation.Mult: return "*";
                default: return "";
            }
        }

        public void SetClipboard(string buffer)
        {
            bool toSet;
            var replace = buffer.Replace('.', dot).ToUpper();
            if (replace.Length > 30) toSet = false;
            else
            {
                if (replace.Contains(dot) && replace.Split(dot).ToArray().Length != 2) toSet = false;
                else if (replace.Contains('-') && replace.LastIndexOf('-') != 0) toSet = false;
                else
                {
                    var maxChar = Converter10p.IntToPChar(Base - 1);
                    toSet = (Base > 10)
                       ? !(replace.Any(c => (c < '0' || c > '9') && (c < 'A' || c > maxChar) && c != '-' && c != dot))
                       : !(replace.Any(c => (c < '0' || c > maxChar) && c != '-' && c != dot));
                }
            }
            if (toSet)
            {
                editor.Number = replace;
                isNewCalc = false;
            }
            else SystemSounds.Beep.Play();
        }
    }
}
