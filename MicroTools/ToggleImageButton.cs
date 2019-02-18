using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:ToggleImageButton/>
    ///
    /// </summary>
    public class ToggleImageButton : ToggleButton
    {
        static ToggleImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleImageButton), new FrameworkPropertyMetadata(typeof(ToggleImageButton)));
        }

        #region 依赖属性

        public string CheckNormalImagePath
        {
            get { return (string)GetValue(CheckNormalImagePathProperty); }
            set { SetValue(CheckNormalImagePathProperty, value); }
        }
        public static readonly DependencyProperty CheckNormalImagePathProperty = DependencyProperty.Register("CheckNormalImagePath", typeof(string), typeof(ToggleImageButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        public string CheckHoverImagePath
        {
            get { return (string)GetValue(CheckHoverImagePathProperty); }
            set { SetValue(CheckHoverImagePathProperty, value); }
        }
        public static readonly DependencyProperty CheckHoverImagePathProperty = DependencyProperty.Register("CheckHoverImagePath", typeof(string), typeof(ToggleImageButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        public string UnCheckNormalImagePath
        {
            get { return (string)GetValue(UnCheckNormalImagePathProperty); }
            set { SetValue(UnCheckNormalImagePathProperty, value); }
        }
        public static readonly DependencyProperty UnCheckNormalImagePathProperty = DependencyProperty.Register("UnCheckNormalImagePath", typeof(string), typeof(ToggleImageButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        public string UnCheckHoverImagePath
        {
            get { return (string)GetValue(UnCheckHoverImagePathProperty); }
            set { SetValue(UnCheckHoverImagePathProperty, value); }
        }
        public static readonly DependencyProperty UnCheckHoverImagePathProperty = DependencyProperty.Register("UnCheckHoverImagePath", typeof(string), typeof(ToggleImageButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));


        #endregion 依赖属性
        #region 事件
        //依赖属性发生改变时候触发
        private static void ImageSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

            try
            {
                Application.GetResourceStream(new Uri("pack://application:,,," + (string)e.NewValue));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }

        }
        #endregion 事件

    }
}
