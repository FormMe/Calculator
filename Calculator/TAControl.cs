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

        public Tuple<string,string,string> preH => MakePreH();
        public string editable => GetNum();
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
                    case "Re/Im":
                        ClearForNewCalc();
                        editor.ComplexSeparate();
                        isNewCalc = false;
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
                        if (isTimeToEqual || editor.Number == "∞")
                        {
                            proc.Reset();
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
            if (editor.Number == "∞" || proc.l?.ToString() == "∞" || proc.r?.ToString() == "∞")
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

        private Tuple<string, string, string> MakePreH()
        {
            if (proc?.l == null || isTimeToEqual) return null;
            var command = proc.func.ToString();
            
            var isContains = operations.Contains(command);
            
            var func = command + "(" + prevNum + ")";

            if (isContains && proc.r == null && proc.l != null)
                return new Tuple<string, string, string>(func, null,null);

           // var upString = proc.l + " " + OperationToString(proc.op) + " ";
            if (!isContains) return new Tuple<string, string, string>(proc.l.ToString(), OperationToString(proc.op), null);

            return new Tuple<string, string, string>(proc.l.ToString(), OperationToString(proc.op), func);
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
            buffer = buffer.ToUpper();
            if (buffer.Length > 30) SystemSounds.Beep.Play();
            else
            {
                var toSet = false;
                switch (mode)
                {
                    case Mode.Real:
                        toSet = SetNumClipboard(buffer.Replace('.', dot), dot);
                        break;
                    case Mode.Complex:
                        if (buffer.Contains("+"))
                        {
                            var splitedNumber = buffer.Split('+').ToArray();
                            toSet = SetNumClipboard(splitedNumber[0].Replace('.', dot), dot) &&
                                splitedNumber[1].Last() == 'I' &&
                                SetNumClipboard(splitedNumber[1].Replace('.', dot).Substring(0, splitedNumber[1].Length - 1), dot);
                        }
                        else
                        {
                            if (buffer.Last() == 'I' &&
                                SetNumClipboard(buffer.Replace('.', dot).Substring(0, buffer.Length - 1), dot))
                            {
                                buffer = "0 + " + buffer;
                                toSet = true;
                            }
                            else if (SetNumClipboard(buffer.Replace('.', dot), dot)) toSet = true;
                        }
                        break;
                    case Mode.Frac:
                        toSet = SetNumClipboard(buffer, '/');
                        break;
                    default:
                        SystemSounds.Beep.Play();
                        break;
                }

                if (toSet)
                {
                    editor.Number = buffer.Replace('.', dot);
                    isNewCalc = false;
                }
                else SystemSounds.Beep.Play();
            }
        }

        private bool SetNumClipboard(string buffer, char separator)
        {
            if (buffer.Contains(separator) && buffer.Split(separator).ToArray().Length != 2) return false;
            if (buffer.Contains('-') && buffer.LastIndexOf('-') != 0) return false;
            var maxChar = Converter10p.IntToPChar(Base - 1);
            return (Base > 10)
                ? !(buffer.Any(c => (c < '0' || c > '9') && (c < 'A' || c > maxChar) && c != '-' && c != separator))
                : !(buffer.Any(c => (c < '0' || c > maxChar) && c != '-' && c != separator));
        }
    }
}
