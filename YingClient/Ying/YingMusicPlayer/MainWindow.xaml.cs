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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace YingMusicPlayer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {



    private Timer _Timer1;//绘制谱线
                          //private Timer _Timer2;//屏幕取色

    public MainWindow()
    {
        InitializeComponent();

        //开启双缓存
        SetStyle(ControlStyles.UserPaint, true);
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle(ControlStyles.ResizeRedraw, true);
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);//ControlStyles.Opaque | 

        _Timer1 = new Timer();
        _Timer1.Interval = _Speed;
        _Timer1.Tick += new EventHandler(_Timer1_Tick);

        //_Timer2 = new Timer();
        //_Timer2.Interval = 1000;
        //_Timer2.Tick += new EventHandler(_Timer2_Tick);

    }


    #region 谱线的字段及属性

    Pen p = new Pen(Color.FromArgb(150, 0, 0, 255), 3);
    Pen p2 = new Pen(Color.FromArgb(150, 0, 0, 255), 4);
    private float _DeviationAngle = 0;//偏移角度
    double offsetAngle = (double)360 / 90;//谱线间相隔角度（180/45）
    private PointF _CenterPointF = new PointF(250, 250);//频谱中心点
    private int _LineNum = 45;//谱线数目（一半，为了对称好看）
    private int _LineWidth = 3;//线宽
    private int _LineAlpha = 50;//线透明度
    private int _LineStopHeight = 5;//谱线平常高度
    private int _LineHeight = 0;//数据填充时谱线高度
    private int _LineR = 100;//谱线的RGB值
    private int _LineG = 100;
    private int _LineB = 100;
    private int _Speed = 100;//刷新频率
    private float[] fft;
    //private float[] _OldFFT;//上一次的数据
    private int[] _LineAlphas = new int[45];//谱线的渐变透明度
    private int[] _OldH = new int[45];//外圈谱线上一次高度
    private int[] _OldH2 = new int[45];//内圈谱线上一次高度
    private string _NewTime = "00:00";//已播放的时间
    private string _EndTime = "00:00";//播放总时间
    private string _Title = "听音乐";//歌曲名
    private string _Artist = "心灵之音";//作家
    private int _FontAlpha = 200;//字体透明度

    /// <summary>
    /// 歌曲作家
    /// </summary>
    public string Artist
    {
        get { return _Artist; }
        set
        {
            if (value.Length >= 32)
            {
                _Artist = value.Substring(0, 30) + "...";
            }
            else
            {
                _Artist = value;
            }
        }
    }

    /// <summary>
    /// 歌曲名
    /// </summary>
    public string Title
    {
        get { return _Title; }
        set
        {
            if (value.Length >= 32)
            {
                if (value.Contains('-'))
                {
                    string[] s = value.Split(new char[] { '-' });
                    _Title = s[0].Trim();
                }
                else
                {
                    _Title = value.Substring(0, 15) + "...";
                }
            }
            else if (value.Length > 12 && value.Length < 32)
            {
                if (value.Contains('-'))
                {
                    string[] s = value.Split(new char[] { '-' });
                    _Title = s[1].Trim();
                    _Artist = s[0].Trim();
                }
                else
                {
                    _Title = value.Substring(0, 10) + "...";
                }
            }
            else
            {
                _Title = value;
            }
        }
    }

    /// <summary>
    /// 字体透明度
    /// </summary>
    public int FontAlpha
    {
        get { return _FontAlpha; }
        set
        {
            if (value >= 0 && value <= 255)
            {
                _FontAlpha = value;
            }
            else if (value < 0)
            {
                _FontAlpha = 0;
            }
            else
            {
                _FontAlpha = 255;
            }
        }
    }

    /// <summary>
    /// 歌曲总时间
    /// </summary>
    public string EndTime
    {
        get { return _EndTime; }
        set { _EndTime = value; }
    }

    /// <summary>
    /// 歌曲当前播放时间
    /// </summary>
    public string NewTime
    {
        get { return _NewTime; }
        set { _NewTime = value; }
    }

    /// <summary>
    /// 傅氏转化数组
    /// </summary>
    public float[] FFT
    {
        get { return fft; }
        set
        {
            if (value != null && value.Length >= _LineNum)
            {
                fft = value;
            }
            else
            {
                fft = null;
            }
        }
    }

    /// <summary>
    /// 谱线无数据填充时的高度
    /// </summary>
    public int LineStopHeight
    {
        get { return _LineStopHeight; }
        set
        {
            if (value >= 0)
            {
                _LineStopHeight = value;
            }
        }
    }

    /// <summary>
    /// 谱线透明度
    /// </summary>
    public int LineAlpha
    {
        get { return _LineAlpha; }
        set
        {
            if (value >= 0 && value <= 255)
            {
                _LineAlpha = value;
            }
        }
    }

    /// <summary>
    /// 谱线宽度
    /// </summary>
    public int LineWidth
    {
        get { return _LineWidth; }
        set
        {
            if (value >= 0)
            {
                _LineWidth = value;
            }
        }
    }

    /// <summary>
    /// 谱线数目
    /// </summary>
    public int LineNum
    {
        get { return _LineNum; }
        set
        {
            if (value >= 0)
            {
                _LineNum = value;
                offsetAngle = (double)180 / _LineNum;
            }
        }
    }

    /// <summary>
    /// 谱线刷新频率
    /// </summary>
    public int Speed
    {
        get { return _Speed; }
        set
        {
            if (value >= 0)
            {
                _Speed = value;
            }
        }
    }

    #endregion

    #region 绘制频谱

    private void _Timer1_Tick(object sender, EventArgs e)
    {
        //内圈画笔
        p = new Pen(Color.FromArgb(this._LineAlpha, this._LineG, this._LineR, this._LineB), _LineWidth);
        //外圈画笔
        p2 = new Pen(Color.FromArgb(this._LineAlpha, this._LineG, this._LineR, this._LineB), _LineWidth + 1);
        _CenterPointF = new PointF(this.Width / 2, this.Height / 2);

        using (bmp = new Bitmap(this.Width, this.Height))
        {
            g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.FillRectangle(sb, r);
            //g.Clear(this._BackColor);

            //_OldFFT = fft;
            int _LineHeight1 = 0;
            int _LineHeight2 = 0;

            for (int i = 0; i < _LineNum; i++)
            {
                if (fft == null)
                {
                    g.DrawLine(p2, GetPointF(_CenterPointF, 140, offsetAngle * i + _DeviationAngle), GetPointF(_CenterPointF, 140 + _LineStopHeight, offsetAngle * i + _DeviationAngle));
                    g.DrawLine(p2, GetPointF(_CenterPointF, 140, 360 - offsetAngle * (i + 1) + _DeviationAngle), GetPointF(_CenterPointF, 140 + _LineStopHeight, 360 - offsetAngle * (i + 1) + _DeviationAngle));
                    g.DrawLine(p, GetPointF(_CenterPointF, 136, offsetAngle * i - _DeviationAngle), GetPointF(_CenterPointF, 136, offsetAngle * i - _DeviationAngle));
                    g.DrawLine(p, GetPointF(_CenterPointF, 136, 360 - offsetAngle * (i + 1) - _DeviationAngle), GetPointF(_CenterPointF, 136, 360 - offsetAngle * (i + 1) - _DeviationAngle));
                }
                else
                {
                    _LineHeight = (int)(fft[i] * 6000 * 5000);//
                    #region  外圈
                    _LineHeight1 = (int)Math.Pow(_LineHeight, 1 / 3f);//减小谱线间的差距
                    if (_LineHeight1 <= 45)
                    {
                        _LineHeight1 = _LineStopHeight;
                    }
                    else if (_LineHeight1 >= 170)
                    {
                        _LineHeight1 = 170;
                    }
                    if (_OldH[i] > _LineHeight1)
                    {
                        _OldH[i] -= 7;//回落速度
                        if (_LineAlphas[i] > _LineAlpha)
                        {
                            _LineAlphas[i] -= 18;//150 - (int)(100 * (_LineStopHeight * 1.0 / _OldH[i]));本来想动态计算的，结果失败了
                        }
                        else { _LineAlphas[i] = _LineAlpha; }
                    }
                    else
                    {
                        _OldH[i] = _LineHeight1;
                        _LineAlphas[i] = 255;
                    }

                    if (_OldH[i] <= _LineStopHeight)
                    {
                        _OldH[i] = _LineStopHeight;
                        _LineAlphas[i] = _LineAlpha;
                    }
                    #endregion

                    #region 内圈
                    _LineHeight2 = (int)Math.Pow(_LineHeight, 2 / 9f);
                    if (_LineHeight2 <= 15)
                    {
                        _LineHeight2 = 1;
                    }
                    if (_OldH2[i] > _LineHeight2)
                    {
                        _OldH2[i] -= 1;
                    }
                    else
                    {
                        _OldH2[i] = _LineHeight2;
                    }
                    if (_OldH2[i] <= 1)
                    {
                        _OldH2[i] = 1;
                    }
                    #endregion

                    p2.Color = Color.FromArgb(_LineAlphas[i], this._LineG, this._LineR, this._LineB);
                    p.Color = Color.FromArgb(_LineAlphas[i], this._LineG, this._LineR, this._LineB);
                    //p2 = new Pen(Color.FromArgb(this._LineAlphas[i], this._LineG, this._LineR, this._LineB), _LineWidth + 1);
                    //为了频谱对称好看，所以采取对半绘制
                    g.DrawLine(p2, GetPointF(_CenterPointF, 140, offsetAngle * i + _DeviationAngle), GetPointF(_CenterPointF, 140 + _OldH[i], offsetAngle * i + _DeviationAngle));
                    g.DrawLine(p2, GetPointF(_CenterPointF, 140, 360 - offsetAngle * (i + 1) + _DeviationAngle), GetPointF(_CenterPointF, 140 + _OldH[i], 360 - offsetAngle * (i + 1) + _DeviationAngle));
                    g.DrawLine(p, GetPointF(_CenterPointF, 136, offsetAngle * i - _DeviationAngle), GetPointF(_CenterPointF, 136 - _OldH2[i], offsetAngle * i - _DeviationAngle));
                    g.DrawLine(p, GetPointF(_CenterPointF, 136, 360 - offsetAngle * (i + 1) - _DeviationAngle), GetPointF(_CenterPointF, 136 - _OldH2[i], 360 - offsetAngle * (i + 1) - _DeviationAngle));
                }
            }

            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            g.DrawString(_NewTime + "\n\n" + _EndTime, new Font("方正行楷简体", 50, FontStyle.Bold), new SolidBrush(Color.FromArgb(this._FontAlpha, this._LineG, this._LineR, this._LineB)), this.ClientRectangle, sf);
            g.DrawString(_Title + "\n" + _Artist, new Font("宋体", 15, FontStyle.Bold), new SolidBrush(Color.FromArgb(this._FontAlpha, this._LineG, this._LineR, this._LineB)), this.ClientRectangle, sf);

            #region 作废

            //StringFormat sf2 = new StringFormat();
            //sf2.LineAlignment = StringAlignment.Center;
            //sf2.Alignment = StringAlignment.Center;
            //g.DrawString(_EndTime, new Font("李旭科书法 v1.4", 50, FontStyle.Bold), new SolidBrush(Color.FromArgb(this._LineAlpha, this._LineR, this._LineG, this._LineB)), this.ClientRectangle, sf2);

            //_DeviationAngle += 0.4f;
            //if (_DeviationAngle >= 360)
            //{
            //    _DeviationAngle = 0;
            //}

            #endregion
            _DeviationAngle = _DeviationAngle >= 360 ? 0 : _DeviationAngle + 0.4f;

            SetBits(bmp);

            bmp.Dispose();
            g.Dispose();
        }
        p.Dispose();
        p2.Dispose();
    }

    /// <summary>
    /// 根据中心点 半径 和角度，获取从中心点出发的线段终点
    /// </summary>
    /// <param name="centerPointF">中心点</param>
    /// <param name="r">半径</param>
    /// <param name="angle">角度</param>
    /// <returns>从中心点出发的线段终点</returns>
    private PointF GetPointF(PointF centerPointF, int r, double angle)
    {
        double A = Math.PI * angle / 180;//(angle/360)*2PI
        float xF = centerPointF.X + r * (float)Math.Cos(A);
        float yF = centerPointF.Y + r * (float)Math.Sin(A);

        return (new PointF(xF, yF));
    }

    #endregion

    #region 窗体的字段及属性（没用上）

    private int _AlphaValue = 0;//窗体的Alpha值

    public int AlphaValue
    {
        get { return _AlphaValue; }
        set
        {
            if (value >= 0 && value <= 255)
            {
                _AlphaValue = value;
            }
            else if (value > 255)
            {
                _AlphaValue = 255;
            }
            else
            {
                _AlphaValue = 0;
            }
            this._BackColor = Color.FromArgb(this._AlphaValue, this.R, this.G, this.B);
            UpdateWindowTransparency();
        }
    }

    private int R = 100;
    private int G = 100;
    private int B = 100;

    private Color _BackColor = Color.FromArgb(0, 100, 100, 100);
    public override Color BackColor
    {
        get
        {
            return base.BackColor;
        }
        set
        {
            base.BackColor = value;
            //this._AlphaValue = value.A;
            this.R = value.R;
            this.G = value.G;
            this.B = value.B;
            this._BackColor = Color.FromArgb(this._AlphaValue, this.R, this.G, this.B);
            UpdateWindowTransparency();
        }
    }

    #endregion

    #region 窗体事件

    private void WaveForm_Load(object sender, EventArgs e)
    {
        //bmp = new Bitmap(this.Width, this.Height);

        UpdateWindowTransparency();

        _Timer1.Start();
        //_Timer2.Start();

        Control.CheckForIllegalCrossThreadCalls = false;
        thread = new System.Threading.Thread(GetColorOfScreen);
        thread.IsBackground = true;
        thread.Start();
    }
    public System.Threading.Thread thread;//声明一个线程
    #endregion

    //#region 窗体移动
    //[DllImport("user32.dll")]
    //public static extern bool ReleaseCapture();
    //[DllImport("user32.dll")]
    //public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
    //public const int WM_SYSCOMMAND = 0x0112;
    //public const int SC_MOVE = 0xF010;
    //public const int HTCAPTION = 0x0002;//无边框窗体移动
    //#endregion

    #region 调用UpdateLayeredWindow函数绘制透明窗体

    protected override CreateParams CreateParams
    {//重载窗体的CreateParams方法
        get
        {
            //const int WS_MINIMIZEBOX = 0x00020000;  // Winuser.h中定义   
            CreateParams cp = base.CreateParams;
            //cp.Style = cp.Style | WS_MINIMIZEBOX;   // 允许最小化操作
            cp.ExStyle |= 0x00080000; // WS_EX_LAYERED
            return cp;
        }
    }

    public void SetBits(Bitmap bitmap)//调用UpdateLayeredWindow（）方法。this.BackgroundImage为你事先准备的带透明图片。
    {
        //if (!haveHandle) return;

        if (!Bitmap.IsCanonicalPixelFormat(bitmap.PixelFormat) || !Bitmap.IsAlphaPixelFormat(bitmap.PixelFormat))
            throw new ApplicationException("图片必须是32位带Alhpa通道的图片。");

        IntPtr oldBits = IntPtr.Zero;
        IntPtr screenDC = Win32.GetDC(IntPtr.Zero);
        IntPtr hBitmap = IntPtr.Zero;
        IntPtr memDc = Win32.CreateCompatibleDC(screenDC);

        try
        {
            Win32.Point topLoc = new Win32.Point(Left, Top);
            Win32.Size bitMapSize = new Win32.Size(bitmap.Width, bitmap.Height);
            Win32.BLENDFUNCTION blendFunc = new Win32.BLENDFUNCTION();
            Win32.Point srcLoc = new Win32.Point(0, 0);

            hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
            oldBits = Win32.SelectObject(memDc, hBitmap);

            blendFunc.BlendOp = Win32.AC_SRC_OVER;
            blendFunc.SourceConstantAlpha = 255;
            blendFunc.AlphaFormat = Win32.AC_SRC_ALPHA;
            blendFunc.BlendFlags = 0;

            Win32.UpdateLayeredWindow(Handle, screenDC, ref topLoc, ref bitMapSize, memDc, ref srcLoc, 0, ref blendFunc, Win32.ULW_ALPHA);
        }
        finally
        {
            if (hBitmap != IntPtr.Zero)
            {
                Win32.SelectObject(memDc, oldBits);
                Win32.DeleteObject(hBitmap);
            }
            Win32.ReleaseDC(IntPtr.Zero, screenDC);
            Win32.DeleteDC(memDc);
        }
    }

    private Bitmap bmp;//背景图
    private Graphics g;//画布
    private SolidBrush sb = new SolidBrush(Color.FromArgb(0, 0, 0, 0));//画刷
    private Rectangle r;//填充区域

    /// <summary>
    /// 更新窗体背景颜色及透明度
    /// </summary>
    private void UpdateWindowTransparency()
    {
        bmp = new Bitmap(this.Width, this.Height);
        g = Graphics.FromImage(bmp);
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //sb = new SolidBrush(Color.FromArgb(_AlphaValue, this.R, this.G, this.B));
        r = new Rectangle(0, 0, this.Width, this.Height);
        g.FillRectangle(sb, r);
        //g.DrawEllipse(p, new Rectangle(0, 0, this.Width, this.Height));
        SetBits(bmp);
        //sb.Dispose();
        g.Dispose();
        bmp.Dispose();
    }

    #endregion

    #region 屏幕取色（用于改变谱线颜色）

    //private void _Timer2_Tick(object sender, EventArgs e)
    //{
    //    SetColorOfScreen();
    //}

    private void GetColorOfScreen()
    {
        while (true)
        {
            if (!this.IsDisposed)//获取颜色之前先判断窗体是否被释放
            {
                SetColorOfScreen();
                System.Threading.Thread.Sleep(1000);//线程睡眠一秒
            }
        }
    }

    private void SetColorOfScreen()
    {
        //取9个点的坐标
        Point p1 = new Point(this.Width / 2 - 150, this.Height / 2);//左边中间的点
        Point p2 = new Point(this.Width / 2, this.Height / 2 + 150);//下边中间的点
        Point p3 = new Point(this.Width / 2 + 150, this.Height / 2);//右边中间的点
        Point p4 = new Point(this.Width / 2, this.Height / 2 - 150);//上边中间的点
        Point p5 = new Point(this.Width / 2, this.Height / 2);//中心点
        Point p6 = new Point(this.Width / 2 - 200, this.Height / 2 - 200);//左上角点
        Point p7 = new Point(this.Width / 2 - 200, this.Height / 2 + 200);//左下角点
        Point p8 = new Point(this.Width / 2 + 200, this.Height / 2 - 200);//右上角点
        Point p9 = new Point(this.Width / 2 + 200, this.Height / 2 + 200);//右下角点
        Point[] p = { p1, p2, p3, p4, p5, p6, p7, p8, p9 };
        //PointToScreen将控件坐标转换为相对屏幕坐标
        int[] r = new int[9];
        int[] g = new int[9];
        int[] b = new int[9];
        for (int i = 0; i < 9; i++)
        {
            GetColorOfRGB(p[i], out r[i], out g[i], out b[i]);
        }

        this._LineR = GetAVGByArray(r) < 205 ? GetAVGByArray(r) + 50 : GetAVGByArray(r) - 30;
        this._LineG = GetAVGByArray(g) < 205 ? GetAVGByArray(g) + 50 : GetAVGByArray(g) - 30;
        this._LineB = GetAVGByArray(b) < 205 ? GetAVGByArray(b) + 50 : GetAVGByArray(b) - 30;
    }

    private void GetColorOfRGB(Point p, out int r, out int g, out int b)
    {
        try
        {
            IntPtr hdc = GetDC(new IntPtr(0));//取到设备场景(0就是全屏的设备场景)
            int c = GetPixel(hdc, PointToScreen(p));//取指定点颜色
            r = (c & 0xFF);//转换R
            g = (c & 0xFF00) / 256;//转换G
            b = (c & 0xFF0000) / 65536;//转换B
        }
        catch
        {
            r = _LineR;
            g = _LineG;
            b = _LineB;
        }
    }

    private int GetAVGByArray(int[] num)
    {
        int sum = 0;
        for (int i = 0; i < num.Length; i++)
        {
            sum += num[i];
        }
        return sum / num.Length;
    }

    [DllImport("user32.dll")]//取设备场景
    private static extern IntPtr GetDC(IntPtr hwnd);//返回设备场景句柄
    [DllImport("gdi32.dll")]//取指定点颜色
    private static extern int GetPixel(IntPtr hdc, Point p);

    #endregion

    private void WaveForm_FormClosing(object sender, FormClosingEventArgs e)
    {

    }
}
}

