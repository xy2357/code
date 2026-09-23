using System.Runtime.InteropServices;
using System.Text;

namespace WindowPins;

internal static class NativeMethods
{
    private const int GwlExStyle = -20;
    private const int GwlpHwndParent = -8;
    private const long WsExTopmost = 0x00000008;
    private const uint GaRoot = 2;
    private const uint SwpNoSize = 0x0001;
    private const uint SwpNoMove = 0x0002;
    private const uint SwpNoZOrder = 0x0004;
    private const uint SwpNoActivate = 0x0010;
    private const uint SwpNoOwnerZOrder = 0x0200;
    private static readonly IntPtr HwndTopmost = new(-1);
    private static readonly IntPtr HwndNoTopmost = new(-2);

    [StructLayout(LayoutKind.Sequential)]
    internal struct Point
    {
        internal int X;
        internal int Y;

        internal Point(int x, int y) => (X, Y) = (x, y);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Rect
    {
        internal int Left;
        internal int Top;
        internal int Right;
        internal int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct WindowSize
    {
        internal int Width;
        internal int Height;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct BlendFunction
    {
        internal byte BlendOp;
        internal byte BlendFlags;
        internal byte SourceConstantAlpha;
        internal byte AlphaFormat;
    }

    [DllImport("user32.dll")]
    private static extern IntPtr WindowFromPoint(Point point);

    [DllImport("user32.dll")]
    private static extern IntPtr GetAncestor(IntPtr window, uint flags);

    [DllImport("user32.dll")]
    internal static extern bool IsWindow(IntPtr window);

    [DllImport("user32.dll")]
    internal static extern bool IsWindowVisible(IntPtr window);

    [DllImport("user32.dll")]
    internal static extern bool IsIconic(IntPtr window);

    [DllImport("user32.dll")]
    internal static extern bool GetWindowRect(IntPtr window, out Rect rect);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPos(IntPtr window, IntPtr insertAfter,
        int x, int y, int width, int height, uint flags);

    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtrW", SetLastError = true)]
    private static extern IntPtr GetWindowLongPtr(IntPtr window, int index);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtrW", SetLastError = true)]
    private static extern IntPtr SetWindowLongPtr(IntPtr window, int index, IntPtr value);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetWindowTextLength(IntPtr window);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetWindowText(IntPtr window, StringBuilder text, int maxCount);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetClassName(IntPtr window, StringBuilder name, int maxCount);

    [DllImport("user32.dll")]
    private static extern IntPtr GetDesktopWindow();

    [DllImport("user32.dll")]
    private static extern IntPtr GetShellWindow();

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr window, out uint processId);

    [DllImport("user32.dll")]
    internal static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    internal static extern bool SetForegroundWindow(IntPtr window);

    [DllImport("user32.dll")]
    internal static extern bool DestroyIcon(IntPtr icon);

    [DllImport("user32.dll")]
    private static extern IntPtr GetDC(IntPtr window);

    [DllImport("user32.dll")]
    private static extern int ReleaseDC(IntPtr window, IntPtr dc);

    [DllImport("gdi32.dll")]
    private static extern IntPtr CreateCompatibleDC(IntPtr dc);

    [DllImport("gdi32.dll")]
    private static extern bool DeleteDC(IntPtr dc);

    [DllImport("gdi32.dll")]
    private static extern IntPtr SelectObject(IntPtr dc, IntPtr bitmap);

    [DllImport("gdi32.dll")]
    private static extern bool DeleteObject(IntPtr bitmap);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool UpdateLayeredWindow(IntPtr window, IntPtr targetDc,
        ref Point destination, ref WindowSize size, IntPtr sourceDc, ref Point source,
        uint colorKey, ref BlendFunction blend, uint flags);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern bool SetProp(IntPtr window, string name, IntPtr data);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    internal static extern IntPtr GetProp(IntPtr window, string name);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    internal static extern IntPtr RemoveProp(IntPtr window, string name);

    [DllImport("user32.dll")]
    internal static extern uint GetDpiForWindow(IntPtr window);

    [DllImport("dwmapi.dll", EntryPoint = "DwmGetWindowAttribute")]
    private static extern int GetFrameBounds(IntPtr window, int attribute, out Rect rect, int size);

    [DllImport("dwmapi.dll", EntryPoint = "DwmGetWindowAttribute")]
    private static extern int GetCloaked(IntPtr window, int attribute, out int cloaked, int size);

    internal static bool IsCloaked(IntPtr window) =>
        GetCloaked(window, 14, out var cloaked, sizeof(int)) == 0 && cloaked != 0;

    internal static bool TryGetFrameBounds(IntPtr window, out Rect rect) =>
        GetFrameBounds(window, 9, out rect, Marshal.SizeOf<Rect>()) == 0 || GetWindowRect(window, out rect);

    internal static IntPtr WindowAt(System.Drawing.Point point)
    {
        var child = WindowFromPoint(new Point(point.X, point.Y));
        return child == IntPtr.Zero ? IntPtr.Zero : GetAncestor(child, GaRoot);
    }

    internal static bool IsSelectableWindow(IntPtr window)
    {
        if (window == IntPtr.Zero || !IsWindow(window) || !IsWindowVisible(window) ||
            window == GetDesktopWindow() || window == GetShellWindow())
            return false;

        GetWindowThreadProcessId(window, out var processId);
        if (processId == Environment.ProcessId)
            return false;

        var className = new StringBuilder(256);
        GetClassName(window, className, className.Capacity);
        return className.ToString() is not ("Progman" or "WorkerW" or "Shell_TrayWnd" or "Shell_SecondaryTrayWnd");
    }

    internal static bool IsTopmost(IntPtr window) =>
        (GetWindowLongPtr(window, GwlExStyle).ToInt64() & WsExTopmost) != 0;

    internal static bool SetTopmost(IntPtr window, bool topmost) =>
        SetWindowPos(window, topmost ? HwndTopmost : HwndNoTopmost,
            0, 0, 0, 0, SwpNoMove | SwpNoSize | SwpNoActivate);

    internal static bool RaiseBadge(IntPtr window) =>
        SetWindowPos(window, HwndTopmost, 0, 0, 0, 0,
            SwpNoMove | SwpNoSize | SwpNoActivate | SwpNoOwnerZOrder);

    internal static void SetLayeredImage(IntPtr window, Bitmap bitmap, System.Drawing.Point location)
    {
        var screenDc = GetDC(IntPtr.Zero);
        if (screenDc == IntPtr.Zero)
            throw new System.ComponentModel.Win32Exception(Marshal.GetLastPInvokeError());

        IntPtr imageDc = IntPtr.Zero;
        IntPtr imageHandle = IntPtr.Zero;
        IntPtr previousImage = IntPtr.Zero;
        try
        {
            imageDc = CreateCompatibleDC(screenDc);
            if (imageDc == IntPtr.Zero)
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastPInvokeError());

            imageHandle = bitmap.GetHbitmap(Color.FromArgb(0));
            previousImage = SelectObject(imageDc, imageHandle);
            if (previousImage == IntPtr.Zero)
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastPInvokeError());

            var destination = new Point(location.X, location.Y);
            var source = new Point(0, 0);
            var size = new WindowSize { Width = bitmap.Width, Height = bitmap.Height };
            var blend = new BlendFunction { SourceConstantAlpha = 255, AlphaFormat = 1 };
            if (!UpdateLayeredWindow(window, screenDc, ref destination, ref size,
                imageDc, ref source, 0, ref blend, 2))
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastPInvokeError());
        }
        finally
        {
            if (previousImage != IntPtr.Zero)
                SelectObject(imageDc, previousImage);
            if (imageHandle != IntPtr.Zero)
                DeleteObject(imageHandle);
            if (imageDc != IntPtr.Zero)
                DeleteDC(imageDc);
            ReleaseDC(IntPtr.Zero, screenDc);
        }
    }

    internal static void SetOwner(IntPtr child, IntPtr owner)
    {
        Marshal.SetLastPInvokeError(0);
        var previous = SetWindowLongPtr(child, GwlpHwndParent, owner);
        if (previous == IntPtr.Zero && Marshal.GetLastPInvokeError() != 0)
            throw new System.ComponentModel.Win32Exception(Marshal.GetLastPInvokeError());
    }

    internal static void MoveWithoutActivation(IntPtr window, int x, int y, int width, int height) =>
        SetWindowPos(window, IntPtr.Zero, x, y, width, height, SwpNoZOrder | SwpNoActivate);

    internal static string WindowTitle(IntPtr window)
    {
        var length = GetWindowTextLength(window);
        if (length > 0)
        {
            var text = new StringBuilder(length + 1);
            GetWindowText(window, text, text.Capacity);
            if (text.Length > 0)
                return text.ToString();
        }

        return $"窗口 0x{window.ToInt64():X}";
    }
}
