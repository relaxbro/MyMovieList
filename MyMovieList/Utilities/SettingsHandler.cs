using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyMovieList.Utilities
{
    class SettingsHandler
    {
        private string _SettingsFile = "settings.config";

        private string _PreviousFile = "N/A";
        public string PreviousFile
        {
            get { return _PreviousFile; }
            set { _PreviousFile = value; }
        }

        private bool _OpenPreviousOnStartup = false;
        public bool OpenPreviousOnStartup
        {
            get { return _OpenPreviousOnStartup; }
            set { _OpenPreviousOnStartup = value; }
        }

        public void LoadSettings()
        {
            // check if settings file exists
            if (File.Exists(_SettingsFile))
            {
                using (var reader = new StreamReader(_SettingsFile))
                {
                    string line;
                    string prevLine = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (prevLine == "[OpenPreviousOnStartup]")
                        {
                            OpenPreviousOnStartup = Convert.ToBoolean(line);
                        }
                        else if (prevLine == "[PreviousFile]")
                        {
                            PreviousFile = line;
                        }
                        prevLine = line;
                    }
                }
            }
            else
            {
                CreateSettingsFile();
            }
        }

        private void CreateSettingsFile()
        {
            StreamWriter sw = new StreamWriter(_SettingsFile, true);
            sw.WriteLine(string.Format("[AppName]\nMyMovieList\n\n[OpenPreviousOnStartup]\n{0}\n\n[PreviousFile]\n{1}", OpenPreviousOnStartup.ToString().ToLower(), PreviousFile));
            sw.Close();
        }

        public void SaveSettings()
        {
            StreamWriter sw = new StreamWriter(_SettingsFile, false);
            sw.WriteLine(string.Format("[AppName]\nMyMovieList\n\n[OpenPreviousOnStartup]\n{0}\n\n[PreviousFile]\n{1}", OpenPreviousOnStartup.ToString().ToLower(), PreviousFile));
            sw.Close();
        }
    }
}
