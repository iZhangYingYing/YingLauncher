﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    }

    public class YingFileHandle : YingYing
    {
        private static DirectoryInfo yfolder = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".Ying"));

        //yresources
        private static DirectoryInfo yresourcesfolder = new DirectoryInfo(Path.Combine(yfolder.FullName, "yresources"));
        private static YingResources yresouces = new YingResources
        {
            yimages = new DirectoryInfo(Path.Combine(yresourcesfolder.FullName, "yimages")),
            ymedia = new DirectoryInfo(Path.Combine(yresourcesfolder.FullName, "ymedia")),
            yplugins = new DirectoryInfo(Path.Combine(yresourcesfolder.FullName, "yplugins")),
            ysongs = new DirectoryInfo(Path.Combine(yresourcesfolder.FullName, "ysongs")),
        };

        
        public DirectoryInfo getYDataFolder() {
            if (!yfolder.Exists) yfolder.Create();
            return yfolder;
        }

        public DirectoryInfo getYResourcesFolder()
        {
            this.getYDataFolder();
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
            return yresouces;
        }
    }
}
