using NitroSystem.Dnn.BusinessEngine.Core.Contract;
using NitroSystem.Dnn.BusinessEngine.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NitroSystem.Dnn.BusinessEngine.Common;

namespace NitroSystem.Dnn.BusinessEngine.Core.Common
{
    public class ProgressMonitoring
    {
        private readonly string _monitoringFile;
        private readonly string _progressValueFile;

        public ProgressMonitoring(string monitoringFile, string progressValueFile, string startMessage, int progressValue = 0)
        {
            this._monitoringFile = monitoringFile;
            this._progressValueFile = progressValueFile;

            var directory = new DirectoryInfo(Path.GetDirectoryName(this._monitoringFile));
            directory.Empty();

            FileUtil.CreateTextFile(this._monitoringFile, startMessage, true);
            FileUtil.CreateTextFile(this._progressValueFile, progressValue.ToString(), true);
        }

        public void Progress(string message, byte value)
        {
            FileUtil.CreateTextFile(this._monitoringFile, message, true);
            FileUtil.CreateTextFile(this._progressValueFile, value.ToString(), true);
        }

        public void End()
        {
            FileUtil.DeleteFile(this._monitoringFile);
        }
    }
}
