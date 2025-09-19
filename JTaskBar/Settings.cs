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
        public Settings() { }
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
                    File.WriteAllText(configFile, JsonSerializer.Serialize(this, JsonSerializerOptions.Default));
                }
            }
        }

        public int BarWidth { get; set; } = 101;

        //change to enum:
        //public enum DockSideEnum : uint
        //{
        //    Left = 0,
        //    Top = 1,
        //    Right = 2,
        //    Bottom = 3
        //}
        public uint DockSide { get; set; } = AppBar.ABE_LEFT;

        public Dictionary<string, string> MenuFolders { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Desktop", "%USERPROFILE%\\Desktop" },
            //{ "Downloads", "%USERPROFILE%\\Downloads" },
        };

        public string ClockFormat { get; set; } = "HH:mm \ndddd\nyyyy-MM-dd";
        public bool ShowISOWeek { get; set; } = true;

        public double Opacity { get; set; } = 0;

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

        public bool MultiMonitor { get; set; } = false;

        public Dictionary<string, string> Monitors { get; set; }

    }

    public class MonSetting
    {
        public uint DockSide { get; set; } = AppBar.ABE_LEFT;
        public int BarWidth { get; set; } = 101;

        //potential coloring options
    }
}
