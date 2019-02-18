using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ying.YingUtil
{

    public enum MagneticLocation
    {
        Left = 0,
        Right = 1,
        Top = 2,
        Bottom = 3
    }

    public enum MagneticState
    {
        Adsorbent, // 吸附
        Separation // 分离
    }

    public class YingMagneticManager : YingYing
    {
        public class ChildWindowInfo
        {
            public Window Child { get; set; }
            public MagneticLocation Location { get; set; }
            public MagneticState State { get; set; }
            public bool CutstomSetLocation { get; set; }
        }

        public int Step { get; set; }

        private Window m_mainWindow = null;
        private List<ChildWindowInfo> m_childs = new List<ChildWindowInfo>();

        public YingMagneticManager(Window form)
        {
            m_mainWindow = form;
            form.LocationChanged += MainWindow_LocationChanged;
            form.SizeChanged += MainWindow_SizeChanged;
            form.Closed += MainWindow_WindowClosed;
            Step = 20;
        }

        public void addChild(Window childWindow, MagneticLocation loc)
        {
            foreach (ChildWindowInfo info in m_childs)
            {
                if (info.Child == childWindow)
                {
                    return;
                }
            }

            ChildWindowInfo childInfo = new ChildWindowInfo();
            childInfo.Child = childWindow;
            childInfo.Location = loc;
            childInfo.State = MagneticState.Adsorbent;
            childInfo.CutstomSetLocation = false;
            childWindow.LocationChanged += ChildWindow_LocationChanged;
            childWindow.SizeChanged += ChildWindow_SizeChanged;
            childWindow.Closed += ChildWindow_WindowClosed;

            m_childs.Add(childInfo);
            adsorbentChild(childInfo);
        }

        private ChildWindowInfo getInfo(Window form)
        {
            if (form == null)
            {
                return null;
            }

            foreach (ChildWindowInfo info in m_childs)
            {
                if (info.Child == form)
                {
                    return info;
                }
            }

            return null;
        }

        private Point getLocation(ChildWindowInfo info)
        {
            Point pos = new Point();

            switch (info.Location)
            {
                case MagneticLocation.Left:
                    pos = new Point(m_mainWindow.Left - info.Child.ActualWidth - 6.64, m_mainWindow.Top);
                    break;
                case MagneticLocation.Right:
                    pos = new Point(m_mainWindow.Left + m_mainWindow.ActualWidth + 6.64, m_mainWindow.Top + ((m_mainWindow.ActualHeight - info.Child.ActualHeight) / 2));
                    break;
                case MagneticLocation.Top:
                    pos = new Point(m_mainWindow.Left, m_mainWindow.Top - info.Child.ActualHeight);
                    break;
                case MagneticLocation.Bottom:
                    pos = new Point(m_mainWindow.Left, m_mainWindow.Top + m_mainWindow.ActualHeight);
                    break;
                default:
                    break;
            }

            return pos;
        }

        private void setChildLocation(ChildWindowInfo info, Point location)
        {
            if (info.Child == null)
            {
                return;
            }

            info.CutstomSetLocation = true;
            //info.Child.Location = location;

            info.Child.Top = location.Y;
            info.Child.Left = location.X;

            info.CutstomSetLocation = false;    //getYing();
        }

        private void setChildLocation(ChildWindowInfo info, int x, int y)
        {
            setChildLocation(info, new Point(x, y));
        }

        private void resetChildLocation(ChildWindowInfo info)
        {
            if (info.Child == null)
            {
                return;
            }

            Point pos = getLocation(info);
            setChildLocation(info, pos);
        }

        private void adsorbentChild(ChildWindowInfo info)
        {
            info.State = MagneticState.Adsorbent;
            resetChildLocation(info);
        }

        private void separationChild(ChildWindowInfo info)
        {
            info.State = MagneticState.Separation;
        }

        private void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            foreach (ChildWindowInfo info in m_childs)
            {
                if (info.State == MagneticState.Adsorbent)
                {
                    resetChildLocation(info);
                }
            }
        }

        private void MainWindow_SizeChanged(object sender, EventArgs e)
        {
            foreach (ChildWindowInfo info in m_childs)
            {
                if (info.State == MagneticState.Adsorbent)
                {
                    resetChildLocation(info);
                }
            }
        }

        private void MainWindow_WindowClosed(object sender, EventArgs e)
        {
        }

        private void ChildWindow_LocationChanged(object sender, EventArgs e)
        {
            ChildWindowInfo info = getInfo(sender as Window);

            if (info == null)
            {
                return;
            }

            if (info.CutstomSetLocation == true)
            {
                return;
            }

            Point location = getLocation(info);

            if (info.Child.Left > location.X && info.Location == MagneticLocation.Right)
            {
                if (info.Child.Left - location.X > Step)
                {
                    separationChild(info);
                }
                else
                {
                    adsorbentChild(info);
                }
            }
            else if (info.Child.Left < location.X && info.Location == MagneticLocation.Left)
            {
                if (info.Child.Left - location.X < -Step)
                {
                    separationChild(info);
                }
                else
                {
                    adsorbentChild(info);
                }
            }
            if (info.Child.Top > location.Y && info.Location == MagneticLocation.Bottom)
            {
                if (info.Child.Top - location.Y > Step)
                {
                    separationChild(info);
                }
                else
                {
                    adsorbentChild(info);
                }
            }
            else if (info.Child.Top < location.Y && info.Location == MagneticLocation.Top)
            {
                if (info.Child.Top - location.Y < -Step)
                {
                    separationChild(info);
                }
                else
                {
                    adsorbentChild(info);
                }
            }
        }

        private void ChildWindow_SizeChanged(object sender, EventArgs e)
        {
            ChildWindowInfo info = getInfo(sender as Window);

            if (info != null && info.State == MagneticState.Adsorbent)
            {
                resetChildLocation(info);
            }
        }

        private void ChildWindow_WindowClosed(object sender, EventArgs e)
        {
            ChildWindowInfo info = getInfo(sender as Window);

            if (info != null)
            {
                m_childs.Remove(info);
            }
        }
    }
}

