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
        /// <summary>
        /// Http方式下载文件
        /// </summary>
        /// <param name="yurl">http地址</param>
        /// <param name="ypath">本地文件</param>
        /// <returns></returns>
        public YingDownload(String yurl, String ypath)
        {
            bool yflag = false;
            long ystartPosition = 0; // 上次下载的文件起始位置
            FileStream yStream; // 写入本地文件流对象

            long yremoteFileLength = GetHttpLength(yurl);// 取得远程文件长度
            System.Console.WriteLine("remoteFileLength=" + yremoteFileLength);
            if (yremoteFileLength == 745)
            {
                System.Console.WriteLine("远程文件不存在.");
                return;
            }

            // 判断要下载的文件夹是否存在
            if (File.Exists(ypath))
            {

                yStream = File.OpenWrite(ypath);             // 存在则打开要下载的文件
                ystartPosition = yStream.Length;                  // 获取已经下载的长度

                if (ystartPosition >= yremoteFileLength)
                {
                    System.Console.WriteLine("本地文件长度" + ystartPosition + "已经大于等于远程文件长度" + yremoteFileLength);
                    yStream.Close();

                    return;
                }
                else
                {
                    yStream.Seek(ystartPosition, SeekOrigin.Current); // 本地文件写入位置定位
                }
            }
            else
            {
                yStream = new FileStream(ypath, FileMode.Create);// 文件不保存创建一个文件
                ystartPosition = 0;
            }


            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(yurl);// 打开网络连接

                if (ystartPosition > 0)
                {
                    myRequest.AddRange((int)ystartPosition);// 设置Range值,与上面的writeStream.Seek用意相同,是为了定义远程文件读取位置
                }


                Stream readStream = myRequest.GetResponse().GetResponseStream();// 向服务器请求,获得服务器的回应数据流


                byte[] btArray = new byte[512];// 定义一个字节数据,用来向readStream读取内容和向writeStream写入内容
                int contentSize = readStream.Read(btArray, 0, btArray.Length);// 向远程文件读第一次

                long currPostion = ystartPosition;

                while (contentSize > 0)// 如果读取长度大于零则继续读
                {
                    currPostion += contentSize;
                    int percent = (int)(currPostion * 100 / yremoteFileLength);
                    System.Console.WriteLine("percent=" + percent + "%");

                    yStream.Write(btArray, 0, contentSize);// 写入本地文件
                    contentSize = readStream.Read(btArray, 0, btArray.Length);// 继续向远程文件读取
                }

                //关闭流
                yStream.Close();
                readStream.Close();

                yflag = true;        //返回true下载成功
            }
            catch (Exception)
            {
                yStream.Close();
                yflag = false;       //返回false下载失败
            }

            return;
        }

        // 从文件头得到远程文件的长度
        private static long GetHttpLength(string url)
        {
            long length = 0;

            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);// 打开网络连接
                HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();

                if (rsp.StatusCode == HttpStatusCode.OK)
                {
                    length = rsp.ContentLength;// 从文件头得到远程文件的长度
                }

                rsp.Close();
                return length;
            }
            catch (Exception e)
            {
                return length;
            }

        }

    }
}
