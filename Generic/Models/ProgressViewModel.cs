using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions.Enums;

namespace Generic.Models
{
    public class ProgressModel
    {
        public long Total { get; set; }
        public long Progress { get; set; }
        public double InRatio { get { return Total == 0 ? 0 : (double)((Progress * 1d) / (Total * 1d)); } }
        public double InPercantage { get { return Total == 0 ? 0 : (double)(InRatio * 100); } }

        public ProgressModel()
        {
            Progress = 0;
            Total = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="total"></param>
        public ProgressModel(long progress, long total)
        {
            Total = total;
            Progress = progress;
        }

        /// <summary>
        /// Return download progress with the time left and download speed
        /// </summary>
        /// <param name="offsetInSeconds">the time for each progress sample</param>
        /// <param name="progress">the progress in the given offset time</param>
        /// <returns></returns>
        public DownloadProgressModel GetDownloadProgress(int offsetInSeconds, long progress)
        {
            var result = new DownloadProgressModel(offsetInSeconds, progress, Total);
            return result;
        }
    }

    public class DownloadProgressModel
    {
        private readonly TimeSpan _timeleft;
        private readonly DownloadSpeedModel _downloadSpeed;

        public TimeSpan TimeLeft { get { return _timeleft; }  }

        public DownloadSpeedModel DownloadSpeed
        {
            get { return _downloadSpeed; }
        }

        public DownloadProgressModel(int offsetInSeconds, long progress,long total)
        {
            int secondsLeft = (int)((double)total * offsetInSeconds / progress);

            _timeleft = new TimeSpan(0, 0, secondsLeft);


            var progressInSeconds = (double)progress / offsetInSeconds;

            _downloadSpeed = new DownloadSpeedModel(progressInSeconds);
        }
        

    }

    public class DownloadSpeedModel
    {
        private readonly double _progressInSeconds;

        public double InBytesPerSeconds
        {
            get { return _progressInSeconds; }
        }

        public double InKiloBytesPerSeconds
        {
            get { return _progressInSeconds / 1024; }
        }

        public double InMegaBytesPerSeconds
        {
            get { return _progressInSeconds / 1000000; }
        }

        public DownloadSpeedModel(double progressInSeconds)
        {
            _progressInSeconds = progressInSeconds;
        }

        public string ToString(SpeedType speedType)
        {
            double value = 0;
            switch (speedType)
            {
                case SpeedType.BytePerSeconds:
                    value = InBytesPerSeconds;
                    break;
                case SpeedType.KiloBytesPerSeconds:
                    value = InKiloBytesPerSeconds;
                    break;
                case SpeedType.MegaBytesPerSeconds:
                    value = InMegaBytesPerSeconds;
                    break;
            }

            var result = String.Format(" {0:F} {1}", value, speedType.ToDescription());

            return result;
        }
    }

    public enum SpeedType
    {
        [Description("B/Sec")]
        BytePerSeconds,

        [Description("Kb/Sec")]
        KiloBytesPerSeconds,

        [Description("Mb/Sec")]
        MegaBytesPerSeconds
        
    }
}
