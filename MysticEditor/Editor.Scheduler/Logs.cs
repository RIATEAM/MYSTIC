using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;

namespace Editor.Scheduler {

    public class Logs {
        public static StreamWriter LOG;
        private static string _LOGPATH = ConfigurationSettings.AppSettings["LOGPATH"];
        public static void Start(string type) {
            if (!Directory.Exists(_LOGPATH)) {
                Directory.CreateDirectory(_LOGPATH);
            }
            LOG = new StreamWriter(Path.Combine(_LOGPATH, String.Format("{0:dd-MM-yyyy HH-mm-ss}", DateTime.Now) + "-" + type + ".log"));
        }

        public static void Write(string s) {
            if (LOG != null) {
                LOG.Write(s);
            }
        }
        public static void WriteLine(string s) {
            if (LOG != null) {
                LOG.WriteLine(s);
            }
        }

        public static void Dispose() {
            if (LOG != null) {
                LOG.Dispose();
            }
        }
    }
}
