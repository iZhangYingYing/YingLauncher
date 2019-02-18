using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ying.YingUtil
{
    class YingFile
    {


        public static Byte[] YReadFileAsync(FileInfo yfile)
        {
            if (!yfile.Exists) throw new FileNotFoundException();


            FileStream ystream = yfile.OpenRead();
            //Read all bytes into an array from the specified file.
            int ylength = (int)ystream.Length;//计算流的长度
            Byte[] ybuffer = new Byte[ylength];//初始化用于MemoryStream的Buffer
            int yread = ystream.Read(ybuffer, 0, ylength);//将File里的内容一次性的全部读到byteArray中去

            return ybuffer;

            /*using (MemoryStream yystream = new MemoryStream(byteArray))//初始化MemoryStream,并将Buffer指向FileStream的读取结果数组
            {
                // your code
            }*/
        }
    }
}
