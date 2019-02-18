using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ying.YingUtil
{
    class YingDownload
    {
        public StreamReader YReader { get; }
        public YingDownload(String yurl, Encoding yencoding)
        {
            try
            {
                WebRequest yrequest = WebRequest.Create(yurl);
                WebResponse yresponse = yrequest.GetResponse();
                Stream ystream = yresponse.GetResponseStream();
                this.YReader = new StreamReader(ystream, yencoding);
            }
            catch (Exception yexception)
            {
                this.YReader = null;
            }
            /*
            //yreader.ReadBlock

            /*Console.WriteLine("Ying: " + ystream.Length);

            MemoryStream ms = new MemoryStream();
            Byte[] ybuffer = new Byte[1024];
            int ysize = ystream.Read(ybuffer, 0, (int)ybuffer.Length);
            while (ysize > 0)
            {
                //stream.Write(bArr, 0, size);
                ms.Write(ybuffer, 0, ysize);
                ysize = ystream.Read(ybuffer, 0, (int)ybuffer.Length);
            }*/

            /*//Stream stream = new FileStream(tempFile, FileMode.Create);
            Char[] ybuffer = new Char[1024];
            int ysize = yreader.ReadBlock(ybuffer, 0, 1024);
            while (ysize > 0)
            {
                ysize = yreader.ReadBlock(ybuffer, 0, 1024);
            }
            MemoryStream ms = new MemoryStream();
            
            return ms;*/
        }

    }
}
