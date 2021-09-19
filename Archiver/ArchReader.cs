using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver
{
    class ArchReader
    {
        const byte bufSize = 10;
        byte[] buf;
        byte[] oneByte; //actually bits[]
        byte bitsCount;
        byte bytesCount, byteIdx;
        FileStream fs;
        /// <summary>
        /// Создает класс для побитового чтения из архива. Для окончании чтения используйте метод Finish
        /// </summary>
        /// <param name="fileName"></param>
        public ArchReader(string fileName)
        {
            buf = new byte[bufSize];
            oneByte = new byte[8];
            bitsCount = bytesCount = byteIdx = 0;
            fs = new FileStream(fileName, FileMode.Open);
        }
        private bool ReadByte()
        {
            if (bytesCount == byteIdx)
            {
                bytesCount = (byte)fs.Read(buf, 0, bufSize);
                byteIdx = 0;
            }
            if (bytesCount == 0)
                return false;
            var onebyte = buf[byteIdx++];
            for (int i = 0; i < 8; i++)
            {
                oneByte[i] = (byte)(onebyte & 1);
                onebyte = (byte)(onebyte >> 1);
            }
            bitsCount = 8;
            return true;
        }
        /// <summary>
        /// Чтение из файла одного бита
        /// </summary>
        /// <param name="bit">Значение прочитанного бита</param>
        /// <returns>true, если чтение успешно; false если достигнут конец файла</returns>
        public bool ReadBit(out byte bit)
        {
            bool res = true;
            if (bitsCount == 0)
                res = ReadByte();
            if (res)
                bit = oneByte[--bitsCount];
            else
                bit = 0;
            return res;
        }
        /// <summary>
        /// Завершает чтение из файла (закрывает файл)
        /// </summary>
        public void Finish()
        {
            fs.Close();
        }
    }
}
