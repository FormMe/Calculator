using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    class Cntrl
    {
        public History history;
        private Editor editor;
        private Memory memory;
        private Proc proc;
        public Number num;

        public Mode mode;
        
        private UT_10_p converter10P;
        private UT_p_10 converterP10;

        public string DoCommand(int n, string command)
        {
            try
            {
                switch (n)
                {
                    case 0:
                        DoEditCommand(command);
                        break;
                    case 1:
                        DoMemoryCommand(command);
                        break;
                    case 2:
                        DoProcCommand(command);
                        break;
                    case 3:
                        DoEditCommand(command);
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return "0";
            }

            return editor.Number;

        }

        private void DoEditCommand(string command)
        {
            switch (command)
            {
                case "Преобразовать":
                case "Enter":
                case "Return":
                    break;
                case ".":
                case "Decimal":
                    editor.Separate(); break;
                case "+/-":
                case "Subtract":
                    editor.Sign(); break;
                case "Clear":
                case "Delete":
                    editor.Clear(); break;
                case "BS":
                case "Back":
                    editor.BackSpace(); break;
                default:
                    editor.AddDigit(char.Parse(command)); break;
            }
        }

        private void DoMemoryCommand(string command)
        {
            switch (command)
            {
                case "Преобразовать":
                case "Enter":
                case "Return":
                    break;
                case ".":
                case "Decimal":
                    editor.Separate(); break;
                case "+/-":
                case "Subtract":
                    editor.Sign(); break;
                case "Clear":
                case "Delete":
                    editor.Clear(); break;
                case "BS":
                case "Back":
                    editor.BackSpace(); break;
                default:
                    editor.AddDigit(char.Parse(command)); break;
            }
        }

        private void DoProcCommand(string command)
        {
            switch (command)
            {
                case "Преобразовать":
                case "Enter":
                case "Return":
                    break;
                case ".":
                case "Decimal":
                    editor.Separate(); break;
                case "+/-":
                case "Subtract":
                    editor.Sign(); break;
                case "Clear":
                case "Delete":
                    editor.Clear(); break;
                case "BS":
                case "Back":
                    editor.BackSpace(); break;
                default:
                    editor.AddDigit(char.Parse(command)); break;
            }
        }
    }
}
