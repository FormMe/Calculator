using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class Memory
    {
        public Number num { set; get; }
        private bool stat = false;

        public Memory()
        {
            
        }

        public void Store(Number num)
        {
            this.num = num;
            stat = true;
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
