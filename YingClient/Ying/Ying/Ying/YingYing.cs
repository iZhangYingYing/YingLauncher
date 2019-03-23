using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ying.YingWindows;
using Ying.YingUtil;
using Ying.YingUtils;
using System.IO;
using Redbus.Interfaces;
using Redbus;
using Ying.YingWebsocket;

namespace Ying
{
    public struct YingBehaviors
    {
        public YingBehavior ybehavior { get; set; }
    }

    public class YingYing
    {
        private static YingMagneticManager ymagnetic = new YingMagneticManager(YingApp.Current.MainWindow);
        private static YingPlayer yplayer = new YingPlayer();
        private static YingMusicWindow ymusic = new YingMusicWindow();
        private static YingConsole yconsole = new YingConsole();
        private static YingKeyboardHook yhook = new YingKeyboardHook();
        private static YingFileHandle yfiles = new YingFileHandle();
        private static EventBus yevent = new EventBus();
        private static YingAuhenticator yauhenticator = new YingAuhenticator();

        private static YingBehaviors ybehaviors = new YingBehaviors();



        public static YingPlayer getYPlayer()
        {
            if (yplayer == null) yplayer = new YingPlayer();
            return yplayer;
        }

        public static YingMusicWindow getYMusic()
        {
            return ymusic;
        }

        public static YingMagneticManager getYMagnetic()
        {
            return ymagnetic;
        }

        public static YingFileHandle getYFiles()
        {
            return yfiles;
        }

        public static YingConsole getYConsole()
        {
            if (yconsole == null) yconsole = new YingConsole();
            return yconsole;
        }

        public static YingKeyboardHook getYHook()
        {
            return yhook;
        }

        public static EventBus getYEvent()
        {
            return yevent;
        }

        public static YingAuhenticator getYAuhenticator()
        {
            return yauhenticator;
        }

        public static YingBehaviors getYBehaviors()
        {
            if (ybehaviors.ybehavior == null) ybehaviors.ybehavior = new YingBehavior();
            return ybehaviors;
        }




    }
}
