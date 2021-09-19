using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver
{
    class ArchWriter
    {
        const byte bufSize = 10;
        byte[] buf;
        byte oneByte;
        byte bitsCount;
        byte bytesCount;
        FileStream fs;
        /// <summary>
        /// Создает класс для побитовой записи архива. Для окончании записи используйте метод Finish
        /// </summary>
        /// <param name="fileName"> имя файла архива </param>
        public ArchWriter(string fileName)
        {
            buf = new byte[bufSize];
            oneByte = 0;
            bitsCount = bytesCount = 0;
            fs = new FileStream(fileName, FileMode.Create);
        }
        private void WriteByte()
        {
            buf[bytesCount++] = oneByte;
            oneByte = 0;
            bitsCount = 0;
            if (bytesCount == bufSize)
            {
                fs.Write(buf, 0, bufSize);
                bytesCount = 0;
            }
        }
        /// <summary>
        /// Запись в файл одного бита
        /// </summary>
        /// <param name="bit">Значение записываемого бита (0 или 1)</param>
        public void WriteBit(byte bit)
        {
            oneByte = (byte)((oneByte << 1) + bit);
            bitsCount++;
            if (bitsCount == 8)
                WriteByte();
        }
        /// <summary>
        /// Запись в файл битовой строки
        /// </summary>
        /// <param name="w">Битовая строка (должна содержать только символы 0 и 1)</param>
        public void WriteWord(string w)
        {
            foreach (var c in w)
                if (c == '0')
                    WriteBit(0);
                else if (c == '1')
                    WriteBit(1);
        }
        /// <summary>
        /// Завершает запись файла
        /// </summary>
        public void Finish()
        {
            while (bitsCount > 0) WriteBit(0);
            if (bytesCount > 0)
                fs.Write(buf, 0, bytesCount);
            fs.Close();
        }
    }
}
