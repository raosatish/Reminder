using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Interop;

namespace NotificationAreaService
{
    public class NotificationAreaManager
    {

        private readonly int _id;
        private HandleRef _icon;
        private HandleRef _window;
        private NotificationWindow _notificationWindow = new NotificationWindow();
        object _synclock = new object();

        bool _isCreated = false;

        public void Start(IntPtr iconHandle)
        {
            _icon = new HandleRef(this, iconHandle);
            MessageSink sink = new MessageSink();
            _window = new HandleRef(this, sink.WindowHandle);
            sink.MouseEventReceived += Sink_MouseEventReceived;
            CreateNotifyIcon();
        }

        private void Sink_MouseEventReceived(NativeMethods.MouseEvent obj)
        {
        }

        private void CreateNotifyIcon()
        {
            if (_isCreated) return;
            lock (_synclock)
            {
                NativeMethods.NOTIFYICONDATA iconData = CreateDefaultData();
                _isCreated = NativeMethods.Shell_NotifyIcon(
                        NativeMethods.NOTIFYICONMESSAGE.NIM_ADD,
                        ref iconData);

            }
        }


        private NativeMethods.NOTIFYICONDATA CreateDefaultData()
        {
            NativeMethods.NOTIFYICONDATA data = new NativeMethods.NOTIFYICONDATA();

            // Get the size of the structure based on what Windows supports.
            if (Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Revision > 6) // >v6.0.6
            {
                data.cbSize = Marshal.SizeOf(typeof(NativeMethods.NOTIFYICONDATA));
            }
            else if (Environment.OSVersion.Version.Major >= 6)
            {
                data.cbSize = NativeMethods.NOTIFYICONDATA_V3_SIZE;
            }
            else if (Environment.OSVersion.Version.Major >= 5)
            {
                data.cbSize = NativeMethods.NOTIFYICONDATA_V2_SIZE;
            }
            else
            {
                data.cbSize = NativeMethods.NOTIFYICONDATA_V1_SIZE;
            }

            data.uFlags = NativeMethods.NOTIFYICONFLAGS.NIF_MESSAGE;
            data.hwnd = _window.Handle;
            data.uID = _id;
            data.uCallbackMessage = MessageSink.CallbackMessageId;
            {
                if (_icon.Handle != IntPtr.Zero)
                {
                    data.hIcon = _icon.Handle;
                    data.uFlags |= NativeMethods.NOTIFYICONFLAGS.NIF_ICON;
                }
            }
            {
                data.szTip = "Default Tool Tip";
                data.uFlags |= NativeMethods.NOTIFYICONFLAGS.NIF_TIP;
                data.uFlags |= NativeMethods.NOTIFYICONFLAGS.NIF_SHOWTIP;
                data.uFlags |= NativeMethods.NOTIFYICONFLAGS.NIF_INFO;
            }
            data.dwState = NativeMethods.NOTIFYICONSTATE.NIS_HIDDEN;
            data.dwStateMask = NativeMethods.NOTIFYICONSTATE.NIS_HIDDEN;
            data.szInfo = string.Empty;
            data.uTimeoutOrVersion = NativeMethods.NOTIFYICONVERSION.NOTIFYICON_VERSION_4;
            data.szInfoTitle = string.Empty;
            data.dwInfoFlags = NativeMethods.NOTIFYICONINFOFLAGS.NIIF_NONE;
            data.guidItem = Guid.Empty; // This is used for icon identification on Windows 7 or greater, currently not supported.
            data.hBalloonIcon = IntPtr.Zero; // This is used for custom icons on Vista or greater, currently not supported.

            return data;
        }

        public void ShowBalloon(string title, string text, uint timeout)
        {
            if (!_isCreated) return;
            lock (_synclock)
            {
                NativeMethods.NOTIFYICONDATA iconData = CreateDefaultData();
                iconData.uFlags = NativeMethods.NOTIFYICONFLAGS.NIF_INFO;
                iconData.szInfo = text;
                iconData.szInfoTitle = title;
                _isCreated = NativeMethods.Shell_NotifyIcon(
                        NativeMethods.NOTIFYICONMESSAGE.NIM_MODIFY,
                        ref iconData);

            }
        }
    }
}
