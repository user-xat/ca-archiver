using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Archiver
{
    class HuffmanInfo
    {
        HuffmanTree Tree; // дерево кода Хаффмана, потребуется для распаковки
        Dictionary<char, string> Table; // словарь, хранящий коды всех символов, будет удобен для сжатия

        public HuffmanInfo(string fileName)
        {
            //var temp = GetFreq(fileName);
            //foreach (char key in temp.Keys)
            //{
            //    Console.WriteLine(key + " = " + temp[key]);
            //}
            Console.WriteLine("Create freq-table");
            PriorityQueue queue = new PriorityQueue(GetFreq(fileName));
            Tree = CreateHuffmanTree(queue);
            Table = new Dictionary<char, string>();
            GetSymbolCodes(Tree, "");
            //foreach (char key in Table.Keys)
            //{
            //    Console.WriteLine(key + " = " + Table[key]);
            //}
        }

        private Dictionary<char, double> GetFreq(string filename)
        {
            string line;
            Dictionary<char, double> freq = new Dictionary<char, double>();
            StreamReader sr = new StreamReader(filename, Encoding.Unicode);
            // считать информацию о частотах символов
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Length == 0)
                {
                    //TODO: отдельная обработка строки, которой соответствует частота символа "конец строки" 
                    line = sr.ReadLine().Trim();
                    freq.Add('\n', Convert.ToDouble(line));
                }
                else
                {
                    //TODO: обработка частот символов
                    freq.Add(line[0], Convert.ToDouble(line.Substring(2)));
                }
            }
            sr.Close();
            freq.Add('\0', 0);
            return freq;
        }

        private HuffmanTree CreateHuffmanTree(PriorityQueue queue)
        {
            while(queue.Size() > 1)
            {
                var first = queue.Top();
                var second = queue.Top();
                HuffmanTree node = new HuffmanTree(first, second);
                queue.Insert(node);
            }
            return queue.Top();
        }

        private void GetSymbolCodes(HuffmanTree node, string code)
        {
            if(node.isTerminal)
            {
                Table.Add(node.ch, code);
            }
            else
            {
                GetSymbolCodes(node.left, code + "0");
                GetSymbolCodes(node.rigth, code + "1");
            }
        }

        public void Compress(string inpFile, string outFile)
        {
            Console.WriteLine("Archiving...");
            var sr = new StreamReader(inpFile, Encoding.Unicode);
            var sw = new ArchWriter(outFile); //нужна побитовая запись, поэтому использовать StreamWriter напрямую нельзя
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                // TODO: посимвольно обрабатываем строку, кодируем, пишем в sw
                foreach(char ch in line)
                {
                    sw.WriteWord(Table[ch]);
                }
                sw.WriteWord(Table['\n']);
            }
            sr.Close();
            sw.WriteWord(Table['\0']); // записываем признак конца файла
            sw.Finish();
        }

        public void Decompress(string archFile, string txtFile)
        {
            Console.WriteLine("Unarchiving...");
            var sr = new ArchReader(archFile); // нужно побитовое чтение
            var sw = new StreamWriter(txtFile, false, Encoding.Unicode);
            byte curBit;
            HuffmanTree node = Tree;
            while (sr.ReadBit(out curBit))
            {
                if (curBit == 0) node = node.left;
                else node = node.rigth;
                if (node.isTerminal)
                {
                    if (node.ch == '\0') break;
                    sw.Write(node.ch);
                    node = Tree;
                }
            }
            sr.Finish();
            sw.Close();
        }
    }
}
