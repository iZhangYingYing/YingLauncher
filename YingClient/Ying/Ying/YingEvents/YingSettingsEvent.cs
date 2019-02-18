using Redbus.Events;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Ying.YingEvents
{

    public class YingSettingsEvent : EventBase, iYingEvent
    {
        public struct YingSettings
        {
            public String YTag { get; set; }
            public String YDisplayName { get; set; }
            public String YImagePath { get; set; }
            public UIElement YContent { get; set; }
        }

        public List<YingSettings> ysettings { get; } = new List<YingSettings>();

        public static String YTag_YLauncher = "启动器设置";

        public bool isYCanceled()
        {
            throw new NotImplementedException();
        }
    }
}
