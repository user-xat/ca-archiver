using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver
{
    class HuffmanTree
    {
        public char ch { get; private set; }
        public double freq { get; private set; }
        public bool isTerminal { get; private set; }
        public HuffmanTree left { get; private set; }
        public HuffmanTree rigth { get; private set; }

        public HuffmanTree(char c, double frequency)
        {
            ch = c;
            freq = frequency;
            isTerminal = true;
            left = rigth = null;
        }
        public HuffmanTree(HuffmanTree l, HuffmanTree r)
        {
            freq = l.freq + r.freq;
            isTerminal = false;
            left = l;
            rigth = r;
        }
    }
}
