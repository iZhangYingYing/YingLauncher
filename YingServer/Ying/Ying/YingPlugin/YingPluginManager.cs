using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ying.YingPlugin
{
    public class YingPluginManager : YingYing
    {

        private List<iYingPlugin> yplugins = new List<iYingPlugin>();

        public YingPluginManager()
        {
            Task.Run(() => {
                this.Ying();
            });
            
        }

        public void Ying()
        {
            
            List<string> pluginpath = this.FindPlugin();

            getYConsole().sendYMessage(JsonMapper.ToJson(pluginpath));

            pluginpath = this.DeleteInvalidPlungin(pluginpath);

            foreach (string filename in pluginpath)
            {


                try
                {
                    //获取文件名
                    string asmfile = filename;
                    string asmname = Path.GetFileNameWithoutExtension(asmfile);
                    if (asmname != string.Empty)
                    {
                        // 利用反射,构造DLL文件的实例
                        Assembly asm = Assembly.LoadFile(asmfile);
                        //利用反射,从程序集(DLL)中,提取类,并把此类实例化
                        Type[] t = asm.GetExportedTypes();
                        foreach (Type type in t)
                        {
                            if (type.GetInterface("iYingPlugin") != null)
                            {
                                iYingPlugin y = (iYingPlugin)Activator.CreateInstance(type);
                                y.onYEnable();


                                yplugins.Add(y);

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    getYConsole().sendYMessage(ex.Message);
                }
            }
        }

        //查找所有插件的路径
        private List<string> FindPlugin()
        {
            List<string> pluginpath = new List<string>();
            try
            {
                //获取程序的基目录
                string ypath = AppDomain.CurrentDomain.BaseDirectory;
                //合并路径，指向插件所在目录。
                ypath = Path.Combine(ypath, "yplugins");
                foreach (string filename in Directory.GetFiles(ypath, "*.dll"))
                {
                    pluginpath.Add(filename);
                }
            }
            catch (Exception ex)
            {
                getYConsole().sendYMessage(ex.Message);
            }
            return pluginpath;
        }
        //载入插件，在Assembly中查找类型
        private object LoadObject(Assembly asm, string className, string interfacename
                        , object[] param)
        {
            try
            {
                //取得className的类型
                Type t = asm.GetType(className);
                if (t == null
                    || !t.IsClass
                    || !t.IsPublic
                    || t.IsAbstract
                    || t.GetInterface(interfacename) == null
                   )
                {
                    return null;
                }
                //创建对象
                Object o = Activator.CreateInstance(t, param);
                if (o == null)
                {
                    //创建失败，返回null
                    return null;
                }
                return o;
            }
            catch
            {
                return null;
            }
        }
        //移除无效的的插件，返回正确的插件路径列表，Invalid:无效的
        private List<string> DeleteInvalidPlungin(List<string> PlunginPath)
        {
            string interfacename = typeof(iYingPlugin).FullName;
            List<string> rightPluginPath = new List<string>();
            //遍历所有插件。
            foreach (string filename in PlunginPath)
            {
                try
                {
                    Assembly asm = Assembly.LoadFile(filename);
                    //遍历导出插件的类。
                    foreach (Type t in asm.GetExportedTypes())
                    {
                        //查找指定接口
                        Object plugin = LoadObject(asm, t.FullName, interfacename, null);
                        //如果找到，将插件路径添加到rightPluginPath列表里，并结束循环。
                        if (plugin != null)
                        {
                            rightPluginPath.Add(filename);
                            break;
                        }
                    }
                }
                catch
                {
                    throw new Exception(filename + "不是有效插件");
                }
            }
            return rightPluginPath;
        }

        public async void YDisableAll() {
            await Task.Run(() => {
                foreach (iYingPlugin y in this.yplugins)
                {
                    Task.Run(() => {
                        y.onYDisable();
                    });
                }
            });         
        }
    }

}
