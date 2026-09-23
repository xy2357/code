using System.Drawing.Drawing2D;

namespace WindowPins;

internal static class PinArtwork
{
    internal static Icon CreateTrayIcon()
    {
        using var image = new Bitmap(32, 32);
        using (var graphics = Graphics.FromImage(image))
        {
            graphics.Clear(Color.Transparent);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            Draw(graphics, 32);
        }

        var handle = image.GetHicon();
        try
        {
            using var borrowed = Icon.FromHandle(handle);
            return (Icon)borrowed.Clone();
        }
        finally
        {
            NativeMethods.DestroyIcon(handle);
        }
    }

    internal static void Draw(Graphics graphics, int size)
    {
        var state = graphics.Save();
        graphics.ScaleTransform(size / 32f, size / 32f);

        using var contrastEdge = new Pen(Color.FromArgb(232, 255, 255, 255), 3.7f)
        {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round,
            LineJoin = LineJoin.Round
        };
        using var pinOutline = new Pen(Color.FromArgb(38, 41, 47), 2.5f)
        {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round,
            LineJoin = LineJoin.Round
        };

        using var needle = new GraphicsPath();
        needle.AddPolygon([
            new PointF(6.6f, 25.5f),
            new PointF(16.2f, 12.5f),
            new PointF(19.1f, 15.4f)
        ]);
        using var head = new GraphicsPath();
        head.AddPolygon([
            new PointF(12.8f, 9.5f),
            new PointF(18.4f, 8.5f),
            new PointF(21.1f, 3.9f),
            new PointF(28.1f, 10.6f),
            new PointF(23.7f, 13.6f),
            new PointF(22.8f, 19.1f),
            new PointF(16.2f, 12.5f)
        ]);
        graphics.DrawPath(contrastEdge, needle);
        graphics.DrawPath(contrastEdge, head);
        graphics.DrawPath(pinOutline, needle);
        graphics.DrawPath(pinOutline, head);
        graphics.Restore(state);
    }
}
