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

        private Operation _op;

        public Operation op
        {
            get { return _op; }
            set
            {
                if (_op != Operation.None)
                    RunOperation();
                _op = value;
            }
        }

        private Function _func;

        public Function func
        {
            get { return _func; }
            set
            {
                if (_func != Function.None)
                    RunFunction();
                _func = value;
            }
        }

        public Proc()
        {
            op = Operation.None;
            func = Function.None;
        }

        public void RunOperation()
        {
            try
            {
                if(r == null || l == null) return;
                switch (_op)
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
