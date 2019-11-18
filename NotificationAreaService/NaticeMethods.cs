using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace NotificationAreaService
{
    using DWORD = Int32;
    using HICON = IntPtr;

    public class NativeMethods
    {
        /// <summary>
        /// The maximum number of characters, usually including a null terminator, of a standard fully-pathed filename for most Windows API functions that operate on filenames.
        /// </summary>
        public const int MAX_PATH = 260;
        public const int NOTIFYICONDATA_V1_SIZE = 88;
        public const int NOTIFYICONDATA_V2_SIZE = 488;
        public const int NOTIFYICONDATA_V3_SIZE = 504;

        private const string User32 = "user32.dll";

        public delegate IntPtr WindowProcedureHandler(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport(User32, EntryPoint = "RegisterClassW", SetLastError = true)]
        public static extern short RegisterClass(ref WindowClass lpWndClass);

        [DllImport(User32)]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, uint msg, IntPtr wparam, IntPtr lparam);

        [DllImport(User32, EntryPoint = "RegisterWindowMessageW")]
        public static extern uint RegisterWindowMessage([MarshalAs(UnmanagedType.LPWStr)] string lpString);

        [DllImport(User32, EntryPoint = "CreateWindowExW", SetLastError = true)]
        public static extern IntPtr CreateWindowEx(int dwExStyle, [MarshalAs(UnmanagedType.LPWStr)] string lpClassName,
            [MarshalAs(UnmanagedType.LPWStr)] string lpWindowName, int dwStyle, int x, int y,
            int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance,
            IntPtr lpParam);

        [DllImport(User32, SetLastError = true)]
        public static extern bool DestroyWindow(IntPtr hWnd);


        [StructLayout(LayoutKind.Sequential)]
        public struct WindowClass
        {
#pragma warning disable 1591

            public uint style;
            public WindowProcedureHandler lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            [MarshalAs(UnmanagedType.LPWStr)] public string lpszMenuName;
            [MarshalAs(UnmanagedType.LPWStr)] public string lpszClassName;

#pragma warning restore 1591
        }

        public enum MouseEvent
        {
            /// <summary>
            /// The mouse was moved withing the
            /// taskbar icon's area.
            /// </summary>
            MouseMove,

            /// <summary>
            /// The right mouse button was clicked.
            /// </summary>
            IconRightMouseDown,

            /// <summary>
            /// The left mouse button was clicked.
            /// </summary>
            IconLeftMouseDown,

            /// <summary>
            /// The right mouse button was released.
            /// </summary>
            IconRightMouseUp,

            /// <summary>
            /// The left mouse button was released.
            /// </summary>
            IconLeftMouseUp,

            /// <summary>
            /// The middle mouse button was clicked.
            /// </summary>
            IconMiddleMouseDown,

            /// <summary>
            /// The middle mouse button was released.
            /// </summary>
            IconMiddleMouseUp,

            /// <summary>
            /// The taskbar icon was double clicked.
            /// </summary>
            IconDoubleClick,

            /// <summary>
            /// The balloon tip was clicked.
            /// </summary>
            BalloonToolTipClicked
        }


        [SuppressMessage("ReSharper", "IdentifierTypo")]
        public enum WindowsMessages : uint
        {
            /// <summary>
            /// Notifies a window that the user clicked the right mouse button (right-clicked) in the window.
            /// See <a href="https://docs.microsoft.com/en-us/windows/win32/menurc/wm-contextmenu">WM_CONTEXTMENU message</a>
            /// 
            /// In case of a notify icon: 
            /// If a user selects a notify icon's shortcut menu with the keyboard, the Shell now sends the associated application a WM_CONTEXTMENU message. Earlier versions send WM_RBUTTONDOWN and WM_RBUTTONUP messages.
            /// See <a href="https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shell_notifyiconw">Shell_NotifyIcon function</a>
            /// </summary>
            WM_CONTEXTMENU = 0x007b,

            /// <summary>
            /// Posted to a window when the cursor moves.
            /// If the mouse is not captured, the message is posted to the window that contains the cursor.
            /// Otherwise, the message is posted to the window that has captured the mouse.
            ///
            /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-mousemove">WM_MOUSEMOVE message</a>
            /// </summary>
            WM_MOUSEMOVE = 0x0200,

            /// <summary>
            /// Posted when the user presses the left mouse button while the cursor is in the client area of a window.
            /// If the mouse is not captured, the message is posted to the window beneath the cursor.
            /// Otherwise, the message is posted to the window that has captured the mouse.
            /// 
            /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-lbuttondown">WM_LBUTTONDOWN message</a>
            /// </summary>
            WM_LBUTTONDOWN = 0x0201,

            /// <summary>
            /// Posted when the user releases the left mouse button while the cursor is in the client area of a window.
            /// If the mouse is not captured, the message is posted to the window beneath the cursor.
            /// Otherwise, the message is posted to the window that has captured the mouse.
            ///
            /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-lbuttonup">WM_LBUTTONUP message</a>
            /// </summary>
            WM_LBUTTONUP = 0x0202,

            /// <summary>
            /// Posted when the user double-clicks the left mouse button while the cursor is in the client area of a window.
            /// If the mouse is not captured, the message is posted to the window beneath the cursor.
            /// Otherwise, the message is posted to the window that has captured the mouse.
            ///
            /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-lbuttondblclk">WM_LBUTTONDBLCLK message</a>
            /// </summary>
            WM_LBUTTONDBLCLK = 0x0203,

            /// <summary>
            /// Posted when the user presses the right mouse button while the cursor is in the client area of a window.
            /// If the mouse is not captured, the message is posted to the window beneath the cursor.
            /// Otherwise, the message is posted to the window that has captured the mouse.
            ///
            /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-rbuttondown">WM_RBUTTONDOWN message</a>
            /// </summary>
            WM_RBUTTONDOWN = 0x0204,

            /// <summary>
            /// Posted when the user releases the right mouse button while the cursor is in the client area of a window.
            /// If the mouse is not captured, the message is posted to the window beneath the cursor.
            /// Otherwise, the message is posted to the window that has captured the mouse.
            ///
            /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-rbuttonup">WM_RBUTTONUP message</a>
            /// </summary>
            WM_RBUTTONUP = 0x0205,

            /// <summary>
            /// Posted when the user double-clicks the right mouse button while the cursor is in the client area of a window.
            /// If the mouse is not captured, the message is posted to the window beneath the cursor.
            /// Otherwise, the message is posted to the window that has captured the mouse.
            ///
            /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-rbuttondblclk">WM_RBUTTONDBLCLK message</a>
            /// </summary>
            WM_RBUTTONDBLCLK = 0x0206,

            /// <summary>
            /// Posted when the user presses the middle mouse button while the cursor is in the client area of a window.
            /// If the mouse is not captured, the message is posted to the window beneath the cursor.
            /// Otherwise, the message is posted to the window that has captured the mouse.
            ///
            /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-mbuttondown">WM_MBUTTONDOWN message</a>
            /// </summary>
            WM_MBUTTONDOWN = 0x0207,

            /// <summary>
            /// Posted when the user releases the middle mouse button while the cursor is in the client area of a window.
            /// If the mouse is not captured, the message is posted to the window beneath the cursor.
            /// Otherwise, the message is posted to the window that has captured the mouse.
            ///
            /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-mbuttonup">WM_MBUTTONUP message</a>
            /// </summary>
            WM_MBUTTONUP = 0x0208,

            /// <summary>
            /// Posted when the user double-clicks the middle mouse button while the cursor is in the client area of a window.
            /// If the mouse is not captured, the message is posted to the window beneath the cursor.
            /// Otherwise, the message is posted to the window that has captured the mouse.
            ///
            /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-mbuttondblclk">WM_MBUTTONDBLCLK message</a>
            /// </summary>
            WM_MBUTTONDBLCLK = 0x0209,

            /// <summary>
            /// Used to define private messages for use by private window classes, usually of the form WM_USER+x, where x is an integer value.
            /// </summary>
            WM_USER = 0x0400,

            /// <summary>
            /// This message is only send when using NOTIFYICON_VERSION_4, the Shell now sends the associated application an NIN_SELECT notification.
            /// Send when a notify icon is activated with mouse or ENTER key.
            /// Earlier versions send WM_RBUTTONDOWN and WM_RBUTTONUP messages.
            /// </summary>
            NIN_SELECT = WM_USER,

            /// <summary>
            /// This message is only send when using NOTIFYICON_VERSION_4, the Shell now sends the associated application an NIN_SELECT notification.
            /// Send when a notify icon is activated with SPACEBAR or ENTER key.
            /// Earlier versions send WM_RBUTTONDOWN and WM_RBUTTONUP messages.
            /// </summary>
            NIN_KEYSELECT = WM_USER + 1,

            /// <summary>
            /// Sent when the balloon is shown (balloons are queued).
            /// </summary>
            NIN_BALLOONSHOW = WM_USER + 2,

            /// <summary>
            /// Sent when the balloon disappears. For example, when the icon is deleted.
            /// This message is not sent if the balloon is dismissed because of a timeout or if the user clicks the mouse.
            ///
            /// As of Windows 7, NIN_BALLOONHIDE is also sent when a notification with the NIIF_RESPECT_QUIET_TIME flag set attempts to display during quiet time (a user's first hour on a new computer).
            /// In that case, the balloon is never displayed at all.
            /// </summary>
            NIN_BALLOONHIDE = WM_USER + 3,

            /// <summary>
            /// Sent when the balloon is dismissed because of a timeout.
            /// </summary>
            NIN_BALLOONTIMEOUT = WM_USER + 4,

            /// <summary>
            /// Sent when the balloon is dismissed because the user clicked the mouse.
            /// </summary>
            NIN_BALLOONUSERCLICK = WM_USER + 5,

            /// <summary>
            /// Sent when the user hovers the cursor over an icon to indicate that the richer pop-up UI should be used in place of a standard textual tooltip.
            /// </summary>
            NIN_POPUPOPEN = WM_USER + 6,

            /// <summary>
            /// Sent when a cursor no longer hovers over an icon to indicate that the rich pop-up UI should be closed.
            /// </summary>
            NIN_POPUPCLOSE = WM_USER + 7
        }




        public struct MSG
        {
            public IntPtr hwnd;
            public UInt32 message;
            public UIntPtr wParam;
            public UIntPtr lParam;
            public UInt32 time;
            public POINT pt;
        }
        public enum NOTIFYICONMESSAGE : int
        {
            /// <summary>
            /// Adds an icon to the status area. The icon is given an identifier in the NOTIFYICONDATA structure pointed to by lpdata—either through its uID or guidItem member. This identifier is used in subsequent calls to Shell_NotifyIcon to perform later actions on the icon.
            /// </summary>
            NIM_ADD = 0x00000000,

            /// <summary>
            /// Modifies an icon in the status area. NOTIFYICONDATA structure pointed to by lpdata uses the ID originally assigned to the icon when it was added to the notification area (NIM_ADD) to identify the icon to be modified.
            /// </summary>
            NIM_MODIFY = 0x00000001,

            /// <summary>
            /// Deletes an icon from the status area. NOTIFYICONDATA structure pointed to by lpdata uses the ID originally assigned to the icon when it was added to the notification area (NIM_ADD) to identify the icon to be deleted.
            /// </summary>
            NIM_DELETE = 0x00000002,

            /// <summary>
            /// Shell32.dll version 5.0 and later only. Returns focus to the taskbar notification area. Notification area icons should use this message when they have completed their UI operation. For example, if the icon displays a shortcut menu, but the user presses ESC to cancel it, use NIM_SETFOCUS to return focus to the notification area.
            /// </summary>
            NIM_SETFOCUS = 0x00000003,

            /// <summary>
            /// Shell32.dll version 5.0 and later only. Instructs the notification area to behave according to the version number specified in the uVersion member of the structure pointed to by lpdata. The version number specifies which members are recognized.
            /// NIM_SETVERSION must be called every time a notification area icon is added (NIM_ADD)>. It does not need to be called with NIM_MOFIDY. The version setting is not persisted once a user logs off.
            /// </summary>
            NIM_SETVERSION = 0x00000004,
        }

        /// <summary>
        /// Sends a message to the taskbar's status area.
        /// </summary>
        /// <param name="dwMessage">A value that specifies the action to be taken by this function.</param>
        /// <param name="pnid">A pointer to a NOTIFYICONDATA structure. The content of the structure depends on the value of dwMessage. It can define an icon to add to the notification area, cause that icon to display a notification, or identify an icon to modify or delete.</param>
        /// <returns>Returns TRUE if successful, or FALSE otherwise. If dwMessage is set to NIM_SETVERSION, the function returns TRUE if the version was successfully changed, or FALSE if the requested version is not supported.</returns>
        [DllImport("shell32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool Shell_NotifyIcon(
            [In] NOTIFYICONMESSAGE dwMessage,
            [In] ref NOTIFYICONDATA pnid);
        /// <summary>
        /// Defines a new window message that is guaranteed to be unique throughout the system. The message value can be used when sending or posting messages.
        /// </summary>
        /// <param name="lpString">The message to be registered.</param>
        /// <returns>If the message is successfully registered, the return value is a message identifier in the range 0xC000 through 0xFFFF.  If the function fails, the return value is zero.</returns>
        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <param name="lpPoint">A pointer to a POINT structure that receives the screen coordinates of the cursor.</param>
        /// <returns>True, if successful.  Otherwise, false.</returns>
        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(
            ref POINT lpPoint);
        /// <summary>
        /// Retrieves the signed x-coordinate from the specified LPARAM value.
        /// </summary>
        /// <param name="ptr">The value to be converted.</param>
        /// <returns>The return value is the low-order int of the specified value.</returns>
        public static int GET_X_LPARAM(IntPtr ptr)
        {
            return LOWORD(ptr);
        }

        /// <summary>
        /// Retrieves the signed y-coordinate from the given LPARAM value.
        /// </summary>
        /// <param name="ptr">The value to be converted.</param>
        /// <returns>The return value is the high-order int of the specified value.</returns>
        public static int GET_Y_LPARAM(IntPtr ptr)
        {
            return HIWORD(ptr);
        }

        public static int LOWORD(IntPtr ptr)
        {
            return (ptr.ToInt32() & 0xFFFF);
        }
        public static int HIWORD(IntPtr ptr)
        {
            return (ptr.ToInt32() >> 16) & 0xFFFF;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>
            /// The x-coordinate of the point.
            /// </summary>
            public int X;

            /// <summary>
            /// The y-coordinate of the point.
            /// </summary>
            public int Y;

            /// <summary>
            /// Initializes a new instance of the POINT struct.
            /// </summary>
            /// <param name="x">The x-coordinate of the point.</param>
            /// <param name="y">The y-coordinate of the point.</param>
            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            /// <summary>
            /// Implicitly converts a POINT instance to a System.Drawing.Point instance.
            /// </summary>
            /// <param name="p">The POINT instance to be converted.</param>
            /// <returns>A System.Drawing.Point instance with the same X and Y values as the source.</returns>
            public static implicit operator System.Drawing.Point(POINT p)
            {
                return new System.Drawing.Point(p.X, p.Y);
            }

            /// <summary>
            /// Implicitly converts a System.Drawing.Point instance to a POINT instance.
            /// </summary>
            /// <param name="p">The System.Drawing.Point instance to be converted.</param>
            /// <returns>A POINT instance with the same X and Y values as the source.</returns>
            public static implicit operator POINT(System.Drawing.Point p)
            {
                return new POINT(p.X, p.Y);
            }
        }

        public enum WindowMessages : uint
        {
            WM_NULL = 0x00,
            WM_DESTROY = 0x02,
            WM_CLOSE = 0x10,
            WM_GETICON = 0x7F,
            WM_SETICON = 0x80,
            WM_MEASUREITEM = 0x2C,
            WM_MOUSEMOVE = 0x200,
            WM_LBUTTONDOWN = 0x201,
            WM_LBUTTONUP = 0x202,
            WM_LBUTTONDBLCLK = 0x203,
            WM_RBUTTONDOWN = 0x204,
            WM_RBUTTONUP = 0x205,
            WM_RBUTTONDBLCLK = 0x206,
            WM_MBUTTONDOWN = 0x207,
            WM_MBUTTONUP = 0x208,
            WM_MBUTTONDBLCLK = 0x209,
            WM_MOUSEWHEEL = 0x20A,
            WM_MOUSEHWHEEL = 0x20E,
            WM_USER = 0x400,

            NIN_SELECT = WindowMessages.WM_USER + 0,
            NIN_KEYSELECT = WindowMessages.WM_USER + 1,
            NIN_BALLOONSHOW = WindowMessages.WM_USER + 2,
            NIN_BALLOONHIDE = WindowMessages.WM_USER + 3,
            NIN_BALLOONTIMEOUT = WindowMessages.WM_USER + 4,
            NIN_BALLOONCLICK = WindowMessages.WM_USER + 5,
            NIN_POPUPOPEN = WindowMessages.WM_USER + 6,
            NIN_POPUPCLOSE = WindowMessages.WM_USER + 7,
        }

        /// <summary>
        /// Receives information used to retrieve a stock Shell icon. This structure is used in a call SHGetStockIconInfo.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHSTOCKICONINFO
        {
            /// <summary>
            /// The size of this structure, in bytes.
            /// </summary>
            public DWORD cbSize;

            /// <summary>
            /// When SHGetStockIconInfo is called with the SHGSI_ICON flag, this member receives a handle to the icon.
            /// </summary>
            public HICON hIcon;

            /// <summary>
            /// When SHGetStockIconInfo is called with the SHGSI_SYSICONINDEX flag, this member receives the index of the image in the system icon cache.
            /// </summary>
            public int iSysIconIndex;

            /// <summary>
            /// When SHGetStockIconInfo is called with the SHGSI_ICONLOCATION flag, this member receives the index of the icon in the resource whose path is received in szPath.
            /// </summary>
            public int iIcon;

            /// <summary>
            /// When SHGetStockIconInfo is called with the SHGSI_ICONLOCATION flag, this member receives the path of the resource that contains the icon. The index of the icon within the resource is received in iIcon.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string szPath;
        }

        /// <summary>
        /// Retrieves information about system-defined Shell icons.
        /// </summary>
        /// <param name="siid">One of the values from the SHSTOCKICONID enumeration that specifies which icon should be retrieved.</param>
        /// <param name="uFlags">A combination of zero or more of the following flags that specify which information is requested.</param>
        /// <param name="psii">A pointer to a SHSTOCKICONINFO structure. When this function is called, the cbSize member of this structure needs to be set to the size of the SHSTOCKICONINFO structure. When this function returns, contains a pointer to a SHSTOCKICONINFO structure that contains the requested information.</param>
        /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <remarks>If this function returns an icon handle in the hIcon member of the SHSTOCKICONINFO structure pointed to by psii, you are responsible for freeing the icon with DestroyIcon when you no longer need it.</remarks>
        [DllImport("shell32")]
        public static extern uint SHGetStockIconInfo(
            SHSTOCKICONID siid,
            SHGetStockIconFlags uFlags,
            ref SHSTOCKICONINFO psii);

        public enum SHSTOCKICONID
        {
            SIID_DOCNOASSOC = 0,
            SIID_DOCASSOC = 1,
            SIID_APPLICATION = 2,
            SIID_FOLDER = 3,
            SIID_FOLDEROPEN = 4,
            SIID_DRIVE525 = 5,
            SIID_DRIVE35 = 6,
            SIID_DRIVEREMOVE = 7,
            SIID_DRIVEFIXED = 8,
            SIID_DRIVENET = 9,
            SIID_DRIVENETDISABLED = 10,
            SIID_DRIVECD = 11,
            SIID_DRIVERAM = 12,
            SIID_WORLD = 13,
            SIID_SERVER = 15,
            SIID_PRINTER = 16,
            SIID_MYNETWORK = 17,
            SIID_FIND = 22,
            SIID_HELP = 23,
            SIID_SHARE = 28,
            SIID_LINK = 29,
            SIID_SLOWFILE = 30,
            SIID_RECYCLER = 31,
            SIID_RECYCLERFULL = 32,
            SIID_MEDIACDAUDIO = 40,
            SIID_LOCK = 47,
            SIID_AUTOLIST = 49,
            SIID_PRINTERNET = 50,
            SIID_SERVERSHARE = 51,
            SIID_PRINTERFAX = 52,
            SIID_PRINTERFAXNET = 53,
            SIID_PRINTERFILE = 54,
            SIID_STACK = 55,
            SIID_MEDIASVCD = 56,
            SIID_STUFFEDFOLDER = 57,
            SIID_DRIVEUNKNOWN = 58,
            SIID_DRIVEDVD = 59,
            SIID_MEDIADVD = 60,
            SIID_MEDIADVDRAM = 61,
            SIID_MEDIADVDRW = 62,
            SIID_MEDIADVDR = 63,
            SIID_MEDIADVDROM = 64,
            SIID_MEDIACDAUDIOPLUS = 65,
            SIID_MEDIACDRW = 66,
            SIID_MEDIACDR = 67,
            SIID_MEDIACDBURN = 68,
            SIID_MEDIABLANKCD = 69,
            SIID_MEDIACDROM = 70,
            SIID_AUDIOFILES = 71,
            SIID_IMAGEFILES = 72,
            SIID_VIDEOFILES = 73,
            SIID_MIXEDFILES = 74,
            SIID_FOLDERBACK = 75,
            SIID_FOLDERFRONT = 76,
            SIID_SHIELD = 77,
            SIID_WARNING = 78,
            SIID_INFO = 79,
            SIID_ERROR = 80,
            SIID_KEY = 81,
            SIID_SOFTWARE = 82,
            SIID_RENAME = 83,
            SIID_DELETE = 84,
            SIID_MEDIAAUDIODVD = 85,
            SIID_MEDIAMOVIEDVD = 86,
            SIID_MEDIAENHANCEDCD = 87,
            SIID_MEDIAENHANCEDDVD = 88,
            SIID_MEDIAHDDVD = 89,
            SIID_MEDIABLURAY = 90,
            SIID_MEDIAVCD = 91,
            SIID_MEDIADVDPLUSR = 92,
            SIID_MEDIADVDPLUSRW = 93,
            SIID_DESKTOPPC = 94,
            SIID_MOBILEPC = 95,
            SIID_USERS = 96,
            SIID_MEDIASMARTMEDIA = 97,
            SIID_MEDIACOMPACTFLASH = 98,
            SIID_DEVICECELLPHONE = 99,
            SIID_DEVICECAMERA = 100,
            SIID_DEVICEVIDEOCAMERA = 101,
            SIID_DEVICEAUDIOPLAYER = 102,
            SIID_NETWORKCONNECT = 103,
            SIID_INTERNET = 104,
            SIID_ZIPFILE = 105,
            SIID_SETTINGS = 106,
            SIID_DRIVEHDDVD = 132,
            SIID_DRIVEBD = 133,
            SIID_MEDIAHDDVDROM = 134,
            SIID_MEDIAHDDVDR = 135,
            SIID_MEDIAHDDVDRAM = 136,
            SIID_MEDIABDROM = 137,
            SIID_MEDIABDR = 138,
            SIID_MEDIABDRE = 139,
            SIID_CLUSTEREDDRIVE = 140,
            SIID_MAX_ICONS = 175
        }

        [Flags]
        public enum SHGetStockIconFlags : uint
        {
            /// <summary>
            /// The szPath and iIcon members of the SHSTOCKICONINFO structure receive the path and icon index of the requested icon, in a format suitable for passing to the ExtractIcon function. The numerical value of this flag is zero, so you always get the icon location regardless of other flags.
            /// </summary>
            SHGSI_ICONLOCATION = 0,

            /// <summary>
            /// The hIcon member of the SHSTOCKICONINFO structure receives a handle to the specified icon.
            /// </summary>
            SHGSI_ICON = 0x000000100,

            /// <summary>
            /// The iSysImageImage member of the SHSTOCKICONINFO structure receives the index of the specified icon in the system imagelist.
            /// </summary>
            SHGSI_SYSICONINDEX = 0x000004000,

            /// <summary>
            /// Modifies the SHGSI_ICON value by causing the function to add the link overlay to the file's icon.
            /// </summary>
            SHGSI_LINKOVERLAY = 0x000008000,

            /// <summary>
            /// Modifies the SHGSI_ICON value by causing the function to blend the icon with the system highlight color.
            /// </summary>
            SHGSI_SELECTED = 0x000010000,

            /// <summary>
            /// Modifies the SHGSI_ICON value by causing the function to retrieve the large version of the icon, as specified by the SM_CXICON and SM_CYICON system metrics.
            /// </summary>
            SHGSI_LARGEICON = 0x000000000,

            /// <summary>
            /// Modifies the SHGSI_ICON value by causing the function to retrieve the small version of the icon, as specified by the SM_CXSMICON and SM_CYSMICON system metrics.
            /// </summary>
            SHGSI_SMALLICON = 0x000000001,

            /// <summary>
            /// Modifies the SHGSI_LARGEICON or SHGSI_SMALLICON values by causing the function to retrieve the Shell-sized icons rather than the sizes specified by the system metrics.
            /// </summary>
            SHGSI_SHELLICONSIZE = 0x000000004
        }

        [Flags]
        public enum NOTIFYICONFLAGS : int
        {
            /// <summary>
            /// The uCallbackMessage member is valid.
            /// </summary>
            NIF_MESSAGE = 0x00000001,

            /// <summary>
            /// The hIcon member is valid.
            /// </summary>
            NIF_ICON = 0x00000002,

            /// <summary>
            /// The szTip member is valid.
            /// </summary>
            NIF_TIP = 0x00000004,

            /// <summary>
            /// The dwState and dwStateMask members are valid.
            /// </summary>
            NIF_STATE = 0x00000008,

            /// <summary>
            /// Display a balloon notification. The szInfo, szInfoTitle, dwInfoFlags, and uTimeout members are valid. Note that uTimeout is valid only in Windows 2000 and Windows XP.
            /// To display the balloon notification, specify NIF_INFO and provide text in szInfo.
            /// To remove a balloon notification, specify NIF_INFO and provide an empty string through szInfo.
            /// To add a notification area icon without displaying a notification, do not set the NIF_INFO flag.
            /// </summary>
            NIF_INFO = 0x00000010,

            /// <summary>
            /// Windows 7 and later: The guidItem is valid.
            /// Windows Vista and earlier: Reserved.
            /// </summary>
            NIF_GUID = 0x00000020,

            /// <summary>
            /// Windows Vista and later. If the balloon notification cannot be displayed immediately, discard it. Use this flag for notifications that represent real-time information which would be meaningless or misleading if displayed at a later time. For example, a message that states "Your telephone is ringing." NIF_REALTIME is meaningful only when combined with the NIF_INFO flag.
            /// </summary>
            NIF_REALTIME = 0x00000040,

            /// <summary>
            /// Windows Vista and later. Use the standard tooltip. Normally, when uVersion is set to NOTIFYICON_VERSION_4, the standard tooltip is suppressed and can be replaced by the application-drawn, pop-up UI. If the application wants to show the standard tooltip with NOTIFYICON_VERSION_4, it can specify NIF_SHOWTIP to indicate the standard tooltip should still be shown.
            /// </summary>
            NIF_SHOWTIP = 0x00000080,
        }

        public enum NOTIFYICONSTATE : int
        {
            /// <summary>
            /// The icon is hidden.
            /// </summary>
            NIS_HIDDEN = 0x00000001,

            /// <summary>
            /// The icon resource is shared between multiple icons.
            /// </summary>
            NIS_SHAREDICON = 0x00000002,
        }

        public enum NOTIFYICONVERSION : uint
        {
            /// <summary>
            /// Legacy support - expects a size of 488.
            /// </summary>
            NOTIFYICON_VERSION = 0x0,

            /// <summary>
            /// Windows 2000 support - expects a size of 504.
            /// </summary>
            NOTIFYICON_VERSION_3 = 0x3,

            /// <summary>
            /// Vista support.
            /// </summary>
            NOTIFYICON_VERSION_4 = 0x4,
        }

        [Flags]
        public enum NOTIFYICONINFOFLAGS : int
        {
            /// <summary>
            /// No icon.
            /// </summary>
            NIIF_NONE = 0x00000000,

            /// <summary>
            /// An information icon.
            /// </summary>
            NIIF_INFO = 0x00000001,

            /// <summary>
            /// A warning icon.
            /// </summary>
            NIIF_WARNING = 0x00000002,

            /// <summary>
            /// An error icon.
            /// </summary>
            NIIF_ERROR = 0x00000003,

            /// <summary>
            /// Windows XP: Use the icon identified in hIcon as the notification balloon's title icon.
            /// Windows Vista and later: Use the icon identified in hBalloonIcon as the notification balloon's title icon.
            /// </summary>                
            NIIF_USER = 0x00000004,

            /// <summary>
            /// Windows XP and later. Do not play the associated sound. Applies only to notifications.
            /// </summary>
            NIIF_NOSOUND = 0x00000010,

            /// <summary>
            /// Windows Vista and later. The large version of the icon should be used as the notification icon. This corresponds to the icon with dimensions SM_CXICON x SM_CYICON. If this flag is not set, the icon with dimensions XM_CXSMICON x SM_CYSMICON is used.
            /// <list type="">
            /// <item>This flag can be used with all stock icons.</item>
            /// <item>Applications that use older customized icons (NIIF_USER with hIcon) must provide a new SM_CXICON x SM_CYICON version in the tray icon (hIcon). These icons are scaled down when they are displayed in the System Tray or System Control Area (SCA).</item>
            /// <item>New customized icons (NIIF_USER with hBalloonIcon) must supply an SM_CXICON x SM_CYICON version in the supplied icon (hBalloonIcon).</item>
            /// </list>
            /// </summary>
            NIIF_LARGE_ICON = 0x00000020,

            /// <summary>
            /// Windows 7 and later. Do not display the balloon notification if the current user is in "quiet time", which is the first hour after a new user logs into his or her account for the first time. During this time, most notifications should not be sent or shown. This lets a user become accustomed to a new computer system without those distractions. Quiet time also occurs for each user after an operating system upgrade or clean installation. A notification sent with this flag during quiet time is not queued; it is simply dismissed unshown. The application can resend the notification later if it is still valid at that time.
            /// Because an application cannot predict when it might encounter quiet time, we recommended that this flag always be set on all appropriate notifications by any application that means to honor quiet time.
            /// During quiet time, certain notifications should still be sent because they are expected by the user as feedback in response to a user action, for instance when he or she plugs in a USB device or prints a document.
            /// If the current user is not in quiet time, this flag has no effect.
            /// </summary>
            NIIF_RESPECT_QUIET_TIME = 0x00000080,

            /// <summary>
            /// Windows XP and later. Reserved.
            /// </summary>
            NIIF_ICON_MASK = 0x0000000F,
        }


        [StructLayout(LayoutKind.Sequential)] //, CharSet = CharSet.Auto)]
        public struct NOTIFYICONDATA
        {
            #region Members

            /// <summary>
            /// Size of this structure, in bytes.
            /// </summary>
            public DWORD cbSize;

            /// <summary>
            /// Handle to the window that receives notification messages associated with an icon in the
            /// taskbar status area. The Shell uses hWnd and uID to identify which icon to operate on
            /// when Shell_NotifyIcon is invoked.
            /// </summary>
            public IntPtr hwnd;

            /// <summary>
            /// Application-defined identifier of the taskbar icon. The Shell uses hWnd and uID to identify
            /// which icon to operate on when Shell_NotifyIcon is invoked. You can have multiple icons
            /// associated with a single hWnd by assigning each a different uID.
            /// </summary>
            public int uID;

            /// <summary>
            /// Flags that indicate which of the other members contain valid data. This member can be
            /// a combination of the NIF_XXX constants.
            /// </summary>
            public NOTIFYICONFLAGS uFlags;

            /// <summary>
            /// Application-defined message identifier. The system uses this identifier to send
            /// notifications to the window identified in hWnd.
            /// </summary>
            public int uCallbackMessage;

            /// <summary>
            /// Handle to the icon to be added, modified, or deleted.
            /// </summary>
            public HICON hIcon;

            /// <summary>
            /// String with the text for a standard ToolTip. It can have a maximum of 64 characters including
            /// the terminating NULL. For Version 5.0 and later, szTip can have a maximum of
            /// 128 characters, including the terminating NULL.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szTip;

            /// <summary>
            /// State of the icon.
            /// </summary>
            public NOTIFYICONSTATE dwState;

            /// <summary>
            /// A value that specifies which bits of the state member are retrieved or modified.
            /// For example, setting this member to NIS_HIDDEN causes only the item's hidden state to be retrieved.
            /// </summary>
            public NOTIFYICONSTATE dwStateMask;

            /// <summary>
            /// String with the text for a balloon ToolTip. It can have a maximum of 255 characters.
            /// To remove the ToolTip, set the NIF_INFO flag in uFlags and set szInfo to an empty string.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szInfo;

            /// <summary>
            /// NOTE: This field is also used for the Timeout value. Specifies whether the Shell notify
            /// icon interface should use Windows 95 or Windows 2000
            /// behavior. For more information on the differences in these two behaviors, see
            /// Shell_NotifyIcon. This member is only employed when using 
            /// to send an
            /// NIM_VERSION message.
            /// </summary>
            public NOTIFYICONVERSION uTimeoutOrVersion;

            /// <summary>
            /// String containing a title for a balloon ToolTip. This title appears in boldface
            /// above the text. It can have a maximum of 63 characters.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szInfoTitle;

            /// <summary>
            /// Adds an icon to a balloon ToolTip. It is placed to the left of the title. If the
            /// szTitleInfo member is zero-length, the icon is not shown. See BalloonIconStyle
            /// for more information.
            /// </summary>
            public NOTIFYICONINFOFLAGS dwInfoFlags;

            public Guid guidItem;

            public HICON hBalloonIcon;

            #endregion

            [DllImport("user32.dll")]
            public static extern int GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

            [DllImport("user32.dll")]
            public static extern bool TranslateMessage([In] ref MSG lpMsg);

            [DllImport("user32.dll")]
            public static extern IntPtr DispatchMessage([In] ref MSG lpmsg);
        }

    }
}
