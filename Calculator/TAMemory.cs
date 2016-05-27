using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class Memory
    {
        private bool stat = false;

        private Number _num;

        public Number Num
        {
            get { return _num; }
            set
            {
                stat = true;
                _num = value;
            }
        }
        public void Clear()
        {
            _num = null;
            stat = false;
        }

        public void Add(Number e)
        {
            if (!stat)
            {
                _num = e;
                stat = true;
            }
            else _num += e;
        }

        public void Sub(Number e)
        {
            if (!stat)
            {
                _num = -e;
                stat = true;
            }
            else _num -= e;
        }
    }
}
