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

## Core Goals

- Bring a window to focus on click
- Minimize a window if it already has focus
- Show date and time
- Use a JSON config file to control date/time format and other settings
- Minimize all windows with a single click, and restore them if clicked again
- Show tooltips on hover
- Provide a context menu for window actions
- Support additional toolbars (planned)

---

## Current Features (as of July 2025)

- Enumerates and displays open windows
- Click-to-focus and minimize-on-reselect
- AppBar-style docking (left or right)
- Adjustable width
- Tooltip with delayed display and improved positioning
- Foreground window is auto-highlighted
- Ghost/utility windows are filtered out
- Icons displayed for most windows (with fallback for legacy apps)
- Desktop button to minimize/restore all windows
- Context menu with:
  - Restore
  - Minimize
  - Close
  - Open file location
  - Open Task Manager
- Custom spacing between list items
- Responsive to screen resolution and monitor layout changes

---

## To-Do List

1. **Overflow Handling**  
   - Collapse by process name when vertical space is exceeded  
   - Show grouped windows on hover or click

2. **Background Color Property**  
   - Configurable background color for the taskbar and/or window list

3. **Transparency Property**  
   - Allow setting opacity (e.g., 80%) via config or UI

4. **Config File**  
   - JSON-based config (`jtaskbar.config.json`)  
   - Properties:
     - `DockSide`
     - `BarWidth`
     - `ClockFormat`
     - `BackgroundColor`
     - `Opacity`

5. **Config Window**  
   - Lightweight UI to edit and save config settings  
   - Optional: live preview of changes

---

## Nice-to-Have Features

- Dim or style minimized windows in the list
- Keyboard navigation (e.g., arrow keys, Enter to focus)
- Multi-monitor support (dock to specific screen)
- Auto-hide or pin/unpin behavior
- Logging/debug mode toggle
- COM-based icon support for UWP/system apps (e.g., Settings, Media Player)

---