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

using JLIB3.JCalendar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace JLIB3
{
    public class JLog
    {
        public JLog(string logFilePath)
        {
            if (string.IsNullOrWhiteSpace(logFilePath))
            {
                throw new ArgumentException("Log file path cannot be null or empty.");
            }

            var directory = Path.GetDirectoryName(logFilePath);
            if (string.IsNullOrWhiteSpace(directory))
            {
                directory = "Logs";
                logFilePath = Path.Combine(directory, logFilePath);
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            LogFile = logFilePath;

            if (!File.Exists(LogFile))
            {
                File.WriteAllText(LogFile, $"Logfile created: {JTimeStamp.FullLog}{JGen.NL}");
            }
        }

        private readonly object _lock = new();
        private readonly List<string> _queue = new();

        public string LogFile { get; }
        public string LastEntry { get; private set; } = string.Empty;

        public event EventHandler<JEventArgs>? Updated;

        public void Add(string message)
        {
            if (message == null)
            {
                return;
            }

            var timestamped = $"[{JTimeStamp.FullLog}] {message}{JGen.NL}";
            LastEntry = message;

            lock (_lock)
            {
                File.AppendAllText(LogFile, timestamped);
            }

            Updated?.Invoke(this, new JEventArgs(message));
        }

        public async Task AddAsync(string message)
        {
            if (message == null)
            {
                return;
            }

            var timestamped = $"[{JTimeStamp.FullLog}] {message}{JGen.NL}";
            LastEntry = message;

            // Write outside lock — File.AppendAllTextAsync is thread-safe
            await File.AppendAllTextAsync(LogFile, timestamped);

            // Raise event after write
            Updated?.Invoke(this, new JEventArgs(message));
        }


        public void Add(object obj)
        {
            if (obj != null)
            {
                Add(obj.ToString() ?? "[null]");
            }
        }

        public void AddQueued(string message)
        {
            if (message == null)
            {
                return;
            }

            lock (_lock)
            {
                _queue.Add($"[{JTimeStamp.FullLog}] {message}");
            }

            Updated?.Invoke(this, new JEventArgs(message));
        }

        public async Task WriteQueueAsync()
        {
            List<string> toWrite;

            lock (_lock)
            {
                toWrite = new List<string>(_queue);
                _queue.Clear();
            }

            await File.AppendAllLinesAsync(LogFile, toWrite);
        }

        public void WriteQueue()
        {
            lock (_lock)
            {
                File.AppendAllLines(LogFile, _queue);
                _queue.Clear();
            }
        }

        public Action<string> LogAction => Add;
        public Action<string> QueuedLogAction => AddQueued;
    }
}

