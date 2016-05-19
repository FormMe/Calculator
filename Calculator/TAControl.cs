using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO.Packaging;
using System.Linq;
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

        public string preH = "";
        public string editable => editor.Number;
        public string memN => GetMem();
        public int Base
        {
            set
            {
                editor.Base = value;
                if (proc.r != null) proc.r.Base = value;
                if (proc.l != null) proc.l.Base = value;
                if (memory.Num != null) memory.Num.Base = value;
            }
            private get { return editor.Base; }
        }

        public Mode mode;

        string[] operations =
        {
            "Sqr",
            "1/x",
            "Sqrt"
        };

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
            if (!isNewCalc) return;
            editor.Clear();
            isNewCalc = false;
        }

        private void SetNum(string command)
        {
            if (editor.Number == "") return;

            if (proc.l == null)
                proc.l = ToNumber(editor.Number);
            else
            {
                if (!isNewCalc || operations.Contains(command))
                    proc.r = ToNumber(editor.Number);
            }
            MakePreH(command);
        }
        private void SetResult(string command)
        {
            if (proc.r == null) MakePreH(command);
            var num = proc.r ?? proc.l;
            editor.Number = num.ToString();
            isNewCalc = true;
        }

        private void MakePreH(string command)
        {
            var tmp = "";
            if (operations.Contains(command))
            {
                tmp = command == "1/x" ? "Rev" : command;
                var num = proc.r ?? proc.l;
                tmp += "(" + num + ")";
                preH = preH.Remove(preH.LastIndexOf(' '));
            }
            else
                preH = proc.l + " " + command + " ";
            preH += tmp;
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
            if (string.IsNullOrEmpty(number) || number == "0" ) return "0";
            if (p1 == p2) return number;
            return p1 == 10
                ? Converter10p.DoTrasfer(double.Parse(number.Replace('.', ',')), p2)
                : (p2 == 10
                    ? ConverterP10.DoTrasfer(number, p1).ToString(CultureInfo.InvariantCulture)
                    : Converter10p.DoTrasfer(ConverterP10.DoTrasfer(number, p1), p2));
        }
    }
}
