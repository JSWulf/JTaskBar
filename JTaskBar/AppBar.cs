/*==================================================================================================================================================*\
|               .%#@+                                                                       *#%.                     ..    ..                 .*- *. |
|              -##+@.                                      +##.                             @##                    -###*  *#@              -**-  .+* |
|             -###*                                      .##%*             +##%##+   -##*   =#=                   %#++%  :##%            .#%=-=.  .* |
|            :##:                                       :#@%+          -%#*   %##.  -##%   -##.                 .##.=%  .###.          :@#=%##*..-*  |
|       . -=###-                                       :###*         +##.    @##.  *##@.   @#-                 :#% #=   @##.         *%%#=:@+-:%:+.  |
|      %#=--##:                        **          :*--###-         .*:     %##. .@###.   %#.                 *#+*#:   +#%            +@+*:+@*@:*%-  |
|         -##*           -@####=    -####*      :###%@##%                  %##. +#%##*   @#.  .%#+   .@#+.   -#@##.  .##%          ..=#+*=#+-+.=*#*  |
|        +##*         .%###=  ##*.%#=.-##*   . @#=  *###.  :#.            @##.:##-*##. .#%  =####=  @###@. ..###+  .@##@  :@@.      .##*:%%-##@=@@:  |
|       =##@%%%%%+:-. --##-   @#-..  *#%  .%#:@#: -#*:#@ +#=             *##@##*  .##%##-    -##:.##.:##.:#%:#=   %#+##-@#*    .==:+*..+* -=*+=+%=:  |
|  -+%@###:      .+###*.##  +#@.     @#=##@- -#@*#=   :@@:               %###-      .-       -####-  +###*  .#@*##::###+       .:=:.  . .*--+=%*@#   |
|.%@=@##+            -.  *@=-         :+-     .+:                         ..                   ..     ..      ..   @#+#%         ..   .   --.-+#%.   |
|                   /\                                                                                            %#% :#*                            |
|                    \\ _____________________________________________________________________                    +#@ -#%                             |
|      (O)[\\\\\\\\\\(O)#####################################################################>                  -##..#+                              |
|                    //                                                                                         @#:+#.                               |
|                   \/                                                                                         *##@-                                 |
\*==================================================================================================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static JTaskBar.Win;

namespace JTaskBar
{
    public static class AppBar
    {
        public const int ABM_NEW = 0x00000000;
        public const int ABM_REMOVE = 0x00000001;
        public const int ABM_SETPOS = 0x00000003;
        public const int ABE_LEFT = 0;
        public const int ABE_RIGHT = 2;

        public static void UpdateAppBarPosition(Form form, uint side, int width)
        {
            APPBARDATA abd = new APPBARDATA
            {
                cbSize = (uint)Marshal.SizeOf(typeof(APPBARDATA)),
                hWnd = form.Handle,
                uEdge = side
            };

            Rectangle screen = Screen.PrimaryScreen.WorkingArea;

            abd.rc.top = screen.Top;
            abd.rc.bottom = screen.Bottom;

            if (side == ABE_LEFT)
            {
                abd.rc.left = screen.Left;
                abd.rc.right = screen.Left + width;
            }
            else if (side == ABE_RIGHT)
            {
                abd.rc.right = screen.Right;
                abd.rc.left = screen.Right - width;
            }

            Win.SHAppBarMessage(ABM_NEW, ref abd);
            Win.SHAppBarMessage(ABM_SETPOS, ref abd);

            form.Location = new Point(abd.rc.left, abd.rc.top);
            form.Size = new Size(abd.rc.right - abd.rc.left, abd.rc.bottom - abd.rc.top);

        }

        
    }
}
