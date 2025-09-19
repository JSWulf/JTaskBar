/*======================================================================================================================================================*\
|               .%#@+                                                                       *#%.                     ..    ..                     -*--@: |
|              -##+@.                                      +##.                             @##                    -###*  *#@                   -+:   *+ |
|             -###*                                      .##%*             +##%##+   -##*   =#=                   %#++%  :##%               -%=::.   .=+=|
|            :##:                                       :#@%+          -%#*   %##.  -##%   -##.                 .##.=%  .###.             .@%+@ -+. .. -+|
|       . -=###-                                       :###*         +##.    @##.  *##@.   @#-                 :#% #=   @##.             *@#=####:  ..=* |
|      %#=--##:                        **          :*--###-         .*:     %##. .@###.   %#.                 *#+*#:   +#%            +#@=+=%%@#%:*- *+  |
|         -##*           -@####=    -####*      :###%@##%                  %##. +#%##*   @#.  .%#+   .@#+.   -#@##.  .##%           =@+##@=:=#.  -@+.@.  |
|        +##*         .%###=  ##*.%#=.-##*   . @#=  *###.  :#.            @##.:##-*##. .#%  =####=  @###@. ..###+  .@##@  :@@.        %#@*.*=%#+--=+:@   |
|       =##@%%%%%+:-. --##-   @#-..  *#%  .%#:@#: -#*:#@ +#=             *##@##*  .##%##-    -##:.##.:##.:#%:#=   %#+##-@#*          =% +%++.+@:%@ :.@-  |
|  -+%@###:      .+###*.##  +#@.     @#=##@- -#@*#=   :@@:               %###-      .-       -####-  +###*  .#@*##::###+          .:##=:=%=#@.:@..%+=#+. |
|.%@=@##+            -.  *@=-         :+-     .+:                         ..                   ..     ..      ..   @#+#%          .*###+*%#:.##%*-=@*%:  |
|                   /\                                                                                            %#% :#*      .--=###-  ..#:.#+-%#-%#:  |
|                    \\ _____________________________________________________________________                    +#@ -#%    +#%. :..  *@@.  +=:*##:+@=-  |
|      (O)[\\\\\\\\\\(O)#####################################################################>                  -##..#+    . :@=-     .  :=-.+=+%**%#@   |
|                    //                                                                                         @#:+#.      .  .      ..-*+:.. -.*@.-..  |
|                   \/                                                                                         *##@-                ..      ..::.###:    |
\*======================================================================================================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JTaskBar
{
    public class Settings
    {
        public Settings() 
        {
            ConfigFile = configFile;
        }
        public Settings(string file) 
        {
            ConfigFile = file;
        }

        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        };

        private string configFile = "JTaskBar.conf";

        [JsonIgnore]
        public string ConfigFile { 
            get { return configFile; } 
            set
            {
                configFile = value;

                if (!File.Exists(configFile))
                {
                    File.WriteAllText(configFile, JsonSerializer.Serialize(this, jsonOptions));
                }
            }
        }

        public int BarWidth { get; set; } = 101;

        public AppBar.DockSide DockSideSel { get; set; } = AppBar.DockSide.Left;

        public Dictionary<string, string> MenuFolders { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Desktop", "%USERPROFILE%\\Desktop" },
            //{ "Downloads", "%USERPROFILE%\\Downloads" },
        };

        public string ClockFormat { get; set; } = "HH:mm \ndddd\nyyyy-MM-dd";
        public bool ShowISOWeek { get; set; } = true;

        public double Opacity { get; set; } = 1;

        #region Background Color property
        private string backGroundColorHex = "#000000";

        public string BackGroundColorHex
        {
            get { return backGroundColorHex; }
            set {
                if (value.Length != 7 || !value.StartsWith('#'))
                {
                    throw new Exception("Invalid color - must be hexadecimal with leading '#'");
                }
                backGroundColorHex = value;
                int r = Convert.ToInt32(value.Substring(1, 2), 16);
                int g = Convert.ToInt32(value.Substring(3, 2), 16);
                int b = Convert.ToInt32(value.Substring(5, 2), 16);
                backGroundColor =  Color.FromArgb(255, r, g, b);
            }
        }


        private Color backGroundColor = Color.Black;
        [JsonIgnore]
        public Color BackGroundColor { 
            get { return backGroundColor; } 
            set {
                backGroundColor = value;
                backGroundColorHex = $"#{value.R:X2}{value.G:X2}{value.B:X2}";
            } 
        }
        #endregion

        #region Foreground Color property
        private string foreGroundColorHex = "#FFFFFF";

        public string ForeGroundColorHex
        {
            get { return foreGroundColorHex; }
            set
            {
                if (value.Length != 7 || !value.StartsWith('#'))
                {
                    throw new Exception("Invalid color - must be hexadecimal with leading '#'");
                }
                foreGroundColorHex = value;
                int r = Convert.ToInt32(value.Substring(1, 2), 16);
                int g = Convert.ToInt32(value.Substring(3, 2), 16);
                int b = Convert.ToInt32(value.Substring(5, 2), 16);
                foreGroundColor = Color.FromArgb(255, r, g, b);
            }
        }


        private Color foreGroundColor = Color.White;
        [JsonIgnore]
        public Color ForeGroundColor
        {
            get { return foreGroundColor; }
            set
            {
                foreGroundColor = value;
                foreGroundColorHex = $"#{value.R:X2}{value.G:X2}{value.B:X2}";
            }
        }
        #endregion

        public string PrimaryMonitor { get; set; } = "";

        public bool MultiMonitor { get; set; } = false;

        public Dictionary<string, MonSetting> Monitors { get; set; } = new Dictionary<string, MonSetting>()
        {
            //{ "LUID+targetID", new MonSetting() }
        };


        public static Settings Load(string path)
        {
            using var fs = File.OpenRead(path);
            return JsonSerializer.Deserialize<Settings>(fs, jsonOptions) ?? new Settings();
        }

        public static Settings LoadOrCreate(string path)
        {
            if (!File.Exists(path))
            {
                var s = new Settings { ConfigFile = path };
                s.Save(); // writes using jsonOptions
                return s;
            }

            try
            {
                var s = Load(path);
                s.ConfigFile = path; // keep file path tracked
                return s;
            }
            catch
            {
                //preserve the bad file and regen defaults
                try { 
                    File.Move(path, path + ".bad", overwrite: true); 
                } catch 
                {
                    throw new Exception($"Bad configuration file - delete {path} and try again.");
                }
                var s = new Settings { ConfigFile = path };
                s.Save();
                return s;
            }
        }

        public void Save()
        {
            var json = JsonSerializer.Serialize(this, jsonOptions);
            File.WriteAllText(ConfigFile, json);
        }

        public void Reload()
        {
            CopyFrom(Load(ConfigFile));
        }

        public void CopyFrom(Settings s)
        {
            
            BarWidth = s.BarWidth;
            DockSideSel = s.DockSideSel;
            MenuFolders = new Dictionary<string, string>(s.MenuFolders, StringComparer.OrdinalIgnoreCase);
            ClockFormat = s.ClockFormat;
            ShowISOWeek = s.ShowISOWeek;
            Opacity = s.Opacity;
            BackGroundColorHex = s.BackGroundColorHex; 
            ForeGroundColorHex = s.ForeGroundColorHex;
            PrimaryMonitor = s.PrimaryMonitor;
            MultiMonitor = s.MultiMonitor;
            Monitors = new Dictionary<string, MonSetting>(s.Monitors); 
        }


    }

    public class MonSetting
    {
        public MonSetting(AppBar.DockSide dockSide = AppBar.DockSide.Left, int barWidth = 101)
        {
            DockSideSel = dockSide;
            BarWidth = barWidth;
        }
        public AppBar.DockSide DockSideSel { get; set; } = AppBar.DockSide.Left;
        public int BarWidth { get; set; } = 101;

        //potential coloring options
    }

}
