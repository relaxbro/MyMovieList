using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace MyMovieList.Utilities
{
    public class SettingsHandler
    {
        private string _SettingsFile = "settings.config";

        private string _PreviousFile = "N/A";
        public string PreviousFile
        {
            get { return _PreviousFile; }
            set { _PreviousFile = value; }
        }

        private string _PreviousCurrentMovieID = "N/A";
        public string PreviousCurrentMovieID
        {
            get { return _PreviousCurrentMovieID; }
            set
            {
                _PreviousCurrentMovieID = value;
                //Debug.WriteLine("PreviousCurrentMovieID " + _PreviousCurrentMovieID);
            }
        }

        private bool _OpenPreviousOnStartup = true;
        public bool OpenPreviousOnStartup
        {
            get { return _OpenPreviousOnStartup; }
            set { _OpenPreviousOnStartup = value; }
        }

        private bool _RememberSeenNotSeenOnStartup = false;
        public bool RememberSeenNotSeenOnStartup
        {
            get { return _RememberSeenNotSeenOnStartup; }
            set { _RememberSeenNotSeenOnStartup = value; }
        }

        private bool _ShowSeen = true;
        public bool ShowSeen
        {
            get
            {
                if (RememberSeenNotSeenOnStartup)
                {
                    return _ShowSeen;
                }
                return true;
            }
            set
            {
                _ShowSeen = value;
            }
        }

        private bool _ShowNotSeen = true;
        public bool ShowNotSeen
        {
            get
            {
                if (RememberSeenNotSeenOnStartup)
                {
                    return _ShowNotSeen;
                }
                return true;
            }
            set
            {
                _ShowNotSeen = value;
            }
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
                        else if (prevLine == "[RememberSeenNotSeenOnStartup]")
                        {
                            RememberSeenNotSeenOnStartup = Convert.ToBoolean(line);
                        }
                        else if (prevLine == "[ShowSeen]")
                        {
                            ShowSeen = Convert.ToBoolean(line);
                        }
                        else if (prevLine == "[ShowNotSeen]")
                        {
                            ShowNotSeen = Convert.ToBoolean(line);
                        }
                        else if (prevLine == "[PreviousFile]")
                        {
                            PreviousFile = line;
                        }
                        else if (prevLine == "[PreviousCurrentMovieID]")
                        {
                            PreviousCurrentMovieID = line;
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
            sw.WriteLine(string.Format("[AppName]\nMyMovieList\n\n[OpenPreviousOnStartup]\n{0}\n\n[RememberSeenNotSeenOnStartup]\n{1}\n\n[ShowSeen]\n{2}\n\n[ShowNotSeen]\n{3}\n\n[PreviousFile]\n{4}\n\n[PreviousCurrentMovieID]\n{5}", 
                OpenPreviousOnStartup.ToString().ToLower(), RememberSeenNotSeenOnStartup.ToString().ToLower(), ShowSeen.ToString().ToLower(), ShowNotSeen.ToString().ToLower(), PreviousFile, PreviousCurrentMovieID));
            sw.Close();
        }

        public void SaveSettings()
        {
            StreamWriter sw = new StreamWriter(_SettingsFile, false);
            sw.WriteLine(string.Format("[AppName]\nMyMovieList\n\n[OpenPreviousOnStartup]\n{0}\n\n[RememberSeenNotSeenOnStartup]\n{1}\n\n[ShowSeen]\n{2}\n\n[ShowNotSeen]\n{3}\n\n[PreviousFile]\n{4}\n\n[PreviousCurrentMovieID]\n{5}",
                OpenPreviousOnStartup.ToString().ToLower(), RememberSeenNotSeenOnStartup.ToString().ToLower(), ShowSeen.ToString().ToLower(), ShowNotSeen.ToString().ToLower(), PreviousFile, PreviousCurrentMovieID));
            sw.Close();
        }
    }
}
