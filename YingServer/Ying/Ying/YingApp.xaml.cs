using System.Windows;
using System.Windows.Media;
using System.Threading;

using System.IO;

using System.Linq;
using System.Collections.ObjectModel;

namespace Ying
{
    /// <summary>
    /// YingApp.xaml 的交互逻辑
    /// </summary>
    public partial class YingApp : Application
    {
       

        private static Mutex _mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            _mutex = new Mutex(true, "Ying Server", out bool ret);
            if (!ret)
            {
                MessageBox.Show("已经有一个我在运行了", "(>ㅂ< )", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Shutdown(0);
            }

            Dispatcher.UnhandledException += UnhandledExceptionHandler;

            System.Console.WriteLine("Ying: App Run");

            base.OnStartup(e);
        }

        

       

        void UnhandledExceptionHandler(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"异常信息：{e.Exception.Message}\n异常源：{e.Exception.StackTrace}", "程序发生了无法处理的异常！", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown(-1);
            e.Handled = true;
        }

    }
}
