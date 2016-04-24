using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
