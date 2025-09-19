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

namespace JLIB3
{
    public static class JGen
    {
        public static string NL
        {
            get { return Environment.NewLine; }
        }
        public static long CalRunTime(DateTime StartTime)
        {
            return Convert.ToInt64(DateTime.Now.Ticks) - Convert.ToInt64(StartTime.Ticks);
        }

        /// <summary>
        /// returns the value converted to T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="v"></param>
        /// <returns></returns>
        public static T? GetVal<T>(object v)
        {
            if (v == null)
            {
                return default(T);
            }
            else
            {
                return (T)Convert.ChangeType(v, typeof(T));
            }
        }

        public static void NullCheck(string n, object x, JLog l)
        {
            if (x == null)
            {
                var str = n + " is null";
                if (l != null)
                {
                    l.Add(str);
                }
                throw new Exception(str);
            }
        }

        public static decimal? SetDecimal(string str)
        {
            var r = decimal.TryParse(str, out var d);

            return (r) ? (decimal?)d : null;
        }

        public static decimal GetDecimal(decimal? dec)
        {
            return (dec == null) ? 0m : (decimal)dec;
        }
    }
}
