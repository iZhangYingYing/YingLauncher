using System;
using System.IO;
using System.Reflection;

namespace Ying.YingUtils
{
    public static class YingTools
    {

        /// <summary>
        /// 对类型为T的数组进行扩展，把满足条件的元素移动到数组的最前面
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="yarray">源数组</param>
        /// <param name="ymatch">lamda表达式</param>
        /// <returns></returns>
        public static bool YMoveToFront<T>(this T[] yarray, Predicate<T> ymatch)
        {
            //如果数组的长度为0
            if (yarray.Length == 0)
            {
                return false;
            }
            //获取满足条件的数组元素的索引
            int yindex = Array.FindIndex(yarray, ymatch);
            //如果没有找到满足条件的数组元素
            if (yindex == -1)
            {
                return false;
            }
            //把满足条件的数组元素赋值给临时变量
            var ytemp = yarray[yindex];
            Array.Copy(yarray, 0, yarray, 1, yindex);
            yarray[0] = ytemp;
            return true;
        }

        /// <summary>
        /// 从资源文件中抽取资源文件
        /// </summary>
        /// <param name="yresFileName">资源文件名称（资源文件名称必须包含目录，目录间用“.”隔开,最外层是项目默认命名空间）</param>
        /// <param name="youtputFile">输出文件</param>
        public static void YExtractResFile(string yresFileName, string youtputFile)
        {
            BufferedStream yinStream = null;
            FileStream youtStream = null;
            try
            {
                Assembly asm = Assembly.GetExecutingAssembly(); //读取嵌入式资源
                yinStream = new BufferedStream(asm.GetManifestResourceStream(yresFileName));
                youtStream = new FileStream(youtputFile, FileMode.Create, FileAccess.Write);

                byte[] buffer = new byte[1024];
                int length;

                while ((length = yinStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    youtStream.Write(buffer, 0, length);
                }
                youtStream.Flush();
            }
            finally
            {
                if (youtStream != null)
                {
                    youtStream.Close();
                }
                if (yinStream != null)
                {
                    yinStream.Close();
                }
            }
        }

    }
}
