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
        public Number num { set; get; }
        public string number => num.number;
        private bool stat = false;

        public Memory()
        {
        }
        
        public void Clear()
        {
            num = null;
            stat = false;
        }

        public void Add(Number e)
        {
            if (stat) num += e;
        }
    }
}
