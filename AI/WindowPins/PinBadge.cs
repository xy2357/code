using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace WindowPins;

internal sealed class PinBadge : Form
{
    private readonly IntPtr _target;
    private readonly Action _unpin;
    private readonly ToolTip _tooltip = new();

    internal PinBadge(IntPtr target, Action unpin)
    {
        _target = target;
        _unpin = unpin;
        FormBorderStyle = FormBorderStyle.None;
        AutoScaleMode = AutoScaleMode.None;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.Manual;
        Size = new Size(28, 28);
        Cursor = Cursors.Hand;
        _tooltip.SetToolTip(this, "单击取消置顶");
    }

    protected override bool ShowWithoutActivation => true;

    protected override CreateParams CreateParams
    {
        get
        {
            var parameters = base.CreateParams;
            parameters.ExStyle |= 0x00000080 | 0x00080000 | 0x08000000; // Tool window, layered, no activation.
            return parameters;
        }
    }

    internal void Attach()
    {
        NativeMethods.SetOwner(Handle, _target);
        SyncPosition();
    }

    internal void SyncPosition()
    {
        if (!NativeMethods.IsWindow(_target) || !NativeMethods.IsWindowVisible(_target) ||
            NativeMethods.IsIconic(_target) || NativeMethods.IsCloaked(_target) ||
            !NativeMethods.TryGetFrameBounds(_target, out var rect))
        {
            if (Visible)
                Hide();
            return;
        }

        var scale = Math.Max(96, NativeMethods.GetDpiForWindow(_target)) / 96f;
        var size = (int)Math.Round(24 * scale);
        if (Width != size)
        {
            Size = new Size(size, size);
            RenderImage();
        }
        var offset = (int)Math.Round(4 * scale);
        var captionButtons = (int)Math.Round(144 * scale);
        var location = new Point(Math.Max(rect.Left + offset, rect.Right - captionButtons - size), rect.Top + offset);
        var badgeBounds = new Rectangle(location, Size);
        if (!Screen.AllScreens.Any(screen => screen.Bounds.IntersectsWith(badgeBounds)))
        {
            if (Visible)
                Hide();
            return;
        }

        if (!Visible)
        {
            Location = location;
            Show();
            RenderImage();
            NativeMethods.RaiseBadge(Handle);
        }
        else
        {
            NativeMethods.MoveWithoutActivation(Handle, location.X, location.Y, Width, Height);
        }
    }

    private void RenderImage()
    {
        if (!IsHandleCreated)
            return;

        using var image = new Bitmap(Width, Height, PixelFormat.Format32bppPArgb);
        using (var graphics = Graphics.FromImage(image))
        {
            graphics.Clear(Color.Transparent);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            PinArtwork.Draw(graphics, Width);
        }
        NativeMethods.SetLayeredImage(Handle, image, Location);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button == MouseButtons.Left)
            _unpin();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _tooltip.Dispose();
        base.Dispose(disposing);
    }
}
