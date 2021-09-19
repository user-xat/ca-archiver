using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver
{
    class PriorityQueue
    {
        int curNode;
        HuffmanTree[] heap;

        public PriorityQueue(Dictionary<char, double> data)
        {
            heap = new HuffmanTree[data.Keys.Count];
            curNode = -1;

            foreach (char key in data.Keys)
                Insert(new HuffmanTree(key, data[key]));
        }

        public int Size() => curNode + 1;
        private int Parent(int i) => i / 2;
        private int Left(int i) => 2 * i;
        private int Right(int i) => 2*i + 1;

        private void HeapifyUp(int i)
        {
            while (i > 0 && heap[Parent(i)].freq > heap[i].freq)
            {
                HuffmanTree temp = heap[Parent(i)];
                heap[Parent(i)] = heap[i];
                heap[i] = temp;
                i = Parent(i);
            }
        }

        private void HeapifyDown(int i)
        {
            while(i <= curNode)
            {
                int left = Left(i);
                int right = Right(i);

                int smallest = i;
                if (left <= curNode && heap[left].freq < heap[i].freq)
                    smallest = left;
                else if (right <= curNode && heap[right].freq < heap[i].freq)
                    smallest = right;
                if (smallest != i)
                {
                    HuffmanTree temp = heap[i];
                    heap[i] = heap[smallest];
                    heap[smallest] = temp;
                }
                else
                    break;
                i = smallest;
            }
        }

        public bool Insert(HuffmanTree node)
        {
            if (curNode + 1 == heap.Length)
                return false;
            curNode++;
            heap[curNode] = node;
            HeapifyUp(curNode);
            return true;
        }

        public HuffmanTree Top()
        {
            if (curNode < 0)
                return null;
            HuffmanTree min = heap[0];
            heap[0] = heap[curNode];
            curNode--;
            HeapifyDown(0);
            return min;
        }
    }
}
