using NitroSystem.Dnn.BusinessEngine.Utilities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Utilities
{
    public class VideoUtil
    {
        private int Count;
        private string VideoTask;
        private string VideoFile;
        private string Watermark;
        private HttpRequest CurrentRequest;

        private static BackgroundWorker bgWorker;

        public UploadVideoInfo SetVideo(HttpRequest currentRequest, string fileName, string watermark)
        {
            var result = new UploadVideoInfo();

            CurrentRequest = currentRequest;

            Count = 3;

            VideoFile = fileName;

            Process process = new Process();
            process.StartInfo.FileName = CurrentRequest.MapPath("~/DesktopModules/BusinessEngine/Content/ffmpeg/ffprobe.exe");
            process.StartInfo.Arguments = string.Format("-v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 {0}", VideoFile);
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
            var duration = process.StandardOutput.ReadToEnd();
            var seconds = Regex.Replace(duration, @"\r\n?|\n", string.Empty).Split('.')[0];
            result.Duration = TimeSpan.FromSeconds(Convert.ToDouble(seconds)).ToString(@"mm\:ss");

            VideoTask = "CreateThumbnail";

            Watermark = watermark;

            //bgWorker = new BackgroundWorker();

            //bgWorker.DoWork += BgWorker_DoWork;
            //bgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;

            //bgWorker.RunWorkerAsync();

            ExecTask();

            return result;
        }

        //private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        private void ExecTask()
        {
            string videoFolder = Path.GetDirectoryName(VideoFile);

            Process process = new Process();
            process.StartInfo.FileName = CurrentRequest.MapPath("~/DesktopModules/BusinessEngine/Content/ffmpeg/ffmpeg.exe");

            switch (VideoTask)
            {
                case "CreateThumbnail":
                    process.StartInfo.Arguments = string.Format("-i {0} -ss 00:00:10 -vframes 1 -filter:v scale=\"240:160\" {1}\\thumbnail.png", VideoFile, videoFolder);
                    break;
                case "Watermark":
                    process.StartInfo.Arguments = string.Format("-i {0} -i {1} -filter_complex \"overlay=main_w-overlay_w-5:5\" -codec:a copy {2}\\watermark.mp4", VideoFile, Watermark, videoFolder);
                    break;
                case "Preloader":
                    process.StartInfo.Arguments = string.Format("-i {0} -ss 00:00:10 -t 00:00:5 -vf scale=240:160 -an {1}\\preloader.mp4", VideoFile, videoFolder);
                    break;
            }

            process.StartInfo.RedirectStandardOutput = true;

            //process.StartInfo.Arguments = "-i d:\\a\\output1.mp4 -s 480x320 -codec:a copy d:\\a\\output2.mp4";

            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();

            var result = process.StandardOutput.ReadToEnd();

            process.Close();
            process.Dispose();

            Thread.Sleep(500);

            switch (VideoTask)
            {
                case "CreateThumbnail":
                    VideoTask = "Watermark";
                    ExecTask();
                    break;
                case "Watermark":
                    VideoTask = "Preloader";
                    ExecTask();
                    break;
            }
        }

        private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            switch (VideoTask)
            {
                case "CreateThumbnail":
                    VideoTask = "Watermark";
                    bgWorker.RunWorkerAsync();
                    break;
                case "Watermark":
                    VideoTask = "Preloader";
                    bgWorker.RunWorkerAsync();
                    break;
            }
        }
    }
}
