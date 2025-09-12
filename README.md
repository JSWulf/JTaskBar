---

# JTaskBar — A Vertical Taskbar for Windows 11

A lightweight, configurable vertical taskbar for Windows 11, designed for power users in restricted environments.

---

## Why This Exists

Windows 11 removed support for vertical taskbars. This project restores that functionality with the following constraints:

- Must not require administrative rights
- Cannot rely on Microsoft Store distribution
- Must be usable in secured enterprise environments

---

## Core Features

- Bring a window to focus on click
- Minimize a window if it already has focus
- Show date and time
- Use a JSON config file to control date/time format and other settings (Not yet implemented)
- Desktop button - Minimize all windows with a single click
- Show tooltips on hover
- Provide a context menu for window actions
- Support additional toolbars (planned)

---

## Current Features (as of September 2025)

- Enumerates and displays open windows
- Click-to-focus and minimize-on-reselect
- AppBar-style docking (left or right)
- Adjustable width (via property - setting interface not yet created)
- Tooltips on hover
- Foreground window is auto-highlighted
- Ghost/utility windows are filtered out
- Icons displayed for most windows (with fallback for legacy applications)
- Desktop button to minimize all windows
- Context menu with:
  - Restore
  - Minimize
  - Close
  - Open file location
  - Open Task Manager
- Responsive to screen resolution and monitor layout changes
- Auto Sort Windows list by name and process ID
- Changed list container from ListView to DataGridView
- Calendar view on click of clock

---

## To-Do List

1. **Background Color Property**  
   - Configurable background color for the taskbar and/or window list

2. **Config File**  
   - JSON-based config (`jtaskbar.json`)  
   - Properties:
     - `DockSide`
     - `BarWidth`
     - `ClockFormat`
     - `BackgroundColor`
     - `Opacity`

3. **Config Window**  
   - Lightweight UI to edit and save config settings  
   - Optional: live preview of changes
  
4. **Multi-monitor**  
   - Add mechanism to add toolbars to additional monitors
   - Filter windows to their taskbars on their windows
   - Save settings to config file
   
5. **Additional Toolbars** 
   - Add method for adding and configuring custom toolbars linked to a folder
   - Save configuration to config file

---

## Nice-to-Have Features

- Keyboard navigation (e.g., arrow keys, Enter to focus)
- Multi-monitor support (dock to specific screen)
- Auto-hide or pin/unpin behavior
- Logging/debug mode toggle
- COM-based icon support for UWP/system apps (e.g., Settings, Media Player)

---