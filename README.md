A Vertical TaskBar for Windows (11)

If you are looking at this project, please consider contributing.

Constraints:

- Cannot be distrubuted via the Microsoft Store
- Must not require admin rights (to enable use in secured workplaces)

Goals:

- Bring a window to focus on click
- Minimize a window if it already has focus
- Show date/time
- Use a JSON config file to configure date/time format
- Minimize all windows (on doing so, add windows to a temporary list)
- Restore all windows if all windows had just been minimized.
- Menu items
-   - to be determined
- Context menu
-   - to be determined
- Show tooltip on hovering over window name
- Additional toolbars
-   - to be determined


Update July 2025:
---

Current Status

- Core functionality:
  - Enumerates and displays open windows
  - Click-to-focus and minimize-on-reselect
  - AppBar-style docking (left/right)
  - Adjustable width via property
  - Tooltip with delayed display and improved positioning
  - Foreground window is auto-highlighted
  - Ghost/utility windows filtered out

---

To-Do List

1. Add Icons  
   - Load and display window icons in the `ListView` using `SmallImageList`.

2. Desktop Button Behavior  
   - Minimize all windows on click.
   - Track which windows were minimized.
   - Restore them if clicked again.

3. Context Menu  
   - Right-click on a window to show options like:
     - Restore
     - Minimize
     - Close
     - Open file location (if applicable)

4. Overflow Handling  
   - If window count exceeds vertical space:
     - Collapse by process name
     - On hover or click, show a submenu `ListView` with grouped windows

5. Background Color Property  
   - Configurable background color for the taskbar and/or `ListView`.

6. Transparency Property  
   - Allow setting opacity (e.g., 80%) via config or UI.

7. Config File  
   - JSON-based config (e.g., `jtaskbar.config.json`)
   - Properties:
     - `DockSide`
     - `BarWidth`
     - `ClockFormat`
     - `BackgroundColor`
     - `Opacity`

8. Config Window  
   - Lightweight UI to edit and save config settings
   - Optional: live preview of changes

---

Nice to have ToDo:

- Dim or style minimized windows in the list
- Keyboard navigation (e.g., Enter to focus, arrows to move)
- Multi-monitor support (dock to specific screen)
- Auto-hide or pin/unpin behavior
- Logging/debug mode toggle

---
