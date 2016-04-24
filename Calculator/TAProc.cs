using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Calculator
{
    enum Operation
    {
        None,
        Add,
        Sub,
        Mult,
        Div
    };
    enum Function
    {
        None,
        Rev,
        Sqr
    };
    class Proc
    {
        public Number l { get; set; }
        public Number r { get; set; }
        public Operation op { get; set; }
        public Function func { get; set; }

        public Proc(Number l, Number r)
        {
            op = Operation.None;
            func = Function.None;
            this.l = l;
            this.r = r;
        }

        public void RunOperation()
        {
            try
            {
                switch (op)
                {
                    case Operation.None:
                        break;
                    case Operation.Add:
                        l += r;
                        break;
                    case Operation.Sub:
                        l -= r;
                        break;
                    case Operation.Mult:
                        l *= r;
                        break;
                    case Operation.Div:
                        l /= r;
                        break;
                }
                r = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void RunFunction()
        {
            try
            {
                switch (func)
                {
                    case Function.None:
                        break;
                    case Function.Rev:
                        l.Rev();
                        break;
                    case Function.Sqr:
                        l.Sqr();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Reset()
        {
            op = Operation.None;
            func = Function.None;
            l = r = null;
        }


        //private Mode _mode;
        //public Mode mode
        //{
        //    get { return _mode; }
        //    set { SetMode(value); }
        //}
        //private void SetMode(Mode m)
        //{
        //    switch (m)
        //    {
        //        case Mode.real:
        //            {
        //                L = new Real(0);
        //                R = new Real(0);
        //                break;
        //            }
        //        case Mode.complex:
        //            {
        //                L = new Complex(0,0);
        //                R = new Complex(0,0);
        //                break;
        //            }
        //        case Mode.frac:
        //            {
        //                L = new Frac(0,1);
        //                R = new Frac(0,1);
        //                break;
        //            }

        //    }
        //}


    }
}
