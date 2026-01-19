using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServerBackupper
{
    public class Appsettings
    {
        public List<HostSettings> Hosts { get; set; }
        public BackUpSettings BackUpSetting { get; set; }


        #region nested classes
        public class HostSettings
        {
            public string HostName { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public int Port { get; set; } = 22;
        }

        public class BackUpSettings
        {
            public string LocalBackUpFolder { get; set; }
            public string TargetServerFolder { get; set; }
            public string TempArchiveFileFullPath { get; set; }
            public List<string> ExcludeDirectory { get; set; }
        }
        #endregion
    }
}
