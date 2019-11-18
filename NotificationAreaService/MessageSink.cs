using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static NotificationAreaService.NativeMethods;

namespace NotificationAreaService
{
    public class MessageSink : IDisposable
    {

        public const int CallbackMessageId = 0x400;
        internal string WindowId { get; private set; }
        private WindowProcedureHandler messageHandler;
        bool isDoubleClick = false;

        private IntPtr _windowHandle;
        public IntPtr WindowHandle
        {
            get { return _windowHandle; }
            private set { _windowHandle = value; }
        }

        public event Action<MouseEvent> MouseEventReceived;
        public event Action<bool> BalloonToolTipChanged;
        public event Action<bool> ChangeToolTipStateRequest;

        public MessageSink()
        {
            CreateMessageWindow();
        }

        private void CreateMessageWindow()
        {
            //generate a unique ID for the window
            WindowId = "MessageSinkWindow" + Guid.NewGuid();
            //register window message handler
            messageHandler = OnWindowMessageReceived;

            WindowClass wc;

            wc.style = 0;
            wc.lpfnWndProc = messageHandler;
            wc.cbClsExtra = 0;
            wc.cbWndExtra = 0;
            wc.hInstance = IntPtr.Zero;
            wc.hIcon = IntPtr.Zero;
            wc.hCursor = IntPtr.Zero;
            wc.hbrBackground = IntPtr.Zero;
            wc.lpszMenuName = string.Empty;
            wc.lpszClassName = WindowId;

            RegisterClass(ref wc);

            WindowHandle = CreateWindowEx(0, WindowId, "", 0, 0, 0, 1, 1, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

            if (WindowHandle == IntPtr.Zero) throw new Exception("Message sink could not be created.");

        }

        private IntPtr OnWindowMessageReceived(IntPtr hWnd, uint messageId, IntPtr wParam, IntPtr lParam)
        {
            ProcessMessage(messageId, wParam, lParam);

            // Pass the message to the default window procedure
            return DefWindowProc(hWnd, messageId, wParam, lParam);
        }

        private void ProcessMessage(uint msg, IntPtr wParam, IntPtr lParam)
        {
            // nothing to do if not registered callback.
            if (msg != CallbackMessageId) return;

            var message = (WindowsMessages)lParam.ToInt32();

            switch (message)
            {
                case WindowsMessages.WM_CONTEXTMENU:
                    // TODO: Handle WM_CONTEXTMENU, see https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shell_notifyiconw
                    Debug.WriteLine("Unhandled WM_CONTEXTMENU");
                    break;

                case WindowsMessages.WM_MOUSEMOVE:
                    MouseEventReceived?.Invoke(MouseEvent.MouseMove);
                    break;

                case WindowsMessages.WM_LBUTTONDOWN:
                    MouseEventReceived?.Invoke(MouseEvent.IconLeftMouseDown);
                    break;

                case WindowsMessages.WM_LBUTTONUP:
                    if (!isDoubleClick)
                    {
                        MouseEventReceived?.Invoke(MouseEvent.IconLeftMouseUp);
                    }
                    isDoubleClick = false;
                    break;

                case WindowsMessages.WM_LBUTTONDBLCLK:
                    isDoubleClick = true;
                    MouseEventReceived?.Invoke(MouseEvent.IconDoubleClick);
                    break;

                case WindowsMessages.WM_RBUTTONDOWN:
                    MouseEventReceived?.Invoke(MouseEvent.IconRightMouseDown);
                    break;

                case WindowsMessages.WM_RBUTTONUP:
                    MouseEventReceived?.Invoke(MouseEvent.IconRightMouseUp);
                    break;

                case WindowsMessages.WM_RBUTTONDBLCLK:
                    //double click with right mouse button - do not trigger event
                    break;

                case WindowsMessages.WM_MBUTTONDOWN:
                    MouseEventReceived?.Invoke(MouseEvent.IconMiddleMouseDown);
                    break;

                case WindowsMessages.WM_MBUTTONUP:
                    MouseEventReceived?.Invoke(MouseEvent.IconMiddleMouseUp);
                    break;

                case WindowsMessages.WM_MBUTTONDBLCLK:
                    //double click with middle mouse button - do not trigger event
                    break;

                case WindowsMessages.NIN_BALLOONSHOW:
                    BalloonToolTipChanged?.Invoke(true);
                    break;

                case WindowsMessages.NIN_BALLOONHIDE:
                case WindowsMessages.NIN_BALLOONTIMEOUT:
                    BalloonToolTipChanged?.Invoke(false);
                    break;

                case WindowsMessages.NIN_BALLOONUSERCLICK:
                    MouseEventReceived?.Invoke(MouseEvent.BalloonToolTipClicked);
                    break;

                case WindowsMessages.NIN_POPUPOPEN:
                    ChangeToolTipStateRequest?.Invoke(true);
                    break;

                case WindowsMessages.NIN_POPUPCLOSE:
                    ChangeToolTipStateRequest?.Invoke(false);
                    break;

                case WindowsMessages.NIN_SELECT:
                    // TODO: Handle NIN_SELECT see https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shell_notifyiconw
                    Debug.WriteLine("Unhandled NIN_SELECT");
                    break;

                case WindowsMessages.NIN_KEYSELECT:
                    // TODO: Handle NIN_KEYSELECT see https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shell_notifyiconw
                    Debug.WriteLine("Unhandled NIN_KEYSELECT");
                    break;

                default:
                    Debug.WriteLine("Unhandled NotifyIcon message ID: " + lParam);
                    break;

            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
