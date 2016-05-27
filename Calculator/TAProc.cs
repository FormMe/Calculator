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
        None = ' ',
        Add = '+',
        Sub = '-',
        Mult = '*',
        Div = '/'
    };
    enum Function
    {
        None,
        Rev,
        Sqr,
        Sqrt
    };

    class Proc
    {
        public Number l { get; set; }
        public Number r { get; set; }

        public Operation op;
        public Function func;

        public bool isRightFunc = false;

        public Proc()
        {
            op = Operation.None;
            func = Function.None;
        }

        public void RunOperation()
        {
            try
            {
                if (r == null || l == null) return;
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
                op = Operation.None;
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
                        if (r != null) r = r.Rev();
                        else l = l.Rev();
                        break;
                    case Function.Sqr:
                        if (r != null) r = r.Sqr();
                        else l = l.Sqr();
                        break;
                    case Function.Sqrt:
                        if (r != null) r = r.Sqrt();
                        else l = l.Sqrt();
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
