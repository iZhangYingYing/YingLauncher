using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Un4seen.Bass;
using Ying.YingWinFormControls;

namespace Ying.YingUtils
{
    public class YingPlayer : YingYing
    {

        private int y { get; set; } = 20020604;
        private WaveForm ywave = new WaveForm();
        

        public YingPlayer()
        {
            
        }

        public Boolean YInit(IntPtr yhandle)
        {
            if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, yhandle))
            {   
                Console.WriteLine("Ying: 无法初始化音乐引擎，可能是采样率不支持。" + Environment.NewLine + Bass.BASS_ErrorGetCode());
                return false;//Application.Current.Shutdown();
            }
            return true;
        }

        public int YPrepare(String yurl)
        {
            if (this.y != 20020604) Bass.BASS_StreamFree(this.y);
            this.y = Bass.BASS_StreamCreateURL(yurl, 0, BASSFlag.BASS_SAMPLE_FLOAT, null, IntPtr.Zero);

            getYConsole().sendYMessage(String.Join(Environment.NewLine, Bass.BASS_ChannelGetTagsID3V2(this.y)));


            ywave.Show();

            DispatcherTimer yyyy = new DispatcherTimer();
            yyyy.Interval = new TimeSpan(604);
            yyyy.Tick+=(ysender, yevent) => {
                if (true)
                {
                    //ywave.FFT = music.GetFFTData();
                    fft = this.GetFFTData();
                    float[] temp = new float[45];
                    for (int i = 0; i < 45; i++)
                    {
                        temp[i] = fft[i * 2];
                    }
                    ywave.FFT = temp;
                    //layeredTrackBar1.Value = music.Schedule;
                    ywave.NewTime = this.GetFormatTime();
                }
                else
                {
                    ywave.FFT = null;
                    ywave.Title = "听音乐";
                    ywave.Artist = "心灵之音";
                    ywave.NewTime = "00:00";
                    ywave.EndTime = "00:00";
                }
            };
            yyyy.Start();

            return this.y;
        }

        private float[] fft = new float[45];

        public float[] GetFFTData()
        {
            float[] fft = new float[512];
            Bass.BASS_ChannelGetData(this.y, fft, (int)BASSData.BASS_DATA_FFT1024);
            return fft;
        }

        /// <summary>
        /// 获取或设置当前歌曲时间,单位秒
        /// </summary>
        public double MusicTime
        {
            get { return Bass.BASS_ChannelBytes2Seconds(y, Bass.BASS_ChannelGetPosition(y)); }
            set { Bass.BASS_ChannelSetPosition(y, Bass.BASS_ChannelSeconds2Bytes(y, value)); }
        }

        /// <summary>
        /// 获取当前歌曲播放进度的时间00:00格式
        /// </summary>
        /// <returns>00:00格式时间</returns>
        public string GetFormatTime()
        {
            return FormatMusicTime(MusicTime);
        }

        /// <summary>
        /// 格式化歌曲时间 00:00
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string FormatMusicTime(double time)
        {
            StringBuilder sb = new StringBuilder();
            int temp = (int)time / 60;
            if (time < 0)
            {
                time = 0;
            }
            sb.Remove(0, sb.Length);
            sb.AppendFormat("{0:00}", temp);
            sb.Append(':');
            sb.AppendFormat("{0:00}", time - temp * 60);
            return sb.ToString();
        }

        public Boolean YPlay(Boolean yrestart = false)
        {
            //int y = Bass.BASS_StreamCreateFile("C:\\Users\\Administrator\\Music\\不才 - 参商 (纯歌版).mp3", 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT);
            return Bass.BASS_ChannelPlay(y, yrestart);
        }

        public Boolean YPause()
        {
            return Bass.BASS_ChannelPause(y);
        }

        public Boolean YStop()
        {
            return Bass.BASS_ChannelStop(y);
        }

        public Image getYPicture()
        {
            return Bass.BASS_ChannelGetTagsAPEPictures(y).FirstOrDefault().PictureImage;
        }

        public Double getYCurrentTime()
        {
            return Bass.BASS_ChannelBytes2Seconds(y, Bass.BASS_ChannelGetPosition(y));
        }

        public BASSActive isYActive()
        {
            return Bass.BASS_ChannelIsActive(y);
        }
    }
}
