using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ying.YingUtils
{

    public struct YingResources
    {

        public DirectoryInfo yimages { get; set; }
        public DirectoryInfo ymedia { get; set; }
        public DirectoryInfo yplugins { get; set; }
        public DirectoryInfo ysongs { get; set; }
        public DirectoryInfo ygame { get; set; }
    }

    public class YingFileHandle : YingYing
    {
        //yresources
        private static DirectoryInfo yresourcesfolder = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "yresources"));
        private static YingResources yresouces = new YingResources
        {
            yimages = new DirectoryInfo(Path.Combine(yresourcesfolder.FullName, "yimages")),
            ymedia = new DirectoryInfo(Path.Combine(yresourcesfolder.FullName, "ymedia")),
            yplugins = new DirectoryInfo(Path.Combine(yresourcesfolder.FullName, "yplugins")),
            ysongs = new DirectoryInfo(Path.Combine(yresourcesfolder.FullName, "ysongs")),
            ygame = new DirectoryInfo(Path.Combine(yresourcesfolder.FullName, "ygame"))
        };

        public DirectoryInfo getYResourcesFolder()
        {
            if (!yresourcesfolder.Exists) yresourcesfolder.Create();
            return yresourcesfolder;
        }

        public YingResources getYResources()
        {
            this.getYResourcesFolder();
            if (!yresouces.yimages.Exists) yresouces.yimages.Create();
            if (!yresouces.ymedia.Exists) yresouces.ymedia.Create();
            if (!yresouces.yplugins.Exists) yresouces.yplugins.Create();
            if (!yresouces.ysongs.Exists) yresouces.ysongs.Create();
            if (!yresouces.ygame.Exists) yresouces.ygame.Create();
            return yresouces;
        }
        
        /// <summary>
        /// 从资源文件中抽取资源文件
        /// </summary>
        /// <param name="yresFileName">资源文件名称（资源文件名称必须包含目录，目录间用“.”隔开,最外层是项目默认命名空间）</param>
        /// <param name="youtputFile">输出文件</param>
        public void getYResource(string yresFileName, string youtputFile)
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

        public Stream getYResource(string yresFileName)
        {
            try
            {
                Assembly asm = Assembly.GetExecutingAssembly();//读取嵌入式资源
                return asm.GetManifestResourceStream(yresFileName);
            }
            catch (Exception yexception)
            {
                return null;
            }


        }
    }




}
