using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MicroTools
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:MicroTools"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:MicroTools;assembly=MicroTools"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class ImageButton : Button
    {
        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }
        #region 依赖属性

        public double ImageSize
        {
            get { return (double)GetValue(ImageSizeProperty); }
            set { SetValue(ImageSizeProperty, value); }
        }
        public static readonly DependencyProperty ImageSizeProperty = DependencyProperty.Register("ImageSize", typeof(double), typeof(ImageButton),
            new FrameworkPropertyMetadata(30.0, FrameworkPropertyMetadataOptions.AffectsRender));


        public string NormalImagePath
        {
            get { return (string)GetValue(NormalImagePathProperty); }
            set { SetValue(NormalImagePathProperty, value); }
        }
        public static readonly DependencyProperty NormalImagePathProperty = DependencyProperty.Register("NormalImagePath", typeof(string), typeof(ImageButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        public string HoverImagePath
        {
            get { return (string)GetValue(HoverImagePathProperty); }
            set { SetValue(HoverImagePathProperty, value); }
        }
        public static readonly DependencyProperty HoverImagePathProperty = DependencyProperty.Register("HoverImagePath", typeof(string), typeof(ImageButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        public string PressImagePath
        {
            get { return (string)GetValue(PressImagePathProperty); }
            set { SetValue(PressImagePathProperty, value); }
        }
        public static readonly DependencyProperty PressImagePathProperty = DependencyProperty.Register("PressImagePath", typeof(string), typeof(ImageButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        public string DisableImagePath
        {
            get { return (string)GetValue(DisableImagePathProperty); }
            set { SetValue(DisableImagePathProperty, value); }
        }
        public static readonly DependencyProperty DisableImagePathProperty = DependencyProperty.Register("DisableImagePath", typeof(string), typeof(ImageButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        public Visibility BorderVisibility
        {
            get { return (Visibility)GetValue(BorderVisibilityProperty); }
            set { SetValue(BorderVisibilityProperty, value); }
        }
        public static readonly DependencyProperty BorderVisibilityProperty =
            DependencyProperty.Register("BorderVisibility", typeof(Visibility), typeof(ImageButton),
            new FrameworkPropertyMetadata(Visibility.Hidden, FrameworkPropertyMetadataOptions.AffectsRender));

        #endregion 依赖属性

        #region 事件
        //依赖属性发生改变时候触发
        private static void ImageSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {


            try
            {
                Application.GetResourceStream(new Uri("pack://application:,,," + (string)e.NewValue));
                //string path = System.IO.Path.GetFullPath((string)e.NewValue);
                //Uri url = new Uri(path, UriKind.Relative);
                // Application.GetResourceStream(url);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }

        }
        #endregion 事件
   
    }
}
